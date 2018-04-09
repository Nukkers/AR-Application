using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigToggleButton : MonoBehaviour {

    Text mText;
    public string originalIcon; // The original (i.e., off state) icon to use
    public string swappedIcon; // The icon to use in the swapped state (i.e., when on)
    public bool selectionState = false;

    // Use this for initialization
    void Start()
    {
        mText = this.GetComponent<Text>();
        if (selectionState == false)
            mText.text = originalIcon;
        else
            mText.text = swappedIcon;


    }
}
