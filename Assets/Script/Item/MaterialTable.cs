using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MaterialTable", menuName = "StaticObject/MaterialTable")]
public class MaterialTable : ScriptableObject {
    [SerializeField] private List<Material> materialList;

    public Material this[int i]{
        get{
          if(i<materialList.Count)
            return materialList[i];
            return null;
        }
    }

    public Material[] ToArray(){
        return materialList.ToArray();
    }

    public int Count{get{
        return materialList.Count;
    }}
}
