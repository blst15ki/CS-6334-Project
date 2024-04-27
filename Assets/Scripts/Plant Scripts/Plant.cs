using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Plant : MonoBehaviour
{
    [SerializeField] GameObject potObj;
    public int water, deadWater, maxWater;
    public string type;
    public string id;
    public string potID = null;
    public string stage;
    public DateTime cur, timeHalf, timeMature;
    public bool isHalf, isMature, isDead;
    public Guid uuid = Guid.NewGuid();
    public bool isDataLoaded = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (!isDataLoaded) {
            water = 10;
            deadWater = -20; // limit on how low water can reach before plant dies
            maxWater = 100;
            type = "";
            id = uuid.ToString();
            stage = "Seedling";
            cur = DateTime.Now;
            timeHalf = cur.AddMinutes(2f);
            timeMature = cur.AddMinutes(4f);
            isHalf = false;
            isMature = false;
            isDead = false;
            InitializePlant();
        }
    }

    public abstract void InitializePlant();

    void Start()
    {
        // trigger lose water every 10 seconds
        InvokeRepeating("LoseWater", 0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        cur = DateTime.Now;
        
        // check if dead (mature plants cannot die)
        if(!isMature && !isDead && water <= deadWater) {
            isDead = true;
            stage = "Dead";
            GetComponent<Renderer>().material.color = new Color(95f / 255, 25f / 255, 28f / 255);
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            transform.localPosition = potObj.transform.position + new Vector3(0f, 0.75f, -0.15f);
        } else if(!isDead) { // (dead plants cannot grow)
            // reach half grown
            if(!isHalf && DateTime.Compare(cur, timeHalf) >= 0) {
                isHalf = true;
                stage = "Adult";
                transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
                transform.localPosition = potObj.transform.position + new Vector3(0f, 0.85f, -0.15f);
            }
            // reach full grown
            if(!isMature && DateTime.Compare(cur, timeMature) >= 0) {
                isMature = true;
                stage = "Mature";
                transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
                transform.localPosition = potObj.transform.position + new Vector3(0f, 0.95f, -0.15f);
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
            timeHalf = cur.AddMinutes((timeHalf - cur).TotalMinutes * 0.8);
        }

        if(!isMature) {
            timeMature = cur.AddMinutes((timeMature - cur).TotalMinutes * 0.8);
        }
    }

    public GameObject GetPot() { return potObj; }
    public void SetPot(GameObject obj) { potObj = obj; }
    public string GetPotID() { return potID; }
    public void SetPotID(string id) { potID = id; }
}

