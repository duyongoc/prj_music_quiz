using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonFileReader
{
    public static string pathWindows;
    public static string pathAndroid;

    // public static void SaveJson(Playlist i)
    // {
    //     pathWindows = Application.dataPath; 
    //     pathAndroid = Application.persistentDataPath;

    //     string json = JsonUtility.ToJson(i);
    //     File.WriteAllText(pathWindows + "/" + i.itemName + ".json", json);
    // }

    public static void LoadJson(string name)
    {
        pathWindows = Application.dataPath;
        string path = pathWindows + "/" + name;
        string jsonFilePath = path.Replace(".json", "");
        TextAsset loadedJsonFile = Resources.Load<TextAsset>(jsonFilePath);
        Debug.Log( loadedJsonFile);
    }


    public static string LoadJsonAsResource(string path)
    {
        string jsonFilePath = path.Replace(".json", "");
        TextAsset loadedJsonFile = Resources.Load<TextAsset>(jsonFilePath);
        return loadedJsonFile.text;
 
    }

    public static void LoadGameData(string name)
    {
        Debug.Log(Application.streamingAssetsPath);
        string filePath = Path.Combine(Application.streamingAssetsPath, name);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            Playlist loadedData = JsonUtility.FromJson<Playlist>(dataAsJson);

            Debug.Log(loadedData);
        }
        else
        {
            //Debug.LogError("Cannot load game data");
        }
    }




}
