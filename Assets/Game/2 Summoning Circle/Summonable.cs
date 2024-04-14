using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Summonable : PoolItem
{
    [SerializeField, Min(1)] private int minimumSummonLevel = 1;
    [SerializeField] private SummonData data;
    [SerializeField] private float summonedAnimaitionTime;

    public UnityEvent onBeginSummon;
    public UnityEvent onGetSummoned;
    public UnityEvent onGetReturned;

    public SummonData Data { get => data; }

    private bool isSummoned;

    public bool CanBeSummoned(int magicLevel)
    {
        return magicLevel >= minimumSummonLevel;
    }

    public void GetSummoned()
    {
        if (isSummoned) return;

        isSummoned = true;
        onBeginSummon.Invoke();
        StartCoroutine(WaitForAnimation());

        IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(summonedAnimaitionTime);
            onGetSummoned.Invoke();
            BattlefieldManager.Instance.SummonSkeleton(this);
        }
    }

    public void GetReturned()
    {
        isSummoned = false;
        onGetReturned.Invoke();
    }
}


[Serializable]
public class SummonData
{
    public float speed;
    public float increaseTimeValue;
    public bool isValueOnPercentage;
    public float experience;
}