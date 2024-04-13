using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattlefieldManager : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject wizard;  
    [SerializeField] private GameObject paladin;
    private Vector3 wizardStartingPoint;
    private Vector3 paladinStartingPoint;
    private Vector3 paladinToWizardvector;

    [Header("Timer")]
    [SerializeField] private float maxTimerDuration;
    private float currentTimerDuration;

    void Awake()
    {
        wizardStartingPoint = wizard.transform.position;
        paladinStartingPoint = paladin.transform.position;
        paladinToWizardvector = (wizard.transform.position - paladin.transform.position);

        currentTimerDuration = maxTimerDuration;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) IncreaseTimer(1, true);
        else if (Input.GetKeyDown(KeyCode.LeftShift)) IncreaseTimer(1, false);

        if(currentTimerDuration > 0)
        {
            currentTimerDuration -= Time.deltaTime;
            updatePaladinPosition();
        }
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


}
