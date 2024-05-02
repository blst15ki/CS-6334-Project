using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerGameManager : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;

    void Start()
    {
        if (GameManager.Instance == null) {
            return;
        }

        LoadItemsIntoHotbar();
    }

    public void LoadItemsIntoHotbar()
    {
        // if (hotbar == null) {
        //     hotbar = FindObjectOfType<Hotbar>();
        // }
        Debug.Log("Inside Lobby loading items to hotbar");
        Debug.Log(hotbar.name);
        List<GameObject> savedItems = GameManager.Instance.GetItems();

        if (savedItems.Count != 0) {
            hotbar.LoadItems(savedItems);
        }
        
    }
}
