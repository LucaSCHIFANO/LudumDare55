using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Paladin : MonoBehaviour
{
    public event UnityAction<Summonable> OnSummonCollision;
    [SerializeField] private PaladinVisual visual;
    private bool isDead = false;
    private Rigidbody2D rb;
    [SerializeField] Vector2 deathVector;
    [SerializeField] float rotationSpeed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (isDead) 
        {
            rb.rotation += rotationSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Summonable summonable = collision.gameObject.GetComponent<Summonable>();
        if (summonable != null)
        {
            if (collision.gameObject.tag == "King")
            {
                if (isDead) return;
                BattlefieldManager.Instance.StopTimer();
                OnDeath();
            }
            else
            {
                summonable.GetReturned();
                BattlefieldManager.Instance.IncreaseTimer(summonable.Data.increaseTimeValue, summonable.Data.isValueOnPercentage);
                OnSummonCollision.Invoke(summonable);
            }
        }
    }

    private void OnDeath()
    {
        isDead = true;
        visual.enabled = false;
        visual.transform.position = transform.position;
        visual.transform.parent = transform;
        rb.gravityScale = 1f;
        rb.velocity = deathVector;
    }
}
