using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ModelDefaults : MonoBehaviour, ITrackableEventHandler {

	const float TIGER_X = 0.5;
	const float TIGER_Y = 0.5;
	const float TIGER_Z = 0.5;
	bool TigerVisible = false;

	private TrackableBehaviour mTrackableBehaviour;

	// Use this for initialization
	void Start () 
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}

	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
		
			TigerVisible = true;
			//make model visibke, start at size 0 and transform to full size?

		} else 
		{
			//Hide model
			TigerVisible = false;
		}
	
	}
}
