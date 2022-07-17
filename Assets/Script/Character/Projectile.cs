using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject player;
    private bool onHit = false;
    private bool onReturn = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1,0);
        StartCoroutine(SelfReturn(10));

    }

    // Update is called once per frame
    void Update()
    {
        if(onHit){
            gameObject.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            if(Input.GetMouseButton(0)){
                transform.parent=null;
                onHit=false;
                onReturn = true;
            }
        }
    }
    private void FixedUpdate() {
        if(onReturn){
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            gameObject.GetComponent<Rigidbody2D>().AddTorque(20);
            gameObject.GetComponent<Rigidbody2D>().velocity*=0.95f;
            gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.GetComponent<Rigidbody2D>().angularVelocity*direction/20);
            //transform.position = Vector3.Lerp(transform.position,player.transform.position,0.05f);
            //gameObject.GetComponent<Rigidbody2D>().SetRotation(34);
        }
    }

    public void Enter(Vector3 direction){
        gameObject.GetComponent<Rigidbody2D>().AddForce(1000*direction);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag =="Enemy"||other.tag == "Wall"){
            gameObject.transform.SetParent(other.transform);

            onReturn=false;
            onHit= true;
        }
        else if(other.tag=="Player"){
            if(onReturn ||gameObject.transform.parent!=null){
                Debug.Log("back to hand");
                AbilityManager.Instance.offHand=false;
                WeaponManager.Instance.weaponInHand.SetActive(true);
                onReturn=false;
                Destroy(gameObject);                
            }
        }
    }
    IEnumerator SelfReturn(float time){
        yield return new WaitForSeconds(time);
        onReturn = true;
    }
}
