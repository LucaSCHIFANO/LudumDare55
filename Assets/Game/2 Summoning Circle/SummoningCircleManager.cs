using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummoningCircleManager : MonoBehaviour
{
    [SerializeField] private SummoningCricle circlePrefab;
    [SerializeField] private SkullyController player;
    [SerializeField, MinMaxSlider(0f, 20f)] private Vector2 circleSpawnRadius;

    private Pool<SummoningCricle> circlePool;

    [SerializeField] private float circleCooldown;
    private float currentCircleCooldown;
    private int activeCircleCounter = 0;
    
    [Header("Experience")]
    [SerializeField] private List<CircleLevel> LevelThreshold = new List<CircleLevel>();
    private float currentExperience;
    private int currentLevel;

    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI expText;

    private void Awake()
    {
        currentCircleCooldown = circleCooldown;    
    }

    private void Start()
    {
        circlePool = new Pool<SummoningCricle>(circlePrefab);
        if (player == null)
            player = FindFirstObjectByType<SkullyController>();
        SpawnCircle(LevelThreshold[currentLevel]);
    }

    private void Update()
    {
        currentCircleCooldown -= Time.deltaTime;
        if (currentCircleCooldown < 0 && activeCircleCounter < LevelThreshold[currentLevel].maxCircleNumber) SpawnCircle(LevelThreshold[currentLevel]);
    }

    private void OnEntitySummon(SummoningCricle circle, SummonData data)
    {
        circle.onSummonEntity.RemoveListener(OnEntitySummon);
        IncreaseExperience(data.experience);
    }

    private void OnCircleSummon(SummoningCricle circle)
    {
        activeCircleCounter--;
        circle.onSummon.RemoveListener(OnCircleSummon);
    }

    private void SpawnCircle(CircleLevel level)
    {
        activeCircleCounter++;
        currentCircleCooldown = circleCooldown;

        SummoningCricle currentCircle = circlePool.Get(MathExtension.RandomPointInsideCircle(player.transform.position, circleSpawnRadius.y, circleSpawnRadius.x));
        currentCircle.Init(player.transform, currentLevel + 1, level.data);

        currentCircle.onSummonEntity.AddListener(OnEntitySummon);
        currentCircle.onSummon.AddListener(OnCircleSummon);
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
            expBar.fillAmount = (currentExperience - LevelThreshold[currentLevel].experience) / 
                (LevelThreshold[currentLevel + 1].experience - LevelThreshold[currentLevel].experience);
        else expBar.fillAmount = 1;

        expText.text = $"Lvl. {currentLevel+1}";
    }
}

[Serializable]
public class CircleLevel
{
    public GameObject circle;
    public SummoningCricle.Data data;
    public float experience;
    public int maxCircleNumber;
}