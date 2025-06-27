using UnityEditor.VersionControl;
using UnityEngine;


public class prueba : MonoBehaviour
{
    PlayerInputActions input;
    float jump;
    void Awake()
    {
        input = new PlayerInputActions();
        input.PlayerControls.Jump.performed += ctx => jump = ctx.ReadValue<float>();

    }
    void Update()
    {
        // Movimiento
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        float inputX = jump; //This is input for moving left or right
        if (jump != 0)
        {
            Debug.Log('1');
            Debug.Log(inputX);
        }
    }
    //Route and Un-route events
private void OnEnable() => input.Enable();
private void OnDisable() => input.Disable();
}
