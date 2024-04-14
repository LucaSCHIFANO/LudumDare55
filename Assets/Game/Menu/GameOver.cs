using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
   public void RestartButton()
   {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

    public void QuiButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void SetTimer(string text)
    {
        timer.text = text;
    }
}
