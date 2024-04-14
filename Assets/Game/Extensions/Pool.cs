using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool<T> where T : PoolItem
{
    private Queue<PoolItem> pool;
    private T instancePrefab;

    public int Count { get => pool.Count; }

    public Pool(T instance)
    {
        pool = new Queue<PoolItem>();
        instancePrefab = instance;
    }

    public T Get()
    {
        if(pool.Count > 0)
            return pool.Dequeue() as T;

        return InstantiateNewInstance();
    }

    public T Get(Vector3 position)
    {
        var instance = Get();
        instance.transform.position = position;
        return instance;
    }

    public T Get(Transform parent, Vector3 position)
    {
        var instance = Get(position);
        instance.transform.SetParent(parent);
        return instance;
    }

    public T Get(Transform parent)
    {
        return Get(parent, parent.position);
    }

    private T InstantiateNewInstance()
    {
        T instance = Object.Instantiate(instancePrefab);
        instance.OnPoolReturn += ReturnToPool;
        return instance;
    }

    private void ReturnToPool(PoolItem item)
    {
        pool.Enqueue(item);
    }
}
