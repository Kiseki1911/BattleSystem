using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public MapGen mapGen;
    public GameObject forgeUi,eventSystem;

    void Start()
    {
        nextlevel();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
            forgeUi.SetActive(!forgeUi.activeSelf);
            eventSystem.SetActive(!eventSystem.activeSelf);
            player.GetComponent<PlayerManager>().enabled=!player.GetComponent<PlayerManager>().enabled;
        }
    }

    // Update is called once per frame
    public void nextlevel(){
        player.SetActive(false);
        player.transform.position=mapGen.creatMap();
        player.GetComponent<CharacterActions>().targetPos = player.transform.position;
        player.SetActive(true);
    }
}
