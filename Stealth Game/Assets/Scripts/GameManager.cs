using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Transform player;
    public Transform Player
    {
        get
        {
            if (player == null)
                player = FindObjectOfType<PlayerController>().transform;

            return player;
        }
    }

    LevelLoader levelLoader;
    public LevelLoader LevelLoader
    {
        get
        {
            if (levelLoader == null)
                levelLoader = FindObjectOfType<LevelLoader>();
            return levelLoader;
        }
    }

    Spawner spawner;
    public Spawner Spawner
    {
        get
        {
            if (spawner == null)
                spawner = FindObjectOfType<Spawner>();

            return spawner;
        }
    }
   
    private static GameManager singleton = null;
    public static GameManager Singleton
    {
        get
        {
            return singleton;
        }
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }

    void Start ()
    {      
        levelLoader = FindObjectOfType<LevelLoader>();
        spawner = FindObjectOfType<Spawner>();
        player = FindObjectOfType<PlayerController>().transform;
    }
}
