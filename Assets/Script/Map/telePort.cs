using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class telePort : Interactive
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
         if(other.tag=="Player"){
            transform.parent.parent.GetComponent<MapControl>().nextlevel();
         }
    }
}
