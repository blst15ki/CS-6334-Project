using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndoorGameManager : MonoBehaviour
{
    [SerializeField] NormalHotbar hotbar;
    [SerializeField] GameObject character;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject cardboard;
    [SerializeField] GameObject seedChest, itemChest;
    
    void Start()
    {
        if (GameManager.Instance == null) {
            return;
        }

        // Debug.Log("Before Teleport");
        // Debug.Log(GameManager.Instance.indoorSpawnPoint);
        cardboard.GetComponent<XRCardboardController>().enabled = false;
        character.GetComponent<CharacterMovement>().enabled = false;
        character.GetComponent<CharacterController>().enabled = false;

        if (GameManager.Instance.indoorSpawnPoint == "middle") {
            character.transform.position = new Vector3(3.31f, 1.11f, 0f);
            character.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            cardboard.GetComponent<XRCardboardController>().initialRotation = Quaternion.Euler(0f, -90f, 0f);
            // camera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        } else if (GameManager.Instance.indoorSpawnPoint == "front") {
            character.transform.position = new Vector3(9.26f, 1.11f, 0f);
            character.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            cardboard.GetComponent<XRCardboardController>().initialRotation = Quaternion.Euler(0f, -90f, 0f);
            // camera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        } else if (GameManager.Instance.indoorSpawnPoint == "end") {
            character.transform.position = new Vector3(-9.26f, 1.11f, 0f);
            character.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            cardboard.GetComponent<XRCardboardController>().initialRotation = Quaternion.Euler(0f, 90f, 0f);
            // camera.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }

        cardboard.GetComponent<XRCardboardController>().enabled = true;
        character.GetComponent<CharacterMovement>().enabled = true;
        character.GetComponent<CharacterController>().enabled = true;

        if (GameManager.Instance.isInsideHotBarDataNull()) {
            LoadItemsIntoHotbar();
        } else {
            LoadItemsIntoHotBarByInsideHotBarData();
            GameManager.Instance.resetInsideHotBarDataToNull();
        }
        
        LoadGameData();
    }

    public void LoadItemsIntoHotbar() {
        // if (hotbar == null) {
        //     hotbar = FindObjectOfType<Hotbar>();
        // }
        
        List<GameObject> savedItems = GameManager.Instance.GetItems();
        if (savedItems.Count != 0) {
            hotbar.LoadItems(savedItems);
        }
    }

    public void LoadItemsIntoHotBarByInsideHotBarData() {
        List<HotBarItem> savedItems = GameManager.Instance.GetInsideHotBarData().listOfHotBarItem;
        if (savedItems.Count != 0) {
            hotbar.LoadItemsFromHotBarData(savedItems);
        }
    }

    public GameData retrieveObjects(){
        List<PotData> potList = new List<PotData>();
        List<PlantData> plantList = new List<PlantData>();

        GameObject[] potObjectList = GameObject.FindGameObjectsWithTag("Pot");
        foreach (GameObject pot in potObjectList) {
            if (pot.activeInHierarchy) {
                PotData potData = new PotData(pot);
                potList.Add(potData);
                GameObject.Destroy(pot);
            }
        }

        GameObject[] plantObjectList = GameObject.FindGameObjectsWithTag("Plant");
        foreach(GameObject plant in plantObjectList) {
            if(plant.activeInHierarchy) {
                PlantData plantData = new PlantData(plant);
                plantList.Add(plantData);
                GameObject.Destroy(plant);
            }
        }

        PotsAndPlants potsAndPlants = new PotsAndPlants(potList, plantList);

        // empty list since sprinklers cannot be placed in inside scene
        List<SprinklerData> sprinklerList = new List<SprinklerData>();

        List<FertilizerData> fertilizerList = new List<FertilizerData>();
        GameObject[] fertilizers = GameObject.FindGameObjectsWithTag("Fertilizer");
        foreach (GameObject fertilizer in fertilizers) {
            if (fertilizer.activeInHierarchy) {
                FertilizerData fertilizerData = new FertilizerData(fertilizer);
                fertilizerList.Add(fertilizerData);
                GameObject.Destroy(fertilizer);
            }
        }

        List<WateringCanData> wateringCanList = new List<WateringCanData>();
        GameObject[] wateringCans = GameObject.FindGameObjectsWithTag("Watering Can");
        foreach (GameObject wateringCan in wateringCans) {
            if (wateringCan.activeInHierarchy) {
                WateringCanData wateringCanData = new WateringCanData(wateringCan);
                wateringCanList.Add(wateringCanData);
                GameObject.Destroy(wateringCan);
            }
        }

        List<ChestData> chestList = new List<ChestData>();
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach(GameObject chest in chests) {
            ChestData chestData = new ChestData(chest);
            chestList.Add(chestData);
        }

        // LogGameData("Pots", potList);
        // LogGameData("Plants", plantList);
        // LogGameData("Sprinklers", sprinklerList);
        // LogGameData("Fertilizers", fertilizerList);
        // LogGameData("Water Cans", wateringCanList);

        return new GameData(potsAndPlants, sprinklerList, fertilizerList, wateringCanList, chestList);
    }

    public void LogGameData<T>(string label, List<T> items) {
        Debug.Log(label + " Count: " + items.Count);
        foreach (var item in items) {
            Debug.Log(item.ToString());
        }
    }

    public void LoadGameData(){
        if (GameManager.Instance == null || GameManager.Instance.GetIndoorGameData() == null) {
            return;
        }

        GameData gameData = GameManager.Instance.GetIndoorGameData();
        Dictionary<string, GameObject> potsMap = new Dictionary<string, GameObject>();
        foreach (PotData potData in gameData.potsAndPlants.listOfPots) {
            GameObject potPrefab = GameManager.Instance.GetPrefab("Pot");
            GameObject pot = Instantiate(potPrefab, potData.position, potData.rotation);
            Pot potScript = pot.GetComponent<Pot>();
            potScript.isDataLoaded = true;
            potScript.id = potData.potID;
            potScript.SetPlantID(potData.plantID);
            potsMap[potData.potID] = pot; // key is potID so plant and pot can link
        }

        foreach (PlantData plantData in gameData.potsAndPlants.listOfPlants) {
            GameObject plantPrefab = GameManager.Instance.GetPrefab(plantData.type);
            GameObject plant = Instantiate(plantPrefab, plantData.position, plantData.rotation);
            plant.transform.localScale = plantData.scale;

            Plant plantScript = null;
            if (plantData.type == "Basic Plant" || plantData.type == "Fern") {
                plantScript = plant.GetComponent<Plant>();
            } else {
                Debug.Log("Error no script found for plant type");
            }

            if (plantScript != null) {
                plantScript.isDataLoaded = true;
                plantScript.water = plantData.water;
                plantScript.id = plantData.plantID;
                plantScript.SetPotID(plantData.potID);
                plantScript.type = plantData.type;
                plantScript.stage = plantData.stage;
                plantScript.cur = DateTime.Now;
                plantScript.timeLeave = plantData.timeLeave;
                plantScript.delay = plantData.delay;
                plantScript.isHalf = plantData.isHalf;
                plantScript.isMature = plantData.isMature;

                if(plantData.delay) {
                    plantScript.timeHalf = DateTime.Now.AddMinutes((plantData.timeHalf - plantData.timeLeave).TotalMinutes);
                    plantScript.timeMature = DateTime.Now.AddMinutes((plantData.timeMature - plantData.timeLeave).TotalMinutes);
                } else {
                    plantScript.timeHalf = plantData.timeHalf;
                    plantScript.timeMature = plantData.timeMature;
                }

                // link plant and pot objects
                plantScript.SetPot(potsMap[plantData.potID]);
                potsMap[plantData.potID].GetComponent<Pot>().SetPlant(plant);
            }
        }

        foreach (FertilizerData fertilizerData in gameData.listOfFertilizer) {
            GameObject fertilizerPrefab = GameManager.Instance.GetPrefab("Fertilizer");
            GameObject fertilizer = Instantiate(fertilizerPrefab, fertilizerData.position, fertilizerData.rotation);
        }

        foreach (WateringCanData wateringCanData in gameData.listOfWateringCan) {
            GameObject wateringCanPrefab = GameManager.Instance.GetPrefab("Watering Can");
            GameObject wateringcan = Instantiate(wateringCanPrefab, wateringCanData.position, wateringCanData.rotation);
        }

        foreach(ChestData chestData in gameData.listOfChestData) {
            Chest chestScript = null;
            if(chestData.chestName == "Seed Chest") {
                chestScript = seedChest.GetComponent<Chest>();
            } else if(chestData.chestName == "Item Chest") {
                chestScript = itemChest.GetComponent<Chest>();
            }

            TimeSpan diff = chestData.unlockTime - DateTime.Now;
            chestScript.SetTime((int)(diff.TotalSeconds));
        }
    }

    public LobbyHotBarData ConvertHotBarToLobbyHotBarData() {
        List<GameObject> listOfHotBar = hotbar.GetItems();
        List<HotBarItem> hotBarItems = new List<HotBarItem>();

        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (GameObject obj in listOfHotBar) {
            if (obj != null) {
                HotBarItem item = new HotBarItem(obj);
                hotBarItems.Add(item);

                if (obj.tag == "Pot") {
                    Pot pot = obj.GetComponent<Pot>();
                    GameObject plant = pot.GetPlant();
                    if (plant != null) {
                        objectsToDestroy.Add(plant);
                    }
                    
                } else {
                    Debug.Log(obj.tag);
                }
                objectsToDestroy.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj);
        }

        return new LobbyHotBarData(hotBarItems);
    }
}
