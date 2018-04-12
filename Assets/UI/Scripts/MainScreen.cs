using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// OnClick handler for the settings button. Call UIManager to request a UI state change.
    /// </summary>
    public void OnSettingsClicked()
    {
        UIManager.Instance.SetDisplayMode(UIState.Settings);
    }

    public void OnStartGameClicked()
    {
        GameManager.Instance.StartGame();
    }
}
