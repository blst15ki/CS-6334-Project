using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantData {
    public Vector3 position;
    public Quaternion rotation;
    public string plantID;
    public string type;
    public int water;
    public string stage;
    public DateTime timeHalf;
    public DateTime timeMature;
    public bool isHalf;
    public bool isMature;
    public bool isDead;
    public Vector3 scale;
    public Color color;

    public PlantData(Plant plant) {
        position = plant.transform.position;
        rotation = plant.transform.rotation;
        plantID = plant.id;
        type = plant.type;
        water = plant.GetWater();
        stage = plant.GetStage();
        timeHalf = plant.timeHalf;
        timeMature = plant.timeMature;
        isHalf = plant.isHalf;
        isMature = plant.isMature;
        isDead = plant.isDead;
        scale = plant.transform.localScale;
        color = plant.GetComponent<Renderer>().material.color;
    }

    public PlantData() {}

    public override string ToString() {
        return $"PlantData: ID={plantID}, Type={type}, Position={position}, Rotation={rotation.eulerAngles}, Water={water}, Stage={stage}, TimeHalf={timeHalf}, TimeMature={timeMature}, IsHalf={isHalf}, IsMature={isMature}, IsDead={isDead}, Scale={scale}, Color={color}";
    }
}

public class PotData {
    public Vector3 position;
    public Quaternion rotation;
    public string plantID;

    public PotData(GameObject pot) {
        position = pot.transform.position;
        rotation = pot.transform.rotation;
        plantID = pot.GetComponent<Pot>().PlantID;
    }

    public PotData() {}

    public override string ToString() {
        return $"PotData: PlantID={plantID}, Position={position}, Rotation={rotation.eulerAngles}";
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