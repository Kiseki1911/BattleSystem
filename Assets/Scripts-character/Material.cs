/*	Author: 			Runtong Wang
*		Discription:	Material class
*		Date:					6/19/2022
*		Version:			1.0.0
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Material : item {
	public int id=0;
	public int mass=0;
	public int hardness=0;
	public bool sharped=false;
	public SpecialEffect effect;
    public Color color;
    //public Texture2D texture;
    public Material(){
        id=0;
        mass=0;
        hardness=0;
    }
	public Material(int id,int mass,int hardness){
        this.id=id;
        this.mass=mass;
        this.hardness=hardness;
    }
}

[System.Serializable]
public class item{
    public int count;
    public float price;
}

public struct SpecialEffect{
	uint id;
}