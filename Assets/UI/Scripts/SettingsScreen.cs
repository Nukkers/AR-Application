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
    private GameObject[] mModels; // holds all gameobjects of the models (for setting new model size)
    private GameObject mLastSizeSelected = null; 

    public ExampleStreaming mVoiceInput;

	// Use this for initialization
	void Start () {
        mModels = GameObject.FindGameObjectsWithTag("Models");

        // Access screenwidget -> panel ->  button
        mAudioButton = gameObject.transform.Find("LayoutPanel").transform.Find("AudioDescriptionButton");
        mContrastButton = gameObject.transform.Find("LayoutPanel").transform.Find("HighContrastButton");
        mVoiceButton = gameObject.transform.Find("LayoutPanel").transform.Find("VoiceInputButton");

        //mVoiceInput = GameObject.Find("WatsonSpeechRecognition").GetComponent<ExampleStreaming>();

        //Set status of buttons
        //mVoiceButton.GetComponent<BigToggleButton>().OnSetState(mVoiceInput.IsEnabled());

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

        if (mVoiceInput.IsEnabled())
        {
            mVoiceInput.DisableSpeechRecog();
        }
        else
        {
            mVoiceInput.EnableSpeechRecog();
        }

    }

    // Stops playing all sounds of previously clicked button (to avoid sound overlapping)
    private void StopPlayingSound()
    {
        foreach (AudioSource audio in mAudio)
        {
            audio.Stop();
        }
    }

    // Resize models based on the name of the button that was clicked
    public void OnModelSizeClicked(GameObject button)
    {
        // scale of models
        const float defaultScale = 1.0f;
        const float largeScale = 2.0f;
        const float xLargeScale = 2.5f;

        // check if the same model size button was not pressed 2 times in row
        if (mLastSizeSelected != button)
        {
            mLastSizeSelected = button;

            button.GetComponents<AudioSource>();
            
            switch (button.name)
            {
                case "DefaultSizeButton":
                    foreach (GameObject model in mModels)
                        model.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
                    break;

                case "LargeSizeButton":
                    foreach (GameObject model in mModels)
                        model.transform.localScale = new Vector3(largeScale, largeScale, largeScale);
                    break;

                case "XLargeSizeButton":
                    foreach (GameObject model in mModels)
                        model.transform.localScale = new Vector3(xLargeScale, xLargeScale, xLargeScale);
                    break;
            }
            // Play sound if audio description button is toggled on
            if (mAudioButton.GetComponent<BigToggleButton>().selectionState)
            {
                if (mAudio != null)
                    StopPlayingSound();

                mAudio = button.GetComponents<AudioSource>();
                mAudio[0].Play();
            }

        }

    }
}
