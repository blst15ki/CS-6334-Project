using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HotbarData {
    public string[] itemTags;
}

[Serializable]
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
}

public class PotData {
    public Vector3 position;
    public Quaternion rotation;
    public string plantID;
}

public class PotsAndPlants {
    public List<PotData> listOfPots = new List<PotData>();
    public List<PlantData> listOfPlants = new List<PlantData>();
}

[Serializable]
public class GameData {
    public HotbarData inventory;
    public PotsAndPlants potsAndPlants;
}