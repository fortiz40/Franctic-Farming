using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonUI : MonoBehaviour
{
    public Image UIBar;

    private Color32 fallColor = new Color32(191, 121, 16, 255);
    private Color32 winterColor = new Color32(15, 189, 190, 255);
    private Color32 springColor = new Color32(70, 253, 56, 255);
    private Color32 summerColor = new Color32(219, 206, 30, 255);

    // Update is called once per frame
    void Update()
    {
        UIBar.fillAmount = 1 - (SeasonTimer.instance.GetTime() % SeasonTimer.instance.GetSeasonTimer()) / (float)SeasonTimer.instance.GetSeasonTimer();
        switch (SeasonTimer.instance.GetCurrentSeason())
        {
            case Season.fall:
                UIBar.color = fallColor;
                break;
            case Season.winter:
                UIBar.color = winterColor;
                break;
            case Season.spring:
                UIBar.color = springColor;
                break;
            case Season.summer:
                UIBar.color = summerColor;
                break;

        }
    }
}
