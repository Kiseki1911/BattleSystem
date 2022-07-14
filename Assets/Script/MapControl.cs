using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public MapGen mapGen;

    void Start()
    {
        nextlevel();
    }

    // Update is called once per frame
    public void nextlevel(){
        player.transform.position=mapGen.creatMap();
        player.GetComponent<CharacterActions>().targetPos = player.transform.position;
    }
}
