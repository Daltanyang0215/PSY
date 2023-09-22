using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public static class SaveSystem
{
    public static void DataSave()
    {
        // Set our save location and make sure we have a saves folder ready to go.
        string savePath = Application.persistentDataPath + "/saves0/";

        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        Debug.Log("Saving");

        //FileStream fileStream = new FileStream(savePath + "mapObejct", FileMode.Create);
        //byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(mapData));
        //fileStream.Write(data, 0, data.Length);
        //fileStream.Close();

    }


    public static void DataLoad(string mapName)
    {
        string savePath = Application.persistentDataPath + "/saves0/";

        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        string loadpath = Application.persistentDataPath + "/saves/" + mapName + "/";

        if (File.Exists(loadpath + "mapObejct"))
        {
            //Debug.Log(mapName + " mapObejct found. loading from save.");

            //FileStream fileStream = new FileStream(loadpath + "mapObejct", FileMode.Open);
            //byte[] data = new byte[fileStream.Length];
            //fileStream.Read(data, 0, data.Length);
            //fileStream.Close();
            //string jsonData = Encoding.UTF8.GetString(data);
            //return JsonUtility.FromJson<MapSaveData>(jsonData);
        }
        else
        {
            //return null;
        }
    }
}
