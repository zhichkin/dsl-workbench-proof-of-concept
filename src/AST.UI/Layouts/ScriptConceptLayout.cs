using OneCSharp.AST.Model;

namespace OneCSharp.AST.UI
{
    public sealed class ScriptConceptLayout : ConceptLayout<ScriptConcept>
    {
        public override ISyntaxNodeViewModel Layout(ScriptConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("ONE-C-SHARP")
                .Repeatable().Bind(nameof(concept.Languages))
                .Repeatable().Bind(nameof(concept.Statements));
        }
    }
    public sealed class LanguageConceptLayout : ConceptLayout<LanguageConcept>
    {
        public override ISyntaxNodeViewModel Layout(LanguageConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("USE LANGUAGE")
                .Selector().Bind(nameof(concept.Assembly));
        }
    }
}