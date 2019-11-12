using OneCSharp.Metadata;

namespace OneCSharp.OQL.UI.Services
{
    public interface IOneCSharpCodeEditorConsumer
    {
        MetadataProvider Metadata { get; set; }
        void SaveSyntaxNode(IOneCSharpCodeEditor editor, CodeEditorEventArgs args);
    }
}
