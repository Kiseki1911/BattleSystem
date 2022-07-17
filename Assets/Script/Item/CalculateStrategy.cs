/*	Author: 			Runtong Wang
*		Discription:	value calculate for each weapon type
*		Date:					6/19/2022
*		Version:			1.0.0
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		return -1;
	}
	public float calculateAgile(Weapon weapon){
		return -1;
	}
}

public class SwordCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		float dmg=weapon.mass*0.5f;
		dmg*=(Vector2.Dot((weapon.farestPoint-weapon.handle),(weapon.massCenter-weapon.handle)))/((weapon.farestPoint-weapon.handle).magnitude*(weapon.massCenter-weapon.handle).magnitude/50f);
		dmg*=weapon.hardness/5f;
		return dmg;
	}
	public float calculateAgile(Weapon weapon){
		float agi=weapon.mass*0.5f;
		agi/=(weapon.massCenter-weapon.handle).magnitude;
		agi*=(Vector2.Dot((weapon.farestPoint-weapon.handle),(weapon.massCenter-weapon.handle)));
		return agi;
	}
}
public class JianCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		float dmg=weapon.mass*0.2f;
		dmg*=(Vector2.Dot((weapon.farestPoint-weapon.handle),(weapon.massCenter-weapon.handle)))/((weapon.farestPoint-weapon.handle).magnitude*(weapon.massCenter-weapon.handle).magnitude/50f);
		dmg*=weapon.hardness/8f;
		return dmg;
	}
	public float calculateAgile(Weapon weapon){
		float agi=weapon.mass*0.5f;
		agi/=(.5f+(weapon.massCenter-weapon.handle).magnitude);
		agi*=(Vector2.Dot((weapon.farestPoint-weapon.handle),(weapon.massCenter-weapon.handle)));
		return agi;
	}
}
public class AxeCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		float dmg=weapon.mass*.3f;
		float ratio=(Vector2.Dot((weapon.farestPoint-weapon.handle),(weapon.massCenter-weapon.handle)))/((weapon.farestPoint-weapon.handle).magnitude*(weapon.massCenter-weapon.handle).magnitude/50f);
		ratio+=0.25f;
		ratio/=1.25f;
		dmg*=ratio;
		dmg*=weapon.hardness/7f;
		return dmg;
	}
	public float calculateAgile(Weapon weapon){
		return 0;
	}
}
public class HammerCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		float dmg=weapon.mass*.1f;
		dmg=weapon.mass*0.5f;float ratio=(Vector2.Dot((weapon.farestPoint-weapon.handle),(weapon.massCenter-weapon.handle)))/((weapon.farestPoint-weapon.handle).magnitude*(weapon.massCenter-weapon.handle).magnitude/50f);
		ratio+=0.25f;
		ratio/=1.25f;
		dmg*=ratio;
		dmg*=weapon.hardness/10f;
		return dmg;
	}
	public float calculateAgile(Weapon weapon){
		return 0;
	}
}
public class ReaperCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		return 0;
	}
	public float calculateAgile(Weapon weapon){
		return 0;
	}
}