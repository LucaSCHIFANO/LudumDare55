using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCCharacterController : MonoBehaviour
{
    private enum WorldState { SKELETON_REALM, BATTLEFIELD }
    private enum MovementState { IDLE, RUNNING }

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 wanderTargetRadius;
    [SerializeField] private float wanderCooldown;

    public event UnityAction OnSummoned;

    private WorldState worldState = WorldState.SKELETON_REALM;
    private MovementState moveState = MovementState.IDLE;
    private Vector3 targetPoint;
    private float wanderTimer = 0f;

    private void Update()
    {
        switch (worldState)
        {
            case WorldState.SKELETON_REALM:
                RealmUpdate();
                break;

            case WorldState.BATTLEFIELD:
            default:
                break;
        }
    }

    private void RealmUpdate()
    {
        WanderBehaviourUpdate();
        if (moveState != MovementState.RUNNING) return;

        Move();

        if (Vector2.Distance(targetPoint, transform.position) < .05f)
            moveState = MovementState.IDLE;
    }

    private void WanderBehaviourUpdate()
    {
        if (wanderTimer > 0f)
        {
            wanderTimer -= Time.deltaTime;
            return;
        }

        GetNewTargetPoint();
        moveState = MovementState.RUNNING;
        wanderTimer += wanderCooldown;
    }

    private void GetNewTargetPoint()
    {
        targetPoint = MathExtension.RandomPointInsideCircle(transform.position, wanderTargetRadius.y, wanderTargetRadius.x);
    }

    private void Move()
    {
        Vector2 dir = targetPoint - transform.position;
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed);
    }
}
