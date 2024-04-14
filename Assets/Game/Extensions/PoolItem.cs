using UnityEngine;
using UnityEngine.Events;

public abstract class PoolItem : MonoBehaviour, IPoolItem<PoolItem>
{
    public event UnityAction<PoolItem> OnPoolReturn;

    virtual public void ReturnToPool()
    {
        OnPoolReturn?.Invoke(this);
    }
}

public interface IPoolItem<T> where T : IPoolItem<T>
{
    public event UnityAction<T> OnPoolReturn;
    public void ReturnToPool();
}
