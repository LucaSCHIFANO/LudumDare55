using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Summonable : PoolItem
{
    [SerializeField, Min(1)] private int minimumSummonLevel = 1;
    [SerializeField] private SkeletonData data;
    [SerializeField] private float summonedAnimaitionTime;

    public UnityEvent onGetSummoned;
    public UnityEvent onGetReturned;

    public SkeletonData Data { get => data; }

    public bool CanBeSummoned(int magicLevel)
    {
        return magicLevel >= minimumSummonLevel;
    }

    public void GetSummoned()
    {
        onGetSummoned.Invoke();
        StartCoroutine(WaitForAnimation());

        IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(summonedAnimaitionTime);
            BattlefieldManager.Instance.SummonSkeleton(this);
        }
    }

    public void GetReturned()
    {
        onGetReturned.Invoke();
    }
}


[Serializable]
public class SkeletonData
{
    public float speed;
    public float increaseTimeValue;
    public bool isValueOnPercentage;
    public float experience;
}