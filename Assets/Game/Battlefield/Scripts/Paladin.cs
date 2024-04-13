using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Summonable summonable = collision.gameObject.GetComponent<Summonable>();
        if (summonable != null)
        {
            summonable.GetReturned();
            BattlefieldManager.Instance.IncreaseTimer(summonable.Data.increaseTimeValue, summonable.Data.isValueOnPercentage);
        }
    }
}
