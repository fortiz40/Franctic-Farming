using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    // Singleton
    public static ScoreCounter instance;

    // Variables
    [SerializeField]
    private int initialGoal = 2700;
    [SerializeField]
    private int goalCap = 6000;
    [SerializeField]
    private int goalIncrementPerYear = 500;
    [SerializeField]
    private GameObject gameOverScreen = null;
    [SerializeField]
    private TextMeshProUGUI explaination = null;

    public int Goal { get; private set; }
    public int Score { get; set; }
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI goalText = null;

    public int SeasonsSurvived { get; private set; }

    void Awake()
    {
        // Enforce singleton pattern
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Initialize variables
        Score = 0;
        Goal = initialGoal;
        SeasonsSurvived = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        SeasonTimer.instance.m_SeasonChange.AddListener(OnSeasonChange);
        goalText.text = Goal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Score.ToString();
    }

    private void OnSeasonChange(Season newSeason)
    {
        SeasonsSurvived++;

        if (newSeason == Season.fall)
        {
            Debug.Log("NEW YEAR!");
            if (HasReachedGoal())
            {
                Score = 0;
                IncrementGoal();
            }
            else
            {
                // DO NOT wipe score, but show Game Over screen here...
                Time.timeScale = 0f;
                gameOverScreen.SetActive(true);
                explaination.text = "You did not earn enough points for the year!\n" + "" +
                    "(Seasons Survived: " + SeasonsSurvived + ", Time Survived: " + SeasonTimer.instance.GetTime() + ")";
            }
        }
    }

    private bool HasReachedGoal()
    {
        return Score >= Goal;
    }

    private void IncrementGoal()
    {
        // Increment the goal value, but not over the desired goal cap
        Goal += goalIncrementPerYear;

        if (Goal > goalCap)
            Goal = goalCap;

        // Update the UI holding the Goal text
        goalText.text = Goal.ToString();
    }
}
