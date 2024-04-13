using System;
using UnityEngine;
using UnityEngine.Events;

public class Summonable : MonoBehaviour
{
    public UnityEvent onGetSummoned;
    public UnityEvent onGetReturned;

    [SerializeField] private SkeletonData data;
    public SkeletonData Data { get => data; }

    public void GetSummoned()
    {
        BattlefieldManager.Instance.SummonSkeleton(this);
        onGetSummoned.Invoke();
    }

    public void Move()
    {

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