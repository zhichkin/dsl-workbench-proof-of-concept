using OneCSharp.OQL.Model;
using System;

namespace OneCSharp.OQL.UI.Services
{
    public class CodeEditorEventArgs : EventArgs
    {
        public CodeEditorEventArgs(ISyntaxNode node)
        {
            SyntaxNode = node;
        }
        public ISyntaxNode SyntaxNode { get; }
        public bool Cancel { get; set; }
        public string ErrorMessage { get; set; }
    }
    public delegate void SaveSyntaxNodeEventHandler(IOneCSharpCodeEditor sender, CodeEditorEventArgs args);
    public interface IOneCSharpCodeEditor
    {
        bool IsModified { get; }
        void EditSyntaxNode(ISyntaxNode node);
        event SaveSyntaxNodeEventHandler Save;
    }
}