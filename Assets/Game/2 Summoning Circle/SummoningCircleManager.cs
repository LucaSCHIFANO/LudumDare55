using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircleManager : MonoBehaviour
{
    [SerializeField] private SummoningCricle circlePrefab;
    private SummoningCricle circleInstance;
    [SerializeField] private CharacterController player;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        if(player == null)
            player = FindFirstObjectByType<CharacterController>();
        NewSummonCircle();
    }

    private void NewSummonCircle()
    {
        circleInstance = Instantiate<SummoningCricle>(circlePrefab,spawnPoint);
        circleInstance.target = player.transform;
        circleInstance.onSummonEntity.AddListener(NewSummonCircle);
    }
}