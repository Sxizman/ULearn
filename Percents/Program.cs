using System.Globalization;

public static class PercentCalculator
{
    public static void Main()
    {
        Console.WriteLine("Введите через пробел: исходную сумму, процентную ставку и срок вклада в месяцах");
        var input = Console.ReadLine()!;
        var output = Calculate(input);
        Console.WriteLine($"Накопленная сумма составляет {output:F2}");
        Console.ReadLine();
    }

    public static double Calculate(string userInput)
    {
        HandleInput(userInput, out var initBalance, out var monthCoefficient, out int months);
        var totalCoefficient = Math.Pow(monthCoefficient, months);
        return totalCoefficient * initBalance;
    }

    public static void HandleInput(string userInput, out double initBalance, out double monthCoefficient, out int months)
    {
        string[] splittedInput = userInput.Split();
        initBalance = double.Parse(splittedInput[0], CultureInfo.InvariantCulture);
        double monthPercent = double.Parse(splittedInput[1], CultureInfo.InvariantCulture) / 12.0;
        monthCoefficient = 1 + monthPercent / 100;
        months = int.Parse(splittedInput[2]);
    }
}