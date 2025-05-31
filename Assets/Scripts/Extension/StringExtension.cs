namespace Scripts.Extension
{
    public enum TextColor
    {
        red,
        green,
        blue,
        yellow,
        orange,
        magenta,
        black,
        cyan,
        grey
    }
    public static class StringExtension
    {    
        public static string Color(this string str, TextColor color)
        {
            return $"<color={color}>{str}</color>";
        }
        //public static string Color(this string str, Color color) {
        //    return $"<color=#{color.ToHexString()}>{str}</color>";
        //}
        public static string Bold(this string str)
        {
            return $"<b>{str}</b>";
        }
        public static string Italic(this string str)
        {
            return $"<i>{str}</i>";
        }
        public static string Line(this string str)
        {
            return $"<u>{str}</u>";
        }
    }

}