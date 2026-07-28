public interface IUIScreen
{
    UIScreenId Id { get; }
    bool IsVisible { get; }
    void Show();
    void Hide();
}
