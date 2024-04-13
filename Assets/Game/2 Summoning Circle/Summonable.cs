using System;
using UnityEngine;
using UnityEngine.Events;

public class Summonable : MonoBehaviour
{
    [SerializeField, Min(1)] private int minimumSummonLevel = 1;
    [SerializeField] private SkeletonData data;
    
    public UnityEvent onGetSummoned;
    public UnityEvent onGetReturned;

    public SkeletonData Data { get => data; }

    public bool CanBeSummoned(int magicLevel)
    {
        return magicLevel >= minimumSummonLevel;
    }

    public void GetSummoned()
    {
        BattlefieldManager.Instance.SummonSkeleton(this);
        onGetSummoned.Invoke();
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