using KleverensSoft_Test;

//!!!!!!!!!!!!!!!
// Буду благодарен за фидбэк по коду!!!!
// tg: @fotis101
//!!!!!!!!!!!!!!!

Task1();
Task2();
Task3();

// Задача 1
// Дана строка, содержащая n маленьких букв латинского алфавита.
// Требуется реализовать алгоритм компрессии этой строки, замещающий группы последовательно идущих одинаковых букв формой "sc"
// (где "s" – символ, "с" – количество букв в группе), а также алгоритм декомпрессии, возвращающий исходную строку по сжатой.
// Если буква в группе всего одна – количество в сжатой строке не указываем, а пишем её как есть.
// Пример:
// Исходная строка: aaabbcccdde
// Сжатая строка: a3b2c3d2e
void Task1()
{
    Console.WriteLine("Задача 1.");


    string text = string.Empty;
    while (string.IsNullOrEmpty(text))
    {
        Console.Write("Ведите последовательность букв: ");
        text = Console.ReadLine();
    }
    string comprText = TextCompression(text);
    Console.WriteLine($"Сжатый формат: {comprText}");
    string decompText = TextDecompression(text);
    Console.WriteLine($"Развернутый формат: {decompText}");
}

// Алгоритм решения: проходка по строке с проверкой, является ли текущий символ строки такой же как следующий,
// если да, то увечиливает счетчик. В противном случае записываем символ и счетчик в строку, а затем обнуляем счетчик
string TextCompression(string text)
{
    int count = 1;
    string compressedText = string.Empty;

    for (int i = 0; i < text.Length; i++)
    {
        if (i + 1 < text.Length && text[i] == text[i + 1])
        {
            count++;
        }
        else
        {
            compressedText += count != 1 ? $"{text[i]}{count}" : $"{text[i]}";
            count = 1;
        }
    }
    return compressedText;
}

// Алгоритм решения: пробегаясь по строке, если удается запарсить в int следующий символ, то записываем его n раз в цикле в строку, и пропускаем 1 итерацию.
// В противном случае просто записываем символ.
string TextDecompression(string text)
{
    string decompText = string.Empty;

    for (int i = 0; i < text.Length; i++)
    {
        if (i + 1 < text.Length && int.TryParse(text[i + 1].ToString(), out int result))
        {
            for (int j = 0; j < result; j++)
            {
                decompText += text[i];
            }
            i++;
        }
        else
        {
            decompText += text[i];
        }
    }
    return decompText;
}





// Задача 2
// Есть "сервер" в виде статического класса.
// У него есть переменная count (тип int) и два метода, которые позволяют эту переменную читать и писать: GetCount() и AddToCount(int value).
// К классу–"серверу" параллельно обращаются множество клиентов, которые в основном читают, но некоторые добавляют значение к count.
// Нужно реализовать статический класс с методами  GetCount / AddToCount так, чтобы:
// •	читатели могли читать параллельно, не блокируя друг друга;
// •	писатели писали только последовательно и никогда одновременно;
// •	пока писатели добавляют и пишут, читатели должны ждать окончания записи.
void Task2()
{
    Console.WriteLine("\nЗадача 2.");
    Console.Write("Введите целое число для добавления: ");
    string? text = Console.ReadLine();
    int value = 0;
    while (!int.TryParse(text, out value))
    {
        Console.Write("Введите целое число для добавления: ");
        text = Console.ReadLine();
    }
    Server.AddToCount(value);
    int serverValue = Server.GetCount();
    Console.WriteLine($"Получено значение: {serverValue}");
}



// Задача 3
// Консольная программа для стандартизации лог-файлов
// Эта программа предназначена для обработки лог-файлов, содержащих записи в двух разных форматах.
// Цель программы – привести все записи к единому, стандартному виду, упрощая анализ и обработку логов. 
// Необходимо преобразовать записи из входного лог-файла в единый формат и сохранить их в выходной файл.
void Task3()
{
    Console.WriteLine("\nЗадание 3.");

    var logConv = new LogConverter();
    // Пути файлов
    string inputFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input logs");
    string outputFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output logs");
    string normLogPath = Path.Combine(outputFolderPath, "NormalizedLog.txt");
    string problemLogPath = Path.Combine(outputFolderPath, "problems.txt");
    // Порядок аргументов в стандартизированной строке лога
    LogConverter.LogFragment[] logOrder =
{
        LogConverter.LogFragment.Date,
        LogConverter.LogFragment.Time,
        LogConverter.LogFragment.LoggingLevel,
        LogConverter.LogFragment.CallingMethod,
        LogConverter.LogFragment.Message
    };


    if (!Directory.Exists(inputFolderPath))
    {
        Directory.CreateDirectory(inputFolderPath);
    }
    if (!Directory.Exists(outputFolderPath))
    {
        Directory.CreateDirectory(outputFolderPath);
    }
    if (!File.Exists(normLogPath))
    {
        File.Create(normLogPath);
    }
    if (!File.Exists(problemLogPath))
    {
        File.Create(problemLogPath);
    }

    // Счетчики
    int totalLogFiles = 0;
    int successLogLines = 0;
    int failedLogLines = 0;

    // Чтение всех файлов из папки "Input logs"
    foreach (var filePath in Directory.GetFiles(inputFolderPath, "*.txt"))
    {
        using StreamReader reader = new StreamReader(filePath);
        string line;
        // Построчное чтение из файла
        while ((line = reader.ReadLine()) != null)
        {
            // Если удается стандартизировать строку, то записать ее в файл "NormalizedLog.txt"
            if (logConv.TryNormalizeLog(line, logOrder, out string normalizedLog))
            {
                File.AppendAllText(normLogPath, normalizedLog + "\n");
                successLogLines++;
            }
            // В противном случае записать в "problems.txt"
            else
            {
                File.AppendAllText(problemLogPath, line + "\n");
                failedLogLines++;
            }
        }
        totalLogFiles++;
    }

    Console.WriteLine($"Проверено {totalLogFiles} файлов-логов.");
    Console.WriteLine($"Успешно форматировано {successLogLines} строк логов.");
    Console.WriteLine($"Не удалось форматировать {failedLogLines} строк логов.");
}