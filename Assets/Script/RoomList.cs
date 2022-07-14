using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class DunRoom 
{
    [SerializeField] private List<roomGen> room;
    [SerializeField] public int height;
    [SerializeField] public int width;
    [Range(0,15)]
    [SerializeField] public uint doors=0;
    public roomGen Room{get{
        var res=room[Random.Range(0,room.Count)];
        return res;
    }}
    public DunRoom(DunRoom f){
        room=f.room;
        height=f.height;
        width=f.width;
        doors=f.doors;
    }
}

[CreateAssetMenu(fileName = "ScriptableObj", menuName = "Script/ScriptableObj")]
public class RoomList : ScriptableObject
{
    [SerializeField] private List<DunRoom> roomList;

    public DunRoom this[int i]{
        get{
            return roomList[i];
        }
    }

    public DunRoom[] ToArray(){
        return roomList.ToArray();
    }

    public int Count{get{
        return roomList.Count;
    }}

    public roomGen getRoom(int height,int width){
        foreach (var item in roomList)
        {
            if(height==item.height&&width==item.width){
                return item.Room;
            }
        }
        throw new System.Exception("no match room");
    }
}
