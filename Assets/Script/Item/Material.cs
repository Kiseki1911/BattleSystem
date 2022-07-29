/*	Author: 			Runtong Wang
*		Discription:	Material class
*		Date:					6/19/2022
*		Version:			1.0.0
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialInstance{
  public Material material;
  public int count;
}

[CreateAssetMenu(fileName = "Material", menuName = "StaticObject/MaterialItem")]
public class Material : ScriptableItem {
	public int id=0;
	public int mass=0;
	public int hardness=0;
	public bool sharped=false;
	public SpecialEffect effect;
    public Color color;
    //public Texture2D texture;
    public Material(){
        id=0;
        mass=1;
        hardness=1;
        color = new Color(0,0,0,1);
    }
	public Material(int id,int mass,int hardness){
        this.id=id;
        this.mass=mass;
        this.hardness=hardness;
    }
}

public class ScriptableItem : ScriptableObject{
    public string title = "";
    public float price;
}

[System.Serializable]
public struct SpecialEffect{
	public string name;
    public string Discription;
    public DmgType type;
    public int strength;
}

public enum DmgType{
    PHYSICS=0,
    FIRE=1,
    WATER=2,
    WIND=3,
    POISON=4
}