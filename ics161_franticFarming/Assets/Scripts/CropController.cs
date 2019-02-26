using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    
    private SpriteRenderer currentCropStatusSpriteRenderer;

    [SerializeField] private Sprite noCropSprite;
    [SerializeField] private Sprite[] plantedCropSprites;
    [SerializeField] private Sprite matureCropSprite;

    private bool isPlanted;
    private bool isAlive;
    private Season currentSeason;

    public int Maturity { get; private set; } = 0;
    public int Fertilization = 0;

    [SerializeField] private int cropMaturityLevel = 20; // Number to indicate when a crop is fully mature

    [SerializeField] private int maturityIncreaseRate = 2 ;  // Normal rate for crop maturity growth when water = 0
    [SerializeField] private int fertilizedIncreaseRate = 1; // Added rate when crop has > 0 Fertilization
    [SerializeField] private int fertilizedDecreaseRate = 1; // Decreases mautirity growth when Fertilization = 0
    [SerializeField] private int baseFertilizerAddAmount = 50; //Amount to increase Fertilization when crop is clicked
    [SerializeField] private int dropFertilizationRate = 1; // Amount to drop fertilization by each second

    [SerializeField] private float updateCropSeconds = 1.0f; // Number of seconds to wait between each UpdateCropStatus call

    void Awake()
    {
        currentCropStatusSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        isPlanted = false;
        isAlive = false;
        Maturity = 0;
        Fertilization = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentCropStatusSpriteRenderer.sprite = noCropSprite;
        SeasonTimer.instance.m_SeasonChange.AddListener(OnSeasonChangeListener);
        currentSeason = SeasonTimer.instance.GetCurrentSeason();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Call this function when the player plants the crop. It also starts the UpdateCropStatus Coroutine
    /// which updates it's maturity.
    /// </summary>
    public void plantCrop()
    {
        if (!isPlanted && currentSeason == Season.fall)
        {
            Debug.Log("PLANTING from plantCrop()");
            isPlanted = true;
            isAlive = true;
            int randomSprite = Random.Range(0, plantedCropSprites.Length);
            currentCropStatusSpriteRenderer.sprite = plantedCropSprites[randomSprite];
            StartCoroutine(UpdateCropStatus());
        }
    }

    /// <summary>
    /// Call this function when the player wants to add fertilzer to the crop.
    /// </summary>
    public void addFertilizer()
    {
        Fertilization += baseFertilizerAddAmount;
    }

    public void harvestCrop()
    {

    }


    /// <summary>
    /// Updates maturity depending on it's fertilization and murity rates
    /// </summary>
    private void maturityIncrease()
    {
        if (Fertilization > 0)
        {
            Maturity += maturityIncreaseRate + fertilizedIncreaseRate;
        }
        else
        {
            Maturity += maturityIncreaseRate - fertilizedDecreaseRate;
        }
        Debug.LogFormat("Current Maturity: {0}", Maturity);
        if (Maturity >= cropMaturityLevel)
        {
            Debug.Log("MATURING!");
            changeCropToMature();
        }
    }

    private void changeCropToMature()
    {
        currentCropStatusSpriteRenderer.sprite = matureCropSprite;
    }


    /// <summary>
    /// Decrease Fertilization
    /// </summary>
    private void decreaseFertilization()
    {
        if (Fertilization > 0)
        {
            Fertilization -= dropFertilizationRate;
        }
    }

    /// <summary>
    /// Called when the crop tile if left mouse clicked. It adds water to the crop, if it is seeded or grown
    /// </summary>
    void OnMouseDown()
    {
        if(!isPlanted && currentSeason == Season.fall)
        {
            Debug.Log("PLANTING CROP");
            plantCrop();
        }
        else if(isPlanted && isAlive && (currentSeason == Season.winter || currentSeason == Season.spring))
        {
            addFertilizer();
        }
        else if(isPlanted && isAlive && currentSeason == Season.summer)
        {
            harvestCrop();
        }
    }

    private void OnSeasonChangeListener(Season season)
    {
        currentSeason = season;
    }


    /// <summary>
    /// Coroutine that updates the crop status every second.
    /// </summary>
    IEnumerator UpdateCropStatus()
    {
        Debug.Log("STARTING UPDATE CROP STATUS");
        for(; ; )
        {
            if (!isAlive)
            {
                break;
            }
            maturityIncrease();
            decreaseFertilization();
            yield return new WaitForSeconds(updateCropSeconds);
        }
    }




}
