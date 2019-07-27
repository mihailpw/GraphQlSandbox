namespace GQL.Services.Infra
{
    public static class DebugInfo
    {
        public static bool IsDebug { get; set; }


        static DebugInfo()
        {
#if DEBUG
            IsDebug = true;
#endif
        }
    }
}