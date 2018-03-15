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

    public GameObject matchedParticleSystemPrefab;
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
        /* Only perform further checks if the card hasn't already been matched against */
        if (card.matched == false)
        {
            visibleCardList.Add(card); // Add the card to the list of cards being tracked for matchings
            Debug.Log("Tracking card :" + card.name);

            /* If more than one unmatched card is visible we should perform a check to see if they're a pair */
            if (visibleCardList.Count > 1)
            {
                /* Iterate through all visisble cards, checking for a match against the one that just came into view */
                Card otherCard = null;
                foreach (var cardToCheck in visibleCardList)
                {
                    /* Perform the check - check both card type and the pairID (pairID is for future proofing only at this point) */
                    if ((card.cardType == cardToCheck.cardType) && (card.pairID == cardToCheck.pairID)) {
                        otherCard = cardToCheck;
                        break;
                    }
                }
                
                /* If otherCard is non-null we can presume we have a match and continue with the match handling logic */
                if (otherCard != null) 
                {
                    score++; // Increment the score
                    card.matched = true; // Stop that card from being matched in the future
                    otherCard.matched = true; // Stop that card from being matched in the future
                    
                    /* Instantiate the matched particle FX */
                    GameObject fx = Instantiate(matchedParticleSystemPrefab, card.transform);
                    fx.transform.RotateAroundLocal(Vector3.left, -90); // Deprecated, replace me.
                    fx = Instantiate(matchedParticleSystemPrefab, otherCard.transform);
                    fx.transform.RotateAroundLocal(Vector3.left, -90); // Deprecated, replace me.

                    /* Remove the cards from the tracked list, we have no need to keep track of them anymore */
                    visibleCardList.Remove(card);
                    visibleCardList.Remove(otherCard);

                    Debug.Log("Card pair matched! Type: " + card.cardType);


                }
            }
        }
    }

    public void CardLost(Card card)
    {
        visibleCardList.Remove(card);
        Debug.Log("Lost card :" + card.name);
    }
}
