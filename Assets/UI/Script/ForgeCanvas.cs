using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public interface ICanvas
{
  public int GetMatrixSize();
  public int[,] GetMaterialMatrix(int index = 0);
}
public partial class ForgeCanvas : MonoBehaviour, ICanvas
{
  public static ForgeCanvas Instance;
  private MaterialList materialList;
  public List<Texture2D> canvasTex;
  private Color BackgroundColor = new Color(0f, 0f, 0f, 0f);
  private Color currentColor
  {
    get
    {
      if (currentMaterial > 0)
        return materialList[currentMaterial - 1].material.color;
      else
      {
        return BackgroundColor;
      }
    }
  }
  private int _currentMaterial = 0;
  public int currentMaterial
  {
    get
    {
      return _currentMaterial;
    }
    set
    {
      _currentMaterial = value;
      //副作用: UI更新
      updatePreviewerDetail();
      updateElementActiveStatus();
    }
  }

  #region UI文档
  private UIDocument document;
  public VisualElement root;
  private List<VisualElement> canvas;
  private VisualElement canvasCover;
  private VisualElement canvasTransform;
  private VisualElement pixelPreview;
  private VisualElement massPreview;
  private Button prevButton;
  private Button nextButton;
  private Button cancelButton;

  #endregion

  private Rect canvasSize;
  private float scale = 0.1f;

  [Range(0.1f, 1f)]
  public float minZoom = 0.5f;
  private float zoom = 1f;

  [Range(1f, 2f)]
  public float maxZoom = 2f;

  [SerializeField]
  private int currentDrawStep = 0;
  private int maxDrawStep = 1;
  private int pixelSize
  {
    get
    {
      return Mathf.CeilToInt(1f / scale);
    }
  }
  private int pixelNum
  {
    get
    {
      return Mathf.CeilToInt(canvasSize.width / pixelSize);
    }
  }
  private List<List<int[,]>> historyTex = new();
  private Vector2 canvasTranslate = new Vector2();
  public List<int[,]> materialMatrix = new();

  private bool _visible = true;
  public bool Visible
  {
    get
    {
      return _visible;
    }
  }
  public int GetMatrixSize()
  {
    return pixelNum;
  }

  public int[,] GetMaterialMatrix(int index)
  {
    return materialMatrix[index];
  }

  private void Start()
  {
    Instance = this;
    setVisible(false);
  }
  private void OnEnable()
  {
    canvasTranslate = Vector2.zero;
    zoom = 1f;
    materialMatrix.Clear();
    canvasTex.Clear();
    forgingTex.Clear();
    forgingMatrix.Clear();
    materialList = Instantiate(BackPack.Instance.materials);
    document = GetComponent<UIDocument>();
    root = document.rootVisualElement;

    canvasCover = root.Q("canvasCover");
    canvasTransform = canvasCover.Q("transform");
    pixelPreview = canvasCover.Q("pixelPreview");
    massPreview = canvasCover.Q("massPreview");
    canvas = root.Query("drawCanvas").ToList();
    prevButton = root.Q<Button>("Prev");
    nextButton = root.Q<Button>("Next");
    cancelButton = root.Q<Button>("Cancel");

    StartCoroutine(init());
  }

  private Action _onClose;
  private Action _onLoad;
  public void setVisible(bool flag, Action onLoad = null, Action onClose = null)
  {
    if (flag)
    {
      if (onLoad != null)
      {
        _onLoad = onLoad;
      }
      if (onClose != null)
      {
        _onClose = onClose;
      }
    }
    if (!flag && _onClose != null)
    {
      _onClose();
      _onClose = null;
    }
    _visible = flag;
    gameObject.SetActive(flag);
  }
  private IEnumerator init()
  {
    _currentMaterial = 0;
    yield return new WaitForEndOfFrame();
    initTexture();
    registryCallbacks();
    registryButtons();
    StartCoroutine(initBag());
    StartCoroutine(initDetailPannel());
    switchDrawStatus(0, false);
    initForgingPreviewer();
    yield return new WaitForEndOfFrame();
    if (_onLoad != null)
    {
      _onLoad();
      _onLoad = null;
    }
  }


  private void initTexture()
  {
    canvas.ForEach(element =>
    {
      canvasSize = element.contentRect;
      var canvasTex = new Texture2D(Mathf.CeilToInt(canvasSize.width * scale), Mathf.CeilToInt(canvasSize.height * scale));
      for (int x = 0; x < canvasTex.width; x++)
      {
        for (int y = 0; y < canvasTex.height; y++)
        {
          canvasTex.SetPixel(x, y, BackgroundColor);
        }
      }
      canvasTex.filterMode = FilterMode.Point;
      canvasTex.Apply();
      element.style.backgroundImage = canvasTex;
      this.canvasTex.Add(canvasTex);
      materialMatrix.Add(new int[pixelNum, pixelNum]);
    });
  }
  private bool drawPixel(int canvasIndex, Vector2 position)
  {
    var pixelX = Mathf.FloorToInt(position.x * scale);
    var pixelY = Mathf.FloorToInt((canvasSize.y - position.y) * scale);
    if (currentDrawStep == 0)
    {
      if (currentMaterial != 0 && materialList[currentMaterial - 1].count == 0)
      {
        return false;
      }
      if (currentMaterial != materialMatrix[canvasIndex][-pixelY - 1, pixelX])
      {
        canvasTex[canvasIndex].SetPixel(pixelX, pixelY, currentColor);
        canvasTex[canvasIndex].Apply();
        materialMatrix[canvasIndex][-pixelY - 1, pixelX] = currentMaterial;
        if (currentMaterial == 0)
        {
          forgingMatrix[canvasIndex][-pixelY - 1, pixelX] = 0;
          forgingTex[canvasIndex].SetPixel(pixelX, pixelY, BackgroundColor);
          forgingTex[canvasIndex].Apply();

          if (HolderEnable && HolderPosition.x == pixelX && -pixelY - 1 == HolderPosition.y)
          {
            HolderEnable = false;
            updateHolderPreview();
          }
        }

        updateAllForgingData();
        updateMaterialRemain();
        var (mass, center) = WeaponForge.weapon.onChangeMaterial(
          translateMaterialMatrix(canvasIndex)
        );
        updateMassCenter(center);
        return true;
      }
    }
    else if (currentDrawStep == 1)
    {
      return handleForgingPaint(canvasIndex, pixelX, pixelY);
    }
    return false;
  }
  private int[,] translateMaterialMatrix(int canvasIndex)
  {
    var newMatrix = new int[pixelNum, pixelNum];
    for (int i = 0; i < pixelNum; i++)
    {
      for (int j = 0; j < pixelNum; j++)
      {
        var value = materialMatrix[canvasIndex][i, j];
        var id = 0;
        if (value > 0) id = materialList[value - 1].material.id;
        newMatrix[i, j] = id;
      }
    }
    return newMatrix;
  }
  private void registryCallbacks()
  {
    bool isPaint = false;
    bool isDrag = false;
    Vector2 startPosition = Vector2.zero;
    Vector2 startTranslate = Vector2.zero;

    int lastPaintIndex = -1;
    Vector2 lastPaintLocation = Vector2.zero;
    int i = 0;
    canvas.ForEach(item =>
    {
      int index = i;
      item.RegisterCallback<MouseDownEvent>(e =>
      {
        if (e.button == 0)
        {
          drawPixel(index, e.localMousePosition);
          lastPaintLocation = e.localMousePosition;
          lastPaintIndex = index;
        }
      });
      item.RegisterCallback<MouseMoveEvent>(e =>
      {
        if (e.button == 0 && isPaint)
        {
          // 插值绘制
          if (lastPaintIndex == index)
          {
            int interpolationTime = Mathf.CeilToInt((e.localMousePosition - lastPaintLocation).magnitude * scale);
            for (int i = 0; i < interpolationTime; i++)
            {
              float time = (float)i / (float)interpolationTime;
              var position = Vector2.Lerp(lastPaintLocation, e.localMousePosition, time);
              drawPixel(index, position);
            }
          }
          // 填充像素
          drawPixel(index, e.localMousePosition);
          lastPaintLocation = e.localMousePosition;
          lastPaintIndex = index;
        }
      });
      i++;
    });

    canvasCover.RegisterCallback<MouseDownEvent>(e =>
    {
      startPosition = e.localMousePosition;
      if (e.button == 0)
      {
        var position = e.localMousePosition;
        isPaint = true;
      }
      else if (e.button == 1)
      {
        isDrag = true;
        startTranslate = canvasTranslate;
        canvasCover.CaptureMouse();
      }
    });
    canvasCover.RegisterCallback<MouseMoveEvent>(e =>
    {
      if (isDrag)
      {
        var deltaPosition = e.localMousePosition - startPosition;
        canvasTranslate = startTranslate + deltaPosition;
        canvasTransform.style.translate = new Translate(canvasTranslate.x, canvasTranslate.y, 0);
      }
    });
    root.RegisterCallback<MouseUpEvent>(e =>
    {
      if (e.button == 0 && isPaint)
      {
        isPaint = false;
        recordHistory();

      }
      if (isDrag)
      {
        canvasCover.ReleaseMouse();
        isDrag = false;
      }
    });
    canvasCover.RegisterCallback<WheelEvent>(e =>
    {
      if (e.delta.y == 0) return;
      var value = zoom * (e.delta.y < 0 ? 1.1f : 0.9f);
      setZoom(value);
      root.Q<Slider>("zoomSlider").value = value;
    });
    canvasTransform.RegisterCallback<MouseMoveEvent>(e =>
    {
      var xPixel = Mathf.FloorToInt(e.localMousePosition.x / pixelSize);
      var yPixel = Mathf.FloorToInt(e.localMousePosition.y / pixelSize);

      pixelPreview.style.left = xPixel * pixelSize;
      pixelPreview.style.top = yPixel * pixelSize;
      pixelPreview.style.height = pixelSize;
      pixelPreview.style.width = pixelSize;
    });

  }

  private void registryButtons()
  {
    // root.Q<Button>("undoButton")?.RegisterCallback<ClickEvent>(e => { });
    // root.Q<Button>("redoButton")?.RegisterCallback<ClickEvent>(e => { });
    root.Q<Slider>("rotateSlider")?.RegisterCallback<ChangeEvent<float>>(e =>
    {
      canvasTransform.style.rotate = new Rotate(e.newValue);
    });
    prevButton.RegisterCallback<ClickEvent>(e =>
    {
      switchDrawStatus(-1);
    });
    nextButton.RegisterCallback<ClickEvent>(e =>
    {
      switchDrawStatus(1);
    });
    cancelButton.RegisterCallback<ClickEvent>(e =>
    {
      Modal.generateModal("提示", "即将失去当前的打造进度！", () =>
      {
        setVisible(false);
      }, () => { });
    });
    var zoomSlider = root.Q<Slider>("zoomSlider");
    if (zoomSlider != null)
    {
      zoomSlider.lowValue = minZoom;
      zoomSlider.highValue = maxZoom;
      zoomSlider.RegisterCallback<ChangeEvent<float>>(e =>
      {
        setZoom(e.newValue);
      });
    }
  }

  private void updateMassCenter(Vector2 center)
  {
    massPreview.style.display = center == Vector2.zero ? DisplayStyle.None : DisplayStyle.Flex;
    // 更新重心位置
    massPreview.style.left = (center.x - pixelNum / 2) * pixelSize;
    massPreview.style.top = (center.y - pixelNum / 2) * pixelSize;
    massPreview.style.height = pixelSize;
    massPreview.style.width = pixelSize;
  }

  private void setZoom(float zoomLevel)
  {
    var zoomResult = Mathf.Clamp(zoomLevel, minZoom, maxZoom);
    if (zoom != zoomResult)
    {
      canvasTransform.style.scale = new Scale(Vector3.one * zoomResult);
      zoom = zoomResult;
    }
  }

  private void recordHistory()
  {
    return;
    // Texture2D[] historyArray = new Texture2D[canvas.Count];
    // var matrixCopy = new int[1];
    // materialMatrix.CopyTo()
    // for (int i = 0; i < canvas.Count; i++)
    // {
    // Texture2D history = new Texture2D(canvasTex[i].width, canvasTex[i].height);
    // history.SetPixels(canvasTex[i].GetPixels());
    // history.Apply();
    // historyArray[i] = history;
    // }
    // historyTex.Add(historyArray);
  }

  private void updateMaterialRemain()
  {
    for (int i = 0; i < materialList.Count; i++)
    {
      materialList[i].count = BackPack.Instance.materials[i].count;
    }
    foreach (var matrix in materialMatrix)
    {
      for (int i = 0; i < GetMatrixSize(); i++)
      {
        for (int j = 0; j < GetMatrixSize(); j++)
        {
          if (matrix[i, j] > 0)
          {
            materialList[matrix[i, j] - 1].count--;
          }
        }
      }
    }
    updatePreviewerDetail();
    updateElementActiveStatus();
  }

  private void switchDrawStatus(int step, bool relative = true)
  {
    if (currentDrawStep + step == maxDrawStep + 1)
    {
      // 检查绘制情况
      if (!HolderEnable)
      {
        Modal.generateModal("提示", "先制作握把哦");
        return;
      }
      Debug.Log("绘制完成");
      //将材料使用情况应用致背包

      WeaponForge.weapon.onFinishForge(forgingMatrix[0], HolderPosition);
      for (var index = 0; index < materialList.Count; index++)
      {
        BackPack.Instance.materials[index].count = materialList[index].count;
      }
      BackPack.Instance.materials.UpdateList();
      BackPack.Instance.weaponList.Add(WeaponForge.weapon);
      Modal.generateModal("提示", "打造完成！", () =>
      {
        setVisible(false);
      });
    }
    else
    {
      int lastStep = currentDrawStep;
      currentDrawStep = Mathf.Clamp(relative ? lastStep + step : step, 0, maxDrawStep);
      if (lastStep != currentDrawStep)
      {
        switch (lastStep)
        {
          case 0:
            switchPreviewDisplay(false);
            switchSelectorDisplay(false);
            break;
          case 1:
            switchForgingMenuDisplay(false);
            break;
        }
        switch (currentDrawStep)
        {
          case 0:
            switchPreviewDisplay(true);
            switchSelectorDisplay(true);
            nextButton.text = "画好了";
            break;
          case 1:
            switchForgingMenuDisplay(true);
            nextButton.text = "打造";
            break;
        }
      }
      prevButton.SetEnabled(currentDrawStep > 0);
    }
  }
}
