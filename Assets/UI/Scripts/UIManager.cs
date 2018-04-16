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
        //widget.transform.LookAt(Camera.main.transform.forward); // We should be able to safely assume that 0,0,0 is the camera.
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
        SetDisplayMode(UIState.Settings);
        Debug.Log("UI State changed! now displaying settings screen");
    }

    public void DisplayGameplay()
    {
        SetDisplayMode(UIState.Default);
        Debug.Log("UI State changed! now displaying gameplay UI");

    }

}
