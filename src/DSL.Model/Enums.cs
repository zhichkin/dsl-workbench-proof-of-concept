using System.Collections.Generic;

namespace OneCSharp.DSL.Model
{
    public static class JoinTypes
    {
        public static string None = string.Empty;
        public static string Inner = "INNER";
        public static string Left = "LEFT";
        public static string Right = "RIGHT";
        public static string Full = "FULL";
        public static List<string> JoinTypesList = new List<string>()
        {
            JoinTypes.Inner,
            JoinTypes.Left,
            JoinTypes.Right,
            JoinTypes.Full
        };
    }
    public static class HintTypes
    {
        public static string None = string.Empty;
        public static string ReadUncommited = "READUNCOMMITTED"; // NOLOCK
        public static string ReadCommited = "READCOMMITTED";
        public static string ReadCommitedLock = "READCOMMITTEDLOCK";
        public static string RepeatableRead = "REPEATABLEREAD";
        public static string Serializable = "SERIALIZABLE";
        public static string UpdateLock = "UPDLOCK";
        public static string ReadPast = "READPAST";
        public static string RowLock = "ROWLOCK";
        public static List<string> HintTypesList = new List<string>()
        {
            HintTypes.ReadUncommited, //NOLOCK
            HintTypes.ReadCommited,
            HintTypes.ReadCommitedLock,
            HintTypes.ReadPast,
            HintTypes.RepeatableRead,
            HintTypes.Serializable,
            HintTypes.UpdateLock,
            HintTypes.RowLock
        };
    }
    public static class BooleanOperators
    {
        public const string AND = "AND";
        public const string OR = "OR";
        public const string NOT = "NOT";
        public static List<string> BooleanOperatorsList = new List<string>()
        {
            BooleanOperators.AND,
            BooleanOperators.OR,
            BooleanOperators.NOT
        };
    }
    public static class ComparisonOperators
    {
        public const string Equal = "=";
        public const string NotEqual = "<>";
        public const string Greater = ">";
        public const string GreaterOrEqual = ">=";
        public const string Less = "<";
        public const string LessOrEqual = "<=";
        public static List<string> ComparisonOperatorsList = new List<string>()
        {
            ComparisonOperators.Equal,
            ComparisonOperators.NotEqual,
            ComparisonOperators.Greater,
            ComparisonOperators.GreaterOrEqual,
            ComparisonOperators.Less,
            ComparisonOperators.LessOrEqual
        };
    }
}
