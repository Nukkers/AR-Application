using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour {

    private AudioSource[] mAudio; // Will store audio sources that will be played when a button is clicked (if audio description is enabled)

    private Transform mAudioButton; // audio button
    public BigToggleButton mVoiceToggle;
    private Transform mContrastButton; // contrast button
    private Transform mVoiceButton; // voice input button
    private GameObject[] mModels; // holds all gameobjects of the models (for setting new model size)
    private GameObject mLastSelectedObj; // previous object that was selected for model size
    
    // Values of model sizes
    private const float mDefaultScale = 1.0f;
    private const float mLargeScale = 2.0f;
    private const float mXLargeScale = 2.5f;

    // Colours of buttons
    private ColorBlock mDefaultColourblock;
    private ColorBlock mNewColourblock;
    private Color mNewColour;

    public ExampleStreaming mVoiceInput;

	// Use this for initialization
	void Start () {

        // Place all models in an array
        mModels = GameObject.FindGameObjectsWithTag("Models");

        // Access screenwidget -> panel ->  button
        mAudioButton = gameObject.transform.Find("LayoutPanel").transform.Find("AudioDescriptionButton");
        mContrastButton = gameObject.transform.Find("LayoutPanel").transform.Find("HighContrastButton");
        mVoiceButton = gameObject.transform.Find("LayoutPanel").transform.Find("VoiceInputButton");

        //mVoiceInput = GameObject.Find("WatsonSpeechRecognition").GetComponent<ExampleStreaming>();

        //Set status of buttons
        //mVoiceButton.GetComponent<BigToggleButton>().OnSetState(mVoiceInput.IsEnabled());

        // Selectd default size of models when first starting the game
        mLastSelectedObj = GameObject.Find("DefaultSizeButton");
        mNewColour = Color.blue;
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

        if (mAudioButton.GetComponent<BigToggleButton>().selectionState == true)
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

        if (mAudioButton.GetComponent<BigToggleButton>().selectionState == true)
        {
            if (mAudio != null)
                StopPlayingSound();

            mAudio = mContrastButton.GetComponents<AudioSource>();

            if (mContrastButton.GetComponent<BigToggleButton>().selectionState == true)
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

        if (mAudioButton.GetComponent<BigToggleButton>().selectionState == true)
        {
            if (mAudio != null)
                StopPlayingSound();

            mAudio = mVoiceButton.GetComponents<AudioSource>();

            if (mVoiceButton.GetComponent<BigToggleButton>().selectionState == true)
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
    // Change button colour of currently selected size of models
    public void OnModelSizeClicked(GameObject newObj)
    {

        // check if the same model size button object was not pressed 2 times in row
        if (mLastSelectedObj != newObj)
        {
            // Assign default colour (grey) before changing to new colour
            Button activeButton = mLastSelectedObj.GetComponent<Button>();
            Button selectedButton = newObj.GetComponent<Button>();
            mDefaultColourblock = selectedButton.colors;
            activeButton.colors = mDefaultColourblock;

            // change the colour of new colour block
            mNewColourblock = selectedButton.colors;
            mNewColourblock.normalColor = mNewColour;
            selectedButton.colors = mNewColourblock;

            // audio
            newObj.GetComponents<AudioSource>();

            switch (newObj.name)
            {
                case "DefaultSizeButton":
                    foreach (GameObject model in mModels)
                        model.transform.localScale = new Vector3(mDefaultScale, mDefaultScale, mDefaultScale);
                    break;

                case "LargeSizeButton":
                    foreach (GameObject model in mModels)
                        model.transform.localScale = new Vector3(mLargeScale, mLargeScale, mLargeScale);
                    break;

                case "XLargeSizeButton":
                    foreach (GameObject model in mModels)
                        model.transform.localScale = new Vector3(mXLargeScale, mXLargeScale, mXLargeScale);
                    break;
            }
            // Play sound if audio description button is toggled on
            if (mAudioButton.GetComponent<BigToggleButton>().selectionState == true)
            {
                if (mAudio != null)
                    StopPlayingSound();

                mAudio = newObj.GetComponents<AudioSource>();
                mAudio[0].Play();
            }

            // 
            mLastSelectedObj = newObj;
        }

    }
}
