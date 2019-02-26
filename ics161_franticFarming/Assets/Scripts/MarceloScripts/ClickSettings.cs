using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSettings : MonoBehaviour
{
    // Singleton
    public static ClickSettings instance;
    
    // Variables
    [SerializeField]
    private float maxClickDistance = 10f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Public functions
    public float GetMaxClickDistance()
    {
        return maxClickDistance;
    }
}