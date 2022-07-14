using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum WeaponType
{
	nulltype	=	0,
	sword			=	1,
	Jian			=	2,//锏
	axe				=	3,
	hammer		=	4,
	reaper		=	5
}
//Weapon class
[System.Serializable]
public class Weapon :item {
	public float mass;
	public float hardness;
	public float damageRate{get{return calculate.calculateDamage(this);}}
	public float agileRate{get{return calculate.calculateAgile(this);}}
	public Vector2 massCenter;
	public Vector2 handle;
	public Vector2 diraction;
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
	public WeaponType type;
	public List<SpecialEffect> effects;
	private int[,] materials;
	private int[,] sharpedMatrix;
	private int matrixSize;
	private CalculateStrategy calculate=null;
	//initial weapon with type and size
	public Weapon(WeaponType type,int size){
		mass	=	0;
		hardness	=	0;
		massCenter	=	Vector2.zero;
		handle	=	Vector2.zero;
		diraction = Vector2.zero;
		this.type	=	type;
		calculate	=	(type==WeaponType.sword)	?	new SwordCalculate():
								(type==WeaponType.Jian)		?	new JianCalculate():
								(type==WeaponType.axe)		?	new AxeCalculate():
								(type==WeaponType.hammer)	?	new HammerCalculate():
								(type==WeaponType.reaper)	?	new ReaperCalculate():
								(CalculateStrategy)null;
		materials = new int [size,size];
		sharpedMatrix = new int [size,size];
		matrixSize	=	size;
	}
	//live update for mass and material matrix
	public Tuple<float,Vector2> onChangeMaterial(Vector2 position,int material){
		Vector2 massVector	=	mass*massCenter+Global.materials[material].mass*position-this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].mass*position;
		mass	+=	Global.materials[material].mass-this[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)].mass;
		massCenter	=	(mass==0)?Vector2.zero:	massVector/mass;
		materials[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)]	=	material;
		return new Tuple<float,Vector2>(mass,massCenter);
	}
	//update mass and matrix with id matrix
	public Tuple<float,Vector2> onChangeMaterial(int[,] materialmatrix){
		for (int i = 0; i < matrixSize; i++) {
			for (int j = 0; j < matrixSize; j++) {
				onChangeMaterial(new Vector2(i,j),materialmatrix[i,j]);
			}
		}
		return new Tuple<float,Vector2>(mass,massCenter);
	}
	public void onSharpMaterial(Vector2 position){
		sharpedMatrix[Mathf.FloorToInt(position.x),Mathf.FloorToInt(position.y)]=1;
	}
	public Material this[int i,int j]{
		get{
			if(materials[i,j]>0){
				var res	=	Global.materials[materials[i,j]];
				res.sharped	=	sharpedMatrix[i,j]==1;
				return res;
			}
			
			else
			return new Material();
		}
		set{
			materials[i,j]	=	value.id;
		}
	}
	//get damage rate for different weapon type
	
}
static public class Global {
	//material list
	public static Material [] materials=new Material[3];
}