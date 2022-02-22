namespace Source
{
    public static class Constants
    {
        public static class Layers
        {
            public const string Player = nameof(Player);
        }
        
        public static class ButtonCommands
        {
            public const string ShieldEnable = "SHIELD_ENABLE";
            public const string ShieldDisable = "SHIELD_DISABLE";
            public const string GamePause = "GAME_PAUSE";
            public const string GameResume = "GAME_RESUME";
            public const string GameExit = "GAME_EXIT";
            public const string GameRestart = "GAME_RESTART";
            public const string GameRebuild = "GAME_REBUILD";
        }
        
        public static class ColorScheme
        {
            public const string Player = "#FFFF00";
            public const string PlayerShieldActive = "#ADFF2F";
        }
    }
}