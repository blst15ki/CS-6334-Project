using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Detector : MonoBehaviour
{
    public XRCardboardReticle reticle;
	public Hotbar hotbar;
    public float maxDistance = 20f;
    [SerializeField] PlantInterface plantInterface;
    private Outline lastOutline = null;
	private DoorTeleport lastDoorTeleport = null;
	private LobbyDoorTeleport lastLobbyDoorTeleport = null;
	private Interact lastInteract = null;
	private LightController lastLightController = null;
	private Pot lastPot = null;
	private Sprinkler lastSprinkler = null;
	private PlantInterface lastPlantInterface = null;
	private RadioManager lastRadioManager = null;
	private Chest lastChest = null;
	private GameObject lastHitObject = null;
	public GameObject player = null;

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, maxDistance)) {
            GameObject hitObject = hit.collider.gameObject;

			if(lastHitObject != hitObject) {
				DisableLast();
			}

			lastHitObject = hitObject;

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

				LobbyDoorTeleport lobbyDoorTeleport = hitObject.GetComponent<LobbyDoorTeleport>();
				if (lobbyDoorTeleport != null) {
					lobbyDoorTeleport.PointerOn();
					lobbyDoorTeleport.player = player;
					lastLobbyDoorTeleport = lobbyDoorTeleport;
				}

				Interact interact = hitObject.GetComponent<Interact>();
				if (interact != null) {
					interact.PointerOn();
					lastInteract = interact;
					interact.hotbar = hotbar;
					
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
					pot.hotbar = hotbar;

					if(pot.HasPlant()) {
						plantInterface.EnableInterface(pot.GetPlant());
						lastPlantInterface = plantInterface;
					}
				}

				Sprinkler sprinkler = hitObject.GetComponent<Sprinkler>();
				if (sprinkler != null) {
					sprinkler.PointerOn();
					lastSprinkler = sprinkler;
					sprinkler.hotbar = hotbar;

				}
				
				Plant plant = hitObject.GetComponent<Plant>();
				if (plant != null) {
					plantInterface.EnableInterface(hitObject);
					lastPlantInterface = plantInterface;
				}

				RadioManager radioManager = hitObject.GetComponent<RadioManager>();
				if(radioManager != null) {
					radioManager.PointerOn();
					lastRadioManager = radioManager;
				}

				Chest chest = hitObject.GetComponent<Chest>();
				if(chest != null) {
					chest.PointerOn();
					lastChest = chest;
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
	
	void DisableLast() {
		if (lastOutline != null) {
            lastOutline.enabled = false;
            lastOutline = null;
        }

		if (lastDoorTeleport != null) {
			lastDoorTeleport.PointerOff();
			lastDoorTeleport = null;
		}

		if (lastLobbyDoorTeleport != null) {
			lastLobbyDoorTeleport.PointerOff();
			lastLobbyDoorTeleport = null;
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
		
		if(lastChest != null) {
			lastChest.PointerOff();
			lastChest = null;
		}
	}
}