using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour
{
    int water, deadWater, maxWater;
    string stage;
    DateTime cur, timeHalf, timeMature, death;
    bool isHalf, isMature, isDead;
    // death time not utilized, would depend on saving data implementation

    // Start is called before the first frame update
    void Start()
    {
        water = 0;
        deadWater = -20; // limit on how low water can reach before plant dies
        maxWater = 100;
        stage = "Seedling";
        cur = DateTime.Now;
        timeHalf = cur.AddMinutes(1f);
        timeMature = cur.AddMinutes(2f);
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
            if(!isHalf && DateTime.Compare(cur, timeHalf) >= 0) {
                isHalf = true;
                stage = "Adult";
            }

            // reach full grown
            if(!isMature && DateTime.Compare(cur, timeMature) >= 0) {
                isMature = true;
                stage = "Mature";
            }
        }
    }

    void AddWater() {
        if(water < maxWater) {
            water += 10;
        }
        if(water > maxWater) {
            water = maxWater;
        }
    }
    public void GiveWater() { InvokeRepeating("AddWater", 1f, 1f); }
    public void StopWater() { CancelInvoke("AddWater"); }
    void LoseWater() { water--; }

    public int GetWater() { return water; }
    public int GetMaxWater() { return maxWater; }
    public string GetStage() { return stage; }

    public void Fertilize() {
        if(!isHalf) {
            timeHalf = cur.AddMinutes((timeHalf - cur).TotalMinutes * 0.9);
        }

        if(!isMature) {
            timeMature = cur.AddMinutes((timeMature - cur).TotalMinutes * 0.9);
        }
    }
}
