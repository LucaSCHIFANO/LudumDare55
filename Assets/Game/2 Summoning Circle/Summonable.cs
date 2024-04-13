using UnityEngine;
using UnityEngine.Events;

public class Summonable : MonoBehaviour
{
    public UnityEvent onGetSummoned;
    public UnityEvent onGetReturned;

    public void GetSummoned()
    {
        BattlefieldManager.Instance.SummonSkeleton(transform);
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
