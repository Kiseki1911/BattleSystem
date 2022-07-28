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
    private Rigidbody2D target;
    
    private float t;

    #region Battle Stats

    public float damage;
    [Range(0.001f,1)]
    public float swingDelay;
    [Range(0.01f,1)]
    public float moveDelay;
    

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if(Instance==null)
        Instance = this;
        target=cursor.GetComponent<Rigidbody2D>();
        t=0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        swingDelay = 50000f/Mathf.Pow(WeaponInstance.instance.weapon.mass,2);
        moveDelay = 2000/(WeaponInstance.instance.weapon.mass+2000);
        weaponInHand.transform.eulerAngles = boneReference.transform.eulerAngles+Vector3.back*90;
        Vector2 realCursor=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Vector2.Distance(PlayerManager.instance.transform.position,realCursor)>2){
            realCursor=(Vector2)PlayerManager.instance.transform.position+(realCursor-(Vector2)PlayerManager.instance.transform.position).normalized*2;
        }
        if(!onHit){
            //cursor.transform.position = Vector3.Lerp(cursor.transform.position,realCursor,swingDelay);
            //Debug.Log(realCursor);
            //Debug.Log(t);
            if(Vector2.Distance(target.position,realCursor)<=(target.velocity.magnitude/(400*swingDelay))){
                if(t>0){
                    t-=4f*swingDelay;
                    target.velocity=Vector2.Lerp(Vector3.zero,(realCursor-target.position).normalized*5,t);
                }
                else{
                    t=0;
                    target.velocity=Vector2.zero;
                }
            }
            else {
                t+=swingDelay;
                if(t>1)
                    t=1;
                target.velocity=Vector2.Lerp(Vector3.zero,(realCursor-target.position).normalized*5,t);
            }
        }
        
    }

    public IEnumerator OnHit(Vector2 colliPoint){
        yield return new WaitForSeconds(0.2f);
        Instantiate(onHitEffect,colliPoint, Quaternion.Euler(weaponInHand.transform.localRotation.x,weaponInHand.transform.localRotation.y-90,weaponInHand.transform.localRotation.z));
        onHit = false;
    }

    public void changeWeapon(Weapon newWeapon){
        target.velocity=Vector2.zero;
        t=0;
    }
}
