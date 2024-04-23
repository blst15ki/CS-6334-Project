using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantSeed : MonoBehaviour
{
    EventTrigger eventT;
    EventTrigger.Entry entry;
    PlantInterface plantInterface;
    EnableDisable enableDisable;

    // Start is called before the first frame update
    void Start()
    {
        eventT = gameObject.AddComponent<EventTrigger>();

        // add OnPointerEnter
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        eventT.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        eventT.triggers.Add(entry);
        
        plantInterface = GameObject.FindWithTag("Plant Interface").GetComponent<PlantInterface>();
        enableDisable = GameObject.Find("EnableDisable").GetComponent<EnableDisable>();
    }

    public void OnPointerEnterDelegate(PointerEventData data) {
        plantInterface.EnableInterface(gameObject);
        enableDisable.OnPointerEnter(gameObject);
    }

    public void OnPointerExitDelegate(PointerEventData data) {
        plantInterface.DisableInterface();
        enableDisable.OnPointerExit(gameObject);
    }
}
