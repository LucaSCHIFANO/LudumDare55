using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattlefieldManager : MonoBehaviour
{
    private static BattlefieldManager instance = null;
    public static BattlefieldManager Instance => instance;

    [Header("GameObject")]
    [SerializeField] private GameObject wizard;  
    [SerializeField] private GameObject paladin;
    private Vector3 wizardStartingPoint;
    private Vector3 paladinStartingPoint;
    private Vector3 paladinToWizardvector;

    [Header("Timer")]
    [SerializeField] private float maxTimerDuration;
    private float currentTimerDuration;

    [Header("Spawn")]
    [SerializeField] private Transform skeletonSpawnPoint;
    private List<Transform> skeletonWaitingToSpawn = new List<Transform>();
    private List<Transform> skeletonSpawned = new List<Transform>();

    [SerializeField] private float timeBetweenSkeletonSpawn;
    private float currentTimeBetweenSkeletonSpawn;
    [SerializeField] private float skeletonSpeed;
    

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        wizardStartingPoint = wizard.transform.position;
        paladinStartingPoint = paladin.transform.position;
        paladinToWizardvector = (wizard.transform.position - paladin.transform.position);

        currentTimerDuration = maxTimerDuration;
    }

    void Update()
    {
        if(currentTimerDuration > 0)
        {
            currentTimerDuration -= Time.deltaTime;
            updatePaladinPosition();
        }

        if (skeletonWaitingToSpawn.Count > 0 && currentTimeBetweenSkeletonSpawn <= 0) 
        {
            skeletonWaitingToSpawn[0].transform.position = skeletonSpawnPoint.position;
            skeletonSpawned.Add(skeletonWaitingToSpawn[0]);
            skeletonWaitingToSpawn.RemoveAt(0);

            currentTimeBetweenSkeletonSpawn = timeBetweenSkeletonSpawn;
        }

        MoveSkeletons();

        currentTimeBetweenSkeletonSpawn -= Time.deltaTime;
    }

    private void updatePaladinPosition()
    {
        float timerNormalized = currentTimerDuration / maxTimerDuration;
        paladin.transform.position = paladinStartingPoint + paladinToWizardvector * (1 - timerNormalized);
    }

    public void IncreaseTimer(float time, bool isPercentage = false)
    {
        if (isPercentage)
            currentTimerDuration += maxTimerDuration * time;
        else
            currentTimerDuration += time;

        currentTimerDuration = Mathf.Clamp(currentTimerDuration,0 , maxTimerDuration);
    }

    public void SummonSkeleton(Transform npc)
    {
        skeletonWaitingToSpawn.Add(npc);
    }

    private void MoveSkeletons()
    {
        for (int i = 0; i < skeletonSpawned.Count; ++i)
        {
            if (skeletonSpawned[i] == null)
            {
                skeletonSpawned.RemoveAt(i);
                i--;
            } 
            else skeletonSpawned[i].position += new Vector3(skeletonSpeed, 0, 0);
        }
    }
}