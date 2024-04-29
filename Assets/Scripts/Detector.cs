using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public XRCardboardReticle reticle;
    public float maxDistance = 20f;
    private GameObject currentGameObject = null;
    private Outline lastOutline = null;
	private DoorTeleport lastDoorTeleport = null;
	private Interact lastInteract = null;
	private LightController lastLightController = null;
	private Pot lastPot = null;
	private Sprinkler lastSprinkler = null;
	private PlantInterface lastPlantInterface = null;
	private RadioManager lastRadioManager = null;

    void Update()
    {
        if (lastOutline != null) {
            lastOutline.enabled = false;
            lastOutline = null;
        }

		if (lastDoorTeleport != null) {
			lastDoorTeleport.PointerOff();
			lastDoorTeleport = null;
		}

		if (lastInteract != null) {
			lastInteract.PointerOff();
			lastInteract = null;
		}

		if (lastLightController != null) {
			lastLightController.PointerOff();
			lastLightController = null;
		}

		if (lastPot != null) {
			lastPot.PointerOff();
			lastPot = null;
		}

		if (lastSprinkler != null) {
			lastSprinkler.PointerOff();
			lastSprinkler = null;
		}

		if (lastPlantInterface != null) {
			lastPlantInterface.DisableInterface();
			lastPlantInterface = null;
		}

		if (lastRadioManager != null) {
			lastRadioManager.PointerOff();
			lastRadioManager = null;
		}

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, maxDistance)) {
            GameObject hitObject = hit.collider.gameObject;

            Outline outline = hitObject.GetComponent<Outline>();
            if (outline != null) {
                outline.enabled = true;
                lastOutline = outline;

                reticle.OnStartHover(1f);

				DoorTeleport doorTeleport = hitObject.GetComponent<DoorTeleport>();
				if (doorTeleport != null) {
					doorTeleport.PointerOn();
					lastDoorTeleport = doorTeleport;
				}

				Interact interact = hitObject.GetComponent<Interact>();
				if (interact != null) {
					interact.PointerOn();
					lastInteract = interact;
				}

				LightController lightController = hitObject.GetComponent<LightController>();
				if (lightController != null) {
					lightController.PointerOn();
					lastLightController = lightController;
				}

				Pot pot = hitObject.GetComponent<Pot>();
				if (pot != null) {
					pot.PointerOn();
					lastPot = pot;
				}

				Sprinkler sprinkler = hitObject.GetComponent<Sprinkler>();
				if (sprinkler != null) {
					sprinkler.PointerOn();
					lastSprinkler = sprinkler;
				}
				
				PlantInterface plantInterface = FindObjectOfType<PlantInterface>();
				BasicPlant basicPlant = hitObject.GetComponent<BasicPlant>();
				if (basicPlant != null) {
					plantInterface.EnableInterface(hitObject);
					lastPlantInterface = plantInterface;
				}

				RadioManager radioManager = hitObject.GetComponent<RadioManager>();
				if(radioManager != null) {
					radioManager.PointerOn();
					lastRadioManager = radioManager;
				}

            }
            else {
                reticle.OnEndHover();
            }
        }
        else {
            reticle.OnEndHover();
        }
    }
}