using CCalculator;

namespace CCalculator.tests
{
    public class CCalculatorTests
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
            ValidResults = new string [] 
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
            Assert.Throws<ArgumentException>(() => CCalculator.CalcExpressionFromFile_SaveResult(string.Empty));
        }

        [Test]
        public void InputFileWhithValidData_ReturnNewFileWhithCorrectData()
        {
            CCalculator.CalcExpressionFromFile_SaveResult(ValidFilePath);

            var allExpressions = File.ReadAllLines(ValidFilePathResults);
            for (int i = 0; i < allExpressions.Length; i++)
            {
                Assert.That(ValidResults[i], Is.EqualTo(allExpressions[i]));
            }
        }

        [Test]
        public void InputFileWhithInalidData_ReturnNewFileWhithCorrectData()
        {
            CCalculator.CalcExpressionFromFile_SaveResult(InvalidFilePath);

            var allExpressions = File.ReadAllLines(InvalidFilePathResults);
            for (int i = 0; i < allExpressions.Length; i++)
            {
                Assert.That(InvalidResults[i], Is.EqualTo(allExpressions[i]));
            }
        }

        [TestCase("2+2* 3/3+1", 5)]
        [TestCase("1+2*(3+2)", 11)]
        [TestCase("2+15/3+4*2", 15)]
        public void InputExpression_ReturncorrectResult(string expression, double result)
        {
            Assert.That(CCalculator.CalcExpression_GetResult(expression), Is.EqualTo(result));
        }

        [Test]
        public void TryDivideZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => CCalculator.CalcExpression_GetResult("15/(20-20)"));
        }

        [Test]
        public void TrySendEmptyExpression_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => CCalculator.CalcExpression_GetResult(""));
        }
    }
}