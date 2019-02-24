using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Season
{
    fall, winter, spring, summer
}

public class SeasonTimer : MonoBehaviour
{
    public int secondsPerSeason = 15;

    private int time;
    private Season currentSeason;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI seasonText;

    void Awake()
    {
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
                currentSeason = Season.fall;
                break;
            case 1:
                currentSeason = Season.winter;
                break;
            case 2:
                currentSeason = Season.spring;
                break;
            case 3:
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
