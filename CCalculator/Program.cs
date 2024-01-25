using System;

namespace CCalculator
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("\t\tWelcome to the simplest console calculator app");
            Console.WriteLine("\t--------------------------------------------------------------");

            double frstOperand = GetOperand("Entear value: ");
            double scndOperand = GetOperand("Entear value: ");

            double result = 0.0;

            char operation = GetOperation("Operation: ");
            switch (operation)
            {
                case '+':
                    result = Calculator.Plus(frstOperand, scndOperand);
                    break;
                case '-':
                    result = Calculator.Minus(frstOperand, scndOperand);
                    break;
                case '*':
                    result = Calculator.Multiply(frstOperand, scndOperand);
                    break;
                case '/':
                    result = Calculator.Divide(frstOperand, scndOperand);
                    break;
                default:
                    Console.WriteLine("Invalid operation. Try again");
                    break;
            }
            Console.WriteLine($"\n\nResult: {result}");

            Console.ReadKey();
        }

        public static double GetOperand(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (Double.TryParse(Console.ReadLine(), out double operand))
                    return operand;

                Console.WriteLine("Invalid value. Please enter a number");
            }
        }

        public static char GetOperation(string message)
        {
            while (true)
            {
                Console.Write(message);

                char operation = Console.ReadKey().KeyChar;
                if(operation.Equals('+') || operation.Equals("-")|| operation.Equals('*') || operation.Equals("/"))
                    return operation;

                Console.WriteLine("Invalid value. Please enter a number");
            }
        }
    }
}