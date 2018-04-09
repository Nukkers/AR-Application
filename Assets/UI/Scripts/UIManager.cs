using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager mInstance;
    private static void CreateNewInstance()
    {
        GameObject instantiated = new GameObject("UI Manager");
        mInstance = instantiated.AddComponent<UIManager>();
    }
    /// <summary>
    /// Singleton accessor function
    /// </summary>
    public static UIManager instance
    {
        get
        {
            if (!mInstance)
                CreateNewInstance();

            return mInstance;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplaySettingsScreen()
    {
        Debug.Log("DisplaySettingsScreen hit!");
    }
}
