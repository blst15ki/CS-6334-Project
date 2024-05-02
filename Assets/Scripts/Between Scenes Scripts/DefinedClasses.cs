using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantData {
    public Vector3 position;
    public Quaternion rotation;
    public string plantID;
    public string potID;
    public string type;
    public int water;
    public string stage;
    public DateTime timeHalf;
    public DateTime timeMature;
    public DateTime timeLeave;
    public bool delay;
    public bool isHalf;
    public bool isMature;
    public Vector3 scale;

    public PlantData(GameObject plant) {
        Plant plantScript = plant.GetComponent<Plant>();
        position = plant.transform.position;
        rotation = plant.transform.rotation;
        plantID = plantScript.id;
        potID = plantScript.GetPotID();
        type = plantScript.type;
        water = plantScript.GetWater();
        stage = plantScript.GetStage();
        timeHalf = plantScript.timeHalf;
        timeMature = plantScript.timeMature;
        timeLeave = DateTime.Now;
        delay = plantScript.delay;
        isHalf = plantScript.isHalf;
        isMature = plantScript.isMature;
        scale = plant.transform.localScale;
    }

    public PlantData() {}

    public override string ToString() {
        return $"PlantData: ID={plantID}, PotID={potID}, Type={type}, Position={position}, Rotation={rotation.eulerAngles}, Water={water}, Stage={stage}, TimeHalf={timeHalf}, TimeMature={timeMature}, TimeLeave={timeLeave}, Delay={delay}, IsHalf={isHalf}, IsMature={isMature}, Scale={scale}";
    }
}

public class PotData {
    public Vector3 position;
    public Quaternion rotation;
    public string plantID;
    public string potID;

    public PotData(GameObject pot) {
        Pot potScript = pot.GetComponent<Pot>();
        position = pot.transform.position;
        rotation = pot.transform.rotation;
        plantID = potScript.GetPlantID();
        potID = potScript.id;
    }

    public PotData() {}

    public override string ToString() {
        return $"PotData: PlantID={plantID}, PotID={potID}, Position={position}, Rotation={rotation.eulerAngles}";
    }
}

public class PotsAndPlants {
    public List<PotData> listOfPots = new List<PotData>();
    public List<PlantData> listOfPlants = new List<PlantData>();

    public PotsAndPlants(List<PotData> pots, List<PlantData> plants) {
        listOfPots = pots;
        listOfPlants = plants;
    }
}

public class SprinklerData {
    public Vector3 position;
    public Quaternion rotation;

    public SprinklerData(GameObject sprinkler) {
        position = sprinkler.transform.position;
        rotation = sprinkler.transform.rotation;
    }

    public SprinklerData() {}

    public override string ToString() {
        return $"SprinklerData: Position={position}, Rotation={rotation.eulerAngles}";
    }
}

public class FertilizerData {
    public Vector3 position;
    public Quaternion rotation;

    public FertilizerData(GameObject fertilizer) {
        position = fertilizer.transform.position;
        rotation = fertilizer.transform.rotation;
    }

    public FertilizerData() {}

    public override string ToString() {
        return $"FertilizerData: Position={position}, Rotation={rotation.eulerAngles}";
    }
}

public class WateringCanData {
    public Vector3 position;
    public Quaternion rotation;

    public WateringCanData(GameObject wateringCan) {
        position = wateringCan.transform.position;
        rotation = wateringCan.transform.rotation;
    }

    public WateringCanData() {}

    public override string ToString() {
        return $"WateringCanData: Position={position}, Rotation={rotation.eulerAngles}";
    }
}

public class ChestData {
    public string chestName;
    public DateTime unlockTime;

    public ChestData(GameObject chest) {
        chestName = chest.name;
        unlockTime = chest.GetComponent<Chest>().GetUnlockTime();
    }

    public ChestData() {}

    public override string ToString() {
        return $"ChestData: ChestName={chestName}, UnlockTime={unlockTime}";
    }
}

public class GameData {
    public PotsAndPlants potsAndPlants;
    public List<SprinklerData> listOfSprinkler;
    public List<FertilizerData> listOfFertilizer;
    public List<WateringCanData> listOfWateringCan;
    public List<ChestData> listOfChestData;

    public GameData(PotsAndPlants pandp, List<SprinklerData> sprinklers, List<FertilizerData> fertilizers, List<WateringCanData> wateringcans, List<ChestData> chests) {
        potsAndPlants = pandp;
        listOfSprinkler = sprinklers;
        listOfFertilizer = fertilizers;
        listOfWateringCan = wateringcans;
        listOfChestData = chests;
    }
}

public class HotBarItem {
    public string type;
    public bool hasPlant = false;
    public string plantType;
    public int water;
    public string stage;
    public DateTime timeHalf;
    public DateTime timeMature;
    public DateTime timeLeave;
    public bool delay;
    public bool isHalf;
    public bool isMature;
    public Vector3 scale;

    public HotBarItem(GameObject obj){

        if(obj == null) {
            type = "None";
            return;
        }
        
        switch (obj.tag) {
            case "Fertilizer":
                type = "Fertilizer";
                break;
            case "Watering Can":
                type = "Watering Can";
                break;
            case "Sprinkler":
                type = "Sprinkler";
                break;
            case "Pot":
                type = "Pot";
                Pot pot = obj.GetComponent<Pot>();
                hasPlant = pot.HasPlant();
                if(hasPlant) {
                    Plant plant = pot.GetPlant().GetComponent<Plant>();
                    plantType = plant.GetPlantType();
                    water = plant.GetWater();
                    stage = plant.GetStage();
                    timeHalf = plant.timeHalf;
                    timeMature = plant.timeMature;
                    timeLeave = DateTime.Now;
                    delay = plant.delay;
                    isHalf = plant.isHalf;
                    isMature = plant.isMature;
                    scale = pot.GetPlant().transform.localScale;
                }

                break;
            default:
                type = "Sprinkler";
                break;
        }
    }
}

public class LobbyHotBarData {
    public List<HotBarItem> listOfHotBarItem;

    public LobbyHotBarData(List<HotBarItem> hotBarItems) {
        listOfHotBarItem = hotBarItems;
    }
}