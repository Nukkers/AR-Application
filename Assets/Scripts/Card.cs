using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the game logic of a card.
/// </summary>
public class Card : MonoBehaviour {
    public int cardID;
    public int cardType; // TODO : Make me an enum.
    public AudioClip audioCue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        		
	}

    public void PlayAudioCue()
    {
        AudioSource.PlayClipAtPoint(audioCue, Vector3.zero); // TODO : Do this less terribly.
        Debug.Log("Played audio cue " + audioCue.name);
    }
    
}
