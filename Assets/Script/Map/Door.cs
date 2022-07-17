using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactive
{
    // Start is called before the first frame update
    [SerializeField] private BoxCollider2D boxcollider;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"){
            openDoor();
            trigger.enabled=false;
        }
    }
    public void openDoor(){
        //TODO: animation
        boxcollider.enabled=false;
    }
    public void lockDoor(){
        //TODO: animation
        boxcollider.enabled=true;
    }
}
