using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [System.Serializable]
    private class RipenessSpeed
    {
        public float fall = 10f;
        public float winter = 10f;
        public float spring = 10f;
        public float summer = 10f;
    }

    [SerializeField]
    private float continuousPickSpeed = 30f;
    [SerializeField]
    private float discretePickSpeed = 20f;
    [SerializeField]
    private RipenessSpeed ripenessSpeed = null;

    private float ripeness;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;
    private List<GameObject> appleStages;

    private SeasonTimer seasonTimer;
    private Player player;

    void Awake()
    {
        ripeness = 0f;

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        appleStages = GetAllChildren(gameObject);

        seasonTimer = FindObjectOfType<SeasonTimer>();
        player = FindObjectOfType<Player>();
    }

    void Start()
    {
        UpdateVisuals();

        //seasonTimer.m_SeasonChange.AddListener(SayHi);                  // Test Listener to react when the season is changed
    }

    void Update()
    {
        // While the tree is being clicked on, decrement ripeness from the tree and STOP incrementing
        if (OnMouse(true))
        {
            player.addFood(GetFood());
        }

        // Else, just continue incrementing ripeness to the tree
        else
        {
            IncrementRipeness();
        }

        UpdateVisuals();
    }

    // Private functions
    private List<GameObject> GetAllChildren(GameObject go)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in go.transform)
            children.Add(child.gameObject);

        return children;
    }

    private void UpdateVisuals()
    {
        if (ripeness <= 0f)
        {
            appleStages[0].SetActive(false);
            appleStages[1].SetActive(false);
            appleStages[2].SetActive(false);
            appleStages[3].SetActive(false);
            appleStages[4].SetActive(false);
        }
        else if (ripeness > 0f && ripeness < 25f)
        {
            appleStages[0].SetActive(true);
            appleStages[1].SetActive(false);
            appleStages[2].SetActive(false);
            appleStages[3].SetActive(false);
            appleStages[4].SetActive(false);
        }
        else if (ripeness >= 25f && ripeness < 50f)
        {
            appleStages[0].SetActive(true);
            appleStages[1].SetActive(true);
            appleStages[2].SetActive(false);
            appleStages[3].SetActive(false);
            appleStages[4].SetActive(false);
        }
        else if (ripeness >= 50f && ripeness < 75f)
        {
            appleStages[0].SetActive(true);
            appleStages[1].SetActive(true);
            appleStages[2].SetActive(true);
            appleStages[3].SetActive(false);
            appleStages[4].SetActive(false);
        }
        else if (ripeness >= 75f && ripeness < 100f)
        {
            appleStages[0].SetActive(true);
            appleStages[1].SetActive(true);
            appleStages[2].SetActive(true);
            appleStages[3].SetActive(true);
            appleStages[4].SetActive(false);
        }
        else
        {
            appleStages[0].SetActive(true);
            appleStages[1].SetActive(true);
            appleStages[2].SetActive(true);
            appleStages[3].SetActive(true);
            appleStages[4].SetActive(true);
        }
    }

    private void IncrementRipeness()
    {
        Season currentSeason = seasonTimer.GetCurrentSeason();

        // Increment ripeness based on the current season
        switch (currentSeason)
        {
            case Season.fall:
                ripeness += ripenessSpeed.fall * Time.deltaTime;
                break;
            case Season.winter:
                ripeness += ripenessSpeed.winter * Time.deltaTime;
                break;
            case Season.spring:
                ripeness += ripenessSpeed.spring * Time.deltaTime;
                break;
            case Season.summer:
                ripeness += ripenessSpeed.summer * Time.deltaTime;
                break;
            default:
                throw new System.Exception("No condition set for Season " + currentSeason);
        }

        // Clamp the ripeness value to no greater than 100
        if (ripeness > 100f)
            ripeness = 100f;
    }

    private bool OnMouse(bool includeClickDistance)
    {
        // Check for Left Mouse Button
        if (Input.GetMouseButton(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            // Check if the tree's collider is clicked on
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                switch (includeClickDistance)
                {
                    case false:
                        // Merely return true
                        return true;
                    case true:
                        // Check if the player collider is within the distance specified in ClickSettings
                        float distance = collider.Distance(player.GetComponent<Collider2D>()).distance;
                        Debug.LogFormat("Distance between Tree -> Player: {0}", distance);

                        if (distance <= ClickSettings.instance.GetMaxClickDistance())
                            return true;
                        break;
                }
            }
        }
        return false;
    }

    public void SayHi(Season newSeason)
    {
        Debug.Log("Hi! I'm now in the " + newSeason + " season!");
    }

    // Public functions
    public float GetFood()
    {
        float foodTaken = continuousPickSpeed * Time.deltaTime;

        // If taken food is greater than how much food is left on the tree...
        if (foodTaken > ripeness)
        {
            foodTaken = ripeness;
            ripeness = 0f;
        }

        // Else, just decrement the tree's food with the food that will be taken...
        else
        {
            ripeness -= foodTaken;
        }

        return foodTaken;
    }

    public float GetFoodDiscrete()
    {
        float foodTaken = discretePickSpeed;

        // If taken food is greater than how much food is left on the tree...
        if (foodTaken > ripeness)
        {
            foodTaken = ripeness;
            ripeness = 0f;
        }

        // Else, just decrement the tree's food with the food that will be taken...
        else
        {
            ripeness -= foodTaken;
        }

        return foodTaken;
    }
}
