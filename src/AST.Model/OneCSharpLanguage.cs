using OneCSharp.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.AST.Model
{
    public static class OneCSharp
    {
        private const string ONE_C_SHARP = "1C#";
        public readonly static Language ONECSHARP = new Language() { Name = ONE_C_SHARP };

        static OneCSharp()
        {
            FunctionConcept();
            ParameterConcept();
            // TODO: add more new concepts
        }
        private static void FunctionConcept() // TODO: IScopeProvider for ParameterConcept
        {
            _ = ONECSHARP.Concept(nameof(FunctionConcept))
                .Name()
                .Property("ReturnType").Optional() // TODO: cross-reference selection list ? MultipleType is indication for selection ?
                .List("Parameters").Optional().ValueType(ONECSHARP.Concept(nameof(ParameterConcept)));
        }
        private static void ParameterConcept()
        {
            _ = ONECSHARP.Concept(nameof(ParameterConcept))
                .Name()
                .Property("ParameterType") // TODO: cross-reference selected by user ?
                .Property("IsOutput").Optional().ValueType(SimpleType.Boolean);
        }
    }

    #region "LayoutNode"
    public sealed class KeywordNode : SyntaxNode { }
    public sealed class LiteralNode : SyntaxNode { }
    #endregion

    public sealed class FunctionConcept : SyntaxNode, IScopeProvider
    {
        public string Name { get; set; } = string.Empty; // TODO: placeholder ? example: <function name> ?
        // what kind of concept can be scope provider for DataType ???
        // SimpleTypes + imported ComplexTypes by USING statement ???
        public Optional<DataType> ReturnType { get; } = new Optional<DataType>();
        // how to restrict generic type of lists if multiple types are allowed ???
        // use abstract classes as generic type constraint in that case ???
        public Optional<List<ParameterConcept>> Parameters { get; } = new Optional<List<ParameterConcept>>();
        public IEnumerable<ISyntaxNode> Scope<T>() where T : ISyntaxNode
        {
            if (!Parameters.HasValue) return null;
            if (typeof(T) != typeof(ParameterConcept)) return null;

            return Parameters.Value;
        }
        public ISyntaxNode LayoutTemplate
        {
            get
            {
                return new KeywordNode(); // ???
            }
        }
    }
    public sealed class ParameterConcept : SyntaxNode
    {
        public string Name { get; set; } = string.Empty; // TODO: placeholder ?
        public DataType ParameterType { get; set; } = SimpleType.String; // what kind of concept can be scope provider for DataType ???
        public Optional<bool> IsOutput { get; } = new Optional<bool>();
    }
}