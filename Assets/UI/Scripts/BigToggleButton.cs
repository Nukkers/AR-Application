using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the 'big toggle' buttons used for the settings screen. Handles changing the text between various
/// font-awesome icons
/// </summary>
public class BigToggleButton : MonoBehaviour {

    public Text iconTextField;
    public string offIcon; // The original (i.e., off state) icon to use
    public string onIcon; // The icon to use in the swapped state (i.e., when on)
    public bool selectionState = false;

    // Use this for initialization
    void Start()
    {
        if (selectionState == false)
            iconTextField.text = offIcon;
        else
            iconTextField.text = onIcon;
    }

    public void ToggleState()
    {
        OnSetState(!selectionState);
    }

    public void OnSetState(bool state)
    {
        if (state == false)
            iconTextField.text = offIcon;
        else
            iconTextField.text = onIcon;
        selectionState = state;
    }
}
