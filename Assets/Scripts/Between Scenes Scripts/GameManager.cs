using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Sprite wateringCanSprite;
    [SerializeField] Sprite potSprite;
    [SerializeField] Sprite fertilizerSprite;
    [SerializeField] Sprite sprinklerSprite;
    [SerializeField] GameObject wateringCanPrefab;
    [SerializeField] GameObject potPrefab;
    [SerializeField] GameObject fertilizerPrefab;
    [SerializeField] GameObject sprinklerPrefab;
    [SerializeField] GameObject basicPlantPrefab;
    public List<GameObject> savedHotBarItems = new List<GameObject>();
    public GameData outsideGameData = null;
    public GameData indoorGameData = null;
    public bool enableTutorial = true;
    
    
    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start() {
        List<PotData> indoorPotList = new List<PotData>() {
            new PotData { 
                position = new Vector3(-1.50f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = ""
            },
            new PotData { 
                position = new Vector3(-0.75f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = "93ab33b5-05dc-4559-952a-401ff7553ef6"
            },
            new PotData { 
                position = new Vector3(0.00f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = "6209b900-e00c-4cdb-abee-77e43203fb7d"
            }
        };

        List<PlantData> indoorPlantList = new List<PlantData>() {
            new PlantData {
                position = new Vector3(-0.75f, 0.75f, -4.36f),
                rotation = Quaternion.identity,
                plantID = "93ab33b5-05dc-4559-952a-401ff7553ef6",
                type = "Basic Plant",
                water = 20,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(1),
                timeMature = DateTime.Now.AddMinutes(2),
                isHalf = false,
                isMature = false,
                isDead = false
            },
            new PlantData {
                position = new Vector3(0.00f, 0.75f, -4.36f),
                rotation = Quaternion.identity,
                plantID = "6209b900-e00c-4cdb-abee-77e43203fb7d",
                type = "Basic Plant",
                water = 20,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(1),
                timeMature = DateTime.Now.AddMinutes(2),
                isHalf = false,
                isMature = false,
                isDead = false
            }
        };

        PotsAndPlants indoorPotsAndPlants = new PotsAndPlants(indoorPotList, indoorPlantList);

        List<FertilizerData> indoorFertilizerList = new List<FertilizerData>() {
            new FertilizerData {
                position = new Vector3(3.77f, 0.25f, 1.91f),
                rotation = Quaternion.identity
            }
        };

        List<WateringCanData> indoorWateringCanList = new List<WateringCanData>() {
            new WateringCanData {
                position = new Vector3(0.0f, 0.50f, 0.0f),
                rotation = Quaternion.identity
            }
        };

        indoorGameData = new GameData(indoorPotsAndPlants, new List<SprinklerData>(), indoorFertilizerList, indoorWateringCanList);

        List<PotData> outsidePotList = new List<PotData>() {
            new PotData { 
                position = new Vector3(-2.20f, 0.00f, -8.08f),
                rotation = Quaternion.identity,
                plantID = ""
            },
            new PotData { 
                position = new Vector3(-4.02f, 0.00f, -8.08f),
                rotation = Quaternion.identity,
                plantID = "ca5c832e-e979-49d4-b333-59528e746ccb"
            }
        };

        List<PlantData> outsidePlantList = new List<PlantData>() {
            new PlantData {
                position = new Vector3(-4.02f, 0.75f, -8.23f),
                rotation = Quaternion.identity,
                plantID = "ca5c832e-e979-49d4-b333-59528e746ccb",
                type = "Basic Plant",
                water = 20,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(1),
                timeMature = DateTime.Now.AddMinutes(2),
                isHalf = false,
                isMature = false,
                isDead = false
            }
        };

        PotsAndPlants outsidePotsAndPlants = new PotsAndPlants(outsidePotList, outsidePlantList);

        List<SprinklerData> outsideSprinklerList = new List<SprinklerData>() {
            new SprinklerData {
                position = new Vector3(4.70f, 0.08f, -5.30f),
                rotation = Quaternion.identity,
            }
        };

        outsideGameData = new GameData(outsidePotsAndPlants, outsideSprinklerList,  new List<FertilizerData>(), new List<WateringCanData>());
    }

    public Sprite GetSprite(string tag) {
        switch(tag) {
            case "Watering Can":
                return wateringCanSprite;
            case "Pot":
                return potSprite;
            case "Fertilizer":
                return fertilizerSprite;
            case "Sprinkler":
                return sprinklerSprite;
            default:
                return null;
        }
    }
    public GameObject GetPrefab(string tag) {
        switch(tag) {
            case "Watering Can":
                return wateringCanPrefab;
            case "Pot":
                return potPrefab;
            case "Fertilizer":
                return fertilizerPrefab;
            case "Sprinkler":
                return sprinklerPrefab;
            case "Basic Plant":
                return basicPlantPrefab;
            default:
                return null;
        }
    }

    public void SaveItems(List<GameObject> listOfItems) {
        savedHotBarItems = listOfItems;
    }

    public List<GameObject> GetItems() {
        return savedHotBarItems;
    }

    public void SaveIndoorGameData(GameData gameData){
        indoorGameData = gameData;
    }

    public GameData GetIndoorGameData() {
        return indoorGameData;
    }

    public void SaveOutsideGameData(GameData gameData){
        outsideGameData = gameData;
    }

    public GameData GetOutsideGameData() {
        return outsideGameData;
    }
}
