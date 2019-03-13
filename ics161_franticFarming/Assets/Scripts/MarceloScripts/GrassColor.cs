using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassColor : MonoBehaviour
{
    // Singleton
    public static GrassColor instance;

    // Variables
    [SerializeField]
    private Color fallColor;
    [SerializeField]
    private Color winterColor;
    [SerializeField]
    private Color springColor;
    [SerializeField]
    private Color summerColor;
    [SerializeField]
    private float transitionTimePercent = 0f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor(transitionTimePercent);
    }

    // Private functions
    private void UpdateColor(float percentThreshold)
    {
        float seasonPercentElapsed = SeasonTimer.instance.GetTime() % SeasonTimer.instance.GetSeasonTimer() / (float)SeasonTimer.instance.GetSeasonTimer();

        if (seasonPercentElapsed >= percentThreshold)
        {
            float lerpPercent = (seasonPercentElapsed - percentThreshold) / (1 - percentThreshold);

            switch (SeasonTimer.instance.GetCurrentSeason())
            {
                case Season.fall:
                    spriteRenderer.color = Color.Lerp(fallColor, winterColor, lerpPercent);
                    break;
                case Season.winter:
                    spriteRenderer.color = Color.Lerp(winterColor, springColor, lerpPercent);
                    break;
                case Season.spring:
                    spriteRenderer.color = Color.Lerp(springColor, summerColor, lerpPercent);
                    break;
                case Season.summer:
                    spriteRenderer.color = Color.Lerp(summerColor, fallColor, lerpPercent);
                    break;
            }
        }
        else
        {
            switch (SeasonTimer.instance.GetCurrentSeason())
            {
                case Season.fall:
                    spriteRenderer.color = fallColor;
                    break;
                case Season.winter:
                    spriteRenderer.color = winterColor;
                    break;
                case Season.spring:
                    spriteRenderer.color = springColor;
                    break;
                case Season.summer:
                    spriteRenderer.color = summerColor;
                    break;
            }
        }
    }
}
