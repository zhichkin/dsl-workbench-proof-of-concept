using OneCSharp.Core.Model;

namespace OneCSharp.AST.Model
{
    public static class OneCSharp
    {
        private const string ONE_C_SHARP = "1C#";
        public readonly static Language ONECSHARP = new Language() { Name = ONE_C_SHARP };

        public const string FUNCTION = "FUNCTION";
        public const string PARAMETER = "PARAMETER";

        static OneCSharp()
        {
            FunctionConcept();
            ParameterConcept();
            // TODO: add more new concepts
        }
        private static void FunctionConcept()
        {
            _ = ONECSHARP.Concept(FUNCTION)
                .Name()
                .Property("RETURNS").Optional()
                .List("PARAMETERS").Optional().ValueType(ONECSHARP.Concept(PARAMETER));
        }
        private static void ParameterConcept()
        {
            _ = ONECSHARP.Concept(PARAMETER)
                .Name()
                .Property("TYPE")
                .Property("INPUT").Optional().ValueType(SimpleType.Boolean)
                .Property("OUTPUT").Optional().ValueType(SimpleType.Boolean);
        }
    }
}