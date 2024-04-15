using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HotbarData {
    public string[] itemTags;
    // default objects (sprinkler, watering can, fertilizer are generic)
    // when saving a pot with a plant in it, how do you retain that data?
}

[Serializable]
public class PlantData {
    // public Vector3 position; //float[]
    public float[] position;
    // public Quaternion rotation; //float[]
    public float[] rotation;
    public string plantID;
    public string type;
    public int water;
    public string stage;
    public DateTime timeHalf; //covert to string and back
    public DateTime timeMature; //covert to string and back
    public bool isHalf;
    public bool isMature;
    public bool isDead;
    public PlantData (GameObject plant){
        position = new float[3];
        position[0]=plant.transform.position.x;
        position[1]=plant.transform.position.y;
        position[2]=plant.transform.position.z;
    }
}

public class PotData {
    // public Vector3 position;
    public float[] position;
    // public Quaternion rotation;
    public float[] rotation;
    public string plantID;
    public PotData (GameObject pot){
        position = new float[3];
        position[0]=pot.transform.position.x;
        position[1]=pot.transform.position.y;
        position[2]=pot.transform.position.z;
    }
}

public class PotsAndPlants {

    public List<PotData> listOfPots = new List<PotData>();
    public List<PlantData> listOfPlants = new List<PlantData>();
    public PotsAndPlants ( List<PotData>listofPots, List<PlantData>listofPlants){
        this.listOfPlants = listofPlants;
        this.listOfPots = listofPots;
    }
}

[Serializable]
public class GameData {
    public HotbarData inventory;
    public PotsAndPlants potsAndPlants;
    public int money;
}