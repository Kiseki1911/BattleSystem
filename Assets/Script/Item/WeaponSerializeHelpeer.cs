using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponSerializeHelpeer{
  public float mass;
	public float hardness;
	public float _damageRate=-1;
	public float _agileRate=-1;
  public Vector2 massCenter;
	public Vector2 handle;
  public WeaponType type;
  public int matrixSize;
  public string matrix;
  public float durability;
  public List<SpecialEffect> effects;
  public WeaponSerializeHelpeer(){
    
  }
  public WeaponSerializeHelpeer(Weapon weapon){
    mass = weapon.mass;
    hardness = weapon.hardness;
    _damageRate = weapon.damageRate;
    _agileRate = weapon.agileRate;
    massCenter = weapon.massCenter;
    handle = weapon.handle;
    type = weapon.Type;
    matrixSize = weapon.matrixSize;
    matrix = weapon.matrix;
    durability =weapon.durability;
    effects = weapon.effects;
  }
}