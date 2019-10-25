using Microsoft.VisualStudio.PlatformUI;
using System.Windows.Controls;

namespace OneCSharp.OQL.UI
{
    public sealed class OneCSharpDialog : DialogWindow
    {
        internal OneCSharpDialog()
        {
            this.HasMaximizeButton = true;
            this.HasMinimizeButton = true;
            this.Content = new TextBlock() { Text = "Hello from one-c-sharp =)" };
        }
    }
}
