using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummoningCircleManager : MonoBehaviour
{
    [SerializeField] private SummoningCricle circlePrefab;
    private List<SummoningCricle> circleList = new List<SummoningCricle>();
    [SerializeField] private CharacterController player;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float circleCooldown;
    private float currentCircleCooldown;
    
    [Header("Experience")]
    [SerializeField] private List<CircleLevel> LevelThreshold = new List<CircleLevel>();
    private float currentExperience;
    private int currentLevel;

    [SerializeField] private Image ExpBar;

    private void Awake()
    {
        currentCircleCooldown = circleCooldown;    
    }

    private void Start()
    {
        if(player == null)
            player = FindFirstObjectByType<CharacterController>();
        NewSummonCircle();
    }

    private void Update()
    {
        currentCircleCooldown -= Time.deltaTime;
        if (currentCircleCooldown < 0 && circleList.Count < LevelThreshold[currentLevel].maxCircleNumber) NewSummonCircle();
    }

    private void NewSummon(SummoningCricle circle, SkeletonData data)
    {
        circleList.Remove(circle);
        IncreaseExperience(data.experience);
    }

    private void NewSummonCircle()
    {
        currentCircleCooldown = circleCooldown;
        SummoningCricle currentCircle = Instantiate<SummoningCricle>(circlePrefab, spawnPoint);
        circleList.Add(currentCircle);
        currentCircle.target = player.transform;
        currentCircle.onSummonEntity.AddListener(NewSummon);
    }

    private void IncreaseExperience(float exp)
    {
        currentExperience += exp;

        for (int i = 0; i < LevelThreshold.Count; i++)
        {
            if (LevelThreshold[i].experience <= currentExperience) 
                currentLevel = i;
        }

        if (currentLevel != LevelThreshold.Count - 1)
            ExpBar.fillAmount = (currentExperience - LevelThreshold[currentLevel].experience) / 
                (LevelThreshold[currentLevel + 1].experience - LevelThreshold[currentLevel].experience);
        else ExpBar.fillAmount = 1;
    }
}

[Serializable]
public class CircleLevel
{
    public GameObject circle;
    public float experience;
    public int maxCircleNumber;
}