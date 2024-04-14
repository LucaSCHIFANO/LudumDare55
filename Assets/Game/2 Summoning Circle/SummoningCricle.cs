using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummoningCricle : PoolItem
{
    [System.Serializable]
    public class Data
    {
        [Header("Movement")]
        public MovementType MoveType;
        [AllowNesting, HideIf(nameof(MoveType), MovementType.STATIC)] public float MoveSpeed;

        [Header("Summons")]
        [SerializeField] public SummonType SummonType;
        [SerializeField, AllowNesting, HideIf(nameof(SummonType), SummonType.CONTACT)] public float CastTime;

        [Header("Visuals")]
        [SerializeField] public float Size = 1f;
        [SerializeField] public Color Color = new Color(1f, 1f, 1f);
    }

    public enum MovementType { STATIC, FOLLOW, SNAP_FOLLOW }
    public enum SummonType { CONTACT, TIMER, TIMED_CONTACT }

    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer visualRenderer;
    
    public UnityEvent<SummoningCricle, SummonData> onSummonEntity;

    private MovementType moveType;
    private float movementSpeed;

    private int level = 1;
    private SummonType summonType;

    private List<Summonable> entitiesToSummon = new List<Summonable>();
    private float castTimer = 0f;
    private bool useCastTimer;

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

    public void Init(Transform target, int level, Data circleData)
    {
        this.target = target;
        this.level = level;

        moveType = circleData.MoveType;
        movementSpeed = circleData.MoveSpeed;

        summonType = circleData.SummonType;

        useCastTimer = summonType == SummonType.TIMER;
        castTimer = circleData.CastTime;

        transform.localScale = Vector3.one * circleData.Size;
        visualRenderer.color = circleData.Color;
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
        foreach (var entity in entitiesToSummon)
        {
            entity.GetSummoned();
            onSummonEntity.Invoke(this, entity.Data);
        }

        ReturnToPool();
    }
}
