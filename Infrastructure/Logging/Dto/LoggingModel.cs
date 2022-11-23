namespace Infrastructure.Logging.Dto
{
    public class LoggingModel
    {
        public bool ShowInfoMessages { get; set; }
        public bool ShowDebugMessages { get; set; }
        public bool ShowWarningMessages { get; set; }
        public bool ShowErrorMessages { get; set; }
        public bool ShowCriticalMessages { get; set; }
        public bool ShowTraceMessages { get; set; }
    }
}