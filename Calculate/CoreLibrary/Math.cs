using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    internal enum MathOperator
    {
        None,
        Plus,
        Minus,
        Divide,
        Multiply,
        Power,
        Round,
        Factorial
    }

    public static class Math
    {
        public static double Calculate(string expression, int precision = 0)
        {
            Console.WriteLine(expression);

            Stack<char> bracketValidationStack = new Stack<char>();
            Stack<MathOperator> isOperatorStack = new Stack<MathOperator>();
            Stack<double> numberStack = new Stack<double>();

            var currentNumber = 0d;
            var operatorSymbol = '+';
            var isDecimal = false;
            var decimalMultiplier = 10;

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ' ' && i < expression.Length - 2)
                {
                    var nextIsWhitespace = true;
                    while (nextIsWhitespace)
                    {
                        var ch = expression[i];
                        if (ch != ' ')
                        {
                            nextIsWhitespace = false;
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                else if (expression[i] == ' ' && i == expression.Length - 1)
                {
                    break;
                }

                var c = expression[i];
                if (char.IsDigit(c))
                {
                    if (isDecimal)
                    {
                        var decimalNumber = c - '0';
                        double decimalNumberDouble = decimalNumber;

                        if (decimalNumberDouble > 0 || decimalMultiplier > 0)
                            decimalNumberDouble /= decimalMultiplier;

                        currentNumber += decimalNumberDouble;

                        decimalMultiplier *= 10;
                    }
                    else
                    {
                        currentNumber *= 10;
                        currentNumber += c - '0';
                    }
                }
                else if (c == '(')
                {
                    isOperatorStack.Push(GetOperatorFromChar(operatorSymbol));
                    numberStack.Push(operatorSymbol);
                    currentNumber = 0;
                    operatorSymbol = '+';
                    bracketValidationStack.Push(c);
                }
                else if (c == 'r' || c == 'f')
                {
                    isOperatorStack.Push(GetOperatorFromChar(operatorSymbol));
                    numberStack.Push(operatorSymbol);
                    currentNumber = 0;
                    operatorSymbol = c;

                    Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == ')')
                {
                    isDecimal = false;
                    decimalMultiplier = 10;

                    Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                    if (c == ')')
                    {
                        if (bracketValidationStack.Count == 0)
                        {
                            throw new InvalidExpressionException();
                        }

                        bracketValidationStack.Pop();

                        currentNumber = 0;

                        var lastOperator = isOperatorStack.Peek();

                        while (lastOperator == MathOperator.None)
                        {
                            currentNumber += numberStack.Pop();
                            isOperatorStack.Pop();

                            lastOperator = isOperatorStack.Peek();
                        }

                        lastOperator = isOperatorStack.Pop();
                        switch (lastOperator)
                        {
                            case MathOperator.Plus:
                                operatorSymbol = '+';
                                numberStack.Pop();
                                Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                                break;
                            case MathOperator.Minus:
                                operatorSymbol = '-';
                                numberStack.Pop();
                                Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                                break;
                            case MathOperator.Divide:
                                operatorSymbol = '/';
                                numberStack.Pop();
                                Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                                break;
                            case MathOperator.Multiply:
                                operatorSymbol = '*';
                                numberStack.Pop();
                                Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                                break;
                            case MathOperator.Power:
                                operatorSymbol = '^';
                                numberStack.Pop();
                                Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);
                                break;
                            case MathOperator.Round:
                                RoundValues(numberStack, isOperatorStack, currentNumber);                                
                                break;
                            case MathOperator.Factorial:
                                FactorialiseValues(numberStack, isOperatorStack, currentNumber);
                                break;
                        }
                    }

                    currentNumber = 0;
                    operatorSymbol = c;
                }
                else if (c == '.')
                {
                    isDecimal = true;
                }
            }

            Push(operatorSymbol, currentNumber, numberStack, isOperatorStack);

            if (bracketValidationStack.Count != 0)
            {
                throw new InvalidExpressionException();
            }

            double result;
            if (precision != 0)
            {
                result = Round((double)numberStack.Sum(), precision);
            }
            else
            {
                result = (double)numberStack.Sum();
            }

            return result;
        }

        public static int Factorial(int number)
        {
            if (number == 0)
            {
                return 1;
            }

            return number * Factorial(number - 1);
        }

        public static double Round(double value, int precision = 0)
        {
            if (value == 0)
                return 0;

            StringBuilder stringBuilder = new StringBuilder();
            string str = value.ToString();

            var afterDecimal = false;
            var currentPrecision = 1;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (char.IsDigit(c))
                {
                    if (afterDecimal)
                    {
                        if (currentPrecision > precision)
                        {
                            var decimalNumber = c - '0';
                            if (decimalNumber > 5)
                            {
                                if (precision == 0)
                                {
                                    var currentResult = double.Parse(stringBuilder.ToString());
                                    if (currentResult < 0)
                                    {
                                        currentResult--;
                                    }
                                    else
                                    {
                                        currentResult++;
                                    }
                                    return currentResult;
                                }
                            }
                            break;
                        }
                        else if (currentPrecision == precision)
                        {
                            var decimalNumber = c - '0';

                            if (i + 1 < str.Length)
                            {
                                var next = str[i + 1] - '0';
                                if (next < 5)
                                {
                                    stringBuilder.Append(decimalNumber);
                                }
                                else
                                {
                                    stringBuilder.Append(++decimalNumber);
                                }
                            }
                            else
                            {
                                stringBuilder.Append(c);
                            }

                            currentPrecision++;
                        }
                        else
                        {
                            stringBuilder.Append(c);
                            currentPrecision++;
                        }
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }
                }
                else if (c == '.')
                {
                    afterDecimal = true;
                    if (precision != 0)
                        stringBuilder.Append(c);
                }
                else if (c == '-')
                {
                    stringBuilder.Append(c);
                }
            }

            return double.Parse(stringBuilder.ToString());
        }

        private static void Push(char o, double n, Stack<double> numberStack, Stack<MathOperator> isOperatorStack)
        {
            if (o == '+')
            {
                numberStack.Push(n);
                isOperatorStack.Push(MathOperator.None);
            }
            else if (o == '-')
            {
                numberStack.Push(-n);
                isOperatorStack.Push(MathOperator.None);
            }
            else if (o == '*')
            {
                isOperatorStack.Pop();
                var last = numberStack.Pop();
                numberStack.Push(last * n);
                isOperatorStack.Push(MathOperator.None);
            }
            else if (o == '/')
            {
                isOperatorStack.Pop();
                var last = numberStack.Pop();
                numberStack.Push(last / n);
                isOperatorStack.Push(MathOperator.None);
            }
            else if (o == '^')
            {
                isOperatorStack.Pop();
                var last = numberStack.Pop();

                var returnToNegative = false;
                if (last < 0)
                {
                    last = System.Math.Abs(last);
                    returnToNegative = true;
                }

                var copy = last;
                for (var j = n; j != 1; j--)
                {
                    last *= copy;
                }

                if (returnToNegative)
                {
                    last *= -1;
                }

                numberStack.Push(last);
                isOperatorStack.Push(MathOperator.None);
            }
        }

        private static void RoundValues(Stack<double> numberStack, Stack<MathOperator> isOperatorStack, double currentNumber)
        {
            numberStack.Pop();

            if (numberStack.Count > 0)   //  This only occurs if roound was not the very last char in the expression.
                numberStack.Pop();

            var lastOperator = isOperatorStack.Peek();

            if (lastOperator == MathOperator.None || lastOperator == MathOperator.Plus || lastOperator == MathOperator.Minus)
                isOperatorStack.Pop();

            currentNumber = Round(currentNumber);

            if (lastOperator == MathOperator.None || lastOperator == MathOperator.Plus || lastOperator == MathOperator.Minus)
            {
                if (currentNumber > 0)
                {
                    Push('+', currentNumber, numberStack, isOperatorStack);
                }
                else
                {
                    Push('-', currentNumber, numberStack, isOperatorStack);
                }
            }
            else
            {
                Push(GetCharFromOperator(isOperatorStack.Pop()), currentNumber, numberStack, isOperatorStack);
            }
        }

        private static void FactorialiseValues(Stack<double> numberStack, Stack<MathOperator> isOperatorStack, double currentNumber)
        {
            numberStack.Pop();

            if (numberStack.Count > 0)   //  This only occurs if roound was not the very last char in the expression.
                numberStack.Pop();

            var lastOperator = isOperatorStack.Peek();

            if (lastOperator == MathOperator.None || lastOperator == MathOperator.Plus || lastOperator == MathOperator.Minus)
                isOperatorStack.Pop();

            var toIntValue = Convert.ToInt32(currentNumber);

            currentNumber = Factorial(toIntValue);

            if (lastOperator == MathOperator.None || lastOperator == MathOperator.Plus || lastOperator == MathOperator.Minus)
            {
                if (currentNumber > 0)
                {
                    Push('+', currentNumber, numberStack, isOperatorStack);
                }
                else
                {
                    Push('-', currentNumber, numberStack, isOperatorStack);
                }
            }
            else
            {
                Push(GetCharFromOperator(isOperatorStack.Pop()), currentNumber, numberStack, isOperatorStack);
            }
        }

        private static MathOperator GetOperatorFromChar(char operatorSymbol)
        {
            switch (operatorSymbol)
            {
                case '+':
                    return MathOperator.Plus;
                case '-':
                    return MathOperator.Minus;
                case '*':
                    return MathOperator.Multiply;
                case '/':
                    return MathOperator.Divide;
                case '^':
                    return MathOperator.Power;
                case 'r':
                    return MathOperator.Round;
                case 'f':
                    return MathOperator.Factorial;
                default:
                    throw new ArgumentException($"Invalid Char of {operatorSymbol}");
            }
        }

        private static char GetCharFromOperator(MathOperator operation)
        {
            switch (operation)
            {
                case MathOperator.Plus:
                    return '+';
                case MathOperator.Minus:
                    return '-';
                case MathOperator.Divide:
                    return '/';
                case MathOperator.Multiply:
                    return '*';
                case MathOperator.Power:
                    return '^';
                case MathOperator.Round:
                    return 'r';
                case MathOperator.Factorial:
                    return 'f';
                default:
                    return '\0';
            }
        }
    }
}