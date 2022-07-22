using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour
{
    private EnemyManager me;
    public int AIType;

    private int vInputX=1;
    private int vInputY=1;
    private bool wanderTimer = true;
    private bool isHori = true;
    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        AICase(AIType);
    }

    public void EnemyMoveInput(int x, int y){
        if(x!=0||y!=0){
            me.Movement(new Vector3(Mathf.RoundToInt(x),Mathf.RoundToInt(y),0));
        }
        /*
        if(Input.GetMouseButtonUp(0)){
            AbilityManager.Instance.CallAbility(mouseLeftSkill);
        }
        if(Input.GetMouseButtonUp(1)){
            AbilityManager.Instance.CallAbility(mouseRightSkill);
        }
        */
    }
    private void AICase(int type){
        switch(type){
            case 0:
                Wander();
            break;
            case 1:
                if(me.isSeeingPlayer){
                    Chase();
                }
                else{
                    Wander();
                }
            break;
            default:
            break;

        }
    }

    IEnumerator constantWander(float time,int dir){
        yield return new WaitForSeconds(time);
        if(dir ==0){
            vInputX*=-1;
        }
        else{
            vInputY*=-1;
        }
        isHori = Random.value>0.5f;
        wanderTimer = true;
    }
    private void Wander(){
        if(isHori){
            EnemyMoveInput(vInputX,0);
            if(wanderTimer){
                wanderTimer = false;
                StartCoroutine(constantWander(Random.Range(1f,3f),0));
            }
        }
        else{
            EnemyMoveInput(0,vInputY);
            if(wanderTimer){
                wanderTimer = false;
                StartCoroutine(constantWander(Random.Range(1f,3f),1));
            }
        }
    }
    private void Chase(){
    }
}
