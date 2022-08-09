using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "Default";
        GetComponent<Canvas>().sortingOrder =10;
        transform.position+= new Vector3(Random.Range(-1f,1f),Random.value,0);
        StartCoroutine(SelfDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int val){
        text.text = val.ToString();
    }

    IEnumerator SelfDestroy(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
