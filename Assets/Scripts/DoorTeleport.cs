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
    private string BInput;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(sign != null) {
            sign.SetActive(false); 
        } 
        BInput = "js5";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointerOn() { 
        if(sign != null) {
            sign.SetActive(true); 
            if (Input.GetButtonDown(BInput)){
            player = GameObject.FindGameObjectWithTag("Player");
            Hotbar hotbar = player.GetComponentInChildren<Hotbar>();

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
                GameManager.Instance.indoorSpawnPoint = "end";
            } else if (currentScene == "Lobby") {
                GameManager.Instance.indoorSpawnPoint = "front";
            }

            SceneManager.LoadScene(scene);
            }
        }
        
    }
    public void PointerOff() {
        if(sign != null) {
            sign.SetActive(false); 
        } 
    }
}
