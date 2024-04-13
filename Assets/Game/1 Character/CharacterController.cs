using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private FollowTargetPosition cameraFollow;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5;
    private Vector3 direction;
    private float dashTime;
    [SerializeField] private float dashCooldown = 2;
    [SerializeField] private float dashDuration = 1;
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private AnimationCurve dashCurve;
    bool isDashing = false;
    private Vector3 dashDirection = Vector3.right;

    private Summonable summonable;
    private bool isSummoned = false;
    private Vector3 summonedPosition;
    [SerializeField] private Animator animator;

    private void Start()
    {
        summonable = GetComponent<Summonable>();
        summonable.onGetSummoned.AddListener(OnSummoned);
        summonable.onGetSummoned.AddListener(() => cameraFollow.SetTarget(null));

        summonable.onGetReturned.AddListener(OnReturn);
        summonable.onGetReturned.AddListener(() => cameraFollow.SetTarget(transform));
    }

    private void Update()
    {
        if (isSummoned)
            return;


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
            {
                animator.SetBool("IsMoving", true);
                dashDirection = direction;
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
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

    private void OnSummoned()
    {
        animator.SetTrigger("IsSummoned");
        summonedPosition = transform.position;
        isSummoned = true;
    }

    private void OnReturn()
    {
        animator.SetTrigger("IsReturned");
        transform.position = summonedPosition;
        isSummoned = false;
    }

    private void OnDestroy()
    {
        summonable.onGetSummoned.RemoveListener(OnSummoned);
        summonable.onGetSummoned.RemoveListener(() => cameraFollow.SetTarget(null));

        summonable.onGetReturned.RemoveListener(OnReturn);
        summonable.onGetReturned.RemoveListener(() => cameraFollow.SetTarget(transform));
    }
}
