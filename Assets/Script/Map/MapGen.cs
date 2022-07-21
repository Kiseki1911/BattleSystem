using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public class Room : IEnumerable{
	public Vector3Int leftBotConer=Vector3Int.zero;
	public List<Room> nextRooms;
	public GCHandle previousRoom;
	public DunRoom room;
	public uint doors=0;
	public uint direction;//0 start,1 top,2 bottom,3 right,4 left
	public int branch;
	public Room(){
		branch=-1;
		nextRooms = new List<Room>();
		previousRoom=GCHandle.Alloc(null,GCHandleType.Pinned);
	}
	public Room(Room previous,uint direction){
		branch=-1;
		nextRooms = new List<Room>();
		previousRoom=GCHandle.Alloc(previous,GCHandleType.Pinned);
		this.direction=direction;
	}
	public IEnumerator GetEnumerator(){
		Room current=(Room)this.previousRoom.Target;
		while(current!=null){
			Room res=current;
			current = (Room)current.previousRoom.Target;
			yield return res;
		}
	}
}

public static class Generate{
	static int count=0;
	static public RoomList rooms;
	const int ROOMDEFUALTDISTANCE=5;
	static Room startRoom;
	public static Tuple<int,int> mapSize;
	static Queue<GCHandle> roomQueue = new Queue<GCHandle>();
	public static Room generateRooms(RoomList roomsIn, int num,Tuple<int,int> size){
		startRoom =new Room();
		rooms=roomsIn;
		mapSize = size;
		GCHandle startHandle=GCHandle.Alloc(startRoom,GCHandleType.Pinned);
		roomQueue.Enqueue(startHandle);
		num--;
		while (num>=0&&roomQueue.Count>0)
		{
			num=genHelper(num);
		}
		startHandle.Free();
		return startRoom;
	}
	public static int genHelper(int num){
		count=0;
		if(num<0)
			return -1;
		GCHandle roomHandle=roomQueue.Dequeue();
		Room room=(Room)roomHandle.Target;
		GENROOM:
		if(count++>10){
			return num;
		}
		List<uint> dirs=new List<uint>(new uint[]{1,2,3,4});
		room.room=new DunRoom(rooms[UnityEngine.Random.Range(0,rooms.Count)]);
		Room previous=(Room)room.previousRoom.Target;
		switch (room.direction)
		{
			case 0:{
				room.leftBotConer=Vector3Int.zero;
				room.branch=(num==0)?0:UnityEngine.Random.Range(1,(num>4)?5:num);
				break;
			}
			case 1:{
				room.leftBotConer=previous.leftBotConer+new Vector3Int(0,previous.room.height+ROOMDEFUALTDISTANCE);
				room.branch=(num==0)?0:UnityEngine.Random.Range(1,(num>3)?4:num);
				dirs.RemoveAt(0);
				break;
			}
			case 2:{
				room.leftBotConer=previous.leftBotConer-new Vector3Int(0,room.room.height+ROOMDEFUALTDISTANCE);
				room.branch=(num==0)?0:UnityEngine.Random.Range(1,(num>3)?4:num);
				dirs.RemoveAt(1);
				break;
			}
			case 3:{
				room.leftBotConer=previous.leftBotConer+new Vector3Int(previous.room.width+ROOMDEFUALTDISTANCE,0);
				room.branch=(num==0)?0:UnityEngine.Random.Range(1,(num>3)?4:num);
				dirs.RemoveAt(2);
				break;
			}
			case 4:{
				room.leftBotConer=previous.leftBotConer-new Vector3Int(room.room.width+ROOMDEFUALTDISTANCE,0);
				room.branch=(num==0)?0:UnityEngine.Random.Range(1,(num>3)?4:num);
				dirs.RemoveAt(3);
				break;
			}
			default:break;
		}
		foreach (Room item in room)
		{
			if(!((room.leftBotConer.x+room.room.width<item.leftBotConer.x)||(room.leftBotConer.x>item.leftBotConer.x+item.room.width)||(room.leftBotConer.y+room.room.height<item.leftBotConer.y)||(room.leftBotConer.y>item.leftBotConer.y+item.room.height))){
				uint dir=(uint)UnityEngine.Random.Range(0,4);
				room.direction=dir+1;
				goto GENROOM;
			}
				
			foreach (Room item1 in item.nextRooms)
			{
				if(!((room.leftBotConer.x+room.room.width<item1.leftBotConer.x)||(room.leftBotConer.x>item1.leftBotConer.x+item1.room.width)||(room.leftBotConer.y+room.room.height<item1.leftBotConer.y)||(room.leftBotConer.y>item1.leftBotConer.y+item1.room.height))){
					uint dir=(uint)UnityEngine.Random.Range(0,4);
					room.direction=dir+1;
					goto GENROOM;
				}
			}
		}
		//Debug.Log(room.branch);
		num-=room.branch;
		if(room.direction!=0){
			if(room.direction==1){
				previous.doors|=4;
			}
			else if(room.direction==2){
				previous.doors|=8;
			}
			else if(room.direction==3){
				previous.doors|=2;
			}
			else if(room.direction==4){
				previous.doors|=1;
			}
			if(room.direction==1){
				room.doors|=8;
			}
			else if(room.direction==2){
				room.doors|=4;
			}
			else if(room.direction==3){
				room.doors|=1;
			}
			else if(room.direction==4){
				room.doors|=2;
			}
			previous.nextRooms.Add(room);
		}
		for(int i=0;i<room.branch;i++){
			int dir=UnityEngine.Random.Range(0,dirs.Count);
			Room nextR = new Room(room,dirs[dir]);
			dirs.RemoveAt(dir);
			roomQueue.Enqueue(GCHandle.Alloc(nextR,GCHandleType.Pinned));
		}
		roomHandle.Free();
		return num;
	}
}

public class MapGen : MonoBehaviour
{
	// Start is called before the first frame update
	public int mapH=0;
	public int mapW=0;
	public int roomNum;

	public RoomList rooms;
	public UnityEngine.Tilemaps.Tilemap map;
	public UnityEngine.Tilemaps.Tilemap objMap;
	public GridLayout grid;
	public UnityEngine.Tilemaps.TileBase road;
	public Stack<(roomGen,Vector3Int)> roomStack=new Stack<(roomGen,Vector3Int)>();
 	public UnityEngine.Tilemaps.TileBase wall;
	private Vector3Int playerPos;
	private Vector3Int endPos;
	private List<Vector3Int> teleportCandidate = new List<Vector3Int>();
	public Room startRoom=null;
	public GameObject telepoint;
	void showRoom(){
		showRoomHelper(startRoom);
		int count=0;
		foreach (var item in teleportCandidate)
		{
			if(UnityEngine.Random.Range(0,teleportCandidate.Count-count)==0)
			{
				Instantiate(telepoint,item,Quaternion.identity,objMap.transform);
				count=0;
			}
			else
				count++;
		}
		map.SwapTile(road,null);
	}
	void showRoomHelper(Room room){
		roomGen roomdata= room.room.Room;
		roomdata.doors=room.doors;
		UnityEngine.Tilemaps.Tilemap roomS=roomdata.map;
		Vector3Int doorPos=new Vector3Int();
		if(room.direction==0){
			playerPos=roomdata.teleportPos+room.leftBotConer;
		}
		for (int i = 0; i < room.room.width; i++)
		{
			for (int j = 0; j < room.room.height; j++)
			{
				if(roomS.HasTile(new Vector3Int(i,j)))
				map.SetTile(room.leftBotConer+new Vector3Int(i,j),wall);
			}
		}
		Instantiate(roomdata.gameObject,room.leftBotConer,Quaternion.identity,objMap.transform);
		if((room.doors&1)==1){
			doorPos=room.leftBotConer+roomdata.leftDoor;
			map.SetTile(doorPos,road);
		}
		if((room.doors&2)>>1==1){
			doorPos=room.leftBotConer+roomdata.rightDoor;
			map.SetTile(doorPos,road);

		}
		if((room.doors&4)>>2==1){
			doorPos=room.leftBotConer+roomdata.topDoor;
			map.SetTile(doorPos,road);

		}
		if((room.doors&8)>>3==1){
			doorPos=room.leftBotConer+roomdata.bottomDoor;
			map.SetTile(doorPos,road);

		}
		if(roomStack.Count>0){
			var previous=roomStack.Peek();
			switch (room.direction)
			{
				case 1:{
					drawRoad(previous.Item1.topDoor+previous.Item2,roomdata.bottomDoor+room.leftBotConer);
					break;
				}
				case 2:{
					drawRoad(previous.Item1.bottomDoor+previous.Item2,roomdata.topDoor+room.leftBotConer);
					break;
				}
				case 3:{
					drawRoad(previous.Item1.rightDoor+previous.Item2,roomdata.leftDoor+room.leftBotConer);
					break;
				}
				case 4:{
					drawRoad(previous.Item1.leftDoor+previous.Item2,roomdata.rightDoor+room.leftBotConer);
					break;
				}
			}
		}
		roomStack.Push((roomdata,room.leftBotConer));
		foreach (var item in room.nextRooms)
		{
			showRoomHelper(item);
		}
		if(room.branch==0){
			teleportCandidate.Add(room.leftBotConer+roomdata.teleportPos);
		}
		roomStack.Pop();
	}
	void drawRoad(Vector3Int start,Vector3Int end)
	{
		Vector3Int point=start;
		bool horizontal=map.HasTile(start+Vector3Int.up);
		bool changed=false;
		while(point!=end){
			if(horizontal){
				if(end.x-point.x>0){
					point+=Vector3Int.right;
					map.SetTile(point,road);
				}
				else if(end.x-point.x<0) {
					point-=Vector3Int.right;
					map.SetTile(point,road);
				}
				else{
					if(!map.HasTile(point+Vector3Int.right))
						map.SetTile(point+Vector3Int.right,wall);
					if(!map.HasTile(point+Vector3Int.left))
						map.SetTile(point+Vector3Int.left,wall);
					horizontal=!horizontal;
					changed=true;
				}
				/*if(!map.HasTile(point+Vector3Int.up))
					map.SetTile(point+Vector3Int.up,wall);
				if(!map.HasTile(point+Vector3Int.down))
					map.SetTile(point+Vector3Int.down,wall);*/
				if(Math.Abs(end.x-point.x)<Math.Abs(end.x-start.x)/2&&!changed){
					if(!map.HasTile(point+Vector3Int.right))
						map.SetTile(point+Vector3Int.right,wall);
					if(!map.HasTile(point+Vector3Int.left))
						map.SetTile(point+Vector3Int.left,wall);
					horizontal=!horizontal;
					changed=true;
				}
			}
			else{
				if(end.y-point.y>0){
					point+=Vector3Int.up;
					map.SetTile(point,road);
				}
				else if(end.y-point.y<0) {
					point-=Vector3Int.up;
					map.SetTile(point,road);
				}
				else{
					horizontal=!horizontal;
					changed=true;
					if(!map.HasTile(point+Vector3Int.up))
						map.SetTile(point+Vector3Int.up,wall);
					if(!map.HasTile(point+Vector3Int.down))
						map.SetTile(point+Vector3Int.down,wall);
				}
				/*if(!map.HasTile(point+Vector3Int.right))
					map.SetTile(point+Vector3Int.right,wall);
				if(!map.HasTile(point+Vector3Int.left))
					map.SetTile(point+Vector3Int.left,wall);*/
				if(Math.Abs(end.y-point.y)<Math.Abs(end.y-start.y)/2&&!changed){
					if(!map.HasTile(point+Vector3Int.up))
						map.SetTile(point+Vector3Int.up,wall);
					if(!map.HasTile(point+Vector3Int.down))
						map.SetTile(point+Vector3Int.down,wall);
					horizontal=!horizontal;
					changed=true;
				}
			}
			if(point==end){
				break;
			}
			if(!map.HasTile(point+Vector3Int.up+Vector3Int.right)){
				map.SetTile(point+Vector3Int.up+Vector3Int.right,wall);
			}
			if(!map.HasTile(point-Vector3Int.up+Vector3Int.right)){
				map.SetTile(point-Vector3Int.up+Vector3Int.right,wall);
			}
			if(!map.HasTile(point+Vector3Int.up-Vector3Int.right)){
				map.SetTile(point+Vector3Int.up-Vector3Int.right,wall);
			}
			if(!map.HasTile(point-Vector3Int.up-Vector3Int.right)){
				map.SetTile(point-Vector3Int.up-Vector3Int.right,wall);
			}
		}
	}
	public Vector3Int creatMap(){
		startRoom = Generate.generateRooms(rooms,roomNum,new Tuple<int, int>(mapH,mapW));
		map.ClearAllTiles();
		teleportCandidate.Clear();
		for (int i = 0; i < objMap.transform.childCount; i++)
		{
			Destroy(objMap.transform.GetChild(i).gameObject);
		}		
		showRoom();
		return playerPos;
	}
}
