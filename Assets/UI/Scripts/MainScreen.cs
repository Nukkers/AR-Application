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
<<<<<<< HEAD

        UIManager.Instance.SetDisplayMode(UIState.Settings);

=======
>>>>>>> 904543e2e9b56c4d33165828aaed88aab0c68658
        UIManager.Instance.SetDisplayMode(UIState.Settings);
    }

    /// <summary>
    /// OnClick handler for the start game button. Call UI Manager to request a state change and
    /// call GameManager to start the game itself.
    /// </summary>
    public void OnStartGameClicked()
    {
        UIManager.Instance.DisplayGameplay();
        GameManager.Instance.StartGame();
<<<<<<< HEAD

=======
>>>>>>> 904543e2e9b56c4d33165828aaed88aab0c68658
    }
}
