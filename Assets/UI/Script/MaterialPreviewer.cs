using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public partial class ForgeCanvas
{
  [Header("元素面板")]
  public Color defaultTextColor = new Color(1, 1, 1, 1);
  public Color emptyTextColor = new Color(1, 0, 0, 1);
  private VisualElement informationContainer;
  private VisualElement materialPreviewer;
  private VisualElement materialIcon;
  private VisualElement materialDetail;
  private Label weightLabel;
  private Label hardnessLabel;
  private Label remainLabel;
  private Label materialLabel;
  private Translate PreviewerHidePosition = new Translate(-200, 0, 0);
  private Translate PreviewerShowPosition = new Translate();

  IEnumerator initDetailPannel()
  {
    informationContainer = root.Q("elementInformationContainer");
    materialPreviewer = root.Q("materialPreview");
    materialIcon = materialPreviewer.Q("icon");
    materialLabel = materialPreviewer.Q<Label>("label");

    materialDetail = root.Q("materialDetail");
    weightLabel = materialDetail.Q<Label>("weightLabel");
    hardnessLabel = materialDetail.Q<Label>("hardnessLabel");
    remainLabel = materialDetail.Q<Label>("remainLabel");

    updatePreviewerDetail();
    yield return new WaitForEndOfFrame();
    switchPreviewDisplay(true);
  }

  private void updatePreviewerDetail()
  {
    Material material;
    if (currentMaterial > 0)
    {
      material = materialList[currentMaterial - 1].material;
    }
    else
    {
      material = BackPack.Instance.materialTable[0];
    }
    if (material == null) return;

    materialIcon.style.backgroundColor = material.color;

    materialLabel.text = material.title;

    materialDetail.style.display = currentMaterial == 0 ?
      DisplayStyle.None : DisplayStyle.Flex;

    if (currentMaterial > 0)
    {
      remainLabel.text = materialList[currentMaterial - 1].count.ToString();
      remainLabel.style.color = materialList[currentMaterial - 1].count == 0 ?
        emptyTextColor : defaultTextColor;
    }

    hardnessLabel.text = material.hardness.ToString();

    weightLabel.text = material.mass.ToString();
  }

  void switchPreviewDisplay(bool flag)
  {
    informationContainer.style.translate = flag ? PreviewerShowPosition : PreviewerHidePosition;
  }
}