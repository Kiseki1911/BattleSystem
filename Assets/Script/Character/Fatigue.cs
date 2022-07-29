using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fatigue:MonoBehaviour
{
    public static Fatigue Instance;
    public float fat=0f;
    public float decreaseRate=1f;
    public float increaseRate=1f;

    public Image SP_UI;

    private void Awake() {
        Instance = this;
    }

    private void LateUpdate() {
        if(SP_UI != null)
        {
          SP_UI.fillAmount = Mathf.Max(0, 1f - fat / 200f);
        }
    }

    public void IncreaseFat(int value){
        fat+=value*increaseRate;
        if(fat>=220){
            fat=220;
        }
    }

    public void DecreaseFat(int value){
        if(fat>0){
            fat-=value*decreaseRate;
        }
        else{
            fat=0;
        }
    }

}