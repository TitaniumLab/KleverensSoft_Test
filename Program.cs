using KleverensSoft_Test;
using System.Diagnostics;


//Task1();
//Task2();
Task3();

//Задача 1
//Дана строка, содержащая n маленьких букв латинского алфавита.
//Требуется реализовать алгоритм компрессии этой строки, замещающий группы последовательно идущих одинаковых букв формой "sc"
//(где "s" – символ, "с" – количество букв в группе), а также алгоритм декомпрессии, возвращающий исходную строку по сжатой.
//Если буква в группе всего одна – количество в сжатой строке не указываем, а пишем её как есть.
//Пример:
//Исходная строка: aaabbcccdde
//Сжатая строка: a3b2c3d2e
void Task1()
{
    Console.WriteLine("Задача 1.");

    Console.Write("Ведите последовательность букв: ");
    string text = Console.ReadLine();
    string comprText = TextCompression(text);
    Console.WriteLine(comprText);
}

// Алгоритм решения: проходка по строке с проверкой, является ли текущий символ строки такой же как в отдетьной переменной,
// если символ тот же, то увеличить счетчик, в противном случае старый символ и значение счетчика записывается в строку,
// счетчик обнулсяется, а символ в отдетьной переменной переписывается на новый
string TextCompression(string text)
{
    char curSymbol = text[0];
    int count = 0;
    string compressedTest = string.Empty;

    for (int i = 0; i < text.Length; i++) // можно и через linq, но for быстрее 
    {
        if (text[i] != curSymbol)
        {
            compressedTest += $"{curSymbol}{count}";
            curSymbol = text[i];
            count = 0;
        }
        count++;
    }

    compressedTest += $"{curSymbol}{count}";

    return compressedTest;
}


//Задача 2
//Есть "сервер" в виде статического класса.
//У него есть переменная count (тип int) и два метода, которые позволяют эту переменную читать и писать: GetCount() и AddToCount(int value).
//К классу–"серверу" параллельно обращаются множество клиентов, которые в основном читают, но некоторые добавляют значение к count.
//Нужно реализовать статический класс с методами  GetCount / AddToCount так, чтобы:
//•	читатели могли читать параллельно, не блокируя друг друга;
//•	писатели писали только последовательно и никогда одновременно;
//•	пока писатели добавляют и пишут, читатели должны ждать окончания записи.
void Task2()
{
    Console.WriteLine("Задача 2.");
    Console.Write("Введите целое число для добавления: ");
    string text = Console.ReadLine();
    int value = 0;
    while (!int.TryParse(text, out value))
    {
        Console.Write("Введите целое число для добавления: ");
        text = Console.ReadLine();
    }
    Server.AddToCount(value);
    int serverValue = Server.GetCount();
    Console.WriteLine(serverValue);
}


void Task3()
{
    Console.WriteLine("Задание 3.");
    var logConv = new LogConverter();
    string inputFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input logs");
    string outputFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output logs");
    string normLogPath = Path.Combine(outputFolderPath, "NormalizedLog.txt");
    string problemLogPath = Path.Combine(outputFolderPath, "problems.txt");
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

    int totalLogFiles = 0;
    int successLogLines = 0;
    int failedLogLines = 0;
    foreach (var filePath in Directory.GetFiles(inputFolderPath, "*.txt"))
    {
        using StreamReader reader = new StreamReader(filePath);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (logConv.TryNormalizeLog(line, logOrder, out string normalizedLog))
            {
                File.AppendAllText(normLogPath, normalizedLog + "\n");
                successLogLines++;
            }
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