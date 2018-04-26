using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum UIState
{
    Default,
    MainMenu,
    Settings
}

public class UIManager : MonoBehaviour
{
    public string mainScreenResourceName = "MainScreenWidget";
    public string settingsScreenResourceName = "SettingsScreenWidget";
    public string hudResourceName = "HUD";


    private GameObject mHudOverlay;
    private GameObject mSettingsScreen;
    private GameObject mMainScreen;
    private GazeInput mGazeInput;
    private Camera mCamera;
    public UIState currentState = UIState.MainMenu;


    /// <summary>
    /// Singleton accessor function
    /// </summary>
    private static UIManager mInstance = null;
    public static UIManager Instance
    {
        get
        {
            if (!mInstance)
            {
                var temp = new GameObject();
                temp.AddComponent<UIManager>().Initialize();
            }
            return mInstance;
        }
    }

    internal void DisplayMainMenu()
    {
        SetDisplayMode(UIState.MainMenu);
    }


    /// <summary>
    /// Called when the singleton instance is initialized. We use this rather than Start to get around Unity lifecycle limitatinos
    /// </summary>
    void Initialize()
    {
        /* Set mInstance to this instance of the class if null, otherwise kill the object */
        if (mInstance == null )
            mInstance = this;
        else
            Destroy(this);

        /* Instantiate prefabs, we do this ahead of time to minimize any runtime impact */
        mHudOverlay = Instantiate(PrefabManager.Instance.GetPrefab(hudResourceName));
        mSettingsScreen = Instantiate(PrefabManager.Instance.GetPrefab(settingsScreenResourceName));
        mMainScreen = Instantiate(PrefabManager.Instance.GetPrefab(mainScreenResourceName));

        /* Set the default UI state */
        mSettingsScreen.SetActive(false);
        mMainScreen.SetActive(false);

        //mCamera = Camera.main;
        SetDisplayMode(UIState.MainMenu);
    }
    // Use this for initialization
    void Start()
    {
        mGazeInput = gameObject.AddComponent<GazeInput>(); // Initialize gaze input in start, otherwise it seems too early in unity's lifecycle to get a reference to the base game object.
    }

    /// <summary>
    /// Called when a widget (UI panel prefab) is to be set as active. Update the positon of the object to a sane
    /// value and make it look towards the camera, then set it as active / visible.
    /// </summary>
    /// <param name="widget"></param>
    private void SetWidgetActive(GameObject widget)
    {
        widget.transform.position = (Camera.main.transform.forward * 10); // Set the transform 10 units infront of the camera.
        widget.transform.LookAt(Camera.main.transform); // look at the position of the camera (usually _V(0,0,0), ensures the UI doesn't spawn at an odd angle)
        widget.SetActive(true);
    }

    /// <summary>
    /// Helper function to retrieve the currently active widget.
    /// </summary>
    /// <returns></returns>
    private GameObject GetActiveWidget()
    {
        switch (currentState)
        {
            case UIState.MainMenu:
                return mMainScreen;
                break;
            case UIState.Settings:
                return mSettingsScreen;
                break;
            default:
                return null;
                break;
        }
    }
    /// <summary>
    /// Sets the current display mode. Handles setting prefabs as active / inactive as the 
    /// current state requires.
    /// </summary>
    /// <param name="newMode"></param>
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

    /// <summary>
    /// Called when a click is performed into open air.
    /// Here we reset any open UI on the camera. We would also spawn the in-game UI here,
    /// if we had gotten that far...
    /// </summary>
    public void OnOpenAirClick()
    {
        if(currentState != UIState.Default)
            SetWidgetActive(GetActiveWidget());
    }

    /// <summary>
    /// Called when the settings screen should be displayed
    /// </summary>
    public void DisplaySettingsScreen()
    {
        SetDisplayMode(UIState.Settings);
        Debug.Log("UI State changed! now displaying settings screen");
    }

    /// <summary>
    /// Called when the gameplay display mode should be active (i.e, hide everything but the gaze cursor
    /// </summary>
    public void DisplayGameplay()
    {
        SetDisplayMode(UIState.Default);
        Debug.Log("UI State changed! now displaying gameplay UI");

    }

}
