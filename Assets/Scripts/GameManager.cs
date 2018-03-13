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
        mInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // add a list of keeping to keeping track of visible cards
    public void cardTracked(Card card)
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

    public void cardLost(Card card)
    {
        visibleCardList.Remove(card);
        Debug.Log("Lost card :" + card.name);

    }
}
