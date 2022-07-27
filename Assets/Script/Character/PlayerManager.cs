using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float inputX;
    [SerializeField] float inputY;

    public int mouseLeftSkill = 1;
    public int mouseRightSkill = 2;
    static public PlayerManager instance;
    public static bool isControlling = false;
    private CharacterActions me;
    void Start()
    {
        instance=this;
        me = gameObject.GetComponent<CharacterActions>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isControlling){
            PlayerInput();
        }
    }

    private void PlayerInput(){
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if(inputX!=0||inputY!=0){
            me.Movement(new Vector3(Mathf.RoundToInt(inputX),Mathf.RoundToInt(inputY),0));
            me.anim.SetBool("Moving",true);
            if(inputX!=0){
                me.transform.GetChild(0).transform.localScale=new Vector3(-Mathf.RoundToInt(inputX),1,1);
            }
        }
        else{
            me.anim.SetBool("Moving",false);
        }
        //me.isRoundAttack = Input.GetMouseButton(0);
        if(Input.GetMouseButtonUp(0)){
            AbilityManager.Instance.CallAbility(mouseLeftSkill);
        }
        if(Input.GetMouseButtonUp(1)){
            AbilityManager.Instance.CallAbility(mouseRightSkill);
        }
    }
}
