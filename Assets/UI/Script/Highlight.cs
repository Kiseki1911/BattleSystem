using UnityEngine;

public class Highlight : MonoBehaviour
{
  public static Highlight Instance;
  public Texture2D result;
  [SerializeField] private RenderTexture texture;

  private Texture _MainTexture;
  public Texture MainTexture
  {
    set
    {
      if (_MainTexture != value)
      {
        mesh.material.SetTexture("_MainTex", value);
        if (result != null) Texture.Destroy(result);
        result = new Texture2D(value.width, value.height, TextureFormat.ARGB32, false);
        result.filterMode = FilterMode.Point;
      }
      _MainTexture = value;
    }
  }
  public Texture HighlightTexture
  {
    set
    {
      mesh.material.SetTexture("_Tex2", value);
    }
  }

  private int[,] _HighlightMatrix;
  public int[,] HighlightMatrix
  {
    set
    {
      if (_HighlightMatrix != value)
      {
        HighlightTexture = Matrix2Texture(value, Color.white);
      }
      _HighlightMatrix = value;
    }
  }
  private MeshRenderer mesh;
  void Start()
  {
    Instance = this;
    mesh = GetComponent<MeshRenderer>();
  }

  private void Update()
  {
    if (result != null)
    {
      RenderTexture.active = texture;
      result.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
      result.Apply();
      RenderTexture.active = null;
    }
  }

  public static Texture Matrix2Texture(int[,] matrix, Color? forceColor)
  {
    var pixelNum = (int)matrix.GetLongLength(0);
    var tex = new Texture2D(pixelNum, pixelNum, TextureFormat.ARGB32, false);

    tex.filterMode = FilterMode.Point;
    for (int x = 0; x < pixelNum; x++)
    {
      for (int y = 0; y < pixelNum; y++)
      {
        var value = matrix[y, x];
        if (value == 0)
        {
          tex.SetPixel(y, pixelNum - 1 - x, Color.clear);
        }
        else
        {
          if (forceColor != null)
          {
            tex.SetPixel(y, pixelNum - 1 - x, (Color)forceColor);
          }
          else
          {
            tex.SetPixel(y, pixelNum - 1 - x, BackPack.Instance.materialTable[value].color);
          }
        }
      }
    }
    tex.Apply();

    return tex;

  }


}
