using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantInterface : MonoBehaviour
{
    [SerializeField] GameObject plantInterfaceObj;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject waterLevel;
    [SerializeField] TextMeshProUGUI stageTMP, plantTypeTMP, hasLightTMP, growTimeTMP;
    Transform PIOtransform;
    RectTransform WLrt;
    Image WLimage;
    Plant plant;
    // Start is called before the first frame update
    void Start()
    {
        PIOtransform = plantInterfaceObj.transform;
        WLrt = waterLevel.GetComponent<RectTransform>();
        WLimage = waterLevel.GetComponent<Image>();

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
        float amount = (float)plant.GetWater() / plant.GetMaxWater();
        WLrt.sizeDelta = new Vector2(amount * 160, 20);
        if(amount < 0.2 || amount > 0.8) {
            WLimage.color = Color.red;
        } else {
            WLimage.color = new Color(0f, 189f / 255, 255f);
        }

        stageTMP.text = "Stage: " + plant.GetStage();
        plantTypeTMP.text = plant.GetPlantType();

        if(plant.HasLight()) {
            hasLightTMP.text = "Yes";
            hasLightTMP.color = Color.green;
        } else {
            hasLightTMP.text = "No";
            hasLightTMP.color = Color.red;
        }

        growTimeTMP.text = "Time to Next Stage: " + plant.GetTime() + " sec";
    }
}
