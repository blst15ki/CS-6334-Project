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

        List<GameObject> savedItems = GameManager.Instance.GetItems();

        if (savedItems.Count != 0) {
            hotbar.LobbyLoadItems(savedItems);
        }
        
    }
}
