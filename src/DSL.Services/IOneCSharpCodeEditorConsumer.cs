using OneCSharp.Metadata.Services;

namespace OneCSharp.DSL.Services
{
    public interface IOneCSharpCodeEditorConsumer
    {
        IMetadataProvider Metadata { get; set; }
        void SaveSyntaxNode(IOneCSharpCodeEditor editor, CodeEditorEventArgs args);
    }
}
