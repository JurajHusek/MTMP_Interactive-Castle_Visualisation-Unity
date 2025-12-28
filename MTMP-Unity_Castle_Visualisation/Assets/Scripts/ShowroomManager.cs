using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShowroomManager : MonoBehaviour
{
    public CameraController cc;
    public UiManager ui;
    public Light directionalLight;
    public string jsonFileName;
    public Dictionary<string, string> data = new Dictionary<string, string>();
    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"JSON file not found: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(json);

        foreach (JsonItem item in wrapper.items)
        {
            data[item.key] = item.value;
        }
    }
    [System.Serializable]
    private class JsonWrapper
    {
        public List<JsonItem> items;
    }

    [System.Serializable]
    private class JsonItem
    {
        public string key;
        public string value;
    }

    public void QuitShowroom()
    {
        Application.Quit();
    }

    public void DisableShadows()
    {
        directionalLight.shadows = LightShadows.None;
    }

    public void EnableHardShadows()
    {
        directionalLight.shadows = LightShadows.Hard;
    }
}
