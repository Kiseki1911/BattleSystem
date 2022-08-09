using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal; 
using UnityEngine;

public class materialObj : MonoBehaviour
{
    public Material material;
    SpriteRenderer myRenderer;
    Collider2D myCollider;
    public GameObject display;
    public int num = 1;
    // Start is called before the first frame update
    void Awake()
    {
        myRenderer=GetComponent<SpriteRenderer>();
        myCollider=GetComponent<Collider2D>();
        myRenderer.color=material.color;
        transform.GetChild(0).GetComponent<Light2D>().color=this.material.color;
    }

    public void setMaterial(Material material){
        this.material=material;
        myRenderer.color=this.material.color;
        transform.GetChild(0).GetComponent<Light2D>().color=this.material.color;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.tag=="Player"){
            myCollider.enabled=false;
            BackPack.Instance.materials.Add(material, num);
            display.GetComponent<MaterialDisplay>().pick(material.title,num);
            Instantiate(display,transform.position,Quaternion.identity);
            Destroy(gameObject,0.05f);
        }
    }

}
