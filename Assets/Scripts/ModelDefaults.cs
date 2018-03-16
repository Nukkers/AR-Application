using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ModelDefaults : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    Vector3 modelMaxSize;
    public Transform modelTransform;
    bool mScaleModel = false;
    // Use this for initialization
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

            modelMaxSize = modelTransform.transform.localScale;
            modelTransform.transform.localScale = new Vector3(0, 0, 0);
            Debug.Log("Model Scaling started");

        }
    }
    void Update()
    {
        //make model visibke, start at size 0 and transform to full size
        if (mScaleModel)
        {
            if (modelTransform.localScale.sqrMagnitude < modelMaxSize.sqrMagnitude)
            {
                modelTransform.transform.localScale += Time.deltaTime * (new Vector3(0.001f, 0.001f, 0.001f));     //(modelMaxSize);		
                Debug.Log("Model Scaling Called");
            }
        }
    }
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Model Scaling detected ");
            mScaleModel = true;



        }
        else
            mScaleModel = false;
    }
}
