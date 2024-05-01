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
    GetPlayerId playerIdScript;

    // Start is called before the first frame update
    void Start()
    {
        if(sign != null) {
            sign.SetActive(false); 
        } 

        playerIdScript = GameObject.FindObjectOfType<GetPlayerId>();
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

                if (currentScene != "Lobby") {
                    GameManager.Instance.SaveItems(hotbar.GetItems());
                } else {
                    GameManager.Instance.SaveItems(hotbar.getNonPhotonVeiwVersion());
                }
                
            }

            // if(PhotonNetwork.IsConnected != null && PhotonNetwork.IsConnected) {
            //     Debug.Log("Disconnecting");
            //     PhotonNetwork.Disconnect();
            //     Debug.Log("After Disconnecting");
            // }
            TryDisconnect();

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

    public void TryDisconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            TransferOwnershipToLocalPlayer();
            StartCoroutine(DisconnectAndLog());
        }
    }

    private void TransferOwnershipToLocalPlayer()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        int localPlayerId = playerIdScript.GetLocalPlayerID();
        Debug.Log(localPlayerId);
        foreach (PhotonView pv in photonViews)
        {
            if (pv.Owner != null && pv.OwnerActorNr != localPlayerId)
            {
                pv.TransferOwnership(localPlayerId);
            }
        }
    }

    private IEnumerator DisconnectAndLog() {
        Debug.Log("Before Disconnected from Photon Network");
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        Debug.Log("Disconnected from Photon Network");
    }
}
