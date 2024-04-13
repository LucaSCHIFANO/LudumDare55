using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummoningCricle : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float movementSpeed;
    public UnityEvent onSummonEntity;

    private void Update()   
    {
        if (target == null) 
            return;
        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * Time.deltaTime * movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var summonable = collision.GetComponent<ISummonable>();
        if(summonable != null)
        {
            summonable.OnGetSummoned();
            onSummonEntity.Invoke();
            Destroy(gameObject);
        }
    }
}
