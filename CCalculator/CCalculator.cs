namespace CCalculator
{
    public static class CCalculator
    {
        public static double CalcExpression_GetResult(string mathExpression)
        {
            if (string.IsNullOrWhiteSpace(mathExpression))
                throw new ArgumentException("Expression is empty or null");

            mathExpression = mathExpression.Replace(" ", "");

            if(IsOperator(mathExpression[^1]) || mathExpression[0] == '*' || mathExpression[0] == '/') 
                throw new ArgumentException("Incomplete mathematical expression");

            Stack<double> numbers = new();
            Stack<char> operators = new();
            bool isLeftBracketExist = false;

            for (int i = 0; i < mathExpression.Length; i++)
            {
                char ch = mathExpression[i];
                if (char.IsDigit(ch) || ch == '.')
                {
                    if (double.TryParse(ReadNumber(mathExpression, ref i), out double number))
                        numbers.Push(number);
                }
                else if (IsOperator(ch))
                {
                    while ((operators.Count > 0) && (GetPrecedence(ch) <= GetPrecedence(operators.Peek())))
                    {
                        ApplyOperator(numbers, operators.Pop());
                    }
                    operators.Push(ch);
                }
                else if (ch == '(')
                {
                    operators.Push(ch);
                    isLeftBracketExist = true;
                }
                else if (ch == ')')
                {
                    if(!isLeftBracketExist)
                             throw new ArgumentException($"Bracket is unpaired");

                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        ApplyOperator(numbers, operators.Pop());
                    }
                    operators.Pop();
                    isLeftBracketExist = false;
                }              
                else
                    throw new ArgumentException($"Incorrect symbol {ch}");
            }

            return ProcessRemainingOperators(numbers, operators);
        }

        public static void CalcExpressionFromFile_SaveResult(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File name is empty or null");
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
            if (new FileInfo(filePath).Length == 0)
                throw new ArgumentException($"File {filePath} is empty");

            var allExpressions = File.ReadAllLines(filePath);
            string saveFilePath = filePath.Replace(".txt", "_results.txt");

            if (File.Exists(saveFilePath))
                File.Delete(saveFilePath);

            foreach (var expression in allExpressions)
            {
                using FileStream fileStream = new(saveFilePath, FileMode.Append, FileAccess.Write);
                using StreamWriter writer = new(fileStream);
                try
                {
                    double result = CalcExpression_GetResult(expression);
                    writer.WriteLine(result);
                }
                catch (Exception ex)
                {
                    writer.WriteLine(ex.Message);
                }
            }

            Console.WriteLine($"Calculations are successfully written to the file: {saveFilePath}");
        }

        private static string ReadNumber(string expression, ref int i)
        {
            string numberStr = expression[i].ToString();
            while (i + 1 < expression.Length && (char.IsDigit(expression[i + 1]) || expression[i + 1] == '.'))
            {
                numberStr += expression[i + 1];
                i++;
            }
            return numberStr;
        }

        private static bool IsOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/';
        }

        private static void ApplyOperator(Stack<double> numbers, char op)
        {
            double b = numbers.Pop();
            double a = numbers.Pop();

            switch (op)
            {
                case '+':
                    {
                        numbers.Push(a + b);
                        break;
                    }
                case '-':
                    {
                        numbers.Push(a - b);
                        break;
                    }
                case '*':
                    {
                        numbers.Push(a * b);
                        break;
                    }
                case '/':
                    {
                        if(b == 0)
                            throw new ArgumentException("Can't divide by zero");

                        numbers.Push(a / b);
                        break;
                    }
                default:
                    throw new ArgumentException($"Invalid operator {op}");
            }
        }

        private static double ProcessRemainingOperators(Stack<double> numbers, Stack<char> operators)
        {
            while (operators.Count > 0)
            {
                ApplyOperator(numbers, operators.Pop());
            }
            return numbers.Pop();
        }

        private static int GetPrecedence(char op)
        {
            if (op == '+' || op == '-')
                return 1;
            else if (op == '*' || op == '/')
                return 2;
            else
                return 0;
        }
    }
}