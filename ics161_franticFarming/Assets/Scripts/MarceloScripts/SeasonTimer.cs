using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum Season
{
    fall, winter, spring, summer
}

[System.Serializable]
public class SeasonEvent : UnityEvent<Season> { }

public class SeasonTimer : MonoBehaviour
{
    // Singleton
    public static SeasonTimer instance;

    // Unity Events
    [System.NonSerialized]
    public SeasonEvent m_SeasonChange;
    [System.NonSerialized]
    public UnityEvent m_YearChange;

    // Variables
    [SerializeField]
    private int secondsPerSeason = 15;

    private int time;
    private Season currentSeason;
    
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI seasonText;

    void Awake()
    {
        // Enforce Singleton pattern
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Initialize Unity Events
        m_SeasonChange = new SeasonEvent();
        m_YearChange = new UnityEvent();

        // Initialize variables
        time = 0;
        currentSeason = Season.fall;
    }

    void Start()
    {
        StartCoroutine(StartTimer());
    }

    void Update()
    {
        
    }

    // Private functions
    private void CheckForNewSeason()
    {
        int elapsedSeasons = time / secondsPerSeason;

        switch (elapsedSeasons % 4)
        {
            case 0:
                if (currentSeason != Season.fall)
                {
                    m_SeasonChange.Invoke(Season.fall);
                    m_YearChange.Invoke();
                }

                currentSeason = Season.fall;
                break;
            case 1:
                if (currentSeason != Season.winter)
                    m_SeasonChange.Invoke(Season.winter);

                currentSeason = Season.winter;
                break;
            case 2:
                if (currentSeason != Season.spring)
                    m_SeasonChange.Invoke(Season.spring);

                currentSeason = Season.spring;
                break;
            case 3:
                if (currentSeason != Season.summer)
                    m_SeasonChange.Invoke(Season.summer);

                currentSeason = Season.summer;
                break;
            default:
                throw new System.Exception("No condition set for currentSeason = " + (elapsedSeasons % 4));
        }
    }

    private void UpdateUI()
    {
        timerText.text = time.ToString();
        seasonText.text = UppercaseFirstLetter(currentSeason.ToString());
    }

    private string UppercaseFirstLetter(string word)
    {
        if (word != null)
        {
            switch (word.Length)
            {
                case 1:
                    return word.ToUpper();
                default:
                    return word.Substring(0, 1).ToUpper() + word.Substring(1);
            }
        }
        throw new System.Exception("Could not uppercase string; given string is null.");
    }

    // Public functions
    public int GetTime()
    {
        return time;
    }

    public int GetSeasonTimer()
    {
        return secondsPerSeason;
    }

    public Season GetCurrentSeason()
    {
        return currentSeason;
    }

    // Coroutines
    IEnumerator StartTimer()
    {
        while (true)
        {
            CheckForNewSeason();
            UpdateUI();

            yield return new WaitForSeconds(1f);                    // Makes coroutine wait for 1 in-game second

            ++time;                                                 // Increment time by 1
        }
    }
}
