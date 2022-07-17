using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RightMenu : MonoBehaviour
{
  public static RightMenu Instance;

  private UIDocument uiDocument;
  private VisualElement container;

  void Awake()
  {
    Instance = this;
    uiDocument = GetComponent<UIDocument>();
    clearContextMenu();
  }

  public static void SetContextMenu(Vector2 position, (string, Action)[] param)
  {
    Instance.gameObject.SetActive(true);
    var container = Instance.uiDocument.rootVisualElement.Q("container");
    var mask = Instance.uiDocument.rootVisualElement.Q("mask");
    mask.RegisterCallback<PointerUpEvent>(e =>
    {
      Instance.clearContextMenu();

    });
    while (container.childCount > 0)
    {
      container.RemoveAt(0);
    }
    container.style.left = position.x;
    container.style.top = position.y;
    foreach (var item in param)
    {
      var button = new Button();
      button.text = item.Item1;
      button.RegisterCallback<ClickEvent>(e =>
      {
        item.Item2();
        Instance.clearContextMenu();
      });
      container.Add(button);
    }
  }
  public static void ClearContextMenu()
  {
    Instance.clearContextMenu();
  }
  public void clearContextMenu()
  {
    gameObject.SetActive(false);
  }
}
