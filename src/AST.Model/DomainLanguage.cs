using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface IDomainLanguage
    {
        string Name { get; set; }
        List<ILanguageNamespace> Grammar { get; }
    }
    public sealed class DomainLanguage : IDomainLanguage
    {
        public string Name { get; set; }
        public List<ILanguageNamespace> Grammar { get; } = new List<ILanguageNamespace>();
    }
}
