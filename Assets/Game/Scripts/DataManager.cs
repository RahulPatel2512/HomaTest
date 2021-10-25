using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;


public class DataManager : GenericS<DataManager>
{
    public GameData data;
    string path;

#if UNITY_EDITOR
    [MenuItem("GameData/Data clear")]
    public static void Datacleard()
    {
        File.Delete(Application.dataPath + "/homatest.Rk");
    }
#endif

    void Awake()
    {
#if UNITY_EDITOR
        path = Application.dataPath + "/homatest.Rk";
#else
		path = Application.persistentDataPath + "/homatest.Rk";
#endif
        LoadData();
    }

    public void LoadData()
    {
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            data = bf.Deserialize(fs) as GameData;
            fs.Close();
            Debug.Log(data.Current_Level);
            SaveData();
            SetData();
        }
        else
        {
            SetData();
            SaveData();
        }
    }

    public void SetData()
    {
        //Only new Elements
    }

    public void SaveData()
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, data);
        fs.Close();
    }

    void OnApplicationPause(bool ispause)
    {
        if (ispause)
        {
            print("yes");
            SaveData();
        }
        else
        {
            print("No");
        }
    }
    void OnApplicationQuit()
    {
        SaveData();
    }
}

[Serializable]
public class GameData
{
    public int Current_Level=0;
}