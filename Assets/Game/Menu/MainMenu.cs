using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject mainMenuPrefab;
    [SerializeField] private GameObject htpMenuPrefab;
    public void QuitButton()
    {
       /*
        if (Application.isEditor) EditorApplication.isPlaying = false;
        Application.Quit();*/
    }

    public void HowToPlay()
    {
        mainMenuPrefab.SetActive(false);
        htpMenuPrefab.SetActive(true);
    }

    public void BackButton()
    {
        mainMenuPrefab.SetActive(true);
        htpMenuPrefab.SetActive(false);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }
}
