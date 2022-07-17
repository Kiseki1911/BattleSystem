using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveAndLoad
{
  const string BASE_FOLDER = "/save/";
  public static void Save(object SaveTarget, string FileName)
  {
    string Path = getSavePath();
    if (!Directory.Exists(Path))
    {
      Directory.CreateDirectory(Path);
    }
    FileStream File = System.IO.File.Create(Path + FileName);
    BinaryHandler.Save(SaveTarget, File);
  }
  public static object Load(string FileName)
  {
    string Path = getSavePath();
    object TargetObj;

    if (!Directory.Exists(Path) || !File.Exists(Path + FileName))
    {
      return null;
    }

    FileStream TargetFile = File.Open(Path + FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
    TargetObj = BinaryHandler.Load(TargetFile);

    return TargetObj;
  }

  static string getSavePath()
  {
    string savePath = Application.persistentDataPath + BASE_FOLDER;
#if UNITY_EDITOR
    savePath = Application.dataPath + BASE_FOLDER;
#endif
    savePath += "/";
    return savePath;
  }

  class BinaryHandler
  {
    public static void Save(object Obj, FileStream File)
    {
      BinaryFormatter formatter = new();
      formatter.Serialize(File, Obj);
      File.Close();
    }
    public static object Load(FileStream File)
    {
      object TargetObj;
      BinaryFormatter formatter = new();
      TargetObj = formatter.Deserialize(File);
      File.Close();
      return TargetObj;
    }
  }
}
