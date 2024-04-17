using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Sprite wateringCanSprite;
    [SerializeField] Sprite potSprite;
    [SerializeField] Sprite fertilizerSprite;
    [SerializeField] Sprite sprinklerSprite;

    public List<GameObject> savedHotBarItems = new List<GameObject>();
    
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
}
