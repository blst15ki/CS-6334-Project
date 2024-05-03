using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grass : Plant
{
    public override void InitializePlant() {
        water = 25;
        time = 150;
        type = "Grass";
        stage = "Seedling";
        timeHalf = DateTime.Now.AddMinutes(2.5f);
        timeMature = DateTime.Now.AddMinutes(5f);
        maxWater = 100;
    }

    protected override void CheckPlantGrowth() {
        cur = DateTime.Now;
        
        // check if dead (mature plants cannot die)
        // plants die if too underwatered or overwatered
        if(!isMature && (water <= 0 || water >= maxWater)) {
            // remove link to pot and destroy plant
            Pot potScript = potObj.GetComponent<Pot>();
            potScript.ClearPlant();
            Destroy(gameObject);
        } else {
            // reach half grown
            if(!isHalf && DateTime.Compare(cur, timeHalf) >= 0) {
                isHalf = true;
                stage = "Adult";
                transform.localScale = new Vector3(30f, 30f, 30f);
            }
            // reach full grown
            if(!isMature && DateTime.Compare(cur, timeMature) >= 0) {
                isMature = true;
                stage = "Mature";
                transform.localScale = new Vector3(40f, 40f, 40f);
            }
        }
    }
}
