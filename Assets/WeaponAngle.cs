using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAngle : MonoBehaviour
{
    public GameObject refe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles = refe.transform.eulerAngles;
    }
}
