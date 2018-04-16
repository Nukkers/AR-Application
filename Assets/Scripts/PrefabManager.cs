using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles loading arbitrary prefabs from resource files and storing
/// cached references to speed up future instantiation.
/// Implemented as a singleton, though a static class would work IIRC Unity doesn't play nicely with these due to the
/// lifecycle of a MonoBehaviour.
/// </summary>
public class PrefabManager : MonoBehaviour
{
    /// <summary>
    /// Singleton accessor function
    /// </summary>
    private static PrefabManager mInstance = null;
    public static PrefabManager Instance
    {
        get
        {
            if (!mInstance)
            {
                mInstance = new PrefabManager();
            }
            return mInstance;
        }
    }

    private Dictionary<string,GameObject> mLoadedPrefabs = new Dictionary<string, GameObject>(); // Holds the prefabs as key / value pairs for ease of access
    // Use this for initialization
    void Start()
    {
        /* Set mInstance to this instance of the class if null, otherwise kill the object */
        if (mInstance == null)
            mInstance = this;
        else
            Destroy(this);
    }

    /// <summary>
    /// Loads a prefab instance from the resources folder
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public GameObject GetPrefab(string prefabName)
    {
        /* Check if we already have the object loaded and stored, if so return it */
        if (mLoadedPrefabs.ContainsKey(prefabName))
            return mLoadedPrefabs[prefabName];

        /* No object was found, so lets try and load it from disk */
        object loadedObject = Resources.Load(prefabName);

        if (loadedObject != null)
        {
            mLoadedPrefabs.Add(prefabName,(GameObject)loadedObject);
            return (GameObject)loadedObject;
        }
        else
            throw new System.Exception("Unable to load object \"" + prefabName + "\" it doesn't seem to exist!");
    }
}
