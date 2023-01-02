namespace Project_1.Logic;

public static class Extensions
{
    /// <summary>
    /// Расширение нужно для проверки знака перед 0.
    /// Температура откругляется, и когда обрезается часть после запятой, то нужно убрать знак у 0
    /// </summary>
    /// <param name="item">Переменная у которой вызываем расширение</param>
    /// <returns>Возвращаем эту же строку или 0 без знака</returns>
    public static string CheckingString(this string item)
    {
        if (item == "-0")
        {
            return "0";
        }
        else
        {
            return item;
        }
    }
}