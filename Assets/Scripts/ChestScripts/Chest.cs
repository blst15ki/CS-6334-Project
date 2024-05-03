using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public abstract class Chest : MonoBehaviour
{
    [SerializeField] protected Hotbar hotbar;
    [SerializeField] protected GameObject lockObj;
    [SerializeField] protected GameObject[] returnList;
    [SerializeField] protected TextMeshProUGUI statusTMP, durationTMP;
    protected int time = 0, duration = 180; // in seconds
    protected DateTime unlockTime = DateTime.Now;
    protected bool open = false, pointer = false;
    protected string BInput = "js5";
    protected System.Random rand = new System.Random();
    protected AudioSource sound;

    void Start() {
        sound = GetComponent<AudioSource>();
    }

    void Update() {
        CheckTimer();
    }

    protected abstract void CheckTimer();

    protected void ResetTime() {
        time = duration;
        InvokeRepeating("Countdown", 1f, 1f);
        open = false;
        statusTMP.text = "Locked";
        statusTMP.color = Color.red;
        durationTMP.text = "Next Item In: " + time + " sec";
        unlockTime = DateTime.Now.AddSeconds((float)duration);
        lockObj.SetActive(true);
    }
    protected void Countdown() {
        time--;
        durationTMP.text = "Next Item In: " + time + " sec";
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    public DateTime GetUnlockTime() { return unlockTime; }

    public void SetTime(int sec) {
        if(sec > 0) {
            time = sec;
            InvokeRepeating("Countdown", 1f, 1f);
            open = false;
            statusTMP.text = "Locked";
            statusTMP.color = Color.red;
            durationTMP.text = "Next Item In: " + time + " sec";
            unlockTime = DateTime.Now.AddSeconds(time);
            lockObj.SetActive(true);
        }
    }
}
