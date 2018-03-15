using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Represents the game logic of a card.
/// </summary>
public class Card : MonoBehaviour, ITrackableEventHandler
{
    public int cardID;
    public int cardType; // TODO : Make me an enum.
    public AudioClip audioCue;
    public bool matched = false;
    private TrackableBehaviour mTrackableBehaviour;
    // Use this for initialization
    void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
        		
	}

    public void PlayAudioCue()
    {
        AudioSource.PlayClipAtPoint(audioCue, Vector3.zero); // TODO : Do this less terribly.
        Debug.Log("Played audio cue " + audioCue.name);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        
        if (GameManager.Instance.gameStarted)
        {
            if (newStatus == TrackableBehaviour.Status.TRACKED)
                        GameManager.Instance.CardTracked(this);
                    else if (newStatus == TrackableBehaviour.Status.NOT_FOUND)
                        GameManager.Instance.CardLost(this);
        }
        
    }
}
