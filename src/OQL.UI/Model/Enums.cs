using System.Collections.Generic;

namespace OneCSharp.OQL.Model
{
    public static class JoinTypes
    {
        public static string None = string.Empty;
        public static string InnerJoin = "INNER JOIN";
        public static string LeftJoin = "LEFT JOIN";
        public static string RightJoin = "RIGHT JOIN";
        public static string FullJoin = "FULL JOIN";
        public static List<string> JoinTypesList = new List<string>()
        {
            JoinTypes.InnerJoin,
            JoinTypes.LeftJoin,
            JoinTypes.RightJoin,
            JoinTypes.FullJoin
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
            HintTypes.None,
            HintTypes.ReadUncommited, //NOLOCK
            HintTypes.ReadCommited,
            HintTypes.ReadCommitedLock,
            HintTypes.RepeatableRead,
            HintTypes.Serializable,
            HintTypes.UpdateLock,
            HintTypes.ReadPast,
            HintTypes.RowLock
        };
    }
}
