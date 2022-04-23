namespace SDKAddons
{
    public class Config
    {
        public static string[] Experience = new string[] { "New User", "Expert" };
        public static int ExpIndex = 0;

        public static string[] Imports = new string[] { "Avatar", "World" };
        public static int ImpIndex = 0;

        public static string WindowTitle { get { return "SDK Addons Handler"; } }

        public static int[] WindowMinSize = new int[] { 500, 360 };


        public static string[] CustomEditorPrefs = new string[] { "SA.ExpIndex", "SA.ImpIndex", "SA.Selection", "SA.Info" };

        public static bool IsVRC3UdonWorld { get; set; }
        public static bool IsVRC3Avatar { get; set; }
        public static bool IsNoneSDK { get; set; }

        public static string repoDL { get { return "/releases/latest/download/"; } }
        public static string mainlink { get { return "https://AddonsHandler.not90hz.repl.co/"; } }

        public static string[] AvToolsList;
        public static string[] WToolsList;

        public static string Latest { get; set; }
        public static string Current { get { return "1"; } }

        public static bool Sel { get; set; }
        public static bool Inf { get; set; }

        public static string InstallPath { get; set; }
    }
}
