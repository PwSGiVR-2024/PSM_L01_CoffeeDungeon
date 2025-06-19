using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject endRunCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnClickEndRun()
    {
        endRunCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickReturnToGame()
    {
        endRunCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void OnClickGoToEndRunScene()
    {
        PlayerPrefs.SetInt("FinalScore", SatisfactionManager.Instance.GetScore());
        PlayerPrefs.SetString("FinalLevel", SatisfactionManager.Instance.GetSatisfacionLevel().ToString());
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickGoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
