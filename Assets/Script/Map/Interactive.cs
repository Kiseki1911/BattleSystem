using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactive : MonoBehaviour
{
    private bool isNear=false;
    [SerializeField] protected Collider2D trigger;
    //GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = isNear;
    }
    private void OnMouseDown() {
        if(isNear){
            //interact
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player")
            isNear=true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag=="Player")
            isNear=false;
    }
}
