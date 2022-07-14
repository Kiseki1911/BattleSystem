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
    public Door left;
    public Door right;
    public Door top;
    public Door bottom;
    public Vector3Int[] enemyPos;
    public Vector3Int teleportPos;
    [Range(0,15)]
    [SerializeField] public uint doors=0;
    private void Awake() {
        if((doors&1)==1){
			Instantiate(left.gameObject,leftDoor+gameObject.transform.position,Quaternion.identity,gameObject.transform);
		}
		if((doors&2)>>1==1){
			Instantiate(right.gameObject,rightDoor+gameObject.transform.position,Quaternion.identity,gameObject.transform);
		}
		if((doors&4)>>2==1){
			Instantiate(top.gameObject,topDoor+gameObject.transform.position,Quaternion.identity,gameObject.transform);
		}
		if((doors&8)>>3==1){
			Instantiate(bottom.gameObject,bottomDoor+gameObject.transform.position,Quaternion.identity,gameObject.transform);
		}
    }
}
