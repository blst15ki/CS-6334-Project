using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantInterface : MonoBehaviour
{
    [SerializeField] GameObject plantInterfaceObj;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject waterLevel;
    [SerializeField] GameObject stageText;
    TextMeshProUGUI stageTMP;
    Transform PIOtransform;
    RectTransform WLrt;
    Plant plant;
    // Start is called before the first frame update
    void Start()
    {
        stageTMP = stageText.GetComponent<TextMeshProUGUI>();
        PIOtransform = plantInterfaceObj.transform;
        WLrt = waterLevel.GetComponent<RectTransform>();

        plantInterfaceObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(plantInterfaceObj.activeSelf) {
            UpdateInterface();
        }
    }

    public void EnableInterface(GameObject plantObj) {
        // move interface in front of and above object and face player
        PIOtransform.position = new Vector3(plantObj.transform.position.x, mainCamera.transform.position.y, plantObj.transform.position.z);
        PIOtransform.LookAt(mainCamera.transform);
        PIOtransform.position += PIOtransform.forward * 0.5f;
        PIOtransform.eulerAngles += new Vector3(0f, 180f, 0f);

        // update interface with plant information
        plant = plantObj.GetComponent<Plant>();
        UpdateInterface();

        plantInterfaceObj.SetActive(true);
    }
    public void DisableInterface() { plantInterfaceObj.SetActive(false); }
    
    void UpdateInterface() {
        WLrt.sizeDelta = new Vector2((float)plant.GetWater() / plant.GetMaxWater() * 120, 20);
        stageTMP.text = "Stage: " + plant.GetStage();
    }
}
