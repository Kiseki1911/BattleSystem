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
		return 25f;
	}
	public float calculateAgile(Weapon weapon){
		return 0.8f;
	}
}
public class JianCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		return 0;
	}
	public float calculateAgile(Weapon weapon){
		return 0;
	}
}
public class AxeCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		return 0;
	}
	public float calculateAgile(Weapon weapon){
		return 0;
	}
}
public class HammerCalculate : CalculateStrategy{
	public float calculateDamage(Weapon weapon){
		return 0;
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