using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class Save
{
    public static void SavePot(GameObject pot){
        Debug.Log("Entered Save Pot");
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/pot.dat";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        PotData data = new PotData(pot);
        bf.Serialize(fileStream,data);
        fileStream.Close();
    }
    public static PotData LoadPot(){
        Debug.Log("Loading Saved Data");
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/pot.dat";
        if(File.Exists(path)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            PotData data=bf.Deserialize(fileStream) as PotData;
            fileStream.Close();
            return data;
        }
        else{
            Debug.LogError("No saved data found");
            return null;
        }
    }
}
