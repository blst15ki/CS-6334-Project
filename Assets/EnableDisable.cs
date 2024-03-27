using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnableDisable : MonoBehaviour
{
    // Start is called before the first frame update        
    // [SerializeField] private string selectableTag = "Selectable"; // if works rename
    private Transform selection;
    public void OnPointerEnter(GameObject obj)
    {
        // Debug.Log("OnPointerEnter");
        obj.GetComponent<Outline>().enabled = true;
    }
    public void OnPointerExit(GameObject obj)
    {
        // Debug.Log("OnPointerExit");
        obj.GetComponent<Outline>().enabled = false;
    }
}