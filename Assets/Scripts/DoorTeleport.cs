using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField] string currentScene;
    [SerializeField] string scene;
    [SerializeField] GameObject sign;

    // Start is called before the first frame update
    void Start()
    {
        if(sign != null) {
            sign.SetActive(false); 
        } 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointerOn() { 
        if(sign != null) {
            sign.SetActive(true); 
        }
        
    }
    public void PointerOff() {
        if(sign != null) {
            sign.SetActive(false); 
        } 
    }

    // teleport player
    void OnTriggerEnter(Collider collider) {

        if(collider.gameObject.tag == "Player") {

            Hotbar hotbar = collider.gameObject.GetComponentInChildren<Hotbar>();

            if (hotbar != null) {
                List<GameObject> items = hotbar.GetItems();
                GameManager.Instance.SaveItems(items);
            }

            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.Disconnect();
            }

            if(currentScene == "Inside"){
                IndoorGameManager indoorGameManager = FindObjectOfType<IndoorGameManager>();
                GameManager.Instance.SaveIndoorGameData(indoorGameManager.retrieveObjects());
            } else if (currentScene == "Outdoor") {
                OutsideGameManager outsideGameManager = FindObjectOfType<OutsideGameManager>();
                GameManager.Instance.SaveOutsideGameData(outsideGameManager.retrieveObjects());
            } else if (currentScene == "Lobby") {

            }

            SceneManager.LoadScene(scene);
        }
    }
}
