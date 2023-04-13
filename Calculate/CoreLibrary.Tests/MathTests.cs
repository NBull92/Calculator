using NUnit.Framework;
using CoreLibrary;

namespace Calculate.Tests
{
    public class MathTests
    {
        [TestCase("2 + 5", 7)]
        [TestCase("8 - 3", 5)]
        [TestCase("80 - 2", 78)]
        [TestCase("8 + 10", 18)]
        [TestCase("5 * 4", 20)]
        [TestCase("8 / 2", 4)]
        [TestCase("4 ^ 2", 16)]
        public void Calculate_ExpressionIsValidWithTwoNumbers_ResultEqualsExpected(string expression, int expected)
        {
            var result = Math.Calculate(expression);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("1 + 2 * 3", 7)]
        [TestCase(" 2-1 + 2 ", 3)]
        [TestCase("6 + 3 - 2 + 12", 19)]
        [TestCase("2 * 15 + 23", 53)]
        [TestCase("10 - 3 ^ 2", 1)]
        public void Calculate_ExpressionIsValidWithMoreThanTwoNumbers_ResultEqualsExpected(string expression, int expected)
        {
            var result = Math.Calculate(expression);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("(1 + 2) * 3", 9)]
        [TestCase("(1+(4+5+2)-3)+(6+8)", 23)]
        [TestCase("7 - ( -5)", 12)]
        public void Calculate_ExpressionHasValidBrackets_ResultEqualsExpected(string expression, int expected)
        {
            var result = Math.Calculate(expression);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("-53 + -24", -77)]
        [TestCase("(-20 * 1.8) / 2", -18)]
        public void Calculate_ExpressionHasNegativeNumbers_ResultEqualsExpected(string expression, double expected)
        {
            var result = Math.Calculate(expression);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("3.5 * 3", 10.5)]
        [TestCase("10 / 3 ", 3.333, 3)]
        [TestCase("-12.315 - 42", -54.315)]
        public void Calculate_ExpressionHasOrReturnsDecimalNumbers_ResultEqualsExpected(string expression, double expected, int precision = 0)
        {
            var result = Math.Calculate(expression, precision);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("(")]
        [TestCase("(()")]
        [TestCase("(((1 + 2) * 3)")]
        [TestCase("((1 + 2)) * 3)")]
        public void Calculate_InvalidBrackets_ThrowsInvalidExpressionException(string expression)
        {
            Assert.Throws<InvalidExpressionException>(() => Math.Calculate(expression));
        }

        [TestCase(10, 3628800)]
        [TestCase(8, 40320)]
        [TestCase(5, 120)]
        [TestCase(3, 6)]
        [TestCase(0, 1)]
        public void Factorial_WhenCalled_ReturnsExpected(int n, int expected)
        {
            var result = Math.Factorial(n);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(12.315, 0)]
        [TestCase(12.315, 1)]
        [TestCase(12.315, 2)]
        [TestCase(12.315, 3)]
        [TestCase(12.3154, 3)]
        [TestCase(12.3157, 3)]
        [TestCase(12, 3)]
        [TestCase(12.6, 0)]
        [TestCase(-12.315, 0)]
        [TestCase(-12.315, 1)]
        [TestCase(-12.315, 2)]
        [TestCase(-12.315, 3)]
        [TestCase(-12.3154, 3)]
        [TestCase(-12.3157, 3)]
        [TestCase(-12, 3)]
        [TestCase(-12.6, 0)]
        [TestCase(10.5, 0)]
        public void Round_WhenCalled_ReturnsExpected(double value, int precision)
        {
            var result = Math.Round(value, precision);
            Assert.That(result, Is.EqualTo(System.Math.Round(value, precision)));
        }

        [TestCase("r(3.5 * 3)", 10)]
        [TestCase("r(9.6) / r(3.1) ", 3.333, 3)]
        [TestCase("r(r(9.6) / r(3.1) )", 3)]
        public void Calculate_RoundAddedToExpression_ResultEqualsRoundedExpected(string expression, double expected, int precision = 0)
        {
            var result = Math.Calculate(expression, precision);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("f(10)", 3628800)]
        [TestCase("f(3.5 * 3)", 3628800)]
        [TestCase("f(r(9.6) / r(3.1) )", 6)]
        public void Calculate_FactorialAddedToExpression_ResultEqualsRoundedExpected(string expression, double expected, int precision = 0)
        {
            var result = Math.Calculate(expression, precision);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
