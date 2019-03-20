using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // Singleton
    public static Audio instance;

    // Variables
    [SerializeField]
    private AudioClip music = null;

    private AudioSource audioSource;

    void Awake()
    {
        // Enforce Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize variables
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Private functions
    private void PlayMusic()
    {
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }
}
