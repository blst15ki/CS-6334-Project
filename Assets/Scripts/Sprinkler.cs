using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    public Hotbar hotbar;
    Outline outline;
    bool pointer, on;
    string AInput, XInput;
    int plantLayer = 1 << 6; // plants
    AudioSource waterSound;
    [SerializeField] GameObject spsPrefab;
    GameObject spsObj;
    ParticleSystem sprinklerParticleSystem;

    void Start() {
        // if(hotbar == null) {
        //     hotbar = FindObjectOfType<Hotbar>();
        // }
    }

    void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        pointer = false;
        on = false;
        AInput = "js10";
        XInput = "js2";

        if(spsPrefab == null){
            spsPrefab = GameManager.Instance.GetPrefab("Sprinkler Particle");
        }

        waterSound = GetComponent<AudioSource>();

        if(spsObj == null) {
            spsObj = Instantiate(spsPrefab, transform.position + new Vector3(0f, 0.5f, 0f), spsPrefab.transform.rotation);
            sprinklerParticleSystem = spsObj.GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        // if(hotbar == null) {
        //     hotbar = FindObjectOfType<Hotbar>();
        // }
        if(spsObj == null) {
            spsObj = Instantiate(spsPrefab, transform.position + new Vector3(0f, 0.5f, 0f), spsPrefab.transform.rotation);
            sprinklerParticleSystem = spsObj.GetComponent<ParticleSystem>();
        }
        
        if(pointer) {
            if(Input.GetButtonDown(AInput)) { // select object to place
                if(hotbar.SelectObject(gameObject)) {
                    PointerOff();
                    TurnOff();
                }
            } else if(Input.GetButtonDown(XInput)) { // toggle sprinkler
                if(!on) {
                    TurnOn();
                }
            }
        }
    }

    public void PointerOn() { pointer = true; hotbar.DisableHotbar(); }
    public void PointerOff() { pointer = false; hotbar.EnableHotbar(); }

    void TurnOn() {
        on = true;

        // get all plants within 5 unit range (sphere radius) and water for 5 seconds
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 5f, plantLayer);
        foreach(Collider collider in colliders) {
            Plant plantScript = collider.gameObject.GetComponent<Plant>();
            plantScript.GiveWater();
            plantScript.StopWaterTimed(5f);
        }
        
        // only play sound if watering plants
        if(colliders.Length > 0) {
            outline.OutlineColor = Color.cyan;
            waterSound.Play();
            sprinklerParticleSystem.Play();
            Invoke("TurnOff", 5f);
        } else {
            outline.OutlineColor = Color.red;
            Invoke("TurnOff", 3f);
        }
    }
    void TurnOff() { outline.OutlineColor = Color.white; on = false; waterSound.Stop(); sprinklerParticleSystem.Stop(); }

    public GameObject GetSprinklerParticleSystem() { return spsObj; }
}
