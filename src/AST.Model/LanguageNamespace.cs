using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface ILanguageNamespace
    {
        public IDomainLanguage Domain { get; set; }
        public ILanguageNamespace Parent { get; set; }
        public string Name { get; set; }
        public List<ILanguageNamespace> Namespaces { get; }
        public List<ISyntaxElement> Elements { get; }
    }
    public sealed class LanguageNamespace : ILanguageNamespace
    {
        public string Name { get; set; }
        public IDomainLanguage Domain { get; set; }
        public ILanguageNamespace Parent { get; set; }
        public List<ISyntaxElement> Elements { get; } = new List<ISyntaxElement>();
        public List<ILanguageNamespace> Namespaces { get; } = new List<ILanguageNamespace>();
    }
}
