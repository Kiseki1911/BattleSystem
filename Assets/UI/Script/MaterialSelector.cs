using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public partial class ForgeCanvas
{
  [Header("元素选择器")]
  private VisualElement selectorContainer;
  private VisualElement toolCover;
  private VisualElement elementCover;
  private VisualElement elementTransform;
  private VisualElement hasMore;
  private VisualElement hasPrev;

  private VisualElement earser;
  private float translateX;
  private float minTranslate;
  private bool scrollEnable = false;

  private Translate SelectorHidePosition = new Translate(0, 114, 0);
  private Translate SelectorShowPosition = new Translate();

  private Translate ActiveElementPosition = new Translate(0, -10, 0);
  private Translate UnactiveElementPosition = new Translate();
  IEnumerator initBag()
  {
    WeaponForge.init(GetMatrixSize());
    selectorContainer = root.Q("elementSelectorContainer");
    toolCover = root.Q("elementSelector");
    elementCover = toolCover.Q("elementCover");
    elementTransform = elementCover.Q("elementCoverTransform");
    earser = toolCover.Q("earser");

    elementListInit();

    yield return new WaitForEndOfFrame();
    updateElementActiveStatus();

    minTranslate = elementCover.localBound.width - elementTransform.localBound.width;
    scrollEnable = minTranslate < 0;

    hasMore = root.Q("hasMore");
    hasPrev = root.Q("hasPrev");

    checkPointerDisplay();

    RegistryElementCoverOperation();

    switchSelectorDisplay(true);
  }

  private void elementListInit()
  {
    foreach (var item in materialList.ToArray())
    {
      // 跳过橡皮擦
      if (item.material.id == 0) continue;

      var coverElement = new VisualElement();
      coverElement.AddToClassList("element");
      elementTransform.Add(coverElement);

      var previewElement = new VisualElement();
      previewElement.AddToClassList("icon");
      previewElement.style.backgroundColor = item.material.color;
      coverElement.Add(previewElement);

      var labelElement = new Label();
      labelElement.text = item.material.title;
      labelElement.AddToClassList("label");
      coverElement.Add(labelElement);
    }
  }

  private void RegistryElementCoverOperation()
  {
    bool drag = false;
    bool moved = false;
    float startTranslateX = 0;
    Vector2 startPosition = new Vector2();

    EventCallback<PointerMoveEvent> pointerMoveHandler = e =>
    {
      if (!moved && ((Vector2)e.position - startPosition).magnitude > 5f)
      {
        moved = true;
      }
      if (moved)
      {
        changeScroll(e.position.x - startPosition.x);
        checkPointerDisplay();
      }
    };

    elementCover.RegisterCallback<PointerDownEvent>(e =>
    {
      startPosition = e.position;
      moved = false;
      drag = true;
      startTranslateX = translateX;
      root.RegisterCallback<PointerMoveEvent>(pointerMoveHandler);
    });

    root.RegisterCallback<PointerUpEvent>(e =>
    {
      if (drag)
      {
        if (moved)
        {
          //禁止点击事件
        }
        root.UnregisterCallback<PointerMoveEvent>(pointerMoveHandler);
      }
    });

    elementCover.RegisterCallback<WheelEvent>(e =>
    {
      changeScroll(e.delta.y < 0 ? 40 : -40);
      checkPointerDisplay();
    });

    earser.RegisterCallback<ClickEvent>(e =>
    {
      currentMaterial = 0;
    });
    int index = 1;
    foreach (var item in elementTransform.Children())
    {
      int targetIndex = index++;
      item.RegisterCallback<ClickEvent>(e =>
      {
        if (!moved)
        {
          if (materialList[targetIndex - 1].count > 0)
          {
            currentMaterial = targetIndex;
          }
        }
      });
    }

  }

  private void changeScroll(float deltaDistance)
  {
    if (scrollEnable)
    {
      translateX = Mathf.Clamp(translateX + deltaDistance, minTranslate, 0);
      elementTransform.style.translate = new Translate(translateX, 0, 0);
    }
  }
  private void checkPointerDisplay()
  {
    hasMore.style.opacity = translateX > minTranslate ? 1 : 0;
    hasPrev.style.opacity = translateX < 0 ? 1 : 0;
  }

  private void updateElementActiveStatus()
  {
    if (elementTransform.childCount < 1) return;
    int i = 0;
    foreach (var item in elementTransform.Children())
    {
      item.style.translate = i + 1 == currentMaterial ? ActiveElementPosition : UnactiveElementPosition;
      item.SetEnabled(materialList[i].count > 0);
      item.Q(classes: "icon").style.opacity = materialList[i].count > 0 ? 1 : 0.1f;

      i++;
    }
  }

  private void switchSelectorDisplay(bool flag)
  {
    selectorContainer.style.translate = flag ? SelectorShowPosition : SelectorHidePosition;
  }
}