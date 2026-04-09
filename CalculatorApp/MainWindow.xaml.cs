using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double _lastValue = 0;
    private string _currentOperator = string.Empty;
    private bool _isOperatorClicked = false;
    private bool _resultDisplayed = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Number_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        string value = button.Content.ToString()!;

        if (Display.Text == "0" || _isOperatorClicked || _resultDisplayed)
        {
            Display.Text = value == "." ? "0." : value;
            _isOperatorClicked = false;
            _resultDisplayed = false;
        }
        else
        {
            if (value == "." && Display.Text.Contains("."))
                return;

            Display.Text += value;
        }
    }

    private void Operator_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        string newOperator = button.Content.ToString()!;

        if (newOperator == "=")
        {
            Calculate();
            _currentOperator = string.Empty;
            _resultDisplayed = true;
        }
        else
        {
            if (!string.IsNullOrEmpty(_currentOperator) && !_isOperatorClicked)
            {
                Calculate();
            }

            if (double.TryParse(Display.Text, CultureInfo.InvariantCulture, out double val))
            {
                _lastValue = val;
            }
            _currentOperator = newOperator;
            _isOperatorClicked = true;
            _resultDisplayed = false;
        }
    }

    private void Calculate()
    {
        if (!double.TryParse(Display.Text, CultureInfo.InvariantCulture, out double currentValue))
            return;

        double result = 0;

        switch (_currentOperator)
        {
            case "+":
                result = _lastValue + currentValue;
                break;
            case "-":
                result = _lastValue - currentValue;
                break;
            case "×":
                result = _lastValue * currentValue;
                break;
            case "÷":
                if (currentValue != 0)
                    result = _lastValue / currentValue;
                else
                {
                    MessageBox.Show("Cannot divide by zero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    result = 0;
                }
                break;
            default:
                result = currentValue;
                break;
        }

        Display.Text = result.ToString(CultureInfo.InvariantCulture);
        _lastValue = result;
    }

    private void Function_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        string function = button.Content.ToString()!;

        switch (function)
        {
            case "AC":
                Display.Text = "0";
                _lastValue = 0;
                _currentOperator = string.Empty;
                _resultDisplayed = false;
                _isOperatorClicked = false;
                break;
            case "+/-":
                if (double.TryParse(Display.Text, CultureInfo.InvariantCulture, out double val))
                {
                    Display.Text = (-val).ToString(CultureInfo.InvariantCulture);
                }
                break;
            case "%":
                if (double.TryParse(Display.Text, CultureInfo.InvariantCulture, out double valPercent))
                {
                    Display.Text = (valPercent / 100).ToString(CultureInfo.InvariantCulture);
                }
                break;
        }
    }
}
