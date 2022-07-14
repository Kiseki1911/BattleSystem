using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class roomGen
{
    // Start is called before the first frame update
    public UnityEngine.Tilemaps.Tilemap map;
    public Vector3Int leftDoor;
    public Vector3Int rightDoor;
    public Vector3Int topDoor;
    public Vector3Int bottomDoor;
    public Vector3Int[] enemyPos;
    public Vector3Int teleportPos;
}
