using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Season Season;
    public Slider HungerBar;
    public Slider FoodBar;
    private float timer = 0;
    public int HurtCircle = 6;
    public int MaxHunger = 100;
    public int InitialFood = 0;
    private float hunger;
    private float food;
    private float hurt;
    private float move_speed;
    Rigidbody2D m_rigidbody;
    Animator m_animator;
    BoxCollider2D m_collider;
    Vector2 direction;
    float maxValue;


    [SerializeField]
    private GameObject gameOverScreen = null;
    [SerializeField]
    private TextMeshProUGUI explaination = null;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        maxValue = gameObject.GetComponent<BoxCollider2D>().size.y / 2;
        move_speed = 5f;
        hunger = MaxHunger;
        food = InitialFood;
        HungerBar.value = hunger;
        FoodBar.value = food;
        hurt = 5;
        direction = new Vector2(0, -1);

        SeasonTimer.instance.m_SeasonChange.AddListener(UpdateSeason);
    }

    // Update is called once per frame
    void Update()
    {
        SetBars();
        Move();
        timer += Time.deltaTime;
        
        if (timer >= HurtCircle)
        {
            Hurt();
            timer = 0;
        }
        if ( hunger <= 0)
        {
            die();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            interactCrop();
            interactApple();
        }
        
        // Eat
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (hunger < 100)
            {
                hunger += RemoveFood(10);

                if (hunger > 100)
                {
                    hunger = 100;
                }

                HungerBar.value = hunger;
            }
        }

    }

    void Move()
    {
        Vector2 player_movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (player_movement != Vector2.zero)
        {
            m_animator.SetBool("walking", true);
            m_animator.SetFloat("input_x", player_movement.x);
            m_animator.SetFloat("input_y", player_movement.y);
        }
        else
        {
            m_animator.SetBool("walking", false);
        }
        m_rigidbody.MovePosition(m_rigidbody.position + player_movement * Time.deltaTime * move_speed);
    }

    void SetBars()
    {
        HungerBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.5f, 0f));
        
        FoodBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.3f, 0f));

    }

    public float getHunger()
    {
        return hunger;
    }

    public float getFood()
    {
        return food;
    }

    void Hurt()
    {
        hunger -= hurt;
        HungerBar.value = hunger;
    }
    
    public void addFood(float apple)
    {
        food += apple;
        if (food > 100)
        {
            FoodBar.value = 100;
        }
        else
        {
            FoodBar.value = food;
        }
        
    }

    public float RemoveFood(float desired)
    {
        if (desired > food)
        {
            desired = food;
            food = 0;
            FoodBar.value = 0;
        }
        else
        {
            food -= desired;
            FoodBar.value = food;
        }

        return desired;
    }

    void UpdateSeason(Season season)
    {
        if (season == Season.winter)
        {
            hurt = 10;
        }
        else
        {
            hurt = 5;
        }
    }
    void die()
    {
        Debug.Log("You Die!!!");

        //Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        explaination.text = "You have starved to death!\n(Score = " + ScoreCounter.instance.Score + ")";
    }
    
    void interactCrop()
    {

        RaycastHit2D hit;
        Debug.Log("Shoot Ray!!!");
        hit = (Physics2D.Raycast(transform.position, direction, maxValue));
        if ( hit != null && hit.collider.gameObject.CompareTag("Crop"))
        {
            Debug.Log("Interact!!!");
            hit.collider.gameObject.GetComponent<CropController>().interact();
        }
    }

    void interactApple()
    {
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(transform.position, direction, maxValue));
        if (hit != null && hit.collider.gameObject.CompareTag("AppleTree"))
        {
            addFood(hit.collider.gameObject.GetComponent<AppleTree>().GetFoodDiscrete());
        }
    }
}
