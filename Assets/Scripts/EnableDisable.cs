using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    private Transform selection;
    public void OnPointerEnter(GameObject obj)
    {
        obj.GetComponent<Outline>().enabled = true;
    }
    public void OnPointerExit(GameObject obj)
    {
        obj.GetComponent<Outline>().enabled = false;
    }
}
