using System.Collections.Generic;

namespace OneCSharp.Query.Shared
{
    public static class HintTypes
    {
        public static string NoneHint = "NONE HINT";
        public static string ReadUncommited = "READUNCOMMITTED";
        public static string ReadCommited = "READCOMMITTED";
        public static string ReadCommitedLock = "READCOMMITTEDLOCK";
        public static string RepeatableRead = "REPEATABLEREAD";
        public static string Serializable = "SERIALIZABLE";
        public static string UpdateLock = "UPDLOCK";
        public static string ReadPast = "READPAST";
        public static string RowLock = "ROWLOCK";
        public static List<string> HintTypesList = new List<string>()
        {
            HintTypes.NoneHint,
            HintTypes.ReadUncommited,
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
