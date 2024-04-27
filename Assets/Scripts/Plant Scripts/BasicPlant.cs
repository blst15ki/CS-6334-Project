using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicPlant : Plant
{
    public override void InitializePlant() {
        type = "Basic Plant";
        stage = "Seedling";
        timeHalf = DateTime.Now.AddMinutes(1f);
        timeMature = DateTime.Now.AddMinutes(2f);
        deadWater = -10;
        maxWater = 100;
    }
}
