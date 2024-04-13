using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummoningCricle : MonoBehaviour
{
    private enum MovementType { STATIC, FOLLOW, SNAP_FOLLOW }
    private enum SummonType { CONTACT, TIMER, TIMED_CONTACT }

    public Transform target;

    [Header("Movement")]
    [SerializeField] private MovementType moveType;
    [SerializeField, HideIf(nameof(moveType), MovementType.STATIC)] private float movementSpeed;

    [Header("Summons")]
    [SerializeField, Min(1)] private int level = 1;
    [SerializeField] private SummonType summonType;
    [SerializeField, HideIf(nameof(summonType), SummonType.CONTACT)] private float summonCastTime;

    public UnityEvent onSummonEntity;

    private List<Summonable> entitiesToSummon = new List<Summonable>();
    private float castTimer = 0f;
    private bool useCastTimer;

    private void Start()
    {
        useCastTimer = summonType == SummonType.TIMER;
        castTimer = summonCastTime;
    }

    private void Update()   
    {
        Move();
        if (!useCastTimer) return;

        if(castTimer <= 0f)
        {
            Summon();
            return;
        }

        castTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Summonable summonable)) return;
        if (!summonable.CanBeSummoned(level)) return;
        if (entitiesToSummon.Contains(summonable)) return;

        if (moveType == MovementType.SNAP_FOLLOW)
            target = summonable.transform;

        entitiesToSummon.Add(summonable);

        // Summon on contact
        if (summonType == SummonType.CONTACT)
            Summon();
        // Trigger Timer on contact
        else if (summonType == SummonType.TIMED_CONTACT)
            useCastTimer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Summonable summonable)) return;
        if (!entitiesToSummon.Contains(summonable)) return;

        entitiesToSummon.Remove(summonable);
    }

    private void Move()
    {
        if (moveType == MovementType.STATIC) return;
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * Time.deltaTime * movementSpeed;
    }

    private void Summon()
    {
        entitiesToSummon.ForEach(s => s.GetSummoned());

        onSummonEntity.Invoke();
        Destroy(gameObject);
    }
}
