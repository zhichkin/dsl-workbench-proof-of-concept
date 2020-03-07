using OneCSharp.AST.Model;

namespace OneCSharp.Integrator.Model
{
    public sealed class CreateIntegrationNodeConcept : SyntaxRoot, IIdentifiable
    {
        private const string PLACEHOLDER = "<node name>";
        public CreateIntegrationNodeConcept() { Identifier = PLACEHOLDER; }
        public string Identifier { get; set; }
        public IntegrationNode Owner { get; set; }
        public string Address { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
    }
}