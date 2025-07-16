Task1();
Task2();





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


void Task2()
{

}