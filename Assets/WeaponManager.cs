using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    //public GameObject selectedWeapon;
    public GameObject weaponInHand;
    public GameObject boneReference;
    public GameObject cursor;

    public float mass;
    public Vector2 massCenter;
    public Vector2 handle;

    //public elements;
    public bool isSharp; 

    #region Battle Stats

    public float damage;
    [Range(0.001f,1)]
    public float swingDelay=1f;
    [Range(0.01f,1)]
    public float moveDelay;
    

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        weaponInHand.transform.eulerAngles = boneReference.transform.eulerAngles;
        cursor.transform.position = Vector3.Lerp(cursor.transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition),swingDelay);
        
    }

}
