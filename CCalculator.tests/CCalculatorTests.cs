using CCalculator;

namespace СCalculator.tests
{
    public class CalculatorTests
    {
        private const string TestDataDirectory = "TestData";
        private const string ValidFilePath = TestDataDirectory + "\\valid.txt";
        private const string InvalidFilePath = TestDataDirectory + "\\invalid.txt";

        private const string ValidFilePathResults = TestDataDirectory + "\\valid_results.txt";
        private const string InvalidFilePathResults = TestDataDirectory + "\\invalid_results.txt";

        private string[] ValidResults;
        private string[] InvalidResults;

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(TestDataDirectory);

            File.WriteAllText(ValidFilePath, "1+2\n3*4\n5+20/(5+5)");
            ValidResults = new string[]
            {
                "3",
                "12",
                "7",
                "8.5",
            };

            File.WriteAllText(InvalidFilePath, "1+2+\n4+20/(5-5)\n4+20/(5_5)\n2+11)/11");
            InvalidResults = new string[]
            {
                "Incomplete mathematical expression",
                "Can't divide by zero",
                "Incorrect symbol _",
                "Bracket is unpaired"
            };
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(TestDataDirectory, true);
        }

        [Test]
        public void TryAttemptWithEmptyFile_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Calculator.CalcExpressionFromFile_SaveResult(string.Empty));
        }

        [Test]
        public void InputFileWhithValidData_ReturnNewFileWhithCorrectData()
        {
            Calculator.CalcExpressionFromFile_SaveResult(ValidFilePath);

            var allExpressions = File.ReadAllLines(ValidFilePathResults);
            for (int i = 0; i < allExpressions.Length; i++)
            {
                Assert.That(ValidResults[i], Is.EqualTo(allExpressions[i]));
            }
        }

        [Test]
        public void InputFileWhithInalidData_ReturnNewFileWhithCorrectData()
        {
            Calculator.CalcExpressionFromFile_SaveResult(InvalidFilePath);

            var allExpressions = File.ReadAllLines(InvalidFilePathResults);
            for (int i = 0; i < allExpressions.Length; i++)
            {
                Assert.That(InvalidResults[i], Is.EqualTo(allExpressions[i]));
            }
        }

        [TestCase("2+2* 3/3+1", 5)]
        [TestCase("-1+2*(3+2)", 9)]
        [TestCase("2+15/3+4*2", 15)]
        public void InputSimpleExpression_ReturnCorrectResult(string expression, double result)
        {
            Assert.That(Calculator.CalcExpression_GetResult(expression), Is.EqualTo(result));
        }

        [TestCase("(-5+11)/(-2-3)", -1.2)]
        [TestCase("2*(9/3+1)+4", 12)]
        [TestCase("2*(9/3-0.5)+4*(-9/2+1.5)", -7)]
        [TestCase("(2.95-0.45)*(-11-0.5)/4*(2+1.5)", -25.15625)]
        public void InputExpressionWithBrackets_ReturnCorrectResult(string expression, double result)
        {
            Assert.That(Calculator.CalcExpression_GetResult(expression), Is.EqualTo(result));
        }

        [Test]
        public void TryDivideZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Calculator.CalcExpression_GetResult("15/(20-20)"));
        }

        [Test]
        public void TrySendEmptyExpression_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Calculator.CalcExpression_GetResult(""));
        }

        [Test]
        public void TrySendInvalidExpression_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Calculator.CalcExpression_GetResult("*5+10"));
        }

        [Test]
        public void TrySendExpressionWithUnpairedBracket_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Calculator.CalcExpression_GetResult("((5+10)-11"));
        }
    }
}