using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] inside;
    [SerializeField] private GameObject[] outside;

    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private Summonable[] skulliesToSummon;
    private int leftToSummon;

    private void Start()
    {
        leftToSummon = skulliesToSummon.Length;
        for (int i = 0; i < skulliesToSummon.Length; i++)
        {
            skulliesToSummon[i].onGetSummoned.AddListener(SkellySummoned);
        }
        SetObjectsActive(false, true);
    }

    private void SkellySummoned()
    {
        leftToSummon--;
        if(leftToSummon <= 0)
        {
            doorRenderer.sprite = openDoorSprite;
            doorCollider.gameObject.SetActive(false);   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetObjectsActive(true, false) ;
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
