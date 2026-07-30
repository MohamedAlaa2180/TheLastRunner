using TMPro;
using UnityEngine;

public class CountdownScreen : MonoBehaviour, IUIScreen
{
    [SerializeField] TMP_Text valueText;

    public UIScreenId Id => UIScreenId.Countdown;
    public bool IsVisible => gameObject.activeSelf;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetValue(int value) => valueText.text = value.ToString();
}
