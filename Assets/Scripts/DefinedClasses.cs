// using System;
// using System.Collections.Generic;
// using UnityEngine;

// [Serializable]
// public class PlantData {
//     public Vector3 position;
//     public Vector3 rotation;
//     public string plantID;
//     public string type;
//     public int water;
//     public string stage;
//     public DateTime timeHalf;
//     public DateTime timeMature;
//     public bool isHalf;
//     public bool isMature;
//     public bool isDead;
//     public PlantData (GameObject plant){
//         this.position = plant.transform.position;
//         t
//     }
// }

// public class PotData {
//     public Vector3 position;
//     public Vector3 rotation;
//     public string plantID;
//     public PotData (GameObject pot){
//         position = new float[3];
//         position[0]=pot.transform.position.x;
//         position[1]=pot.transform.position.y;
//         position[2]=pot.transform.position.z;
//     }
// }

// public class PotsAndPlants {

//     public List<PotData> listOfPots = new List<PotData>();
//     public List<PlantData> listOfPlants = new List<PlantData>();
//     public PotsAndPlants ( List<PotData>listofPots, List<PlantData>listofPlants){
//         this.listOfPlants = listofPlants;
//         this.listOfPots = listofPots;
//     }
// }

// [Serializable]
// public class GameData {
//     public PotsAndPlants potsAndPlants;
// }