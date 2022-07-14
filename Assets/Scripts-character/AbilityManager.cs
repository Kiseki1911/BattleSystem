using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;
    public GameObject weaponPrefab;
    public GameObject player;

    public bool offHand=false;

    public Ability_SO ability_SO;
    //public GameObject 
    public Abilities GetAbDetails(int ID){
        return ability_SO.AbList.Find(i=>i.AbID == ID);
    }


    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CallAbility(int ID){
        Abilities inUse = GetAbDetails(ID);
        switch(ID){
            case 1:
                if(!offHand)
                {
                    Debug.Log("use ability "+ inUse.AbName);
                    GameObject proj = Instantiate(weaponPrefab,WeaponManager.Instance.weaponInHand.transform.position,WeaponManager.Instance.weaponInHand.transform.rotation);
                    offHand = true;
                    WeaponManager.Instance.weaponInHand.SetActive(!offHand);
                    proj.GetComponent<Projectile>().Enter(proj.transform.right);
                }
                break;
            case 2:
                if(!offHand){
                    Debug.Log("use ability "+ inUse.AbName);
                }
                break;

        }

    }

}
