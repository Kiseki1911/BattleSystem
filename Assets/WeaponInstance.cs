using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    public Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        weapon = new Weapon(WeaponType.sword,10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //gameObject.transform.eulerAngles = refe.transform.eulerAngles;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.GetComponentInParent<CharacterActions>()!=null && other.CompareTag("Enemy")){
            other.gameObject.GetComponentInParent<CharacterActions>().TakeDamage((int)weapon.damageRate);
            Debug.Log(other.gameObject.GetComponentInParent<CharacterActions>().curHealth);
        }
    }
}
