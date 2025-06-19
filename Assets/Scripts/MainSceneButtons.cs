using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneButtons : MonoBehaviour
{
    [SerializeField] private GameObject endRunCanvas;
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
}
