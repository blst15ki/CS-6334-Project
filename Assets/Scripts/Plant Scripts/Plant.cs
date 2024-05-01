using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Plant : MonoBehaviour
{
    [SerializeField] protected GameObject potObj;
    Outline outline;
    public int water, maxWater, time;
    public string type;
    public string id;
    public string potID = null;
    public string stage;
    public DateTime cur, timeHalf, timeMature, timeLeave;
    public bool isHalf, isMature, hasLight = false;
    public Guid uuid = Guid.NewGuid();
    public bool isDataLoaded = false, delay;
    public Light lightSource;

    // Start is called before the first frame update
    void Awake()
    {
        if (!isDataLoaded) {
            water = 30;
            maxWater = 120;
            time = 120;
            type = "";
            id = uuid.ToString();
            stage = "Seedling";
            cur = DateTime.Now;
            timeHalf = cur.AddMinutes(2f);
            timeMature = cur.AddMinutes(4f);
            timeLeave = cur;
            delay = false;
            isHalf = false;
            isMature = false;
            InitializePlant();
        }
        outline = GetComponent<Outline>();
    }

    public abstract void InitializePlant();

    void Start()
    {
        // trigger lose water every 10 seconds
        InvokeRepeating("LoseWater", 0f, 10f);
        InvokeRepeating("UpdateGrowthTimer", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(lightSource == null || !lightSource.gameObject.activeInHierarchy) {
            lightSource = FindObjectOfType<Light>();
        }
        CheckStats();
        CheckPlantGrowth();
    }

    protected abstract void CheckPlantGrowth();

    void CheckStats() {
        if(lightSource.enabled) {
            hasLight = true;
        } else {
            hasLight = false;
        }

        if(!isMature) {
            float amount = (float)water / maxWater;
            // stop growth if no light or too little or too much water
            if((amount < 0.2 || amount > 0.8) || !hasLight) {
                if(!IsInvoking("DelayGrowth")) {
                    InvokeRepeating("DelayGrowth", 1f, 1f);
                    delay = true;
                }
            } else {
                CancelInvoke("DelayGrowth");
                delay = false;
            }
        }
    }

    void UpdateGrowthTimer() {
        if(!isHalf) {
            time = (int)((timeHalf - DateTime.Now).TotalSeconds);
        } else if(!isMature) {
            time = (int)((timeMature - DateTime.Now).TotalSeconds);
        } else {
            time = 0;
            CancelInvoke("UpdateGrowthTimer");
        }
    }

    void DelayGrowth() {
        timeHalf = timeHalf.AddSeconds(1f);
        timeMature = timeMature.AddSeconds(1f);
    }

    void AddWater() {
        water += 5;
    }
    void LoseWater() {
        water--;
    }

    public void GiveWater() {
        InvokeRepeating("AddWater", 0f, 0.5f);
        outline.OutlineColor = Color.cyan;
        outline.enabled = true;
    }
    public void StopWater() {
        CancelInvoke("AddWater");
        outline.OutlineColor = Color.white;
        outline.enabled = false;
    }
    public void StopWaterTimed(float timer) { Invoke("StopWater", timer); } // stop watering after timer seconds

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
    public string GetPlantType() { return type; }
    public bool HasLight() { return hasLight; }
    public int GetTime() { return time; }
}

