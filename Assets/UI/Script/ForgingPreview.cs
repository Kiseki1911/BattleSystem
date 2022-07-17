using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum ForgingToolType
{
  Earser,
  EdgeMaker,
  HolderMaker
}
public partial class ForgeCanvas
{
  private List<Texture2D> forgingTex = new List<Texture2D>();
  private List<int[,]> forgingMatrix = new List<int[,]>();
  private (Color, int) forgingToolInfo
  {
    get
    {
      switch (ForgingTool)
      {
        case ForgingToolType.EdgeMaker:
          return (forgingColor, 1);
        case ForgingToolType.HolderMaker:
          return (holderColor, 2);
        case ForgingToolType.Earser:
        default:
          return (BackgroundColor, 0);
      }
    }
  }
  private Color forgingColor = new Color(1f, 1f, 1f, 0.5f);
  private Color holderColor = new Color();
  private ForgingToolType ForgingTool = ForgingToolType.HolderMaker;
  public MeshRenderer HighLightMesh;
  public Camera HighLightCamera;
  public RenderTexture HighLightTexture;
  private VisualElement ForgingMenuCover;
  private Button ButtonYaiba;
  private Button ButtonNigiru;
  private Button ButtonEarser;
  private VisualElement HolderPreview;
  private Vector2 HolderPosition = new Vector2();
  private bool HolderEnable = false;
  private Translate ActiveForgingMenuPosition = new Translate();
  private Translate UnactiveForgingMenuPosition = new Translate(172, 0, 0);

  void initForgingPreviewer()
  {
    ForgingTool = ForgingToolType.HolderMaker;
    canvas.ForEach(canvas =>
    {
      var forging = canvas.Q("forgingPreview");
      var tex = new Texture2D(pixelNum, pixelNum, TextureFormat.ARGB32, false);
      tex.filterMode = FilterMode.Point;
      for (int x = 0; x < pixelNum; x++)
      {
        for (int y = 0; y < pixelNum; y++)
        {
          tex.SetPixel(x, y, BackgroundColor);
        }
      }
      tex.Apply();
      forgingTex.Add(tex);
      var matrix = new int[pixelNum, pixelNum];
      forgingMatrix.Add(matrix);

      if (forging != null)
      {
        forging.style.backgroundColor = BackgroundColor;
        forging.style.backgroundImage = Background.FromRenderTexture(HighLightTexture);
        HighLightMesh.material.SetTexture("_MainTex", tex);
      }
    });
    ForgingMenuCover = root.Q("forgingMenu");
    ButtonYaiba = ForgingMenuCover.Q<Button>("yaiba");
    ButtonNigiru = ForgingMenuCover.Q<Button>("nigiru");
    ButtonEarser = ForgingMenuCover.Q<Button>("earser");
    HolderPreview = root.Q("holderPreview");
    registryForgingMenuOperation();
    updateForgingMenuStatus();
  }

  private bool handleForgingPaint(int canvasIndex, int pixelX, int pixelY)
  {
    if (ForgingTool == ForgingToolType.HolderMaker)
    {
      HolderEnable = true;
      HolderPosition.x = pixelX;
      HolderPosition.y = -pixelY - 1;
      updateHolderPreview();
      return true;
    }
    else if (materialMatrix[canvasIndex][-pixelY - 1, pixelX] != 0)
    {
      if (ForgingTool == ForgingToolType.Earser && HolderEnable && HolderPosition.x == pixelX && HolderPosition.y == -pixelY - 1)
      {
        HolderEnable = false;
        updateHolderPreview();
        return true;
      }
      if (judgePixelIsEdge(canvasIndex, -pixelY - 1, pixelX))
      {
        forgingTex[canvasIndex].SetPixel(pixelX, pixelY, forgingToolInfo.Item1);
        forgingTex[canvasIndex].Apply();
        forgingMatrix[canvasIndex][-pixelY - 1, pixelX] = forgingToolInfo.Item2;
        return true;
      }
    }
    return false;
  }
  private bool judgePixelIsEdge(int canvasIndex, int y, int x)
  {
    if (y == 0 || y == pixelNum - 1 || x == 0 || x == pixelNum - 1) return true;
    if (
      materialMatrix[canvasIndex][y - 1, x - 1] == 0 ||
      materialMatrix[canvasIndex][y, x - 1] == 0 ||
      materialMatrix[canvasIndex][y + 1, x - 1] == 0 ||
      materialMatrix[canvasIndex][y - 1, x] == 0 ||
      materialMatrix[canvasIndex][y + 1, x] == 0 ||
      materialMatrix[canvasIndex][y - 1, x + 1] == 0 ||
      materialMatrix[canvasIndex][y, x + 1] == 0 ||
      materialMatrix[canvasIndex][y + 1, x + 1] == 0
    ) return true;
    if (materialMatrix[canvasIndex][y, x] != 0 &&
      (
        forgingMatrix[canvasIndex][y - 1, x - 1] != 0 ||
        forgingMatrix[canvasIndex][y, x - 1] != 0 ||
        forgingMatrix[canvasIndex][y + 1, x - 1] != 0 ||
        forgingMatrix[canvasIndex][y - 1, x] != 0 ||
        forgingMatrix[canvasIndex][y + 1, x] != 0 ||
        forgingMatrix[canvasIndex][y - 1, x + 1] != 0 ||
        forgingMatrix[canvasIndex][y, x + 1] != 0 ||
        forgingMatrix[canvasIndex][y + 1, x + 1] != 0
      )
    )
    {
      return true;
    }
    return false;
  }

  private void updateAllForgingData()
  {
    for (var canvasIndex = 0; canvasIndex < materialMatrix.Count; canvasIndex++)
    {
      for (int i = 0; i < pixelNum; i++)
      {
        for (int j = 0; j < pixelNum; j++)
        {
          if (!judgePixelIsEdge(canvasIndex, i, j))
          {
            forgingTex[canvasIndex].SetPixel(j, -i - 1, BackgroundColor);
            forgingTex[canvasIndex].Apply();
            forgingMatrix[canvasIndex][i, j] = 0;
          }
        }
      }
    }
  }

  private void registryForgingMenuOperation()
  {
    ButtonYaiba.RegisterCallback<ClickEvent>(e =>
    {
      ForgingTool = ForgingToolType.EdgeMaker;
      updateForgingMenuStatus();
    });
    ButtonNigiru.RegisterCallback<ClickEvent>(e =>
    {
      ForgingTool = ForgingToolType.HolderMaker;
      updateForgingMenuStatus();
    });
    ButtonEarser.RegisterCallback<ClickEvent>(e =>
    {
      ForgingTool = ForgingToolType.Earser;
      updateForgingMenuStatus();
    });
  }

  private void updateForgingMenuStatus()
  {
    switch (ForgingTool)
    {
      case ForgingToolType.Earser:
        ButtonEarser.RemoveFromClassList("disable");
        ButtonNigiru.AddToClassList("disable");
        ButtonYaiba.AddToClassList("disable");
        break;
      case ForgingToolType.EdgeMaker:
        ButtonEarser.AddToClassList("disable");
        ButtonNigiru.AddToClassList("disable");
        ButtonYaiba.RemoveFromClassList("disable");
        break;
      case ForgingToolType.HolderMaker:
        ButtonEarser.AddToClassList("disable");
        ButtonNigiru.RemoveFromClassList("disable");
        ButtonYaiba.AddToClassList("disable");
        break;
    }
  }

  private void switchForgingMenuDisplay(bool flag)
  {
    ForgingMenuCover.style.translate = flag ? ActiveForgingMenuPosition : UnactiveForgingMenuPosition;
  }

  private void updateHolderPreview()
  {
    if (HolderEnable)
    {
      HolderPreview.style.display = DisplayStyle.Flex;
      HolderPreview.style.left = (HolderPosition.x - pixelNum / 2) * pixelSize;
      HolderPreview.style.top = (HolderPosition.y - pixelNum / 2) * pixelSize;
      HolderPreview.style.height = pixelSize;
      HolderPreview.style.width = pixelSize;
    }
    else
    {
      HolderPreview.style.display = DisplayStyle.None;
    }
  }
}