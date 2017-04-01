using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Songbuilder.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ValidateTextBoxValue(object sender, TextCompositionEventArgs e)
        {
            bool isNumeric = this.IsNumericValue(e.Text);
            bool exceedsMaxLength = ((TextBox)e.Source).Text.Length > 1;
            

            if (exceedsMaxLength)
            {
                e.Handled = true;
            }
            else if(isNumeric == false)
            {
                e.Handled = true;
            }
        }

        private bool IsNumericValue(string text)
        {
            int number;
            return int.TryParse(text, out number);
        }

        private void ClampTimeValue(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) e.Source;

            if (textBox.Text.StartsWith("0") && textBox.Text.Length > 1)
            {
                textBox.Text = textBox.Text.TrimStart('0');
            }
            int number;
            if (int.TryParse(textBox.Text, out number))
            {
                if (number > 59) textBox.Text = "59";
            }
        }
    }
}
