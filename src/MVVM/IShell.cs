namespace OneCSharp.MVVM
{
    public interface IShell
    {
        void AddTabItem(string header, object content);
        void RemoveTabItem(TabViewModel tab);
    }
}
