using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleport : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] GameObject sign;

    // Start is called before the first frame update
    void Start()
    {
        sign.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointerOn() { sign.SetActive(true); }
    public void PointerOff() { sign.SetActive(false); }

    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag == "Player") {
            SceneManager.LoadScene(scene);
        }
    }
}
