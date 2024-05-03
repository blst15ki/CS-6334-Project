using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fern : Plant
{
    public override void InitializePlant() {
        water = 20;
        time = 135;
        type = "Fern";
        stage = "Seedling";
        timeHalf = DateTime.Now.AddMinutes(2.25f);
        timeMature = DateTime.Now.AddMinutes(4.5f);
        maxWater = 80;
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
                transform.localScale = new Vector3(200f, 200f, 200f);
            }
            // reach full grown
            if(!isMature && DateTime.Compare(cur, timeMature) >= 0) {
                isMature = true;
                stage = "Mature";
                transform.localScale = new Vector3(400f, 400f, 400f);
            }
        }
    }
}
