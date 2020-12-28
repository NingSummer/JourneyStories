namespace JourneyStories.stories
{
    public class DefaultModConfigs
    {
        public static bool DebugMode = false;

        public static float DefaultRed = 1f;
        public static float DefaultGreen = 0.5f;
        public static float DefaultBlue = 0.1f;
        public static float DefaultAlpha = 0f;

        public static float CheckIntervalSecond = 2;

        public static float BaseTimeSec = 15;

        // 时间因子
        public static float TimeFactor = 0.5f;

        // 位置因子
        public static float PositionFactor = 5f;

        public static string Language = "cn";
    }

    public class ModConfigs
    {
        public bool DebugMode = false;

        public float DefaultRed = 1f;
        public float DefaultGreen = 0.5f;
        public float DefaultBlue = 0.1f;
        public float DefaultAlpha = 0f;

        public float CheckIntervalSecond = 2;

        public float BaseTimeSec = 15;

        // 时间因子
        public float TimeFactor = 0.5f;

        public float PositionFactor = 5f;

        public string Language = "cn";

        public static ModConfigs GlobalModConfigs = new ModConfigs();

        public ModConfigs()
        {
            DebugMode = DefaultModConfigs.DebugMode;
            DefaultRed = DefaultModConfigs.DefaultRed;
            DefaultGreen = DefaultModConfigs.DefaultGreen;
            DefaultBlue = DefaultModConfigs.DefaultBlue;
            DefaultAlpha = DefaultModConfigs.DefaultAlpha;
            CheckIntervalSecond = DefaultModConfigs.CheckIntervalSecond;
            BaseTimeSec = DefaultModConfigs.BaseTimeSec;
            TimeFactor = DefaultModConfigs.TimeFactor;
            PositionFactor = DefaultModConfigs.PositionFactor;
            Language = DefaultModConfigs.Language;
        }

        public void Copy(ModConfigs configs)
        {
            DebugMode = configs.DebugMode;
            DefaultRed = configs.DefaultRed;
            DefaultGreen = configs.DefaultGreen;
            DefaultBlue = configs.DefaultBlue;
            DefaultAlpha = configs.DefaultAlpha;
            CheckIntervalSecond = configs.CheckIntervalSecond;
            BaseTimeSec = configs.BaseTimeSec;
            TimeFactor = configs.TimeFactor;
            PositionFactor = configs.PositionFactor;
            Language = configs.Language;
        }
    }
}