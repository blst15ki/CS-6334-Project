using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetChest : Chest
{
    protected override void CheckTimer() {
        if(time == 0 && !open) {
            CancelInvoke("Countdown");
            open = true;
            statusTMP.text = "Unlocked";
            statusTMP.color = Color.green;
            lockObj.SetActive(false);
        }

        if(pointer) {
            // grab from chest (no instantiate because asset)
            if(open && Input.GetButtonDown(BInput)) {
                hotbar.SelectObject(returnList[rand.Next(0, returnList.Length)]);
                ResetTime();
            }
        }
    }
}
