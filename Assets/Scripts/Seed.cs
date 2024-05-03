using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [SerializeField] string plantType;
    public string GetPlantType() { return plantType; }
    public void SetPlantType(string ptype) {  plantType = ptype; }
}
