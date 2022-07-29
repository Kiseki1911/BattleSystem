using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaterialDisplay : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        transform.position+= new Vector3(Random.Range(-1f,1f),Random.value,0);
        StartCoroutine(SelfDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pick(string name,int val){
        text.text = "捡到了"+val.ToString()+"个"+name+"真开心";
    }

    IEnumerator SelfDestroy(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
