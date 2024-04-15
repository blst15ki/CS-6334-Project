using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{
    public void SaveGame(){
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameData.dat";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        
        GameData gameData = new GameData {
            inventory = SaveInventory(),
            potsAndPlants = SavePotsAndPlants()
        };

        bf.Serialize(fileStream,gameData);
        fileStream.Close();
    }

    public void LoadGame(){
        Debug.Log("Loading Saved Data");
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/gameData.dat";

        if(File.Exists(path)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            GameData gameData = bf.Deserialize(fileStream) as GameData;
            LoadInventory(gameData.inventory);
            LoadPotsAndPlants(gameData.potsAndPlants);

            fileStream.Close();
            // return gameData;
        }
        else{
            Debug.LogError("No saved data found");
            // return null;
        }
    }

    private HotbarData SaveInventory() {
        Hotbar hotbar = FindObjectOfType<Hotbar>();

        return new HotbarData {
            //TODO: getItemTags need to be implemented
            itemTags = hotbar.getItemTags()
        };

    }

    private PotsAndPlants SavePotsAndPlants() {
        List<PotData> listOfPots = new List<PotData>();
        List<PlantData> listOfPlants = new List<PlantData>();
        foreach (Pot pot in FindObjectsOfType<Pot>()) {
            listOfPots.Add(new PotData{
                // position = new float[3],
                // rotation = new float[3],
                position[0]=pot.transform.position.x,
                position[1]=pot.transform.position.y,
                position[2]=pot.transform.position.z,
                // position = pot.transform.position,
                rotation[0]=pot.transform.rotation.x,
                rotation[1]=pot.transform.rotation.y,
                rotation[2]=pot.transform.rotation.z,
                // rotation = pot.transform.rotation,
                //TODO: plantID need to be set in the pot
                plantID = pot.plantID
            });

            //if pot has a plant id set, add to list of plants
            if(pot.plantID != ""){   
                listOfPlants.Add(new PlantData{
                position = new float[3],
                rotation = new float[3],    
                position[0]=positionPot[0],
                position[1]=positionPot[1],
                position[2]=positionPot[2],
                // position = pot.transform.position,
                rotation[0]=rotationPot[0],
                rotation[1]=rotationPot[1],
                rotation[2]=rotationPot[2]
                // rotation = pot.transform.rotation,
                //TODO: save all the other attribute
            });
            }
        }

        return new PotsAndPlants (
            listOfPots,
            listOfPlants
        );
        
    }

    private void LoadInventory(HotbarData inventory){
        GameObject character = GameObject.Find("Character");

        if (character != null) {
            Hotbar hotbar = character.GetComponentInChildren<Hotbar>();

        //TODO: Implement this setInventory function
        if (hotbar != null) {
            hotbar.setInventory(inventory.itemTags);
        } else {
            Debug.LogError("Fail to load inventory");
        }
        } else {
            Debug.LogError("Character object not found");
        }
    }

    private void LoadPotsAndPlants(PotsAndPlants potsAndPlants){
        List<PotData> listOfPots = potsAndPlants.listOfPots;
        List<PlantData> listOfPlants = potsAndPlants.listOfPlants;
        TrackPrefab prefabTracker = FindObjectOfType<TrackPrefab>();

        if (prefabTracker == null) {
            Debug.LogError("Prefab Tracker not found");
            return;
        }

        foreach (PotData potData in listOfPots) {
            GameObject potPrefab = prefabTracker.GetPrefabByTag("Pot");
            GameObject pot = Instantiate(potPrefab, potData.position, potData.rotation);

            if (potData.plantID != "") {
                PlantData plantData = listOfPlants.Find(plant => plant.plantID == potData.plantID);
                if (plantData != null) {
                    GameObject plantPrefab = prefabTracker.GetPrefabByTag("Plant");

                    GameObject plant = Instantiate(plantPrefab, plantData.position, plantData.rotation);
                    plant.transform.SetParent(pot.transform);
                }
            }
        }
    }
}
