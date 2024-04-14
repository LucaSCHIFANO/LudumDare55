using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
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
}
