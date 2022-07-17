using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollideDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("coli something"+ other.name);
        if(other.gameObject.GetComponentInParent<CharacterActions>()!=null && other.CompareTag("Player")){
            Debug.Log("hit player");
            var collisionPoint = other.ClosestPoint(transform.position);
            Debug.Log(collisionPoint);
            //WeaponManager.Instance.onHit=true;
            StartCoroutine(WeaponManager.Instance.OnHit(collisionPoint));
            Debug.Log(gameObject.transform.parent.GetComponent<EnemyManager>().attackDamange);
            other.gameObject.GetComponentInParent<CharacterActions>().TakeDamage(gameObject.transform.parent.GetComponent<EnemyManager>().attackDamange);
            Debug.Log(other.gameObject.GetComponentInParent<CharacterActions>().curHealth);
        }
    }
}
