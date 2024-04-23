using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject potObj;
    int water, deadWater, maxWater;
    string stage;
    DateTime cur, timeHalf, timeMature;
    bool isHalf, isMature, isDead;
    Vector3 pos; // original position

    // Start is called before the first frame update
    void Start()
    {
        water = 0;
        deadWater = -10; // limit on how low water can reach before plant dies
        maxWater = 100;
        stage = "Seedling";
        cur = DateTime.Now;
        timeHalf = cur.AddMinutes(1f);
        timeMature = cur.AddMinutes(2f);
        isHalf = false;
        isMature = false;
        isDead = false;
        pos = transform.position;

        // trigger lose water every 5 seconds
        InvokeRepeating("LoseWater", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        cur = DateTime.Now;
        
        // obj.transform.position + new Vector3(0f, 0.75f, -0.15f)
        // check if dead (mature plants cannot die)
        if(!isMature && !isDead && water <= deadWater) {
            isDead = true;
            stage = "Dead";
            GetComponent<Renderer>().material.color = new Color(95f / 255, 25f / 255, 28f / 255);
            transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);
            transform.position = potObj.transform.position + new Vector3(0f, 0.75f, -0.15f);
        } else if(!isDead) { // (dead plants cannot grow)
            // reach half grown
            if(!isHalf && DateTime.Compare(cur, timeHalf) >= 0) {
                isHalf = true;
                stage = "Adult";
                transform.localScale = new Vector3(0.15f, 0.2f, 0.15f);
                transform.position = potObj.transform.position + new Vector3(0f, 0.85f, -0.15f);
            }
            // reach full grown
            if(!isMature && DateTime.Compare(cur, timeMature) >= 0) {
                isMature = true;
                stage = "Mature";
                transform.localScale = new Vector3(0.15f, 0.3f, 0.15f);
                transform.position = potObj.transform.position + new Vector3(0f, 0.95f, -0.15f);
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
    void LoseWater() {
        if(water > deadWater) {
            water--;
        }
    }

    public void GiveWater() { InvokeRepeating("AddWater", 1f, 1f); }
    public void StopWater() { CancelInvoke("AddWater"); }

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

    public GameObject GetPot() { return potObj; }
    public void SetPot(GameObject obj) { potObj = obj; }
}
