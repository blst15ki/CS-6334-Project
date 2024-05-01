using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class BasicPlant : Plant
{
    public override void InitializePlant() {
        time = 120;
        type = "Basic Plant";
        stage = "Seedling";
        timeHalf = DateTime.Now.AddMinutes(2f);
        timeMature = DateTime.Now.AddMinutes(4f);
        maxWater = 120;
    }

    [PunRPC]
    void SetActiveState(bool active) {
        gameObject.SetActive(active);
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
                transform.localScale = new Vector3(0.15f, 0.25f, 0.15f);
                transform.localPosition = potObj.transform.position + new Vector3(0f, 0.85f, -0.15f);
            }
            // reach full grown
            if(!isMature && DateTime.Compare(cur, timeMature) >= 0) {
                isMature = true;
                stage = "Mature";
                transform.localScale = new Vector3(0.15f, 0.35f, 0.15f);
                transform.localPosition = potObj.transform.position + new Vector3(0f, 0.95f, -0.15f);
            }
        }
    }
}
