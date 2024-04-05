using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject plantInterface;
    int water, deadWater;
    DateTime cur, growHalf, growMature, death;
    bool isHalf, isMature, isDead;
    // death time not utilized, would depend on saving data implementation

    // Start is called before the first frame update
    void Start()
    {
        water = 0;
        deadWater = -20; // limit on how low water can reach before plant dies
        cur = DateTime.Now;
        growHalf = cur.AddMinutes(1f);
        growMature = cur.AddMinutes(2f);
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
        } else {
            // reach half grown
            if(!isHalf && DateTime.Compare(cur, growHalf) >= 0) {
                isHalf = true;
            }

            // reach full grown
            if(!isMature && DateTime.Compare(cur, growMature) >= 0) {
                isMature = true;
            }
        }
    }

    public void PointerOn() { pointer = true; }
    public void PointerOff() { pointer = false; }

    public void GiveWater() { water++; }
    public int GetWater() { return water; }
    void LoseWater() { water--; }

    // may have to change details of interface and have only one at the start that changes
    public void EnableInterface() { plantInterface.SetActive(true); }
    public void DisableInterface() { plantInterface.SetActive(false); }
}
