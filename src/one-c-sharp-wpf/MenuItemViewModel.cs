using System.Windows.Input;

namespace OneCSharp.Shell
{
    public sealed class MenuItemViewModel
    {
        public string CommandName { get; set; }
        public ICommand CommandAction { get; set; }
    }
}
