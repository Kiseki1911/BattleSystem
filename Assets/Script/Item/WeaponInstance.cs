using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    public Weapon weapon;
    private float speed;
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
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(weapon.texture,new Rect(0,0,36,36),new Vector2(weapon.handle.y+.5f,36-weapon.handle.x+.5f)/36f,32);
        gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
        massCenter.transform.localPosition=(weapon.massCenter-new Vector2(weapon.handle.y,36-weapon.handle.x))/36;
        oldPos=massCenter.transform.position/36;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        speed=(massCenter.transform.position-oldPos).magnitude;
        oldPos=massCenter.transform.position;
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.GetComponentInParent<EnemyManager>()!=null && other.CompareTag("Enemy")){
            var collisionPoint = other.ClosestPoint(transform.position);
            Debug.Log(collisionPoint);
            WeaponManager.Instance.onHit=true;
            StartCoroutine(WeaponManager.Instance.OnHit(collisionPoint));
            Debug.Log((weapon.damageRate*speed));
            other.gameObject.GetComponentInParent<EnemyManager>().TakeDamage((int)(weapon.damageRate*speed),weapon.effects);
            Debug.Log(other.gameObject.GetComponentInParent<EnemyManager>().curHealth);
        }
    }
    public void changeWeapon(int i){
        currentWeapon=i;
        weapon = BackPack.Instance.weaponList[currentWeapon];
        Destroy(GetComponent<PolygonCollider2D>());
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(weapon.texture,new Rect(0,0,36,36),new Vector2(weapon.handle.y+.5f,36-weapon.handle.x+.5f)/36f,32);
        gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
        massCenter.transform.localPosition=(weapon.massCenter-new Vector2(weapon.handle.y,36-weapon.handle.x))/36;
        oldPos=massCenter.transform.position/36;
    }
}
