using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;


public class UI : MonoBehaviour
{

    private GameObject quitButton;
    private GameObject popUpPanel;
    private GameObject settingsPanel;
    private Text confirmText;
    public UnityEngine.UI.Image theBackground;
    private UnityEngine.UI.Toggle toggle;
    // Use this for initialization
    void Start()
    {
        //// disable specific buttons/panels at the start of the game
        //toggle = GameObject.Find("Settings_Panel").GetComponentInChildren<UnityEngine.UI.Toggle>();
        //theBackground = GameObject.Find("Settings_Panel").GetComponentInChildren<UnityEngine.UI.Image>();
        //if (theBackground == null) Debug.Log("Background is null");
        //if (toggle == null) Debug.Log("toggle is null");
        //quitButton = GameObject.Find("QuitCurrent_Button");
        //popUpPanel = GameObject.Find("Popup_Panel");
        //settingsPanel = GameObject.Find("Settings_Panel");
       
        //quitButton.SetActive(false);
        //popUpPanel.SetActive(false);
        //settingsPanel.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    // change background colour of toggle button whenever it changes
    public void ChangeToggleBackground()
    {
        
        if (toggle.isOn)
        {
            // TODO
            //theBackground.material.color = Color.white;
            theBackground.color = Color.blue;
            //theBackground.material
            Debug.Log("Event triggered");
        }
    }

    // Display message as part of a popup
    public void DisplayMessage(string message)
    {
        confirmText = GameObject.Find("Popup_Panel").GetComponentInChildren<Text>();
        confirmText.text = message;
    }
}