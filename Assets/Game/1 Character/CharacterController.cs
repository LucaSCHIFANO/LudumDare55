using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5;
    private Vector3 direction;
    private float dashTime;
    [SerializeField] private float dashCooldown = 2;
    [SerializeField] private float dashDuration = 1;
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private AnimationCurve dashCurve;
    bool isDashing = false;
    private Vector3 dashDirection = Vector3.right;

    private void Update()
    {
        direction = direction.normalized;
        if (isDashing)
        {
            float t = (Time.time - dashTime) / dashDuration;
            transform.position += Time.deltaTime * dashSpeed * dashDirection * dashCurve.Evaluate(t);
            if(t>= 1)
            {
                isDashing = false;
            }
        }
        else
        {
            transform.position += Time.deltaTime * movementSpeed * direction;

            if (direction.sqrMagnitude > .5)
                dashDirection = direction;
        }
    }

    public void HorizontalInput(InputAction.CallbackContext context)
    {
        direction.x = context.ReadValue<float>();
    }
    
    public void VerticalInput(InputAction.CallbackContext context)
    {
        direction.y = context.ReadValue<float>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(Time.time - dashTime >= dashCooldown) 
        {
            isDashing = true;
            dashTime = Time.time;
            dashDirection = dashDirection.normalized;
        }
    }
}
