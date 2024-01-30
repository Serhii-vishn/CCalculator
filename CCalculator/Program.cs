namespace CCalculator
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine("\t\tSimple calculator");
                Console.WriteLine("\t---------------------------------");

                Console.Write("Read from file press 1, input from the console - any: ");

                if (Console.ReadKey().KeyChar == '1')
                {
                    string filePath = GetFilePathFromUser();
                    Calculator.CalcExpressionFromFile_SaveResult(filePath);
                }
                else
                {
                    Console.Write("\nEnter your expression: ");
                    string mathExpression = Console.ReadLine();

                    double result = Calculator.CalcExpression_GetResult(mathExpression);
                    Console.WriteLine($"Result: {result}");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

            Console.ReadLine();
        }

        private static string GetFilePathFromUser()
        {
            string filePath;
            do
            {
                Console.Write("\nEnter file path: ");
                filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: The entered file path is invalid or doesn't exist.");
                }
            } while (!File.Exists(filePath));

            return filePath;
        }
    }
}