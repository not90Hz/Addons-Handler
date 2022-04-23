namespace SDKAddons
{
    public class Config
    {
        public static string[] Experience = new string[] { "New User", "Expert" };
        public static int ExpIndex = 0;

        public static string WindowTitle { get { return "SDK Addons Handler"; } }

        public static int[] WindowMinSize = new int[] { 500, 360 };


        public static string[] CustomEditorPrefs = new string[] { "SA.ExpIndex" };

        public static bool IsVRC3UdonWorld { get; set; }
        public static bool IsVRC3Avatar { get; set; }
        public static bool IsNoneSDK { get; set; }

        public static bool Sel { get; set; }
        public static bool Inf { get; set; }
    }
}
