using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponList
{
  private List<Weapon> weaponList = new();

  public Weapon this[int i]
  {
    get
    {
      if (i < weaponList.Count && i >= 0)
        return weaponList[i];
      return null;
    }
  }

  public Weapon[] ToArray()
  {
    return weaponList.ToArray();
  }

  public int Count
  {
    get
    {
      return weaponList.Count;
    }
  }

  public void Remove(int index)
  {
    if (index >= 0 && index < weaponList.Count)
    {
      weaponList.RemoveAt(index);
    }
    BackPack.SaveChange();
  }

  public void Add(Weapon weapon)
  {
    weaponList.Add(weapon);
    BackPack.SaveChange();
  }

  public void UpdateList()
  {
    for (int index = 0; index < weaponList.Count; index++)
    {
      if (weaponList[index].durability <= 0)
      {
        weaponList.RemoveAt(index);
        index--;
      }
    }
    BackPack.SaveChange();
  }

  public void init(string[] descriptions)
  {
    weaponList.Clear();
    for (int index = 0; index < descriptions.Length; index++)
    {
      var helper = JsonUtility.FromJson<WeaponSerializeHelpeer>(descriptions[index]);
      var weapon = new Weapon(helper);
      weaponList.Add(weapon);
    }
  }
}


