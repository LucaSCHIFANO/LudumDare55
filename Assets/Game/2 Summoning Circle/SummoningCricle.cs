using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummoningCricle : MonoBehaviour
{
    private enum MovementType { STATIC, FOLLOW, SNAP_FOLLOW }

    public Transform target;
    [SerializeField] private float movementSpeed;
    [SerializeField] private MovementType moveType;

    public UnityEvent onSummonEntity;

    private void Update()   
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var summonable = collision.GetComponent<Summonable>();
        if(summonable != null)
        {
            if (moveType == MovementType.SNAP_FOLLOW)
                target = summonable.transform;
            
            summonable.GetSummoned();
            onSummonEntity.Invoke();
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (moveType == MovementType.STATIC) return;
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * Time.deltaTime * movementSpeed;
    }
}
