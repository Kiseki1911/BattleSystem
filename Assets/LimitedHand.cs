using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedHand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //gameObject.GetComponent<Rigidbody2D>().AddForce(10f*((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)-gameObject.transform.parent.GetComponent<Rigidbody2D>().position));
    }
}
