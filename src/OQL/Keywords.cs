using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OQL
{
    /// <summary>
	/// The keywords for the "Colorful" language.
	/// </summary>
	internal static class Keywords
    {
        #region Private data
        
        private static readonly List<string> keywords = new List<string>
        {
            "select", "from", "where"
        };

        private static readonly HashSet<string> keywordSet = new HashSet<string>(
            keywords, StringComparer.OrdinalIgnoreCase);
        #endregion // Private data

        #region Properties
        /// <summary>
        /// Gets the list of all keywords (in alphabetic order).
        /// </summary>
        internal static IReadOnlyList<string> All { get; } =
            new ReadOnlyCollection<string>(keywords);
        #endregion // Properties

        #region Internal methods
        /// <summary>
        /// Gets value indicating if the specified word is a keyword.
        /// </summary>
        /// <param name="word">The word to test.</param>
        /// <returns>True, if the word is a keyword; otherwise, false.</returns>
        internal static bool Contains(string word) =>
            keywordSet.Contains(word);
        #endregion // Internal methods
    }
}
