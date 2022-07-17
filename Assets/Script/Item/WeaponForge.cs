/*	Author: 			Runtong Wang
*		Discription:	Weapon and WeaponForge class
*		Date:					6/19/2022
*		Version:			1.0.0
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Global variables
static public class Global {
	//material list
	public static Material [] materials=new Material[3];
}
//SpecialEffect struct


//just for test mass and mass center of weapon
public static class WeaponForge {
	public static Weapon weapon;
	public static Func<int[,],Tuple<float,Vector2>> canvasUpdateCallback	=	new Func<int[,],Tuple<float,Vector2>>((int[,] m)=>{
		return new Tuple<float, Vector2>(-1f,-1*Vector2.one);
	});
	public static Action<Vector2> setHandle;
	public static Action<Vector2> canvasSharpCallback;
	public static void init(int size) {
		weapon	=	new Weapon(WeaponType.sword,size);
		canvasUpdateCallback	=	new Func<int[,],Tuple<float,Vector2>>(weapon.onChangeMaterial);
		canvasSharpCallback		=	new Action<Vector2>(weapon.onSharpMaterial);
	}
}
