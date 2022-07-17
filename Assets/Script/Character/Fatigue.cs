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

    public Slider playerFatigue;

    private void Start() {
        Instance = this;
    }

    private void LateUpdate() {
        playerFatigue.value = fat;
    }

    public void IncreaseFat(int value){
        fat+=value*increaseRate;
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