using System;
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

    public int maxNumberOfPairs = 1; // maximum number of pairs within the game 
    public string cardOutlinePrefabName = "cardOutline";
    public string matchedParticleFXName = "cardMatchFX";
    public GameObject matchedParticleSystemPrefab;
    public GameObject cardOutlinePrefab; // Retrieved by the cards as needed, saves assigning it manually to each card type.
    /* Singleton implementation */
    private static GameManager mInstance; // Instance of the GameManager object - managed by the singleton accessors (see below)
    public static GameManager Instance
    {

        get
        {
            if (!mInstance)
                ((GameObject)Instantiate(Resources.Load("GameManager"))).GetComponent<GameManager>().Initialize(); // Not the most performant call, but it should only ever be called once (if at all)

            return mInstance;
        }
    }

    /// <summary>
    /// Functions as start usually would, we can't use start as it gets called
    /// too late to be any use for our startup process. An alterantive here would've been to
    /// use a plain-C# class, as we don't actually need any monobehaviour related functionality
    /// this would've worked fine, and probably would've been a tad bit more performant.
    /// </summary>
    void Initialize()
    {
        /* Preload prefabs from resource name */
        cardOutlinePrefab = (GameObject)Resources.Load(cardOutlinePrefabName);
        matchedParticleSystemPrefab = (GameObject)Resources.Load(matchedParticleFXName);

        /* Set mInstance to this instance of the class if null, otherwise kill the object */
        if (mInstance == null)
            mInstance = this;
        else
            Destroy(this);

        //// don't track the images when the application is opened
        Debug.Log("Tracking stopped");
        TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
        Debug.Log("Inside start func");
        visibleCardList = new List<Card>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when a new game should be started. Initializes the variables required, starts Vuforia tracking
    /// and instantiates any prefabs (as appropiate)
    /// </summary>
    public void StartGame()
    {
        if (gameStarted)
            Debug.LogError("Attempted to start the game while it is already running! This shouldn't happen...");
        else
        {
            Debug.Log("Starting the game.");
            gameStarted = true;
            TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
        }
    }

    // quits the current game/application depending on the state of the game
    public void QuitGame()
    {
        if (gameStarted)
        {
            gameStarted = false;
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
                    if ((card.cardType == cardToCheck.cardType) && (card.pairID == cardToCheck.pairID))
                    {
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

            if (score == maxNumberOfPairs)
            {
                MultipleRounds();
            }
        }
    }

    public void CardLost(Card card)
    {
        visibleCardList.Remove(card);
        Debug.Log("Lost card :" + card.name);
    }

    public void MultipleRounds()
    {
        Debug.Log("New game is being started");
        var cardsFound = FindObjectsOfType<Card>();
        Debug.Log(cardsFound + " : " + cardsFound.Length);
        foreach (var cards in cardsFound)
        {
            cards.matched = false;
        }

    }

}
