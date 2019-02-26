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
    private int goal = 300;

    public int Score { get; set; }
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI goalText = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        SeasonTimer.instance.m_SeasonChange.AddListener(OnSeasonChange);
        goalText.text = goal.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Score.ToString();
    }

    private void OnSeasonChange(Season newSeason)
    {
        if (newSeason == Season.fall)
        {
            Debug.Log("NEW YEAR!");
            if (HasReachedGoal())
            {
                Score = 0;
            }
            else
            {
                // DO NOT wipe score, but show Game Over screen here...
            }
        }
    }

    private bool HasReachedGoal()
    {
        return Score >= goal;
    }
}
