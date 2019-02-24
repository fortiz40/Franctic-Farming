using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{

    private SpriteRenderer currentCropStatusSpriteRenderer;
    [SerializeField] private Sprite[] seededGroundSprites;
    private bool isGrown;
    private bool isPlanted;

    int time = 0;

    void Awake()
    {
        currentCropStatusSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        isPlanted = false;
        isGrown = false;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        if (time == 60)
        {
            plantCrop();
            time = 0;
        }
    }

    /// <summary>
    /// Call this function when the player wants to plant seeds on the crop tile.
    /// If a seed has already been planted, or if a crop exists, nothing will happen.
    /// </summary>
    public void plantCrop()
    {
        if (!isPlanted && !isGrown)
        {
            isPlanted = true;
            int randomSprite = Random.Range(0, seededGroundSprites.Length);
            currentCropStatusSpriteRenderer.sprite = seededGroundSprites[randomSprite];
        }
    }

    /// <summary>
    /// Call this function when the player wants to add fertilzer to the crop.
    /// </summary>
    public void addFertilizer()
    {

    }

    private void addWater()
    {

    }

    /// <summary>
    /// This function needs to be called when the crop will grow from seed to crop.
    /// It can only be called once. If crop has already grown from seed to crop, nothing will happen
    /// </summary>
    public void cropGrow()
    {

    }

    /// <summary>
    /// Dimishes health as needed and updates the crop sprites if necessary
    /// </summary>
    private void dropHealth()
    {

    }

    /// <summary>
    /// 
    /// Diminishes 
    /// </summary>
    private void dropWater()
    {

    }


    private void dropFertilizer()
    {

    }

    /// <summary>
    /// Called when the crop tile if left mouse clicked. It adds water to the crop, if it is seeded or grown
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log("LEFT CLICK");
        if(!isPlanted)
        {
            plantCrop();
        }
    }




}
