public interface IUIService
{
    void Show(UIScreenId id);
    void Hide(UIScreenId id);
    void HideAll();
    bool IsVisible(UIScreenId id);
}
