using System;
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
    [SerializeField] private LayerMask wallLayer; 
    [SerializeField] private Animator animator;
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
        summon.onGetReturned.AddListener(OnReturn);
        wanderTimer = wanderCooldown + UnityEngine.Random.Range(-1.5f, 2);
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
        summon.onGetReturned.RemoveListener(OnReturn);
    }

    private void RealmUpdate()
    {
        WanderBehaviourUpdate();
        if (moveState != MovementState.RUNNING) return;

        Move();

        if (Vector2.Distance(targetPoint, transform.position) < .05f)
        {
            moveState = MovementState.IDLE;
            animator.SetBool("IsMoving", false);
        }
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
        animator.SetBool("IsMoving", true);
        wanderTimer += wanderCooldown + UnityEngine.Random.Range(-1.5f, 2);
    }

    private void GetNewTargetPoint()
    {
        RaycastHit2D hit;
        int tries = 500;
        do
        {
            targetPoint = MathExtension.RandomPointInsideCircle(wanderPatrolCenter.position, wanderTargetRadius.y, wanderTargetRadius.x);
            hit = Physics2D.Raycast(transform.position, targetPoint - transform.position, Vector2.Distance(targetPoint,transform.position), wallLayer) ;
            tries--;
        } while (hit.collider != null && tries > 0);
    }

    private void Move()
    {
        Vector2 dir = targetPoint - transform.position;
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed);
    }

    private void OnSummon()
    {
        animator.SetTrigger("Attack");
        isSummoned = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    private void OnReturn()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Idle");
        isSummoned = false;
        summon.ReturnToPool();
        gameObject.SetActive(false);
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}