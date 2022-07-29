using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public enum WeaponType
{
	nulltype	=	0,
	sword			=	1,
	axe				=	2,
	reaper		=	3
}
//Weapon class
[System.Serializable]
public class Weapon :ScriptableItem {
	public float mass;
	public float hardness;
	private float _damageRate=-1;
	private float _agileRate=-1;
  public float durability = 100;
	public float damageRate{
		get{
			if(_damageRate==-1){
				_damageRate=calculate.calculateDamage(this);
			}
			return _damageRate;
			}
		}
	public float agileRate{
		get{
			if(_agileRate==-1){
				_agileRate=calculate.calculateAgile(this);
			}
			return _agileRate;
			}
		}
	public Vector2 massCenter;
	public Vector2 handle;
	public Vector2 diraction;
	public Vector2 farestPoint{get{
		float maxDistance=0;
		Vector2 res=Vector2.zero;
		for (int i = 0; i < matrixSize; i++)
		{
			for (int j = 0; j < matrixSize; j++)
			{
				if(materials[i,j]!=0){
					Vector2 temp=new Vector2(i,j);
					if((temp-handle).magnitude>maxDistance){
						maxDistance = (temp-handle).magnitude;
						res=temp;
					}
				}
			}
		}
		return res;
	}}
	public bool sharped {get{
		int count=0;
		int sharpedCount=0;
		for (int i = 0; i < matrixSize; i++)
		{
			for (int j = 0; j < matrixSize; j++)
			{
				count+=(materials[i,i]>0)?1:0;
				sharpedCount+=sharpedMatrix[i,j];
			}
		}
		return (float)sharpedCount/(float)count>.5f;
	}}
	//[SerializeField] private Texture2D texture2D;
	[SerializeField] private WeaponType type;
	public WeaponType Type{
		set{type	=	value;
		calculate	=	(value==WeaponType.sword)	?	new SwordCalculate():
								(value==WeaponType.axe)		?	new AxeCalculate():
								(value==WeaponType.reaper)	?	new ReaperCalculate():
								(CalculateStrategy)null;}
		get{return type;}
	}
	public List<SpecialEffect> effects;
	private Dictionary<DmgType,SpecialEffect> effectMap;
	private int[,] materials;
	private int[,] sharpedMatrix;
	public int matrixSize;
	private int materialCount;
	private bool foraged;
	private CalculateStrategy calculate=null;
	//initial weapon with type and size
	public Weapon(WeaponType type=WeaponType.nulltype,int size=36){
		mass	=	0;
		hardness	=	0;
		massCenter	=	Vector2.zero;
		handle	=	Vector2.zero;
		diraction = Vector2.zero;
		Type=type;
		materials = new int [size,size];
		sharpedMatrix = new int [size,size];
		matrixSize	=	size;
		foraged = false;
		effects = new List<SpecialEffect>();
		effectMap=new Dictionary<DmgType, SpecialEffect>();
	}
	public Weapon(WeaponSerializeHelpeer helper){
		mass = helper.mass;
    hardness = helper.hardness;
    _damageRate = helper._damageRate;
    _agileRate = helper._agileRate;
    massCenter = helper.massCenter;
    handle = helper.handle;
    Type = helper.type;
    matrixSize = helper.matrixSize;
		materials = new int [matrixSize,matrixSize];
		sharpedMatrix = new int [matrixSize,matrixSize];
		matrix=helper.matrix;
    durability = helper.durability;
		effects=helper.effects;
	}
	//live update for mass and material matrix
	public Tuple<float,Vector2> onChangeMaterial(Vector2 position,int material){
		materialCount += (materials[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)]==0)?1:0;
		Vector2 massVector	=	mass * massCenter+BackPack.Instance.materialTable[material].mass * position - this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].mass * position;
		mass	+=	BackPack.Instance.materialTable[material].mass - this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].mass;
		hardness += BackPack.Instance.materialTable[material].hardness - this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].hardness;
		massCenter	=	(mass==0)?Vector2.zero:	massVector/mass;
		materials[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)] = material;
		if(effectMap.ContainsKey(BackPack.Instance.materialTable[material].effect.type)){
			var temp=effectMap[BackPack.Instance.materialTable[material].effect.type];
			temp.strength+=temp.strength/10;
			effectMap[BackPack.Instance.materialTable[material].effect.type]=temp;
		}
		else{
			var temp=BackPack.Instance.materialTable[material].effect;
			temp.strength/=10;
			effectMap[BackPack.Instance.materialTable[material].effect.type]=temp;
		}
		if(effectMap.ContainsKey(this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].effect.type)){
			var temp=effectMap[this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].effect.type];
			temp.strength-=temp.strength/10;
			effectMap[this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].effect.type]=temp;
		}
		else{
			var temp=BackPack.Instance.materialTable[material].effect;
			temp.strength/=10;
			effectMap[BackPack.Instance.materialTable[material].effect.type]=temp;
		}
		return new Tuple<float,Vector2>(mass,massCenter);
	}
	//update mass and matrix with id matrix
	public Tuple<float,Vector2> onChangeMaterial(int[,] materialmatrix){
		for (int i = 0; i < matrixSize; i++) {
			for (int j = 0; j < matrixSize; j++) {
				onChangeMaterial(new Vector2(j,i),materialmatrix[i,j]);
			}
		}
		Debug.Log(massCenter);
		return new Tuple<float,Vector2>(mass,massCenter);
	}
	public void onSharpMaterial(Vector2 position){
		hardness -= this[Mathf.FloorToInt(position.y),Mathf.FloorToInt(position.x)].hardness/2f;
		sharpedMatrix[Mathf.FloorToInt(position.y),Mathf.FloorToInt(position.x)]=1;
	}

	public void onFinishForge(int[,] materialmatrix,Vector2 pos){
		if(foraged)
			return;
		for (int i = 0; i < matrixSize; i++) {
			for (int j = 0; j < matrixSize; j++) {
				if(materialmatrix[i,j]==1)
				onSharpMaterial(new Vector2(i,j));
			}
		}
		foreach (var item in effectMap.ToList())
		{
			effects.Add(item.Value);
		}
		hardness /= materialCount;
		setHandle(pos);
		foraged = true;

	}

	public void setHandle(Vector2 pos){
		handle.x=pos.x;
		handle.y=36-pos.y;
	}
	public Material this[int i,int j]{
		get{
			if(materials[i,j]>0){
				var res	=	BackPack.Instance.materialTable[materials[i,j]];
				res.sharped	=	sharpedMatrix[i,j]==1;
				return res;
			}
			else
			return BackPack.Instance.materialTable[0];
		}
		set{
			materials[i,j]	=	value.id;
		}
	}
  private Texture2D _texture;
  public Texture2D dynamicTexture{
    get{
      Highlight.Instance.MainTexture = texture;
      Highlight.Instance.HighlightMatrix = sharpedMatrix;
      return Highlight.Instance.result;
    }
  }
	public Texture2D texture{
		get{
      // TODO: 当前缓存模式没有进行版本管理，当对武器进行修改时无法更新贴图
      if(_texture!=null) return _texture;
      Texture2D res = new Texture2D(matrixSize,matrixSize);
      res.filterMode = FilterMode.Point;
			for (int i = 0; i < matrixSize; i++)
			{
				for (int j = 0; j < matrixSize; j++)
				{
					res.SetPixel( i,matrixSize-1-j,this[i,j].color);
				}
			}
      res.Apply();
      _texture = res;
			return res;
		}}
	public string matrix{
		get{
			string res="";
			for (int i = 0; i < matrixSize; i++)
			{
				for (int j = 0; j < matrixSize; j++)
				{
					res+=this[i,j].id*((this[i,j].sharped)?-1:1);
					res+=",";
				}
				res+='\n';
			}
			return res;
		}
	set{
		var lines = value.Split('\n');
		for (int i = 0; i < matrixSize; i++)
		{
			var cellstr = lines[i].Split(',');
			for (int j = 0; j < matrixSize; j++)
			{
				int cell = int.Parse(cellstr[j]);
				materials[i,j]=Math.Abs(cell);
				sharpedMatrix[i,j]=(cell<0)?1:0;
			}
		}
	}}	
}