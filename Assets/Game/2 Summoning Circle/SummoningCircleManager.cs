using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircleManager : MonoBehaviour
{
    [SerializeField] private SummoningCricle circlePrefab;
    private SummoningCricle circleInstance;
    [SerializeField] private CharacterController player;

    private void Start()
    {
        circleInstance = Instantiate<SummoningCricle>(circlePrefab);
        if(player == null)
            player = FindFirstObjectByType<CharacterController>();
        circleInstance.target = player.transform;
    }
}
