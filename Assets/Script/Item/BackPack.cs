using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackPack : MonoBehaviour
{
  public MaterialList materials;
  public MaterialTable materialTable;
  public WeaponList weaponList;
  public static BackPack Instance;
  public int maxWeaponVolumn = 6;
  public int maxMaterialVolumn = 18;
  private void Awake()
  {
    if (Instance == null)
      Instance = this;
    weaponList = new();

    // 尝试读取数据
    var saveData = (SerializedBackpack)SaveAndLoad.Load("backpack.save");
    if (saveData != null)
    {
      maxWeaponVolumn = saveData.maxWeaponVolumn;
      maxMaterialVolumn = saveData.maxMaterialVolumn;
      materials.init(materialTable, saveData.materialIds, saveData.materialCounts);
      weaponList.init(saveData.weaponDescription);
      Debug.Log($"存档读取完成: 存档日期{saveData.date}");
    }
    else
    {
      var helper = new WeaponSerializeHelpeer();
      helper.mass = 2390f;
      helper.hardness = 0.0022f;
      helper._damageRate = 10f;
      helper._agileRate = 1.55f;
      helper.massCenter = new Vector2(16 , 12);
      helper.handle = new Vector2(11, 9);
      helper.matrixSize = 36;
      helper.type = WeaponType.sword;
      helper.matrix = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,\n0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,6,1,6,6,1,4,4,1,1,1,0,0,0,\n0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,1,6,1,1,4,4,1,1,1,1,0,0,\n0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,1,6,1,1,4,4,1,1,1,1,0,0,\n0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,1,6,1,1,4,4,1,1,1,1,0,0,\n0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,6,1,6,6,1,4,4,1,1,1,0,0,0,\n0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,\n0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,\n";
      helper.durability = 100;
      helper.effects = new List<SpecialEffect>();
      var weapon = new Weapon(helper);
      weaponList.Add(weapon);
    }
  }

  public static void SaveChange()
  {
    BackPack.Instance.saveChange();
  }

  void saveChange()
  {
    SaveAndLoad.Save(new SerializedBackpack(this), "backpack.save");
  }

}

[Serializable]
class SerializedBackpack
{
  public int[] materialIds;
  public int[] materialCounts;
  public int maxWeaponVolumn;
  public int maxMaterialVolumn;
  public string[] weaponDescription;
  public string date;
  public SerializedBackpack(BackPack bag)
  {
    maxWeaponVolumn = bag.maxWeaponVolumn;
    maxMaterialVolumn = bag.maxMaterialVolumn;
    materialIds = new int[bag.materials.Count];
    materialCounts = new int[bag.materials.Count];
    date = DateTime.Now.ToString();
    var index = 0;
    foreach (var material in bag.materials.ToArray())
    {
      materialIds[index] = material.material.id;
      materialCounts[index] = material.count;
      index++;
    }
    weaponDescription = new string[bag.weaponList.Count];

    index = 0;
    foreach (var weapon in bag.weaponList.ToArray())
    {
      var SerializedWeapon = new WeaponSerializeHelpeer(weapon);
      weaponDescription[index++] = JsonUtility.ToJson(SerializedWeapon);
    }
  }
  public override string ToString()
  {
    return JsonUtility.ToJson(this);
  }
}