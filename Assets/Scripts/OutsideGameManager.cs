using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideGameManager : MonoBehaviour
{
    [SerializeField] Hotbar hotbar;

    void Start()
    {
        if (GameManager.Instance == null) {
            return;
        }

        LoadItemsIntoHotbar();
    }

    private void LoadItemsIntoHotbar()
    {
        List<GameObject> savedItems = GameManager.Instance.GetItems();
        hotbar.LoadItems(savedItems);
    }
}
