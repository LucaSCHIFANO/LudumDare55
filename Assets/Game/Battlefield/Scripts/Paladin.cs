using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Paladin : MonoBehaviour
{
    public event UnityAction<Summonable> OnSummonCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Summonable summonable = collision.gameObject.GetComponent<Summonable>();
        if (summonable != null)
        {
            summonable.GetReturned();
            BattlefieldManager.Instance.IncreaseTimer(summonable.Data.increaseTimeValue, summonable.Data.isValueOnPercentage);
            OnSummonCollision.Invoke(summonable);
        }
    }
}
