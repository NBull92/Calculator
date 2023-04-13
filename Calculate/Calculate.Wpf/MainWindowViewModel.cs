using Calculate.Wpf.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Math = CoreLibrary.Math;

namespace Calculate.Wpf
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
		private string _expression;
        public string Expression
		{
			get => _expression;
			set 
			{ 
				_expression = value;
                OnPropertyChanged();
            }
		}

        private string _submittedExpression;
        public string SubmittedExpression
        {
            get => _submittedExpression;
            set 
            { 
                _submittedExpression = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand CommandClick { get; }
        public DelegateCommand NumericClick { get; }
        public DelegateCommand OperatorClick { get; }
        public DelegateCommand EqualsClick { get; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MainWindowViewModel()
        {
            ResetExpression();
            SubmittedExpression = null;
            CommandClick = new DelegateCommand(OnCommandClick);
            NumericClick = new DelegateCommand(OnNumericClick);
            OperatorClick = new DelegateCommand(OnOperatorClick);
            EqualsClick = new DelegateCommand(OnEqualsClick);
        }

        private void OnEqualsClick(object obj)
        {
            SubmittedExpression = $"{Expression}=";
            try
            {
                var result = Math.Calculate(Expression);
                Expression = result.ToString();
            }
            catch (Exception ex)
            {
                Expression = ex.Message;
            }
        }

        private void OnOperatorClick(object parameter)
        {
            SubmittedExpression = null;
            Expression += parameter;
        }

        private void OnCommandClick(object parameter)
        {
            switch (parameter)
            {
                case "Backspace":
                    if(Expression.Length > 1)
                    {
                        Expression = Expression.Substring(0, Expression.Length - 1);
                    }
                    else if(Expression.Length == 1)
                    {
                        ResetExpression();
                    }
                    break;
                case "Clear":
                    ResetExpression();
                    break;
                case "Round":
                    Expression = $"r({Expression})";
                    break;
                case "Factorial":
                    Expression = $"f({Expression})";
                    break;
                default:
                    Expression = "Invalid operator";
                    break;
            }

            SubmittedExpression = null;
        }

        private void OnNumericClick(object parameter)
        {
            if (!char.IsDigit(Convert.ToChar(parameter)) && parameter.ToString() != ".")
                throw new ArgumentException($"{nameof(parameter)} is not a numerical value.");

            if(!string.IsNullOrWhiteSpace(SubmittedExpression))
            {
                ResetExpression();
            }

            SubmittedExpression = null;

            if (Expression == "0")
            {
                Expression = parameter.ToString();
            }
            else
            {
                Expression += parameter;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ResetExpression()
        {
            Expression = "0";
        }
    }
}
