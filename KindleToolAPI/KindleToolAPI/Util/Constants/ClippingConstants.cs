namespace KindleToolAPI.Util.Constants
{
    public static class ClippingConstants
    {
        public static string TextSeparator = "==========";
        public static string TitleAuthorRegex = @"(.+) \((.+)\)\r*";
        public static string AddedOnRegex = @"Added\son\s([a-zA-Z]+),";
        public static string DateRegex = "([0-9]+):([0-9]+):([0-9]+)";
        public static int MaximumTextLength = 2000;
    }
}
