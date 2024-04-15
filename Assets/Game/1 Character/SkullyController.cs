using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkullyController : MonoBehaviour
{
    [SerializeField] private FollowTargetPosition cameraFollow;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5;
    private Vector3 inputDirection;
    private float dashTime;
    [SerializeField] private float dashCooldown = 2;
    [SerializeField] private float dashDuration = 1;
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private AnimationCurve dashCurve;
    bool isDashing = false;
    private Vector3 dashDirection = Vector3.right;

    [SerializeField] private float respawnTime = 2;
    private Summonable summonable;
    private bool isSummoned = false;
    private Vector3 summonedPosition;
    private Collider2D summonCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask wallLayer;

    private void Start()
    {
        summonable = GetComponent<Summonable>();
        summonCollider = GetComponent<Collider2D>();

        summonable.onGetSummoned.AddListener(OnSummoned);
        summonable.onGetSummoned.AddListener(() => cameraFollow.SetTarget(null));

        summonable.onGetReturned.AddListener(OnReturn);
        summonable.onGetReturned.AddListener(() => cameraFollow.SetTarget(transform));
    }

    private void Update()
    {
        if (isSummoned)
            return;

        if (isDashing)
        {
            float t = (Time.time - dashTime) / dashDuration;
            Vector3 direction = dashDirection.normalized;
            Vector2 dir = new Vector2(Mathf.Sign(direction.x), 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .5f, wallLayer);
            if (hit.collider != null)
            {
                direction.x = 0;
            }

            dir = new Vector2(0, Mathf.Sign(direction.y));
            hit = Physics2D.Raycast(transform.position, dir, .5f, wallLayer);
            if (hit.collider != null)
            {
                direction.y = 0;
            }
            transform.position += Time.deltaTime * dashSpeed * direction * dashCurve.Evaluate(t);
            if(t>= 1)
            {
                isDashing = false;
            }
        }
        else
        {
            
        Vector3 direction = inputDirection.normalized;
        Vector2 dir = new Vector2 (Mathf.Sign(direction.x), 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .5f, wallLayer);
        if(hit.collider != null)
        {
            direction.x = 0;
        }
        
        dir = new Vector2 (0, Mathf.Sign(direction.y));
        hit = Physics2D.Raycast(transform.position, dir, .5f, wallLayer);
        if(hit.collider != null)
        {
            direction.y = 0;
        }

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
        inputDirection.x = context.ReadValue<float>();
    }
    
    public void VerticalInput(InputAction.CallbackContext context)
    {
        inputDirection.y = context.ReadValue<float>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(Time.time - dashTime >= dashCooldown) 
        {
            isDashing = true;
            dashTime = Time.time;
            dashDirection = dashDirection.normalized;
            SoundManager.Instance.Play("Dash");
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
        summonCollider.enabled = false;
        transform.position = summonedPosition;
        animator.SetTrigger("IsReturned");
        StartCoroutine(WaitRespawnTime());

        IEnumerator WaitRespawnTime()
        {
            yield return new WaitForSeconds(respawnTime);
            isSummoned = false;
            summonCollider.enabled = true;
        }
    }

    private void OnDestroy()
    {
        summonable.onGetSummoned.RemoveListener(OnSummoned);
        summonable.onGetSummoned.RemoveListener(() => cameraFollow.SetTarget(null));

        summonable.onGetReturned.RemoveListener(OnReturn);
        summonable.onGetReturned.RemoveListener(() => cameraFollow.SetTarget(transform));
    }
}
