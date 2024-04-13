using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Summonable))]
public class NPCCharacterController : MonoBehaviour
{
    private enum MovementState { IDLE, RUNNING }

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 wanderTargetRadius;
    [SerializeField] private float wanderCooldown;
    [SerializeField] private Transform wanderPatrolCenter;

    private MovementState moveState = MovementState.IDLE;
    private Vector3 targetPoint;
    private float wanderTimer = 0f;
    private bool isSummoned = false;
    private Summonable summon;

    private void Start()
    {
        summon = GetComponent<Summonable>();
        summon.onGetSummoned.AddListener(OnSummon);

        if (wanderPatrolCenter == null)
            wanderPatrolCenter = transform;
    }

    private void Update()
    {
        if (isSummoned) return;
        RealmUpdate();
    }

    private void OnDisable()
    {
        summon.onGetSummoned.RemoveListener(OnSummon);
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
        targetPoint = MathExtension.RandomPointInsideCircle(wanderPatrolCenter.position, wanderTargetRadius.y, wanderTargetRadius.x);
    }

    private void Move()
    {
        Vector2 dir = targetPoint - transform.position;
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed);
    }

    private void OnSummon()
    {
        isSummoned = true;
    }
}
