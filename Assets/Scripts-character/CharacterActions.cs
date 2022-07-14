using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActions : MonoBehaviour
{
    private Fatigue fatigue;
    public Vector3 targetPos;
    public float speed = 0.5f;
    public GameObject weapon;
    public float weaponRotation_z;
    public float rotation_z;
    public int curHealth;
    public int maxHealth = 100;
    
    public bool isRoundAttack;

    RaycastHit2D[] rayResults;
    // Start is called before the first frame update
    void Start()
    {
        CharacterReset();
        targetPos = transform.position;
        fatigue = Fatigue.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,targetPos,WeaponManager.Instance.moveDelay);
        fatigue.DecreaseFat(1);
    }
    public void Movement(Vector3 directionUnit){
        rayResults = Physics2D.RaycastAll(transform.position,directionUnit,1f);
        for(int i=0; i <rayResults.Length;i++){
            if(rayResults[i].collider.tag=="Wall"){
                //Debug.Log("wall ahead");
                return;
            }
        }
        Debug.DrawRay(transform.position, directionUnit, Color.green);
            if((targetPos-transform.position).magnitude<0.05){
                targetPos =Vector3Int.RoundToInt(transform.position)+directionUnit;
                fatigue.IncreaseFat(30);
            }
    }
    public void RoundAttack(){
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        float mSpeed;
        //weapon.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + offset);
        if(rotation_z<0){
            rotation_z+=360f;
        }
        weaponRotation_z=weapon.transform.eulerAngles.z;
        mSpeed = weaponRotation_z-rotation_z;
        //Debug.Log(mSpeed);
        JointMotor2D motorRef = weapon.GetComponent<HingeJoint2D>().motor;
        motorRef.motorSpeed = mSpeed;
        weapon.GetComponent<HingeJoint2D>().motor = motorRef;
    }
    public void WeaponReset(){
        Debug.Log("in reset");
        weapon.transform.localEulerAngles =Vector3.zero;
        weapon.transform.localPosition =new Vector3(1f,0.01f,0);
        //Debug.Log(weapon.transform.localPosition);
        //weapon.transform.eulerAngles =new Vector3(0f,0f,30f);
        JointMotor2D motorRef = weapon.GetComponent<HingeJoint2D>().motor;
        motorRef.motorSpeed = 0f;
        weapon.GetComponent<HingeJoint2D>().motor = motorRef;
        //Debug.Log(weapon.transform.localPosition);
    }

    public void CharacterReset(){
        curHealth=maxHealth;
    }
    public void TakeDamage(int dmg){
        Debug.Log("curHealth before dmg"+curHealth);
        curHealth -= dmg;
        Debug.Log("player take dmg!"+ curHealth);
        if(curHealth<=0){
            OnDeath();
        }
    }
    private void OnDeath(){
        if(transform.GetComponentInChildren<Projectile>()!=null){
            transform.GetComponentInChildren<Projectile>().gameObject.transform.SetParent(null);
        }
        gameObject.SetActive(false);
    }
}