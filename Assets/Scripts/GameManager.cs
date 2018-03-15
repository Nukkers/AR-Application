using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Vuforia;

/// <summary>
/// Manages overall game state, communication between modules, loading scenes, etc.
/// Implemented as a singleton for ease of use (Every object doesn't need its own reference then)
/// </summary>
public class GameManager : MonoBehaviour
{
    /* Card events */
    public delegate void CardMatchEventHandler(object sender, object args);
    public event CardMatchEventHandler cardEvent;

    public bool gameStarted = false;
    public List<Card> visibleCardList;
    public int score = 0;

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
        // don't track the images when the application is opened
        Debug.Log("Tracking stopped");
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    // Starts the game and initialises the necessary variables (triggered by clicking start game button)
    public void NewGame()
    {
        gameStarted = true;
        Debug.Log("Starting a new instance of the gamemanager class");
        mInstance = this;
        TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
    }

    // quits the current game/application depending on the state of the game
    public void Quit()
    {

        if (Instance.gameStarted)
        {
            gameStarted = false;
            mInstance = null;
            Debug.Log("Quitting instance of the game");
            TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
        }
        else
        {
            Debug.Log("End of app triggered");
            Application.Quit();
        }
        
    }

    // add a list of keeping to keeping track of visible cards
    public void CardTracked(Card card)
    {
        visibleCardList.Add(card);
        Debug.Log("Tracking card :" + card.name);

        if (visibleCardList.Count > 1)
        {
            if (visibleCardList[visibleCardList.Count - 1].cardType == visibleCardList[visibleCardList.Count - 2].cardType)
            {
                score++;
                visibleCardList[visibleCardList.Count - 1].matched = true;
                visibleCardList[visibleCardList.Count - 2].matched = true;
                Debug.Log("Card pair matched! Type: " + card.cardType);
            }

        }
    }

    public void CardLost(Card card)
    {
        visibleCardList.Remove(card);
        Debug.Log("Lost card :" + card.name);
    }
}
