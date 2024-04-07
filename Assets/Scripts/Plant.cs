using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject plantInterface;
    int water, deadWater, maxWater;
    string stage;
    DateTime cur, growHalf, growMature, death;
    bool pointer, isHalf, isMature, isDead;
    // death time not utilized, would depend on saving data implementation

    // Start is called before the first frame update
    void Start()
    {
        water = 0;
        deadWater = -20; // limit on how low water can reach before plant dies
        maxWater = 100;
        stage = "Seedling";
        cur = DateTime.Now;
        growHalf = cur.AddMinutes(1f);
        growMature = cur.AddMinutes(2f);
        pointer = false; // for later use with interacting
        isHalf = false;
        isMature = false;
        isDead = false;

        // trigger lose water every 5 seconds
        InvokeRepeating("LoseWater", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        cur = DateTime.Now;
        
        // check if dead (mature plants cannot die)
        if(!isMature && !isDead && water <= deadWater) {
            isDead = true;
            stage = "Dead";
        } else {
            // reach half grown
            if(!isHalf && DateTime.Compare(cur, growHalf) >= 0) {
                isHalf = true;
                stage = "Adult";
            }

            // reach full grown
            if(!isMature && DateTime.Compare(cur, growMature) >= 0) {
                isMature = true;
                stage = "Mature";
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    void AddWater() {
        if(water < maxWater) {
            water += 10;
        }
        if(water > maxWater) {
            water = maxWater;
        }
    }
    public void GiveWater() { InvokeRepeating("AddWater", 0f, 1f); }
    public void StopWater() { CancelInvoke("AddWater"); }
    void LoseWater() { water--; }

    public int GetWater() { return water; }
    public int GetMaxWater() { return maxWater; }
    public string GetStage() { return stage; }
}
