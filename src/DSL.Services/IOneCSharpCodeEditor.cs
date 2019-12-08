using OneCSharp.DSL.Model;
using OneCSharp.Metadata.Services;
using System;

namespace OneCSharp.DSL.Services
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
        event SaveSyntaxNodeEventHandler Save;
        IMetadataProvider Metadata { get; set; }
    }
}