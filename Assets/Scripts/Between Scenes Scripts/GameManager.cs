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
    [SerializeField] GameObject fernPrefab;
    [SerializeField] GameObject basicPlantSeedPrefab;
    [SerializeField] GameObject fernSeedPrefab;
    [SerializeField] GameObject sprinklerParticlePrefab;
    public List<GameObject> savedHotBarItems = new List<GameObject>();
    public GameData outsideGameData = null;
    public GameData indoorGameData = null;
    public string indoorSpawnPoint = "end";
    public LobbyHotBarData lobbyHotBarData;
    public InsideHotBarData insideHotBarData = null;
    public static Dictionary<string, GameObject> pots = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> plants = new Dictionary<string, GameObject>();
    
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
                plantID = "",
                potID = "be2eb360-320e-48d3-b91f-f760b7e2f035"
            },
            new PotData { 
                position = new Vector3(-0.75f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = "93ab33b5-05dc-4559-952a-401ff7553ef6",
                potID = "981603f4-2d5a-47ca-af5b-d7fe8d7e1c2d"
            },
            new PotData { 
                position = new Vector3(0.00f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = "",
                potID = "9d9bb88b-23bc-441c-a253-931a9fd01469"
            }
        };

        List<PlantData> indoorPlantList = new List<PlantData>() {
            new PlantData {
                position = new Vector3(-0.75f, 0.6f, -4.36f),
                rotation = Quaternion.identity,
                plantID = "93ab33b5-05dc-4559-952a-401ff7553ef6",
                potID = "981603f4-2d5a-47ca-af5b-d7fe8d7e1c2d",
                type = "Fern",
                water = 30,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(1.5f),
                timeMature = DateTime.Now.AddMinutes(3f),
                isHalf = false,
                isMature = false,
                scale = new Vector3(100f, 100f, 100f),
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
                position = new Vector3(0.0f, 0.33f, 0.0f),
                rotation = Quaternion.identity
            }
        };

        List<ChestData> chestList = new List<ChestData>() {
            new ChestData{
                chestName = "Seed Chest",
                unlockTime = DateTime.Now
            },
            new ChestData{
                chestName = "Item Chest",
                unlockTime = DateTime.Now
            }
        };

        indoorGameData = new GameData(indoorPotsAndPlants, new List<SprinklerData>(), indoorFertilizerList, indoorWateringCanList, chestList);

        List<PotData> outsidePotList = new List<PotData>() {
            new PotData { 
                position = new Vector3(-2.20f, 0.00f, -8.08f),
                rotation = Quaternion.identity,
                plantID = "",
                potID = "0774b5a1-9c99-465e-9582-58aa43b7fc1c"
            },
            new PotData { 
                position = new Vector3(-4.02f, 0.00f, -8.08f),
                rotation = Quaternion.identity,
                plantID = "ca5c832e-e979-49d4-b333-59528e746ccb",
                potID = "d2d5baca-e619-473e-a82e-5f58d9fb8de5"
            }
        };

        List<PlantData> outsidePlantList = new List<PlantData>() {
            new PlantData {
                position = new Vector3(-4.02f, 0.75f, -8.23f),
                rotation = Quaternion.identity,
                plantID = "ca5c832e-e979-49d4-b333-59528e746ccb",
                potID = "d2d5baca-e619-473e-a82e-5f58d9fb8de5",
                type = "Basic Plant",
                water = 30,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(2f),
                timeMature = DateTime.Now.AddMinutes(2f),
                isHalf = false,
                isMature = false,
                scale = new Vector3(0.15f, 0.15f, 0.15f),
            }
        };

        PotsAndPlants outsidePotsAndPlants = new PotsAndPlants(outsidePotList, outsidePlantList);

        List<SprinklerData> outsideSprinklerList = new List<SprinklerData>() {
            new SprinklerData {
                position = new Vector3(11f, 0.1f, -6.5f),
                rotation = Quaternion.identity,
            }
        };

        outsideGameData = new GameData(outsidePotsAndPlants, outsideSprinklerList,  new List<FertilizerData>(), new List<WateringCanData>(), new List<ChestData>());
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
            case "Fern":
                return fernPrefab;
            case "Basic Plant Seed":
                return basicPlantSeedPrefab;
            case "Fern Seed":
                return fernSeedPrefab;
            case "Sprinkler Particle":
                return sprinklerParticlePrefab;
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

    public void SaveLobbyHotBarData(LobbyHotBarData hotBarData){
        lobbyHotBarData = hotBarData;
    }

    public LobbyHotBarData GetLobbyHotBarData(){
        return lobbyHotBarData;
    }

    public void SaveInsideHotBarData(InsideHotBarData hotBarData){
        insideHotBarData = hotBarData;
    }

    public InsideHotBarData GetInsideHotBarData(){
        return insideHotBarData;
    }

    public bool isInsideHotBarDataNull(){
        return (insideHotBarData == null);
    }

    public void resetInsideHotBarDataToNull(){
        insideHotBarData = null;
    }

    public GameObject FindPotByID(string id) {
        if (pots.ContainsKey(id)) {
            return pots[id];
        }
        return null;
    }

    public void RegisterPot(string id, GameObject obj) {
        if (!pots.ContainsKey(id)) {
            pots.Add(id, obj);
        }
    }

    public void ResetPots(){
        pots = new Dictionary<string, GameObject>();
    }

    public GameObject FindPlantByID(string id) {
        if (plants.ContainsKey(id)) {
            return plants[id];
        }
        return null;
    }

    public void RegisterPlant(string id, GameObject obj) {
        if (!plants.ContainsKey(id)) {
            plants.Add(id, obj);
        }
    }

    public void ResetPlants(){
        pots = new Dictionary<string, GameObject>();
    }
}
