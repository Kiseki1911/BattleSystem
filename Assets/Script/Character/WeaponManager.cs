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


    public bool onHit;
    public ParticleSystem onHitEffect;

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
        if(Instance==null)
        Instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        swingDelay = 50000f/Mathf.Pow(WeaponInstance.instance.weapon.mass,2);
        moveDelay = 2000/(WeaponInstance.instance.weapon.mass+2000);
        weaponInHand.transform.eulerAngles = boneReference.transform.eulerAngles+Vector3.back*90;
        if(!onHit){
            cursor.transform.position = Vector3.Lerp(cursor.transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition),swingDelay);
        }
        
    }  

    public IEnumerator OnHit(Vector2 colliPoint){
        yield return new WaitForSeconds(0.2f);
        Instantiate(onHitEffect,colliPoint, Quaternion.Euler(weaponInHand.transform.localRotation.x,weaponInHand.transform.localRotation.y-90,weaponInHand.transform.localRotation.z));
        onHit = false;
    }

    public void changeWeapon(Weapon newWeapon){

    }
}
