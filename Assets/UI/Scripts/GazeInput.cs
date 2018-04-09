﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Implements gaze-based navigation.
/// Heavily inspired by the sample code here: https://docs.unity3d.com/ScriptReference/UI.GraphicRaycaster.Raycast.html 
/// </summary>
public class GazeInput : MonoBehaviour
{
    public GraphicRaycaster m_Raycaster = null;
    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public Camera mCamera;
    public Transform mLastCanvas = null;
    public Button lastSelected = null;
    public const int uiLayerMask = 1 << 5; // TODO : Refactor me, generate a layer mask for UI objects.
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
        mCamera = this.GetComponent<Camera>();
    }

    void Update()
    {
        /* Update the looked at canvas */
        /* Send a 'ray' from the middle of the screen towards our scene */
        Ray ray = mCamera.ScreenPointToRay(new Vector3(mCamera.pixelWidth / 2, mCamera.pixelHeight / 2)); // cast from the center of the screen
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            if (rayHit.transform != mLastCanvas)
                OnNewCanvasLookedAt(rayHit.transform);
        }


        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        if (m_Raycaster != null)
        {
            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count == 0)
            {
                if (lastSelected != null)
                {
                    lastSelected.OnDeselect(null);
                    lastSelected = null;
                }
            }

            int count = 0;

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name + " #" + count);
                count++;
                Button obj = result.gameObject.GetComponent<Button>(); // Surely there's a better way?
                if (obj != null)
                {
                    if (obj != lastSelected && lastSelected != null)
                        lastSelected.OnDeselect(null);

                    obj.OnSelect(null);
                    lastSelected = obj;
                    break;
                }
            }
        }
        /* TODO : Make this less mobile specific */
        bool hasClicked = false;
        //Touch _touch = Input.GetTouch(0);
        if (Input.GetMouseButtonDown(0))
            hasClicked = true;

        if (hasClicked && lastSelected != null)
            lastSelected.OnPointerClick(m_PointerEventData);

    }

    private void OnNewCanvasLookedAt(Transform newTransform)
    {
        mLastCanvas = newTransform;
        m_Raycaster = newTransform.GetComponent<GraphicRaycaster>();
    }
}
