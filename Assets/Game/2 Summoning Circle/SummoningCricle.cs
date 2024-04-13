using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCricle : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float movementSpeed;

    private void Update()
    {
        if (target == null) 
            return;
        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * Time.deltaTime * movementSpeed;
    }
}
