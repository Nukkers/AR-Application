﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Enumerated cards, way nicer to work with than bare integers
/// </summary>
public enum CardType
{
    Tiger,
    Tree,
    Plane,
    Car
}

/// <summary>
/// Represents the game logic of a card.
/// </summary>
public class Card : MonoBehaviour, ITrackableEventHandler
{
    public int pairID;
    public CardType cardType; // TODO : Make me an enum.
    public AudioClip audioCue;
    public bool matched = false;
    private TrackableBehaviour mTrackableBehaviour;


    static public string outlineFXResourceName = "cardOutlineFX";
    private GameObject mOutlinePrefab;


    // public GameObject outlinePrefab;

    // Use this for initialization
    void Start()
    {
        mOutlinePrefab = PrefabManager.Instance.GetPrefab("cardOutlineFX");

        Instantiate(mOutlinePrefab, this.transform);
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void PlayAudioCue()
    {
        AudioSource.PlayClipAtPoint(audioCue, Vector3.zero); // TODO : Do this less terribly.
        Debug.Log("Played audio cue " + audioCue.name);
    }

    /// <summary>
    /// Invoked when a new tracking item comes into frame.
    /// </summary>
    /// <param name="previousStatus"></param>
    /// <param name="newStatus"></param>
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {

        if (GameManager.Instance.gameStarted)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                GameManager.Instance.CardTracked(this);
            }
            else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NOT_FOUND)
            {
                GameManager.Instance.CardLost(this);
            }
        }

    }
}
