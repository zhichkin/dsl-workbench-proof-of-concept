using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OneCSharp.Query.Shared
{
    public class BooleanFunction : QueryExpression
    {
        #region " Predefined names "

        private const string CONST_AND = "AND";
        private const string CONST_OR = "OR";
        private const string CONST_NOT = "NOT";
        private const string CONST_Equal = "=";
        private const string CONST_NotEqual = "<>";
        private const string CONST_Greater = ">";
        private const string CONST_GreaterOrEqual = ">=";
        private const string CONST_Less = "<";
        private const string CONST_LessOrEqual = "<=";

        public static string AND { get { return CONST_AND; } }
        public static string OR { get { return CONST_OR; } }
        public static string NOT { get { return CONST_NOT; } }
        public static List<string> BooleanOperators { get; } = new List<string>()
        {
            BooleanFunction.AND,
            BooleanFunction.OR,
            BooleanFunction.NOT
        };

        public static string Equal { get { return CONST_Equal; } }
        public static string NotEqual { get { return CONST_NotEqual; } }
        public static string Greater { get { return CONST_Greater; } }
        public static string GreaterOrEqual { get { return CONST_GreaterOrEqual; } }
        public static string Less { get { return CONST_Less; } }
        public static string LessOrEqual { get { return CONST_LessOrEqual; } }
        public static List<string> ComparisonOperators { get; } = new List<string>()
        {
            BooleanFunction.Equal,
            BooleanFunction.NotEqual,
            BooleanFunction.Greater,
            BooleanFunction.GreaterOrEqual,
            BooleanFunction.Less,
            BooleanFunction.LessOrEqual
        };

        #endregion

        public BooleanFunction() { }
        public string Name { get; set; }
        public bool IsRoot
        {
            get
            {
                return !(this.Parent is BooleanOperator);
            }
        }
    }
    public class BooleanOperator : BooleanFunction
    {
        public BooleanOperator() { }
        public ObservableCollection<BooleanFunction> Operands { get; set; } = new ObservableCollection<BooleanFunction>();
        public bool IsLeaf
        {
            get
            {
                return (this.Operands.Count == 0) || (this.Operands[0] is ComparisonOperator);
            }
        }
        public void AddChild(BooleanFunction child)
        {
            child.Parent = this;
            this.Operands.Add(child);
        }
        public void RemoveChild(BooleanFunction child)
        {
            child.Parent = null;
            this.Operands.Remove(child);
        }
    }
    public class ComparisonOperator : BooleanFunction
    {
        public ComparisonOperator() { }
        public QueryExpression LeftExpression { get; set; }
        public QueryExpression RightExpression { get; set; }
    }
}
