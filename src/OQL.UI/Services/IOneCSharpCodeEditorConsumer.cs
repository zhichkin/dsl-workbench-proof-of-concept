namespace OneCSharp.OQL.UI.Services
{
    public interface IOneCSharpCodeEditorConsumer
    {
        void SaveSyntaxNode(IOneCSharpCodeEditor editor, CodeEditorEventArgs args);
    }
}
