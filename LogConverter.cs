namespace KleverensSoft_Test
{
    internal class LogConverter
    {
        private string _dateFormat = "dd-MM-yyyy";
        private string _callMethDefName = "DEFAULT";
        private Dictionary<string, string> _loggingLevels = new Dictionary<string, string>()
        {
            { "INFO","INFO" },
            { "INFORMATION","INFO" },
            { "WARN","WARN" },
            { "WARNING","WARN" },
            { "ERROR","ERROR" },
            { "DEBUG","DEBUG" },
        };
        public enum LogFragment
        {
            Date = 0,
            Time = 1,
            LoggingLevel = 2,
            CallingMethod = 3,
            Message = 4
        }

        /// <summary>
        /// Стандартизирует строку лога
        /// </summary>
        /// <param name="text">Целая строка лога</param>
        /// <returns>Стандартизированная строка лога</returns>
        public bool TryNormalizeLog(string log, LogFragment[] logOrder, out string normalizedLog)
        {
            Dictionary<LogFragment, string> results = new Dictionary<LogFragment, string>();
            normalizedLog = string.Empty;

            if (TryGetNormalizedDataFormat2(log, logOrder, out results) || TryGetNormalizedDataFormat1(log, logOrder, out results))
            {
                foreach (var item in logOrder)
                {
                    if (!string.IsNullOrEmpty(normalizedLog))
                    {
                        normalizedLog += "\t";
                    }
                    normalizedLog += results[item];
                }
                return true;
            }

            return false;
        }


        public bool TryGetNormalizedDataFormat2(string log, LogFragment[] logFragments, out Dictionary<LogFragment, string> results)
        {
            results = new Dictionary<LogFragment, string>();
            string[] strings = log.Split('|', StringSplitOptions.TrimEntries);
            if (strings.Length < 5)
            {
                return false;
            }
            // Дата
            if (!TryGetDateInFormat(strings[0], out string formatedDate) && logFragments.Contains(LogFragment.Date))
            {
                return false;
            }
            results.Add(LogFragment.Date, formatedDate);

            // Время
            string time = strings[0].Split(' ')[1];
            if (!DateTime.TryParse(time, out DateTime date) && logFragments.Contains(LogFragment.Time))
            {
                return false;
            }
            results.Add(LogFragment.Time, time);

            // Уровень логирования
            if (!TryGetLoggingLevel(strings, out string loggingLevel) && logFragments.Contains(LogFragment.LoggingLevel))
            {
                return false;
            }
            results.Add(LogFragment.LoggingLevel, loggingLevel);

            // Вызывающий метод и сообщение
            string callMeth = strings[3].ToString();
            if (string.IsNullOrEmpty(callMeth))
            {
                callMeth = _callMethDefName;
            }
            results.Add(LogFragment.CallingMethod, callMeth);

            results.Add(LogFragment.Message, strings[4].ToString());

            return true;
        }


        public bool TryGetNormalizedDataFormat1(string log, LogFragment[] logFragments, out Dictionary<LogFragment, string> results)
        {
            results = new Dictionary<LogFragment, string>();
            string[] strings = log.Split(' ', 5);
            if (strings.Length < 5)
            {
                return false;
            }
            // Дата
            if (!TryGetDateInFormat(strings[0], out string formatedDate) && logFragments.Contains(LogFragment.Date))
            {
                return false;
            }
            results.Add(LogFragment.Date, formatedDate);

            // Время
            string time = strings[1];
            if (!DateTime.TryParse(time, out DateTime date) && logFragments.Contains(LogFragment.Time))
            {
                return false;
            }
            results.Add(LogFragment.Time, time);

            // Уровень логирования
            if (!TryGetLoggingLevel(strings, out string loggingLevel) && logFragments.Contains(LogFragment.LoggingLevel))
            {
                return false;
            }
            results.Add(LogFragment.LoggingLevel, loggingLevel);

            // Вызывающий метод и сообщение
            string callMeth = strings[3].ToString();
            if (string.IsNullOrEmpty(callMeth))
            {
                callMeth = _callMethDefName;
            }
            results.Add(LogFragment.CallingMethod, callMeth);

            results.Add(LogFragment.Message, strings[4].ToString());

            return true;
        }


        private bool TryGetDateInFormat(string date, out string formatedDate)
        {
            formatedDate = string.Empty;
            if (!DateTime.TryParse(date, out DateTime dateTime))
            {
                return false;
            }
            formatedDate = dateTime.ToString(_dateFormat);
            return true;
        }


        private bool TryGetLoggingLevel(string[] strings, out string loggingLevel)
        {
            loggingLevel = string.Empty;
            foreach (string s in strings)
            {
                foreach (var item in _loggingLevels)
                {
                    if (item.Key == s)
                    {
                        loggingLevel = item.Value;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
