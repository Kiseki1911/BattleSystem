using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MaterialList", menuName = "StaticObject/MaterialList")]
public class MaterialList : ScriptableObject {
    [SerializeField] private List<MaterialInstance> materialList;

    public MaterialInstance this[int i]{
        get{
          if(i<materialList.Count && i >= 0)
            return materialList[i];
          return null;
        }
    }

    public MaterialInstance[] ToArray(){
        return materialList.ToArray();
    }

    public int Count{get{
        return materialList.Count;
    }}

    public void Remove(int index){
      if(index>=0 && index<materialList.Count){
        materialList.RemoveAt(index);
      }
      BackPack.SaveChange();
    }
    public void Add(MaterialInstance material){
      materialList.Add(material);
      BackPack.SaveChange();
    }
    public void Add(Material material){
      bool find=false;
      foreach (var item in materialList)
      {
        if(item.material.id==material.id){
          item.count++;
          find=true;
        }
      }
      if(!find){
        Add(new MaterialInstance{material=material,count=1});
      }
      BackPack.SaveChange();
    }
    public void UpdateList(){
      for (int index = 0; index < materialList.Count; index++)
      {
        if(materialList[index].count <= 0){
          materialList.RemoveAt(index);
          index--;
        }
      }
      BackPack.SaveChange();
    }

    public void init(MaterialTable table,int[] materialId,int[] count){
      materialList.Clear();
      for(int index= 0;index<materialId.Length;index++){
        var instanceMaterial=new MaterialInstance();
        instanceMaterial.material=table[materialId[index]];
        instanceMaterial.count=count[index];
        materialList.Add(instanceMaterial);
      }
    }
}


