using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    public Weapon weapon;
    private float speed;
    private Vector3 oldPos;

    private int currentWeapon=0;

    static public WeaponInstance instance;
    private void OnEnable() {
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition=-weapon.handle;
        weapon = BackPack.Instance.weaponList[currentWeapon];
        oldPos=transform.position;
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(weapon.texture,new Rect(0,0,36,36),Vector2.zero,64);
        gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        speed=(transform.position-oldPos).magnitude;
        oldPos=transform.position;
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.GetComponentInParent<EnemyManager>()!=null && other.CompareTag("Enemy")){
            var collisionPoint = other.ClosestPoint(transform.position);
            Debug.Log(collisionPoint);
            WeaponManager.Instance.onHit=true;
            StartCoroutine(WeaponManager.Instance.OnHit(collisionPoint));
            Debug.Log((weapon.damageRate*speed));
            other.gameObject.GetComponentInParent<EnemyManager>().TakeDamage((int)(weapon.damageRate*speed));
            Debug.Log(other.gameObject.GetComponentInParent<EnemyManager>().curHealth);
        }
    }
    public void changeWeapon(int i){
        currentWeapon=i;
        weapon = BackPack.Instance.weaponList[currentWeapon];
        Destroy(GetComponent<PolygonCollider2D>());
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(weapon.texture,new Rect(0,0,36,36),Vector2.zero,64);
        gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
    }
}
