using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    public Slider HungerBar;
    public Slider FoodBar;
    private float timer = 0;
    public int HurtCircle = 6;
    public int MaxHunger = 100;
    public int InitialFood = 0;
    private int hunger;
    private int food;
    private float move_speed;
    Rigidbody2D m_rigidbody;
    Animator m_animator;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        move_speed = 5f;
        hunger = MaxHunger;
        food = InitialFood;
        HungerBar.value = hunger;
        FoodBar.value = food;
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
        
        

    }

    void Move()
    {
        Vector2 player_movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (player_movement != Vector2.zero)
        {
            m_animator.SetBool("Walking", true);
            m_animator.SetFloat("input_x", player_movement.x);
            m_animator.SetFloat("input_y", player_movement.y);
        }
        else
        {
            m_animator.SetBool("Walking", false);
        }
        m_rigidbody.MovePosition(m_rigidbody.position + player_movement * Time.deltaTime * move_speed);
    }

    void SetBars()
    {
        HungerBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.5f, 0f));
        
        FoodBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.3f, 0f));

    }

    public int getHunger()
    {
        return hunger;
    }

    public int getFood()
    {
        return food;
    }

    void Hurt()
    {
        hunger -= 1;
        HungerBar.value = hunger;
    }
}
