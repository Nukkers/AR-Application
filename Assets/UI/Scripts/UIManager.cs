using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum UIState
{
    Default,
    MainMenu,
    Settings
}

public class UIManager : MonoBehaviour
{
    public GameObject mainScreenPrefab;
    public GameObject settingsScreenPrefab;
    public GameObject hudPrefab;

    private static UIManager mInstance;

    private GameObject mHudOverlay;
    private GameObject mSettingsScreen;
    private GameObject mMainScreen;
    private GazeInput mGazeInput;
    private Camera mCamera;
    public UIState currentState = UIState.Default;


    /// <summary>
    /// Singleton accessor function
    /// </summary>
    public static UIManager instance
    {
        get
        {
            if (!mInstance)
                new GameObject("UI Manager");

            return mInstance;
        }
    }

    // Use this for initialization
    void Start()
    {
        /* Set mInstance to this instance of the class if null, otherwise kill the object */
        if (mInstance == null)
            mInstance = this;
        else
            Destroy(this);

        /* Instantiate prefabs, we do this ahead of time to minimize any runtime impact */
        mHudOverlay = GameObject.Instantiate(hudPrefab);
        mSettingsScreen = Instantiate(settingsScreenPrefab);
        mMainScreen = Instantiate(mainScreenPrefab);

        /* Set the default UI state */
        mSettingsScreen.SetActive(false);
        mMainScreen.SetActive(false);

        mGazeInput = gameObject.AddComponent<GazeInput>();
        mCamera = Camera.main;
        SetDisplayMode(UIState.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when a widget (UI panel prefab) is to be set as active. Update the positon of the object to a sane
    /// value and make it look towards the camera, then set it as active / visible.
    /// </summary>
    /// <param name="widget"></param>
    private void SetWidgetActive(GameObject widget)
    {
        widget.transform.position = (mCamera.transform.forward * 10); // Set the transform 10 units infront of the camera.
        //widget.transform.LookAt(-mCamera.transform.forward); // We should be able to safely assume that 0,0,0 is the camera.
        widget.SetActive(true);
    }

    public void SetDisplayMode(UIState newMode)
    {
        switch (currentState)
        {
            case UIState.MainMenu:
                mMainScreen.SetActive(false);
                break;
            case UIState.Settings:
                mSettingsScreen.SetActive(false);
                break;
            default:
                break;
        }

        switch (newMode)
        {
            case UIState.Default:
                mSettingsScreen.SetActive(false);
                mMainScreen.SetActive(false);
                break;
            case UIState.MainMenu:
                SetWidgetActive(mMainScreen);
                break;
            case UIState.Settings:
                SetWidgetActive(mSettingsScreen);
                break;
        }

        currentState = newMode; // Set the current state to be the new mode.
    }
    public void DisplaySettingsScreen()
    {
        currentState = UIState.Settings;
        Debug.Log("DisplaySettingsScreen hit!");
    }

}
