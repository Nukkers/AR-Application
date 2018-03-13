using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages overall game state, communication between modules, loading scenes, etc.
/// Implemented as a singleton for ease of use (Every object doens't need its own reference then)
/// </summary>
public class GameManager : MonoBehaviour
{
    /* Card events */
    public delegate void CardMatchEventHandler(object sender, object args);
    public event CardMatchEventHandler cardEvent;


    /* Singleton implementation */
    private static GameManager mInstance; // Instance of the GameManager object - managed by the singleton accessors (see below)
    public static GameManager Instance
    {

        get
        {
            if (!mInstance) // Initialize the singleton.
            {
                mInstance = new GameManager();
                GameObject instanceObject = new GameObject();
                mInstance = instanceObject.AddComponent<GameManager>();
            }
            return mInstance;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
