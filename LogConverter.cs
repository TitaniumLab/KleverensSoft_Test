using static KleverensSoft_Test.LogConverter;

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
        /// Стандартизирует строку лога.
        /// </summary>
        /// <param name="log">Строка лога</param>
        /// <param name="logOrder">Порядок возврата аргуметов строки лога.</param>
        /// <param name="normalizedLog">Стандартизированная строка лога.</param>
        public bool TryNormalizeLog(string log, LogFragment[] logOrder, out string normalizedLog)
        {
            Dictionary<LogFragment, string> results = new Dictionary<LogFragment, string>();
            normalizedLog = string.Empty;

            // Если удается получить все нужные фрагменты строки лога, то производится записть в определенном порядке в строку
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



        /// <summary>
        /// Преобразование лога к стандартному виду из формата 2 (см. задание)
        /// </summary>
        public bool TryGetNormalizedDataFormat2(string log, LogFragment[] logFragments, out Dictionary<LogFragment, string> results)
        {
            results = new Dictionary<LogFragment, string>();
            string[] strings = log.Split('|', StringSplitOptions.TrimEntries);
            // Если слишком мало строк, то стандартизация не удалась
            if (strings.Length < 5)
            {
                return false;
            }

            // Дата
            // Если стандартизация даты не удалась, но при том она нужна
            if (!TryGetDateInFormat(strings[0], out string formatedDate) && logFragments.Contains(LogFragment.Date))
            {
                return false;
            }
            results.Add(LogFragment.Date, formatedDate);

            // Время
            // Если не удалось определить строку как время, но при том она нужна
            string time = strings[0].Split(' ')[1];
            if (!DateTime.TryParse(time, out DateTime date) && logFragments.Contains(LogFragment.Time))
            {
                return false;
            }
            results.Add(LogFragment.Time, time);

            // Уровень логирования
            // Если стандартизация уровеня логирования не удалась, но при том она нужна
            if (!TryGetLoggingLevel(strings, out string loggingLevel) && logFragments.Contains(LogFragment.LoggingLevel))
            {
                return false;
            }
            results.Add(LogFragment.LoggingLevel, loggingLevel);

            // Вызывающий метод
            if (logFragments.Contains(LogFragment.CallingMethod))
            {
                string callMeth = strings[3].ToString();
                if (string.IsNullOrEmpty(callMeth))
                {
                    callMeth = _callMethDefName;
                }
                results.Add(LogFragment.CallingMethod, callMeth);
            }

            // Сообщение 
            if (logFragments.Contains(LogFragment.Message))
            {
                results.Add(LogFragment.Message, strings[4].ToString());
            }
            return true;
        }



        /// <summary>
        /// Преобразование лога к стандартному виду из формата 1 (см. задание)
        /// </summary>
        public bool TryGetNormalizedDataFormat1(string log, LogFragment[] logFragments, out Dictionary<LogFragment, string> results)
        {
            results = new Dictionary<LogFragment, string>();
            string[] strings = log.Split(' ', 5);
            // Если слишком мало строк, то стандартизация не удалась
            if (strings.Length < 5)
            {
                return false;
            }

            // Дата
            // Если стандартизация даты не удалась, но при том она нужна
            if (!TryGetDateInFormat(strings[0], out string formatedDate) && logFragments.Contains(LogFragment.Date))
            {
                return false;
            }
            results.Add(LogFragment.Date, formatedDate);

            // Время
            string time = strings[1];
            // Если не удалось определить строку как время, но при том она нужна
            if (!DateTime.TryParse(time, out DateTime date) && logFragments.Contains(LogFragment.Time))
            {
                return false;
            }
            results.Add(LogFragment.Time, time);

            // Уровень логирования
            // Если стандартизация уровеня логирования не удалась, но при том она нужна
            if (!TryGetLoggingLevel(strings, out string loggingLevel) && logFragments.Contains(LogFragment.LoggingLevel))
            {
                return false;
            }
            results.Add(LogFragment.LoggingLevel, loggingLevel);

            // Вызывающий метод
            if (logFragments.Contains(LogFragment.CallingMethod))
            {
                string callMeth = strings[3].ToString();
                if (string.IsNullOrEmpty(callMeth))
                {
                    callMeth = _callMethDefName;
                }
                results.Add(LogFragment.CallingMethod, callMeth);
            }

            // Сообщение 
            if (logFragments.Contains(LogFragment.Message))
            {
                results.Add(LogFragment.Message, strings[4].ToString());
            }

            return true;
        }



        /// <summary>
        /// Преобразование даты в нужный формат
        /// </summary>
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



        /// <summary>
        /// Определение уровня логирования
        /// </summary>
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
