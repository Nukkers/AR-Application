using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Implements gaze-based navigation.
/// Heavily inspired by the sample code here: https://docs.unity3d.com/ScriptReference/UI.GraphicRaycaster.Raycast.html 
/// </summary>
public class GazeInput : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    Button lastSelected = null;
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
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

        /* TODO : Make this less mobile specific */
        if (Input.GetTouch(0).phase == TouchPhase.Began && lastSelected != null)
            lastSelected.OnPointerClick(null);
    
    }



}