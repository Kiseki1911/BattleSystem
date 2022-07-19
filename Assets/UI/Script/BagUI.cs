using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

enum BagMode
{
  Weapon, Element
}
public class BagUI : MonoBehaviour
{
  public static BagUI Instance;
  private UIDocument uiDocument;
  private VisualElement root;
  #region Tab选择器
  private VisualElement tab;
  private Button buttonWeapon;
  private Button buttonElement;
  private Button buttonForge;
  #endregion

  #region 预览窗口
  private VisualElement previewerCover;
  private VisualElement iconPreviewer;
  private VisualElement weaponInfo;
  private Label weaponName;
  private Label weaponPower;
  private Label weaponWeight;
  private Label weaponDurability;
  private VisualElement elementInfo;
  private Label elementName;
  private Label elementWeight;
  private Label elementHardness;
  private Label elementPrice;
  private Label elementRemain;
  #endregion

  #region 背包主体
  private VisualElement bagCover;
  private VisualElement bagTransform;
  #endregion

  private VisualElement extendsInfo;

  private BagMode mode = BagMode.Element;
  private int _activeWeapon = 0;
  private int activeWeapon
  {
    get
    {
      return Mathf.Clamp(_activeWeapon, 0, BackPack.Instance.weaponList.Count);
    }
    set
    {
      _activeWeapon = value;
    }
  }
  private int _activeElement = 0;
  private int activeElement
  {
    get
    {
      return Mathf.Clamp(_activeElement, 0, BackPack.Instance.materials.Count);
    }
    set
    {
      _activeElement = value;
    }
  }
  private int gridNum
  {
    get
    {
      if (mode == BagMode.Weapon)
        return BackPack.Instance.maxWeaponVolumn;
      return BackPack.Instance.maxMaterialVolumn;
    }
  }
  void Start()
  {
    Instance = this;
  }
  private void OnEnable() {
    init();
  }
  private void init()
  {
    uiDocument = GetComponent<UIDocument>();
    root = uiDocument.rootVisualElement.Q("root");

    tab = root.Q("tab");
    buttonForge = root.Q<Button>("fastForge");
    buttonWeapon = tab.Q<Button>("ButtonWeapon");
    buttonElement = tab.Q<Button>("ButtonElement");

    previewerCover = root.Q("previewCover");
    iconPreviewer = previewerCover.Q("iconPreviewer");
    weaponInfo = previewerCover.Q("weaponInfo");
    weaponName = weaponInfo.Q<Label>("weaponName");
    weaponPower = weaponInfo.Q<Label>("weaponPower");
    weaponWeight = weaponInfo.Q<Label>("weaponWeight");
    weaponDurability = weaponInfo.Q<Label>("weaponDurability");

    elementInfo = previewerCover.Q("elementInfo");
    elementName = elementInfo.Q<Label>("elementName");
    elementWeight = elementInfo.Q<Label>("elementWeight");
    elementHardness = elementInfo.Q<Label>("elementHardness");
    elementPrice = elementInfo.Q<Label>("elementPrice");
    elementRemain = elementInfo.Q<Label>("elementRemain");

    bagCover = root.Q("bagCover");
    bagTransform = bagCover.Q("bagTransform");

    extendsInfo = root.Q("extendsInfo");
    registryTabEvents();
    updateAll();
  }
  private void registryTabEvents()
  {
    buttonElement.RegisterCallback<ClickEvent>(e =>
    {
      mode = BagMode.Element;
      updateAll();
    });
    buttonWeapon.RegisterCallback<ClickEvent>(e =>
    {
      mode = BagMode.Weapon;
      updateAll();
    });
    buttonForge.RegisterCallback<ClickEvent>(e =>
    {
      ForgeCanvas.Instance.setVisible(true,
      onLoad: () =>
      {
        if (BackPack.Instance.materials.Count > 0)
          ForgeCanvas.Instance.currentMaterial = activeElement + 1;
      },
      onClose: () =>
      {
        activeElement = 0;
        updateAll();
      });
    });
  }


  public void updateAll()
  {
    updateTab();
    updateGridsFull();
    updatePreviewer();
  }

  private void updateTab()
  {
    buttonElement.RemoveFromClassList("active");
    buttonWeapon.RemoveFromClassList("active");
    switch (mode)
    {
      case BagMode.Element:
        buttonElement.AddToClassList("active");
        break;
      case BagMode.Weapon:
        buttonWeapon.AddToClassList("active");
        break;

    }
  }

  private void updatePreviewer()
  {
    switch (mode)
    {
      case BagMode.Element:
        {
          weaponInfo.style.display = DisplayStyle.None;
          var display = BackPack.Instance.materials.Count > 0;
          elementInfo.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
          if (display)
          {
            var activeItem = BackPack.Instance.materials[activeElement];
            iconPreviewer.style.backgroundColor = activeItem.material.color;
            iconPreviewer.style.backgroundImage = null;
            elementName.text = activeItem.material.title;
            elementHardness.text = activeItem.material.hardness.ToString();
            elementWeight.text = activeItem.material.mass.ToString();
            elementPrice.text = activeItem.material.price.ToString();
            elementRemain.text = activeItem.count.ToString();
          }
          else
          {
            iconPreviewer.style.backgroundColor = Color.clear;
            iconPreviewer.style.backgroundImage = null;
          }
        }
        break;
      case BagMode.Weapon:
        {
          var display = BackPack.Instance.weaponList.Count > 0;
          weaponInfo.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
          elementInfo.style.display = DisplayStyle.None;
          if (display)
          {
            var activeItem = BackPack.Instance.weaponList[activeWeapon];
            weaponName.text = activeItem.title;
            weaponPower.text = activeItem.damageRate.ToString("#0.00");
            weaponWeight.text = activeItem.mass.ToString();
            weaponDurability.text = "0";
            iconPreviewer.style.backgroundColor = Color.clear;
            iconPreviewer.style.backgroundImage = activeItem.texture;
          }
          else
          {
            iconPreviewer.style.backgroundColor = Color.clear;
            iconPreviewer.style.backgroundImage = null;
          }
        }
        break;
    }
  }

  private void updateGridPartical()
  {
    var bagVisualElementList = bagTransform.Children().ToList();
    switch (mode)
    {
      case BagMode.Weapon:
        for (int index = 0; index < gridNum; index++)
        {
          bagVisualElementList[index].RemoveFromClassList("active");
          if (index < BackPack.Instance.weaponList.Count)
          {
            bagVisualElementList[index].SetEnabled(true);
            if (index == activeWeapon)
            {
              bagVisualElementList[index].AddToClassList("active");
            }
          }
          else
          {
            bagVisualElementList[index].SetEnabled(false);
          }
        }
        break;
      case BagMode.Element:
        for (int index = 0; index < gridNum; index++)
        {
          bagVisualElementList[index].RemoveFromClassList("active");
          if (index < BackPack.Instance.materials.Count)
          {
            bagVisualElementList[index].SetEnabled(true);
            if (index == activeElement)
            {
              bagVisualElementList[index].AddToClassList("active");
            }
          }
          else
          {
            bagVisualElementList[index].SetEnabled(false);
          }
        }
        break;
    }
  }
  private void updateGridsFull()
  {
    while (bagTransform.childCount > 0)
    {
      bagTransform.RemoveAt(0);
    }
    for (int index = 0; index < gridNum; index++)
    {
      var element = new VisualElement();
      element.SetEnabled(false);
      bagTransform.Add(element);
    }
    var bagVisualElementList = bagTransform.Children().ToList();
    switch (mode)
    {
      case BagMode.Weapon:
        for (int index = 0; index < gridNum && index < BackPack.Instance.weaponList.Count; index++)
        {
          var icon = new VisualElement();
          icon.style.backgroundImage = BackPack.Instance.weaponList[index].texture;
          bagVisualElementList[index].Add(icon);
          var label = new Label();
          label.text = BackPack.Instance.weaponList[index].title;
          bagVisualElementList[index].Add(label);
          var i = index;

          bagVisualElementList[index].RegisterCallback<MouseUpEvent>(e =>
          {
            if (e.button == 1)
            {
              RightMenu.SetContextMenu(e.mousePosition, new (string, Action)[] {
                  (
                    "扔掉",
                    () => {
                      BackPack.Instance.weaponList.Remove(i);
                      updateAll();
                    }
                  )
              });
            }
            else if (e.button == 0)
            {
              activeWeapon = i;
              updateGridPartical();
              updatePreviewer();
              WeaponInstance.instance.changeWeapon(activeWeapon);
            }
          });
        }
        break;
      case BagMode.Element:
        for (int index = 0; index < gridNum && index < BackPack.Instance.materials.Count; index++)
        {
          var material = BackPack.Instance.materials[index];
          var icon = new VisualElement();
          icon.style.backgroundColor = material.material.color;

          float h, s, v;
          Color.RGBToHSV(material.material.color, out h, out s, out v);

          bagVisualElementList[index].Add(icon);
          var label = new Label();
          label.text = material.material.title;

          label.style.color = v < 0.5f ? Color.white : Color.black;

          bagVisualElementList[index].Add(label);

          var i = index;

          bagVisualElementList[index].RegisterCallback<MouseUpEvent>(e =>
          {
            if (e.button == 1)
            {
              RightMenu.SetContextMenu(e.mousePosition, new (string, Action)[] {
                (
                  "扔掉",
                  () => {
                    BackPack.Instance.materials.Remove(i);
                    updateAll();
                  }
                )
              });
            }
            else if (e.button == 0)
            {
              activeElement = i;
              updateGridPartical();
              updatePreviewer();
            }
          });
        }
        break;
    }
    updateGridPartical();
  }
}
