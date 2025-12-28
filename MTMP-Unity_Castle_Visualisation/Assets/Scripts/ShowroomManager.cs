using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShowroomManager : MonoBehaviour
{
    public CameraController cc;
    public UiManager ui;
    public Light directionalLight;
    public string jsonFileName; // name of JSON file with castle-parts information
    public Dictionary<string, string> data = new Dictionary<string, string>(); // dictionary for storing information from JSON

    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName); // load JSON from StreamingAssets folder

        if (!File.Exists(path))
        {
            Debug.LogError($"JSON file not found: {path}");
            return;
        }
        string json = File.ReadAllText(path);
        // use defined Wrapper to correctly parse JSON to dictionary
        JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(json);
        foreach (JsonItem item in wrapper.items)
        {
            data[item.key] = item.value;
        }
    }
    // Wrapper - list of entries
    [System.Serializable]
    private class JsonWrapper
    {
        public List<JsonItem> items;
    }
    // Wrapper - each entry with key and stored value
    [System.Serializable]
    private class JsonItem
    {
        public string key;
        public string value;
    }

    public void QuitShowroom() // close app
    {
        Application.Quit();
    }

    public void DisableShadows() // disable all shadows in directional light settings
    {
        directionalLight.shadows = LightShadows.None;
    }

    public void EnableHardShadows() // enable hard-shadows in directional light settings
    {
        directionalLight.shadows = LightShadows.Hard;
    }
}
