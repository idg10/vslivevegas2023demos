#nullable disable annotations

namespace CrashEarlyCrashOften
{
    public class MyAppSettings
    {
        public ErrorSettings Errors { get; set; }

        public class ErrorSettings
        {
            public string ErrorPage { get; set; }
        }
    }
}
