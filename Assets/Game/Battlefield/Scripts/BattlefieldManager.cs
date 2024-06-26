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
    [SerializeField] private Paladin paladin;
    private Vector3 wizardStartingPoint;
    private Vector3 paladinStartingPoint;
    private Vector3 paladinToWizardvector;

    [Header("Timer")]
    [SerializeField] private float maxTimerDuration;
    private float currentTimerDuration;

    [Header("Spawn")]
    [SerializeField] private Transform skeletonSpawnPoint;
    private List<Summonable> skeletonWaitingToSpawn = new List<Summonable>();
    private List<Summonable> skeletonSpawned = new List<Summonable>();

    [SerializeField] private float timeBetweenSkeletonSpawn;
    private float currentTimeBetweenSkeletonSpawn;

    [Header("GameOver")]
    private bool gameOver = false;
    [SerializeField] private GameOver goScreen;

    [Header("Win")]
    private bool win = false;
    [SerializeField] private float winWait;
    [SerializeField] private GameOver winScreen;
    [SerializeField] private Timer timer;

    [SerializeField] private Animator wizardAnimator;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        wizardStartingPoint = wizard.transform.position;
        paladinStartingPoint = paladin.transform.position;
        paladinToWizardvector = (wizardStartingPoint - paladinStartingPoint);

        currentTimerDuration = maxTimerDuration;
    }

    private void Start()
    {
        paladin.OnSummonCollision += RemoveSkeleton;
    }

    void Update()
    {
        if (gameOver == true) return;

        if (currentTimerDuration > 0)
        {
            if (win == false)
            {
                currentTimerDuration -= Time.deltaTime;
                updatePaladinPosition();
            }
        }
        else
        {
            Time.timeScale = 0;
            goScreen.gameObject.SetActive(true);
            goScreen.SetTimer($"You survived {timer.Stop()}...");
            gameOver = true;
        }

        if (skeletonWaitingToSpawn.Count > 0 && currentTimeBetweenSkeletonSpawn <= 0) 
        {
            skeletonSpawned.Add(skeletonWaitingToSpawn[0]);
            skeletonWaitingToSpawn.RemoveAt(0);

            currentTimeBetweenSkeletonSpawn = timeBetweenSkeletonSpawn;
        }

        MoveSkeletons();

        currentTimeBetweenSkeletonSpawn -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        paladin.OnSummonCollision -= RemoveSkeleton;
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

    public void StopTimer()
    {
        win = true;
        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        winScreen.SetTimer($"You win in {timer.Stop()}");
        yield return new WaitForSeconds(winWait);
        winScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SummonSkeleton(Summonable npc)
    {
        skeletonWaitingToSpawn.Add(npc);
        npc.transform.position = skeletonSpawnPoint.position;
        wizardAnimator.SetTrigger("Summon");
    }

    private void MoveSkeletons()
    {
        for (int i = 0; i < skeletonSpawned.Count; ++i)
        {
            skeletonSpawned[i].transform.position += new Vector3(skeletonSpawned[i].Data.speed * Time.deltaTime, 0, 0);
        }
    }

    private void RemoveSkeleton(Summonable npc)
    {
        skeletonSpawned.Remove(npc);
    }
}