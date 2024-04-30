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
    public bool isHalf;
    public bool isMature;
    public Vector3 scale;
    public Color color;

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
        isHalf = plantScript.isHalf;
        isMature = plantScript.isMature;
        scale = plant.transform.localScale;
    }

    public PlantData() {}

    public override string ToString() {
        return $"PlantData: ID={plantID}, PotID={potID}, Type={type}, Position={position}, Rotation={rotation.eulerAngles}, Water={water}, Stage={stage}, TimeHalf={timeHalf}, TimeMature={timeMature}, IsHalf={isHalf}, IsMature={isMature}, Scale={scale}";
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

    public WateringCanData(GameObject fertilizer) {
        position = fertilizer.transform.position;
        rotation = fertilizer.transform.rotation;
    }

    public WateringCanData() {}

    public override string ToString() {
        return $"WateringCanData: Position={position}, Rotation={rotation.eulerAngles}";
    }
}

public class GameData {
    public PotsAndPlants potsAndPlants;
    public List<SprinklerData> listOfSprinkler;
    public List<FertilizerData> listOfFertilizer;
    public List<WateringCanData> listOfWateringCan;

    public GameData(PotsAndPlants pandp, List<SprinklerData> sprinklers, List<FertilizerData> fertilizers, List<WateringCanData> wateringcans) {
        potsAndPlants = pandp;
        listOfSprinkler = sprinklers;
        listOfFertilizer = fertilizers;
        listOfWateringCan = wateringcans;
    }
}