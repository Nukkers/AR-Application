﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSettingsClicked()
    {
        Debug.Log("OnSettingsClicked hit!");
        UIManager.instance.DisplaySettingsScreen();
    }
}