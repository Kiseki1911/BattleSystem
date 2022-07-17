using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Modal : MonoBehaviour
{
  public static Modal Instance;
  public VisualTreeAsset ModalUXML;
  public VisualElement rootElement;
  public VisualElement modalCover;
  public Label titleLabel;
  public Label contentLabel;
  public Button confirmButton;
  public Button cancelButton;
  private static Action onConfirmCallback;
  private static Action onCancelCallback;
  public static void generateModal(string title, string content)
  {
    Instance.titleLabel.text = title;
    Instance.contentLabel.text = content;
    Instance.cancelButton.style.display = DisplayStyle.None;
    onConfirmCallback = null;
    onCancelCallback = null;
    Instance.appearAnimation();
    ForgeCanvas.Instance.root.Add(Instance.rootElement);
  }
  public static void generateModal(string title, string content, Action onConfirm)
  {
    Instance.titleLabel.text = title;
    Instance.contentLabel.text = content;
    Instance.cancelButton.style.display = DisplayStyle.None;
    onConfirmCallback = onConfirm;
    onCancelCallback = null;
    Instance.appearAnimation();
    ForgeCanvas.Instance.root.Add(Instance.rootElement);
  }
  public static void generateModal(string title, string content, Action onConfirm, Action onCancel)
  {
    Instance.titleLabel.text = title;
    Instance.contentLabel.text = content;
    Instance.cancelButton.style.display = DisplayStyle.Flex;
    onConfirmCallback = onConfirm;
    onCancelCallback = onCancel;
    Instance.appearAnimation();
    ForgeCanvas.Instance.root.Add(Instance.rootElement);
  }
  private async void appearAnimation()
  {
    modalCover.style.scale = new Scale(new Vector3(0.7f, 0.7f, 1));
    await Task.Delay(100);
    modalCover.style.scale = new Scale(new Vector3(1, 1, 1));
  }
  private void Start()
  {
    Instance = this;
    rootElement = new VisualElement();
    rootElement.name = "modalContainer";
    ModalUXML.CloneTree(rootElement);
    modalCover = rootElement.Q("modalCover");
    titleLabel = rootElement.Q<Label>("title");
    contentLabel = rootElement.Q<Label>("content");
    confirmButton = rootElement.Q<Button>("confirm");
    confirmButton.RegisterCallback<ClickEvent>(e =>
    {
      if (onConfirmCallback != null)
      {
        onConfirmCallback();
      }
      clearModal();
    });
    cancelButton = rootElement.Q<Button>("cancel");
    cancelButton.RegisterCallback<ClickEvent>(e =>
    {
      if (onCancelCallback != null)
      {
        onCancelCallback();
      }
      clearModal();
    });
  }
  private void clearModal()
  {
    var modalContainer = ForgeCanvas.Instance.root.Q("modalContainer");
    if (modalContainer != null)
    {
      ForgeCanvas.Instance.root.Remove(modalContainer);
    }
  }
}
