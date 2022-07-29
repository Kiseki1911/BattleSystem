using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    public Weapon weapon;
    private Vector3 speed;
    private Vector3 oldPos;

    public int currentWeapon=0;

    static public WeaponInstance instance;
    public GameObject massCenter;
    private void OnEnable() {
        if(instance==null)
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance=this;
        transform.rotation=Quaternion.identity;
        weapon = BackPack.Instance.weaponList[currentWeapon];
        //transform.localPosition=weapon.handle/36;
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(weapon.texture,new Rect(0,0,36,36),(weapon.handle+new Vector2(0.5f,-0.5f))/36f,36);
        gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
        massCenter.transform.localPosition=(new Vector2(weapon.massCenter.x,36-weapon.massCenter.y)-weapon.handle)/36;
        oldPos=massCenter.transform.position/32;
        Debug.Log(weapon.ToString());
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        speed=massCenter.transform.position-oldPos;
        oldPos=massCenter.transform.position;
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.GetComponentInParent<EnemyManager>()!=null && other.CompareTag("Enemy")){
            var collisionPoint = other.ClosestPoint(transform.position);
            Debug.Log(collisionPoint);
            WeaponManager.Instance.onHit=true;
            StartCoroutine(WeaponManager.Instance.OnHit(collisionPoint));
            Debug.Log((speed));
            float speedMag=(speed-other.gameObject.GetComponentInParent<EnemyManager>().curSpeed).magnitude;
            Debug.Log((weapon.damageRate*speedMag*5));
            other.gameObject.GetComponentInParent<EnemyManager>().TakeDamage((int)(weapon.damageRate*speedMag*5),weapon.effects);
        }
    }
    public void changeWeapon(int i){
        currentWeapon=i;
        weapon = BackPack.Instance.weaponList[currentWeapon];
        Destroy(GetComponent<PolygonCollider2D>());
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(weapon.texture,new Rect(0,0,36,36),(weapon.handle+new Vector2(0.5f,-0.5f))/36f,36);
        gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
        massCenter.transform.localPosition=(new Vector2(weapon.massCenter.x,36-weapon.massCenter.y)-weapon.handle)/36;
        oldPos=massCenter.transform.position/36;
        Debug.Log(weapon.ToString());
        WeaponManager.Instance.changeWeapon(weapon);
    }
}
