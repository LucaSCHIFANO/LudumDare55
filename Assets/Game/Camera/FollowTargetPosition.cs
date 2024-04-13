using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetPosition : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Start()
    {
        if (target == null)
            Debug.LogError("Camera follow targe tis null !");
    }

    private void Update()
    {
        if (target == null) return;
        transform.position = target.position;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
