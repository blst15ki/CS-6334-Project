using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips; // currently has 3 audio clips
    private AudioSource audioSource;
    private string YInput, BInput, AInput;
    private int songNum;
    [SerializeField] Hotbar hotbar;
    private bool musicEnable;
    [SerializeField] ParticleSystem musicParticleSystem;
    bool pointer;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        BInput = "js5"; // select
        YInput = "js3"; // cycle through radio
        AInput = "js10"; // stop the song
        songNum = 0;
        musicEnable = false;
        pointer = false;
    }

    void Update()
    {
        if(pointer) {
            if(Input.GetButtonDown(BInput)) {
                musicEnable = true;
                playMusic();
            }
            else if (Input.GetButtonDown(YInput) && musicEnable){
                audioSource.Stop();
                songNum++;
                audioSource.clip = clips[songNum%3];
                audioSource.Play();
            }
            else if (Input.GetButtonDown(AInput) && musicEnable){
                musicEnable = false;
                audioSource.Stop();
                musicParticleSystem.Stop();
            }
        }
    }
    
    void playMusic(){
        if(musicEnable){
            audioSource.clip = clips[songNum%3];
            audioSource.Play();
            musicParticleSystem.Play();
        }
    }

    public void PointerOn() { pointer = true; hotbar.DisableHotbar(); }
    public void PointerOff() { pointer = false; hotbar.EnableHotbar(); }
}
