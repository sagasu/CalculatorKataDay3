using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace kata
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void Add_EmptyString_ReturnsZero()
        {
            var sum = Calculator.Add(string.Empty);
            
            Assert.AreEqual(0, sum);
        }
        
        [TestCase("1",1)]
        [TestCase("2",2)]
        [TestCase("3",3)]
        public void Add_OneNumber_ReturnsThatNumber(string numbers, int expectedSum)
        {
            var sum = Calculator.Add(numbers);
            
            Assert.AreEqual(expectedSum, sum);
        }
        
        
        [TestCase("1,2",3)]
        [TestCase("3,4",7)]
        public void Add_MultipleNumbers_ReturnsSum(string numbers, int expectedSum)
        {
            var sum = Calculator.Add(numbers);
            
            Assert.AreEqual(expectedSum, sum);
        }
        
        [TestCase("3\n4",7)]
        [TestCase("1\n2\n3",6)]
        public void Add_NewLineDelimiter_ReturnsSum(string numbers, int expectedSum)
        {
            var sum = Calculator.Add(numbers);
            
            Assert.AreEqual(expectedSum, sum);
        }

        [TestCase("//;\n1;2", 3)]
        [TestCase("//;\n1;2,3", 6)]
        [TestCase("//;\n1,2;3", 6)]
        [TestCase("//;\n1;2;3", 6)]
        public void Add_CustomDelimiter_ReturnsSum(string numbers, int expectedSum)
        {
            var sum = Calculator.Add(numbers);

            Assert.AreEqual(expectedSum, sum);
        }

        [TestCase("1,-2")]
        [TestCase("-1")]
        public void Add_NegativeNumber_ReturnsSum(string numbers)
        {
            Assert.Throws<ArgumentException>(() => Calculator.Add(numbers));
        }

        [TestCase("1001,2", 2)]
        [TestCase("1000,2", 1002)]
        [TestCase("10000,2", 2)]
        public void Add_NumbersGratedThanThousendAreIgnored_ReturnsSum(string numbers, int expectedSum)
        {
            var sum = Calculator.Add(numbers);

            Assert.AreEqual(expectedSum, sum);
        }

        [TestCase("//;;;\n1;;;2", 3)]
        public void Add_DelimitersCanBeOfAnyLength_ReturnsSum(string numbers, int expectedSum)
        {
            var sum = Calculator.Add(numbers);

            Assert.AreEqual(expectedSum, sum);
        }

    }

    public static class Calculator
    {
        private const char NEW_LINE_DELIMITER = '\n';
        private const char DEFAULT_DELIMITER = ',';

        public static int Add(string numbers)
        {
            if(string.IsNullOrEmpty(numbers))
                return 0;

            var delimiters = new List<char>() {DEFAULT_DELIMITER, NEW_LINE_DELIMITER};

            if (numbers.StartsWith("//"))
            {
                numbers = numbers.Replace("//", "");

                var endOfNewDelimiterIndex = numbers.IndexOf(NEW_LINE_DELIMITER);
                var newDelimiter = numbers.Substring(0,endOfNewDelimiterIndex);

                numbers = numbers.TrimStart(newDelimiter).TrimStart(NEW_LINE_DELIMITER);
                delimiters.Add(newDelimiter);
            }

            var sum = numbers.Split(delimiters.ToArray()).Sum(_ => GetNumberOrThrowIfNegative(_));

            return sum;
        }

        private static int GetNumberOrThrowIfNegative(string numberToParse)
        {
            var number = int.Parse(numberToParse);
            if (number < 0)
                throw new ArgumentException("Negatives not allowed");
            if (number > 1000)
                return 0;
            return number;
        }
    }
}
