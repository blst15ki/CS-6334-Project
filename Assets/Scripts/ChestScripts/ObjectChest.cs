using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChest : Chest
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
            // grab from chest (instantiate since it can be placed)
            if(open && Input.GetButtonDown(BInput)) {
                if(!hotbar.HasObject()) {
                    GameObject obj = Instantiate(returnList[rand.Next(0, returnList.Length)]);
                    if(hotbar.SelectObject(obj)) {
                        sound.Play();
                        ResetTime();
                    }
                }
            }
        }
    }
}
