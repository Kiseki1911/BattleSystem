using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDialog : MonoBehaviour
{
    public FollowDialogControl dialog;
    public TextAsset file;
    Collider2D collider2d;
    string [] strList;
    void Awake()
    {
        collider2d=GetComponent<Collider2D>();
        strList=file.text.Split('\n');
    }

    public void sendDialog(){
        dialog.GetText(strList);
        dialog.gameObject.SetActive(true);
    }
}
