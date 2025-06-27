using UnityEditor.VersionControl;
using UnityEngine;

public class ShadowPlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    PlayerInputActions input;

    Vector2 movementInput;
    float jump;
    float crouch;
    float slamAttack;
    float attack;
    float fire_bow;
    float block;
    float slide;
    [HideInInspector]
    public float inventory;
    [HideInInspector]
    public float use;


    [Header("Sombra")]
    public bool isShadow = false;
    public float shadowDuration = 5f;
    public float shadowCooldown = 3f;
    private float shadowTimer = 0f;
    private float cooldownTimer = 0f;

    [Header("Visual")]
    public Color normalColor = Color.white;
    public Color shadowColor = Color.black;
    public Color normalBackgroundColor = Color.cyan;
    public Color shadowBackgroundColor = Color.gray;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    [Header("Efectos")]
    public ParticleSystem shadowEffectEnter;
    public ParticleSystem shadowEffectExit;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip enterShadowClip;
    public AudioClip exitShadowClip;


    void Awake()
    {
        input = new PlayerInputActions();
        input.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        input.PlayerControls.Jump.performed += ctx => jump = ctx.ReadValue<float>();
        input.PlayerControls.Attack.performed += ctx => attack = ctx.ReadValue<float>();
        input.PlayerControls.FireBow.performed += ctx => fire_bow = ctx.ReadValue<float>();
        input.PlayerControls.Crouch.performed += ctx => crouch = ctx.ReadValue<float>();
        input.PlayerControls.Crouch.performed += ctx => slamAttack = ctx.ReadValue<float>();
        input.PlayerControls.Block.performed += ctx => block = ctx.ReadValue<float>();
        input.PlayerControls.Slide.performed += ctx => slide = ctx.ReadValue<float>();
        input.PlayerControls.Inventory.performed += ctx => inventory = ctx.ReadValue<float>();
        input.PlayerControls.PickUpItem.performed += ctx => use = ctx.ReadValue<float>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        if (mainCamera != null)
            mainCamera.backgroundColor = normalBackgroundColor;
    }

    void Update()
    {
        // Movimiento
        float inputX = movementInput.x; //This is input for moving left or right
        float attackperform = attack;
        if (inputX != 0)
        {
            
            Debug.Log("movement " + inputX);
        }
        if (attackperform != 0)
        {
            Debug.Log("attack " + inputX);
        }

        // Transformación
         /*   if (Input.GetKeyDown(KeyCode.LeftShift) && !isShadow && cooldownTimer <= 0f)
            {
                EnterShadowForm();
            }
            */
 
        if (isShadow)
        {
            shadowTimer -= Time.deltaTime;
            if (shadowTimer <= 0f)
            {
                ExitShadowForm();
            }
        }
        else
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime);

    }

    void EnterShadowForm()
    {
        isShadow = true;
        shadowTimer = shadowDuration;
        spriteRenderer.color = shadowColor;

        if (mainCamera != null)
            mainCamera.backgroundColor = shadowBackgroundColor;

        gameObject.layer = LayerMask.NameToLayer("Shadow");

        if (shadowEffectEnter != null)
            shadowEffectEnter.Play();

        if (audioSource != null && enterShadowClip != null)
            audioSource.PlayOneShot(enterShadowClip);
    }

    void ExitShadowForm()
    {
        isShadow = false;
        cooldownTimer = shadowCooldown;
        spriteRenderer.color = normalColor;

        if (mainCamera != null)
            mainCamera.backgroundColor = normalBackgroundColor;

        gameObject.layer = LayerMask.NameToLayer("Player");

        if (shadowEffectExit != null)
            shadowEffectExit.Play();

        if (audioSource != null && exitShadowClip != null)
            audioSource.PlayOneShot(exitShadowClip);
    }
    

private void OnEnable() => input.Enable();
private void OnDisable() => input.Disable();

}
