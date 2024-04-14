using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] inside;
    [SerializeField] private GameObject[] outside;

    private void Start()
    {
        SetObjectsActive(false, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetObjectsActive(false, true) ;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetObjectsActive(false, true);
        }
    }

    private void SetObjectsActive(bool insideValue, bool outideValue)
    {
        for (int i = 0; i < inside.Length; i++)
        {
            inside[i].SetActive(insideValue);
        }
        
        for (int i = 0; i < outside.Length; i++)
        {
            outside[i].SetActive(outideValue);
        }
    }
}
