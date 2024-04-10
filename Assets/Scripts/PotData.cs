using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotData
{
    public float[] position;
    public PotData (GameObject pot){
        position = new float[3];
        position[0]=pot.transform.position.x;
        position[1]=pot.transform.position.y;
        position[2]=pot.transform.position.z;
    }

}
