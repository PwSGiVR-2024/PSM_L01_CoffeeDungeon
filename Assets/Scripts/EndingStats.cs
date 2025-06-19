using TMPro;
using UnityEngine;

public class EndingStats : MonoBehaviour
{
    private SatisfactionLevel level;
    private int satisfactionScore;

    [Header("References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text satisfacitionText;

    private void Start() 
    {
        SetEndingStats();
    }

    private void SetEndingStats()
    {
        Debug.Log("SetEndingStats called");

        if (SatisfactionManager.Instance != null)
        {
            satisfactionScore = SatisfactionManager.Instance.GetScore();
            level = SatisfactionManager.Instance.GetSatisfacionLevel();

            scoreText.text = satisfactionScore.ToString();
            satisfacitionText.text = level.ToString();
        }
        else
        {
            Debug.LogError("SatisfactionManager.Instance is null!");
        }
    }
}