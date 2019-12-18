namespace OneCSharp.Core
{
    public static class OneCSharp
    {
        public static Binary Binary { get; } = new Binary();
        public static Boolean Boolean { get; } = new Boolean();
        public static Numeric Numeric { get; } = new Numeric();
        public static String String { get; } = new String();
        public static DateTime DateTime { get; } = new DateTime();
        public static UUID UUID { get; } = new UUID();
    }
}