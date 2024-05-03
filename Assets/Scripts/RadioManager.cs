using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips; // currently has 3 audio clips
    private AudioSource audioSource;
    private string BInput, XInput;
    private int songNum;
    [SerializeField] Hotbar hotbar;
    private bool musicEnable;
    [SerializeField] ParticleSystem musicParticleSystem;
    bool pointer;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        BInput = "js5"; // start/stop
        XInput = "js2"; // cycle songs
        songNum = 0;
        musicEnable = false;
        pointer = false;
    }

    void Update()
    {

        if (hotbar == null) {
            hotbar = FindObjectOfType<Hotbar>();
        }

        if(pointer) {
            if(Input.GetButtonDown(BInput)) { // start/stop
                if(!musicEnable) { // start
                    musicEnable = true;
                    audioSource.clip = clips[songNum % clips.Length];
                    audioSource.Play();
                    musicParticleSystem.Play();
                } else { // stop
                    musicEnable = false;
                    audioSource.Stop();
                    musicParticleSystem.Stop();
                }
            }
            else if (Input.GetButtonDown(XInput) && musicEnable) { // cycle
                audioSource.Stop();
                songNum++;
                audioSource.clip = clips[songNum % clips.Length];
                audioSource.Play();
            }
        }
    }

    public void PointerOn() { pointer = true; hotbar.DisableHotbar(); }
    public void PointerOff() { pointer = false; hotbar.EnableHotbar(); }
}
