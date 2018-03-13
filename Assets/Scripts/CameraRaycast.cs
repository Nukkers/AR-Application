using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implememnts behaviour for 'look at' audio cues. 
/// </summary>
public class CameraRaycast : MonoBehaviour
{
    public Camera mCamera; // Reference to the primary scene camera
    public float lookAtDuration; // Duration that the user needs to look at an object before triggering an audio cue
    private float mLookAtElapsedTime;
    private Transform mLastTransform;
    private bool mPlayedAudioCue = false;
    public RectTransform targetReticle;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* Send a 'ray' from the middle of the screen towards our scene */
        Ray ray = mCamera.ScreenPointToRay(new Vector3(mCamera.pixelWidth / 2, mCamera.pixelHeight / 2)); // cast from the center of the screen
        RaycastHit rayHit;

        /* Check that raycast status */
        if (Physics.Raycast(ray, out rayHit))
        {
            if (rayHit.transform == mLastTransform)// Does this work in Unity? 
            {
                if (!mPlayedAudioCue)
                {
                    Debug.Log("Looking at object " + rayHit.transform.name);
                    Debug.Log("Looked at for: " + mLookAtElapsedTime);
                    mLookAtElapsedTime += Time.deltaTime; // Increment the time by the delta time (i.e., the time elapsed between frames)

                    /* The user has looked at the object for x duration, play the audio cue */
                    if ((mLookAtElapsedTime > lookAtDuration) && !mPlayedAudioCue)
                    {
                        Debug.Log("Look at duration hit!");
                        Card _card = rayHit.transform.GetComponent<Card>(); // Attempt to get a handle to a Card object
                        if (_card == null)
                            _card = rayHit.transform.parent.GetComponent<Card>(); // Attempt to retrieve from the parent, if the given object is not in the child.

                        if (_card != null) // Check we have a valid handle before trying to call against the object.
                            _card.PlayAudioCue();
                        else
                            Debug.Log("Not a valid Card object! unable to play cue"); // Report that we've been unable to play a cue.
                        mPlayedAudioCue = true;
                    }
                }
            }
            mLastTransform = rayHit.transform;
        }
        else
        {
            mLookAtElapsedTime -= Time.deltaTime; // Decrease look-at time while the user isn't looking at the object.
            if (mLookAtElapsedTime < 0)
            {
                Debug.Log("Stopped tracking look at target");
                /* Reset status */
                mLastTransform = null;
                mLookAtElapsedTime = 0.0f;
                mPlayedAudioCue = false;

            }
        }
    }

    void FixedUpdate()
    {
       
    }
}
