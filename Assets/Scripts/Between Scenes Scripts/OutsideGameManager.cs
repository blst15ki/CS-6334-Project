using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutsideGameManager : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;

    void Start()
    {
        if (GameManager.Instance == null) {
            return;
        }

        LoadItemsIntoHotbar();

        LoadGameData();
    }

    private void LoadItemsIntoHotbar()
    {
        if (hotbar == null) {
            hotbar = FindObjectOfType<Hotbar>();
        }
        
        List<GameObject> savedItems = GameManager.Instance.GetItems();
        if (savedItems.Count != 0) {
            hotbar.LoadItems(savedItems);
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

                Plant plant = pot.GetComponentInChildren<Plant>();
                if (plant != null) {
                    PlantData plantData = new PlantData(plant);
                    plantList.Add(plantData);
                }
                GameObject.Destroy(pot);
            }
        }

        PotsAndPlants potsAndPlants = new PotsAndPlants(potList, plantList);

        List<SprinklerData> sprinklerList = new List<SprinklerData>();
        GameObject[] sprinklers = GameObject.FindGameObjectsWithTag("Sprinkler");
        foreach (GameObject sprinkler in sprinklers) {
            if (sprinkler.activeInHierarchy) {
                SprinklerData sprinklerData = new SprinklerData(sprinkler);
                sprinklerList.Add(sprinklerData);
                GameObject.Destroy(sprinkler);
            }
        }

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

        LogGameData("Pots", potList);
        LogGameData("Plants", plantList);
        LogGameData("Sprinklers", sprinklerList);
        LogGameData("Fertilizers", fertilizerList);
        LogGameData("Water Cans", wateringCanList);

        return new GameData(potsAndPlants, sprinklerList, fertilizerList, wateringCanList);
    }

    public void LogGameData<T>(string label, List<T> items) {
        Debug.Log(label + " Count: " + items.Count);
        foreach (var item in items) {
            Debug.Log(item.ToString());
        }
    }

    public void LoadGameData(){

        if (GameManager.Instance == null || GameManager.Instance.GetOutsideGameData() == null) {
            return;
        }

        GameData gameData = GameManager.Instance.GetOutsideGameData();
        LogGameData("Pots", gameData.potsAndPlants.listOfPots);
        Dictionary<string, GameObject> potsMap = new Dictionary<string, GameObject>();
        foreach (PotData potData in gameData.potsAndPlants.listOfPots) {
            GameObject potPrefab = GameManager.Instance.GetPrefab("Pot");
            GameObject pot = Instantiate(potPrefab, potData.position, potData.rotation);
            Pot potScript = pot.GetComponent<Pot>();
            potScript.setPlantID(potData.plantID);
            potsMap[potData.plantID] = pot;
        }

        foreach (PlantData plantData in gameData.potsAndPlants.listOfPlants) {
            GameObject plantPrefab = GameManager.Instance.GetPrefab(plantData.type);
            GameObject plant = Instantiate(plantPrefab, plantData.position, plantData.rotation, potsMap[plantData.plantID].transform);
            plant.transform.localScale = plantData.scale;
            plant.GetComponent<Renderer>().material.color = plantData.color;

            Plant plantScript = null;
            if (plantData.type == "Basic Plant") {
                plantScript = plant.GetComponent<BasicPlant>();
            } else {
                Debug.Log("Error no script found for plant type");
            }

            if (plantScript != null) {
                plantScript.isDataLoaded = true;
                plantScript.water = plantData.water;
                plantScript.id = plantData.plantID;
                plantScript.type = plantData.type;
                plantScript.stage = plantData.stage;
                plantScript.cur = DateTime.Now;
                plantScript.timeHalf = plantData.timeHalf;
                plantScript.timeMature = plantData.timeMature;
                plantScript.isHalf = plantData.isHalf;
                plantScript.isMature = plantData.isMature;
                plantScript.isDead = plantData.isDead;
            }
            
        }

        foreach (SprinklerData sprinklerData in gameData.listOfSprinkler) {
            GameObject sprinklerPrefab = GameManager.Instance.GetPrefab("Sprinkler");
            GameObject sprinkler = Instantiate(sprinklerPrefab, sprinklerData.position, sprinklerData.rotation);
        }

        foreach (FertilizerData fertilizerData in gameData.listOfFertilizer) {
            GameObject fertilizerPrefab = GameManager.Instance.GetPrefab("Fertilizer");
            GameObject fertilizer = Instantiate(fertilizerPrefab, fertilizerData.position, fertilizerData.rotation);
        }

        foreach (WateringCanData wateringCanData in gameData.listOfWateringCan) {
            GameObject wateringCanPrefab = GameManager.Instance.GetPrefab("Watering Can");
            GameObject wateringcan = Instantiate(wateringCanPrefab, wateringCanData.position, wateringCanData.rotation);
        }
    }
}
