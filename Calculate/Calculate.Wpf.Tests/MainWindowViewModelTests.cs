using NUnit.Framework;
using System;

namespace Calculate.Wpf.Tests
{
    public sealed class MainWindowViewModelTests
    {
        [Test]
        public void NumericClick_CommandPassedOnNumericalVal_ThrowsExpection()
        {
            var unitOfWork = new MainWindowViewModel();

            Assert.Throws<ArgumentException>(() => { unitOfWork.NumericClick.Execute("H"); });
        }

        [Test]
        public void NumericClick_SubmitExpressionIsNotNull_ResetExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;
            unitOfWork.EqualsClick.Execute(new object());

            var newExpression = "5";
            unitOfWork.NumericClick.Execute(newExpression);

            Assert.That(unitOfWork.Expression, Is.Not.EqualTo(oldExpression+newExpression));
        }

        [Test]
        public void NumericClick_SubmitExpressionIsNotNull_ResetSubmittedExpressionExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;
            unitOfWork.EqualsClick.Execute(new object());

            var newExpression = "3";
            unitOfWork.NumericClick.Execute(newExpression);

            Assert.That(unitOfWork.SubmittedExpression, Is.Null);
        }

        [Test]
        public void NumericClick_ExpressionIsZero_SetExpressionToParameter()
        {
            var unitOfWork = new MainWindowViewModel();

            var newExpression = "4";
            unitOfWork.NumericClick.Execute(newExpression);

            Assert.That(unitOfWork.Expression, Is.EqualTo(newExpression));
        }

        [Test]
        public void NumericClick_ExpressionIsNotZero_AddParameterToExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;

            var operatorSymbol = "+";
            unitOfWork.OperatorClick.Execute(operatorSymbol);

            var newExpression = "3";
            unitOfWork.NumericClick.Execute(newExpression);

            Assert.That(unitOfWork.Expression, Is.EqualTo(oldExpression + operatorSymbol + newExpression));
        }

        [Test]
        public void NumericClick_DecimalPointAsParameter_AddDecimalToExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.NumericClick.Execute(oldExpression);

            var decimalPoint = ".";
            unitOfWork.NumericClick.Execute(decimalPoint);

            var newExpression = "3";
            unitOfWork.NumericClick.Execute(newExpression);

            Assert.That(unitOfWork.Expression, Is.EqualTo(oldExpression + decimalPoint + newExpression));
        }

        [Test]
        public void NumericClick_DecimalPointToZeroAsParameter_AddDecimalToExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var decimalPoint = ".";
            unitOfWork.NumericClick.Execute(decimalPoint);

            var newExpression = "3";
            unitOfWork.NumericClick.Execute(newExpression);

            Assert.That(unitOfWork.Expression, Is.EqualTo(decimalPoint + newExpression));
        }

        [Test]
        public void CommandClick_BackspaceIsMultiCharParameter_RemoveLastCharacter()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "43";
            unitOfWork.Expression = oldExpression;

            unitOfWork.CommandClick.Execute("Backspace");

            Assert.That(unitOfWork.Expression, Is.EqualTo("4"));
        }

        [Test]
        public void CommandClick_BackspaceIsOnCharParameter_RemoveLastCharacter()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;

            unitOfWork.CommandClick.Execute("Backspace");

            Assert.That(unitOfWork.Expression, Is.EqualTo("0"));
        }

        [Test]
        public void CommandClick_ClearIsParameter_ResetExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;

            unitOfWork.CommandClick.Execute("Clear");

            Assert.That(unitOfWork.Expression, Is.EqualTo("0"));
        }

        [Test]
        public void CommandClick_InvalidParameterPassed_ExpressionIsInvalid()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;

            unitOfWork.CommandClick.Execute("Test");

            Assert.That(unitOfWork.Expression, Is.EqualTo("Invalid operator"));
        }

        [Test]
        public void CommandClick_WhenCalled_SubmittedExpressionIsNull()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.SubmittedExpression = oldExpression;

            unitOfWork.CommandClick.Execute("anything");

            Assert.That(unitOfWork.SubmittedExpression, Is.Null);
        }

        [Test]
        public void CommandClick_RoundIsParameter_WrapExpressionInRoundTag()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "43";
            unitOfWork.Expression = oldExpression;

            unitOfWork.CommandClick.Execute("Round");

            Assert.That(unitOfWork.Expression, Is.EqualTo($"r({oldExpression})"));
        }

        [Test]
        public void CommandClick_FactorialIsParameter_WrapExpressionInRoundTag()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "43";
            unitOfWork.Expression = oldExpression;

            unitOfWork.CommandClick.Execute("Factorial");

            Assert.That(unitOfWork.Expression, Is.EqualTo($"f({oldExpression})"));
        }

        [Test]
        public void OperatorClick_WhenCalled_SubmittedExpressionIsNull()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.SubmittedExpression = oldExpression;

            unitOfWork.OperatorClick.Execute("anything");

            Assert.That(unitOfWork.SubmittedExpression, Is.Null);
        }

        [Test]
        public void OperatorClick_ParameterPassed_OperatorAddedToExpression()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            unitOfWork.Expression = oldExpression;

            var operatorSymbol = "+";
            unitOfWork.OperatorClick.Execute(operatorSymbol);

            Assert.That(unitOfWork.Expression, Is.EqualTo(oldExpression+ operatorSymbol));
        }

        [Test]
        public void EqualsClick_WhenCalled_SubmittedExpressionUpdated()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            var operatorSymbol = "+";
            unitOfWork.NumericClick.Execute(oldExpression);
            unitOfWork.OperatorClick.Execute(operatorSymbol);
            unitOfWork.NumericClick.Execute(oldExpression);

            unitOfWork.EqualsClick.Execute(new object());

            Assert.That(unitOfWork.SubmittedExpression, Is.EqualTo(oldExpression + operatorSymbol + oldExpression + "="));
        }

        [Test]
        public void EqualsClick_WhenCalled_ExpressionUpdatedToCalculateResult()
        {
            var unitOfWork = new MainWindowViewModel();

            var oldExpression = "4";
            var operatorSymbol = "+";
            unitOfWork.NumericClick.Execute(oldExpression);
            unitOfWork.OperatorClick.Execute(operatorSymbol);
            unitOfWork.NumericClick.Execute(oldExpression);

            unitOfWork.EqualsClick.Execute(new object());

            Assert.That(unitOfWork.Expression, Is.EqualTo("8"));
        }

        [Test]
        public void EqualsClick_WhenInvalidExpressionPassedToCalculate_ExpressionShowsError()
        {
            var unitOfWork = new MainWindowViewModel();

            unitOfWork.Expression = "(((";

            unitOfWork.EqualsClick.Execute(new object());

            Assert.That(unitOfWork.Expression.Contains("Invalid Expression"));
        }
    }
}
