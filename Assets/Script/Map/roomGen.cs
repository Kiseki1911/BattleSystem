using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class roomGen: MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.Tilemaps.Tilemap map;
    public Vector3Int leftDoor;
    public Vector3Int rightDoor;
    public Vector3Int topDoor;
    public Vector3Int bottomDoor;
    public GameObject left;
    public GameObject right;
    public GameObject top;
    public GameObject bottom;
    public GameObject enemy;
    public Vector3Int[] enemyPos;
    public Vector3Int teleportPos;
    [Range(0,15)]
    [SerializeField] public uint doors=0;
    private int deathCount=0;
    private List<GameObject> realDoors=new List<GameObject>();
    private void Awake() {
        if((doors&1)==1){
            realDoors.Add(Instantiate(left,leftDoor+transform.position,Quaternion.identity,transform.parent));
		}
		if((doors&2)>>1==1){
            realDoors.Add(Instantiate(right,rightDoor+transform.position,Quaternion.identity,transform.parent));
		}
		if((doors&4)>>2==1){
            realDoors.Add(Instantiate(top,topDoor+transform.position,Quaternion.identity,transform.parent));
		}
		if((doors&8)>>3==1){
            realDoors.Add(Instantiate(bottom,bottomDoor+transform.position,Quaternion.identity,transform.parent));
		}
        foreach (var item in enemyPos)
        {
            Instantiate(enemy,item+transform.position,Quaternion.identity,transform);
        }
    }
    public void onEnemyDeath(){
        deathCount++;
        if(deathCount>=enemyPos.Length){
            foreach (var item in realDoors)
            {
                item.GetComponentInChildren<Door>().openDoor();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"){
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            foreach (var item in realDoors)
            {
                item.GetComponentInChildren<Door>().lockDoor();
            }
            GetComponent<Collider2D>().enabled=false;
            
        }
    }
}
