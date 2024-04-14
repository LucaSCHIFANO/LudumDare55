using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummoningCircleManager : MonoBehaviour
{
    [SerializeField] private SummoningCricle circlePrefab;
    [SerializeField] private SkullyController player;
    [SerializeField] private Transform spawnPoint;

    private Pool<SummoningCricle> circlePool;

    [SerializeField] private float circleCooldown;
    private float currentCircleCooldown;
    private int activeCircleCounter = 0;
    
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
        circlePool = new Pool<SummoningCricle>(circlePrefab);
        if (player == null)
            player = FindFirstObjectByType<SkullyController>();
        NewSummonCircle();
    }

    private void Update()
    {
        currentCircleCooldown -= Time.deltaTime;
        if (currentCircleCooldown < 0 && activeCircleCounter < LevelThreshold[currentLevel].maxCircleNumber) NewSummonCircle();
    }

    // A Summon Happened
    private void NewSummon(SummoningCricle circle, SkeletonData data)
    {
        activeCircleCounter--;
        circle.onSummonEntity.RemoveListener(NewSummon);
        IncreaseExperience(data.experience);
    }

    // Instantiate new circle
    private void NewSummonCircle()
    {
        activeCircleCounter++;
        currentCircleCooldown = circleCooldown;

        SummoningCricle currentCircle = circlePool.Get(spawnPoint);
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