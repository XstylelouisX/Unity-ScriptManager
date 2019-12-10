//Creator:HUANG YU XIANG
//Date:2019/12/10
//Update:2019/12/10
//Source:https://www.youtube.com/watch?v=XOjd_qU2Ido&list=PL6guGHtumGIXjmja78xJQFV-DrLVBE1x2&index=33&t=0s
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager {

    /// <summary>
    /// Save(儲存)
    /// </summary>
    /// <param name="player">Save data(儲存資料)</param>
    public static void SavePlayer(SaveManager player)
    {
        //二進位格式化
        BinaryFormatter bf = new BinaryFormatter();
        //儲存路徑
        FileStream stream = new FileStream(Application.persistentDataPath + "/save.sav", FileMode.Create);
        //儲存資料類別
        PlayerData data = new PlayerData(player);
        //序列化
        bf.Serialize(stream, data);
        //暫時關閉(釋放資源)
        stream.Close();
    }

    /// <summary>
    /// Load(讀取)
    /// </summary>
    /// <returns>Save data(儲存資料)</returns>
    public static PlayerData LoadPlayer()
    {
        if(File.Exists(Application.persistentDataPath + "/save.sav"))
        {
            //二進位格式化
            BinaryFormatter bf = new BinaryFormatter();
            //讀取路徑
            FileStream stream = new FileStream(Application.persistentDataPath + "/save.sav", FileMode.Open);
            //反序列化
            PlayerData data = bf.Deserialize(stream) as PlayerData;
            //暫時關閉(釋放資源)
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File does not exist.");
            return null;
        }
    }
}
