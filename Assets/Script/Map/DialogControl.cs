using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogControl : MonoBehaviour
{
    // Start is called before the first frame update
    public FollowDialogControl dialog;
    public TextAsset file;
    Collider2D collider2d;
    string [] strList;
    void Awake()
    {
        collider2d=GetComponent<Collider2D>();
        strList=file.text.Split('\n');
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.tag);
        if(other.tag=="Player"){
            Debug.Log(1);
            dialog.GetText(strList);
            dialog.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
