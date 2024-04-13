using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinVisual : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform target;

    void Awake()
    {
        if(transform.parent != null)
        {
            target = transform.parent;
            transform.parent = null;
        }
    }

    void Update()
    {
        if(target != null) transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
    }
}
