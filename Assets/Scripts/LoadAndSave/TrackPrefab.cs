using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPrefab : MonoBehaviour
{
    public GameObject wateringCanPrefab;
    public GameObject potPrefab;
    public GameObject fertilizerPrefab;
    public GameObject sprinklerPrefab;
    public GameObject plantPrefab;

    public GameObject GetPrefabByTag(string tag) {
        switch (tag) {
            case "Watering Can":
                return wateringCanPrefab;
            case "Pot":
                return potPrefab;
            case "Fertilizer":
                return fertilizerPrefab;
            case "Sprinkler":
                return sprinklerPrefab;
            case "Plant":
                return sprinklerPrefab;
            default:
                Debug.LogError("Prefab not found for tag: " + tag);
                return null;
        }
    }
}
