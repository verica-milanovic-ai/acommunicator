namespace ACommunicator.Helpers
{

    public static class LoggingHelper
    {
        private static readonly string ServiceLogger = "ServiceLogger";
        private static readonly string ErrorLogger = "ErrorLogger";

        public static void LogError(string message)
        {
            var logger = log4net.LogManager.GetLogger(ErrorLogger);

            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
        }

        public static void LogInfo(string message)
        {
            var logger = log4net.LogManager.GetLogger(ServiceLogger);

            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }

        public static void LogDebug(string message)
        {
            var logger = log4net.LogManager.GetLogger(ServiceLogger);

            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }
    }
}