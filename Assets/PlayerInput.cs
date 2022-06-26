using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    public float inputX;
    public float inputY;
    private CharacterActions me;
    void Start()
    {
        me = gameObject.GetComponent<CharacterActions>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if(inputX!=0||inputY!=0){
            me.Movement(new Vector3(Mathf.RoundToInt(inputX),Mathf.RoundToInt(inputY),0));
        }
        //me.isRoundAttack = Input.GetMouseButton(0);
        if(Input.GetMouseButtonUp(0)){
            AbilityManager.Instance.CallAbility(1);
        }

    }
}
