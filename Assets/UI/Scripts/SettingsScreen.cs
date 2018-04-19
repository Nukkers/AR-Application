using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class SettingsScreen : MonoBehaviour {

    private AudioSource[] mAudio; // Will store audio sources that will be played when a button is clicked (if audio description is enabled)

    private Transform mAudioButton; // audio button
    public BigToggleButton mVoiceToggle;
    private Transform mContrastButton; // contrast button
    private Transform mVoiceButton; // voice input button
	// Use this for initialization
	void Start () {

        // Access screenwidget -> layout panel ->  button ---- probably there are easier ways to do it, but this would be faster than just using GameObject.Find ? Uglier though.
        mAudioButton = gameObject.transform.Find("LayoutPanel").transform.Find("AudioDescriptionButton");
        mContrastButton = gameObject.transform.Find("LayoutPanel").transform.Find("HighContrastButton");
        mVoiceButton = gameObject.transform.Find("LayoutPanel").transform.Find("VoiceInputButton");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnBackButtonClicked()
    {
        UIManager.Instance.DisplayMainMenu();
    }
    /// <summary>
    /// OnClick handler for the Audio Description button
    /// </summary>
    public void OnAudioDescriptionClicked()
    {
        // check if audio description is enabled. If true then
        // Get the necessary audio sources to play
        // Stop any sounds that are currently being played (in the settings screen only)
        // play the necessary sound

        if (mAudio !=null)
            StopPlayingSound();

        mAudio = mAudioButton.GetComponents<AudioSource>();

        if (mAudioButton.GetComponent<BigToggleButton>().selectionState)
            mAudio[1].Play();
        else
            mAudio[2].Play();


        // TODO : Implement audio description setting button logic
    }

    /// <summary>
    /// OnClick handler for the High Contrast button
    /// </summary>
    public void OnHighContrastClicked()
    {
        // check if audio description is enabled. If true then
        // Get the necessary audio sources to play
        // Stop any sounds that are currently being played (in the settings screen only)
        // play the necessary sound

        if (mAudioButton.GetComponent<BigToggleButton>().selectionState)
        {
            if (mAudio != null)
                StopPlayingSound();

            mAudio = mContrastButton.GetComponents<AudioSource>();

            if (mContrastButton.GetComponent<BigToggleButton>().selectionState)
                mAudio[1].Play();
            else
                mAudio[2].Play();
        }

        // TODO : Implement high contrast setting button logic
    }

    /// <summary>
    /// OnClick handler for the Voice Input button
    /// </summary>
    public void OnVoiceInputClicked()
    {
        // check if audio description is enabled. If true then
        // Get the necessary audio sources to play
        // Stop any sounds that are currently being played (in the settings screen only)
        // play the necessary sound

        if (mAudioButton.GetComponent<BigToggleButton>().selectionState)
        {
            if (mAudio != null)
                StopPlayingSound();

            mAudio = mVoiceButton.GetComponents<AudioSource>();

            if (mVoiceButton.GetComponent<BigToggleButton>().selectionState)
                mAudio[1].Play();
            else
                mAudio[2].Play();
        }

        // TODO : Implement voice input setting button logic
    }

    // Stops playing all sounds of previously clicked button (to avoid sound overlapping)
    private void StopPlayingSound()
    {
        foreach (AudioSource audio in mAudio)
        {
            audio.Stop();
        }
    }
}
