using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace OQL
{
    internal static class OneCSharp
    {
        internal const string ContentType = "one-c-sharp";
        internal const string FileExtension = ".ocs";

        [Export]
        [Name(ContentType)]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition hidingContentTypeDefinition;

        [Export]
        [ContentType(ContentType)]
        [FileExtension(FileExtension)]
        internal static FileExtensionToContentTypeDefinition hiddenFileExtensionDefinition;
    }
}
