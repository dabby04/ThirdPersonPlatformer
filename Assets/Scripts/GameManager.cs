using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
     private int score = 0;
    public TextMeshProUGUI scoreText;

     // Awake makes sure that when you want to refer to instance in another script
    // Awake method is called
    void Awake()
    {
        // execute singleton pattern: making sure that only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

    }

     public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }
}
