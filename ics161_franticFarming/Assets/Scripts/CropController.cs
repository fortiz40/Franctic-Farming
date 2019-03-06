using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class CropController : MonoBehaviour
{

    private bool DEBUG = true; // Change to see debug messages or not

    private SpriteRenderer currentCropStatusSpriteRenderer;
    private SpriteRenderer crowSprite;

    [SerializeField] private Sprite noCropSprite;
    [SerializeField] private Sprite plantedCropSprite;
    [SerializeField] private Sprite matureCropSprite;
    [SerializeField] private Sprite famishedCropSprite;

    private TextMeshProUGUI maturityText;
    private TextMeshProUGUI fertilizationText;

    private bool isPlanted;
    private bool isAlive;
    private bool isMature;
    private bool isFamished;
    private bool hasCrow;

    private Season currentSeason;

    [SerializeField] private float famishCropChance = 2.0f;
    [SerializeField] private float crowAppearChance = 2.0f;

    public int Maturity { get; private set; } = 0;
    public float Fertilization = 0;

    [SerializeField] private int cropMaturityLevel = 20; // Number to indicate when a crop is fully mature

    [SerializeField] private int maturityIncreaseRate = 2 ;  // Normal rate for crop maturity growth when water = 0

    //[SerializeField] private int fertilizedIncreaseRate = 2; // Added rate when crop has > 0 Fertilization
    //[SerializeField] private int fertilizedDecreaseRate = 1; // Decreases mautirity growth when Fertilization = 0

    [SerializeField] private float baseFertilizerAddAmount = 10; //Amount to increase Fertilization when crop is clicked
    [SerializeField] private int dropFertilizationRate = 1; // Amount to drop fertilization by each second
    [SerializeField] private float updateCropSeconds = 1.0f; // Number of seconds to wait between each UpdateCropStatus call
    [SerializeField] private int crowDropRate = -1; // Number to decrease crop maturity by when bird is on it

    //[SerializeField] private float famishedMaturityPercent = 0.75f; // This percentage will be multiplied by the maturityIncreaseRate when famished

    private Player player;
    void Awake()
    {

        SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].gameObject.CompareTag("CrowSprite")) crowSprite = sprites[i];
            else if (sprites[i].gameObject.CompareTag("CurrentCropStatusSprite")) currentCropStatusSpriteRenderer = sprites[i];
        }

        crowSprite.gameObject.SetActive(false);

        //currentCropStatusSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        //crowSprite = 

        isPlanted = false;
        isAlive = false;
        isMature = false;
        isFamished = false;
        hasCrow = false;

        Maturity = 0;
        Fertilization = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentCropStatusSpriteRenderer.sprite = noCropSprite;
        SeasonTimer.instance.m_SeasonChange.AddListener(OnSeasonChangeListener);
        currentSeason = SeasonTimer.instance.GetCurrentSeason();

        try
        {
            player = GameObject.FindObjectOfType<Player>().GetComponent<Player>();
        }
        catch
        {
            Debug.Log("NO PLAYER FOUND!");
        }

        TextMeshProUGUI[] texts = this.GetComponentsInChildren<TextMeshProUGUI>();

        for (int x = 0; x < texts.Length; x++)
        {
            if (texts[x].CompareTag("CropFertilizationText"))
            {
                fertilizationText = texts[x];
            }
            else
            {
                maturityText = texts[x];
            }
        }

        //fertilizationText.text = "FERTILIZATION";
        maturityText.text = "Score:\n0";
        maturityText.gameObject.SetActive(false);
        //fertilizationText.gameObject.SetActive(false);
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
            if (DEBUG) Debug.Log("PLANTING from plantCrop()");
            isPlanted = true;
            isAlive = true;
            currentCropStatusSpriteRenderer.sprite = plantedCropSprite;
            StartCoroutine(UpdateCropStatus());
        }
    }

    /// <summary>
    /// Call this function when the player wants to add fertilzer to the crop.
    /// </summary>
    public void addFertilizer()
    {
        //Debug.LogFormat("ADDING {0.0f} FERTILIZER AND REMOVING FROM PLAYER", baseFertilizerAddAmount);

        if (isFamished)
        {
            isFamished = false;
            if (!isMature)
            {
                currentCropStatusSpriteRenderer.sprite = plantedCropSprite;
            }
            else currentCropStatusSpriteRenderer.sprite = matureCropSprite;
        }

        Fertilization += player.RemoveFood(baseFertilizerAddAmount);
    }

    public void harvestCrop()
    {
        if (DEBUG)
        {
            Debug.Log("HARVESTING!");
            Debug.LogFormat("CURRENT MATURIRY: {0}", Maturity);
        }
        ScoreCounter.instance.Score += Maturity;
        resetCrop();
    }

    /// <summary>
    /// Resets the crop back to the initial state
    /// </summary>
    private void resetCrop()
    {
        if (DEBUG) Debug.Log("RESETTING CROP");

        isAlive = false;
        isMature = false;
        isPlanted = false;
        currentCropStatusSpriteRenderer.sprite = noCropSprite;

        Maturity = 0;
        Fertilization = 0.0f;
    }


    /// <summary>
    /// Updates maturity depending on it's fertilization and maturity rates
    /// </summary>
    private void maturityIncrease()
    {
        //if (Fertilization > 0)
        //{
        //    Maturity += maturityIncreaseRate + fertilizedIncreaseRate;
        //}
        //else
        //{
        //    Maturity += maturityIncreaseRate - fertilizedDecreaseRate;
        //}

        if (hasCrow)
        {
            Maturity += crowDropRate;
        }
        else if (isFamished)
        {
            Maturity +=  (maturityIncreaseRate / 2);
        }
        else
        {
            Maturity += maturityIncreaseRate;
        }

        maturityText.text = "Score:\n" + Maturity.ToString();
        if (Maturity >= cropMaturityLevel && !isMature)
        {
            if (DEBUG) Debug.Log("MATURING!");
            changeCropToMature();
        }
    }

    private void changeCropToMature()
    {
        isMature = true;
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
    //void OnMouseDown()
    //{
    //    Debug.Log("CLICKED CROP");

    //    print("isPlanted: " + isPlanted);
    //    print("currentSeason: " + currentSeason);
    //    if (!isPlanted)
    //    {
    //        if (DEBUG) Debug.Log("PLANTING CROP");
    //        plantCrop();
    //    }
    //    else if (isPlanted && (currentSeason == Season.winter || currentSeason == Season.spring))
    //    {
    //        Debug.Log("ADDING FERTILIZER");
    //        addFertilizer();
    //    }
    //    else if (isPlanted && currentSeason == Season.summer)
    //    {
    //        harvestCrop();
    //    }
    //}

    public void interact()
    {
        Debug.Log("CLICKED CROP");

        print("isPlanted: " + isPlanted);
        print("currentSeason: " + currentSeason);
        if (!isPlanted)
        {
            if (DEBUG) Debug.Log("PLANTING CROP");
            plantCrop();
        }
        else if (isPlanted && (currentSeason == Season.winter || currentSeason == Season.spring))
        {
            Debug.Log("ADDING FERTILIZER");
            addFertilizer();
        }
        else if (isPlanted && currentSeason == Season.summer)
        {
            harvestCrop();
        }
    }

    private void OnSeasonChangeListener(Season season)
    {
        if (DEBUG) Debug.Log("SEASON CHANGING TO " + season.ToString());
        currentSeason = season;
        if (season == Season.fall && isMature) // If it becomes fall again, and crop was not harvested, it dies and is reset to start state
        {
            resetCrop();
        }
    }


    /// <summary>
    /// Coroutine that updates the crop status every second.
    /// </summary>
    IEnumerator UpdateCropStatus()
    {
        if (DEBUG) Debug.Log("STARTING UPDATE CROP STATUS");
        for(; ; )
        {
            if (!isAlive)
            {
                break;
            }
            maturityIncrease();
            decreaseFertilization();

            float rand = Random.Range(0.0f, 1.0f);

            if (isPlanted && (currentSeason == Season.winter))
            {
                if (DEBUG)
                {
                    Debug.Log("Famish chance");
                    Debug.Log(rand);
                }
                if ( famishCropChance >= rand) famishCrop();
            }

            if (isPlanted && (currentSeason == Season.spring))
            {
                if (crowAppearChance >= rand) crowAppear();
            }

            yield return new WaitForSeconds(updateCropSeconds);
        }
    }

    private void famishCrop()
    {
        if (DEBUG) Debug.Log("FAMISHING CROP!");
        currentCropStatusSpriteRenderer.sprite = famishedCropSprite;
        isFamished = true;
    }

    private void crowAppear()
    {
        if (DEBUG) Debug.Log("BIRD APPEARING!");
        crowSprite.gameObject.SetActive(true);
        hasCrow = true;
    }

    public void OnMouseEnter()
    {
        //fertilizationText.gameObject.SetActive(true);
        maturityText.gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        //fertilizationText.gameObject.SetActive(false);
        maturityText.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision!");
        Debug.Log(other.gameObject.tag.ToString());
        maturityText.gameObject.SetActive(true);

        if (hasCrow)
        {
            crowSprite.gameObject.SetActive(false);
            hasCrow = false;
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        maturityText.gameObject.SetActive(false);

    }
}
