using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject forgeUi,eventSystem,player;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
            forgeUi.SetActive(!forgeUi.activeSelf);
            eventSystem.SetActive(!eventSystem.activeSelf);
            player.GetComponent<PlayerManager>().enabled=!player.GetComponent<PlayerManager>().enabled;
        }
    }
}