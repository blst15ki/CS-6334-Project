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
    [SerializeField] Sprite basicPlantSprite;
    [SerializeField] Sprite fernSprite;
    [SerializeField] Sprite grassSprite;
    [SerializeField] Sprite mintSprite;
    [SerializeField] GameObject wateringCanPrefab;
    [SerializeField] GameObject potPrefab;
    [SerializeField] GameObject fertilizerPrefab;
    [SerializeField] GameObject sprinklerPrefab;
    [SerializeField] GameObject basicPlantPrefab;
    [SerializeField] GameObject fernPrefab;
    [SerializeField] GameObject grassPrefab;
    [SerializeField] GameObject mintPrefab;
    [SerializeField] GameObject basicPlantSeedPrefab;
    [SerializeField] GameObject fernSeedPrefab;
    [SerializeField] GameObject sprinklerParticlePrefab;
    public List<GameObject> savedHotBarItems = new List<GameObject>();
    public GameData outsideGameData = null;
    public GameData indoorGameData = null;
    public string indoorSpawnPoint = "end";
    public int avatarValue = 0;
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
        int potSizeIn = 5;
        string[] potIDin = new string[potSizeIn];
        for(int i = 0; i < potSizeIn; i++) {
            potIDin[i] = Guid.NewGuid().ToString();
        }
        int plantSizeIn = 3;
        string[] plantIDin = new string[plantSizeIn];
        for(int i = 0; i < plantSizeIn; i++) {
            plantIDin[i] = Guid.NewGuid().ToString();
        }

        List<PotData> indoorPotList = new List<PotData>() {
            new PotData { 
                position = new Vector3(-1.50f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = "",
                potID = potIDin[0]
            },
            new PotData { 
                position = new Vector3(-0.75f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = plantIDin[0],
                potID = potIDin[1]
            },
            new PotData { 
                position = new Vector3(0.00f, 0.00f, -4.21f),
                rotation = Quaternion.identity,
                plantID = "",
                potID = potIDin[2]
            },
            new PotData { 
                position = new Vector3(-9.3f, 0f, 4f),
                rotation = Quaternion.identity,
                plantID = plantIDin[1],
                potID = potIDin[3]
            },
            new PotData { 
                position = new Vector3(-9.3f, 0f, 3f),
                rotation = Quaternion.identity,
                plantID = plantIDin[2],
                potID = potIDin[4]
            }
        };

        List<PlantData> indoorPlantList = new List<PlantData>() {
            new PlantData {
                position = new Vector3(-0.75f, 0.6f, -4.36f),
                rotation = Quaternion.identity,
                plantID = plantIDin[0],
                potID = potIDin[1],
                type = "Fern",
                water = 20,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(2.25f),
                timeMature = DateTime.Now.AddMinutes(4.5f),
                isHalf = false,
                isMature = false,
                scale = new Vector3(100f, 100f, 100f),
            },
            new PlantData {
                position = new Vector3(-9.3f, 0.6f, 3.85f),
                rotation = Quaternion.identity,
                plantID = plantIDin[1],
                potID = potIDin[3],
                type = "Grass",
                water = 25,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(2.5f),
                timeMature = DateTime.Now.AddMinutes(5f),
                isHalf = false,
                isMature = false,
                scale = new Vector3(20f, 20f, 20f),
            },
            new PlantData {
                position = new Vector3(-9.3f, 0.6f, 2.85f),
                rotation = Quaternion.identity,
                plantID = plantIDin[2],
                potID = potIDin[4],
                type = "Mint",
                water = 25,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(3f),
                timeMature = DateTime.Now.AddMinutes(6f),
                isHalf = false,
                isMature = false,
                scale = new Vector3(100f, 100f, 100f),
            }
        };

        PotsAndPlants indoorPotsAndPlants = new PotsAndPlants(indoorPotList, indoorPlantList);

        List<FertilizerData> indoorFertilizerList = new List<FertilizerData>() {
            new FertilizerData {
                position = new Vector3(8.2f, 0.25f, 4.35f),
                rotation = Quaternion.identity
            }
        };

        List<WateringCanData> indoorWateringCanList = new List<WateringCanData>() {
            new WateringCanData {
                position = new Vector3(3f, 0.33f, -3.66f),
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

        int potSizeOut = 2;
        string[] potIDout = new string[potSizeOut];
        for(int i = 0; i < potSizeOut; i++) {
            potIDout[i] = Guid.NewGuid().ToString();
        }

        int plantSizeOut = 1;
        string[] plantIDout = new string[plantSizeOut];
        for(int i = 0; i < plantSizeOut; i++) {
            plantIDout[i] = Guid.NewGuid().ToString();
        }
        List<PotData> outsidePotList = new List<PotData>() {
            new PotData { 
                position = new Vector3(-2.20f, 0.00f, -8.08f),
                rotation = Quaternion.identity,
                plantID = "",
                potID = potIDout[0]
            },
            new PotData { 
                position = new Vector3(-4.02f, 0.00f, -8.08f),
                rotation = Quaternion.identity,
                plantID = plantIDout[0],
                potID = potIDout[1]
            }
        };

        List<PlantData> outsidePlantList = new List<PlantData>() {
            new PlantData {
                position = new Vector3(-4.02f, 0.75f, -8.23f),
                rotation = Quaternion.identity,
                plantID = plantIDout[0],
                potID = potIDout[1],
                type = "Basic Plant",
                water = 25,
                stage = "Seedling",
                timeHalf = DateTime.Now.AddMinutes(2.75f),
                timeMature = DateTime.Now.AddMinutes(5.5f),
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
            case "Basic Plant":
                return basicPlantSprite;
            case "Fern":
                return fernSprite;
            case "Grass":
                return grassSprite;
            case "Mint":
                return mintSprite;
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
            case "Grass":
                return grassPrefab;
            case "Mint":
                return mintPrefab;
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
