using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public static class FocusManager
    {
        private static KeywordViewModel focusedElement;
        public static void SetFocus(KeywordViewModel viewModel)
        {
            if (focusedElement != null)
            {
                focusedElement.IsFocused = false;
                focusedElement.BorderBrush = Brushes.White;
            }
            focusedElement = viewModel;
            focusedElement.IsFocused = true;
            focusedElement.BorderBrush = Brushes.Black;
        }
    }
}
