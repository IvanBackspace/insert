using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Xaml
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string leftop = ""; // Левый операнд
        string operation = ""; // Знак операции
        string rightop = ""; // Правый операнд
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          

            string s = (string)((Button)e.OriginalSource).Content;
            if (s != "=")
                HistoryTextBox.Text += s;
            
         
            int num;
           
            bool result = Int32.TryParse(s, out num);
           
            if (result == true)
            {
                
                if (operation == "")
                {
                   
                    leftop += s;
                }
                else
                {
                    
                    rightop += s;
                }
            }
            // Если было введено не число
            else
            {
                // Если равно, то выводим результат операции
                if (s == "=")
                {
                    Update_RightOp();
                    HistoryTextBox2.Text += rightop;
                    operation = "";

                }
                // Очищаем поле и переменные
                else if (s == "CE")
                {
                    leftop = "";
                    rightop = "";
                    operation = "";
                    HistoryTextBox.Text = "";
                    HistoryTextBox2.Text = "";
                }
                // Получаем операцию
                else
                {
                    // Если правый операнд уже имеется, то присваиваем его значение левому
                    // операнду, а правый операнд очищаем
                    if (rightop != "")
                    {
                        Update_RightOp();
                        leftop = rightop;
                        rightop = "";
                    }
                    operation = s;
                }
            }
             void Update_RightOp()
            {
                int num1 = Int32.Parse(leftop);
                int num2 = Int32.Parse(rightop);
                // И выполняем операцию
                switch (operation)
                {
                    case "+":
                        rightop = (num1 + num2).ToString();
                        break;
                    case "-":
                        rightop = (num1 - num2).ToString();
                        break;
                    case "*":
                        rightop = (num1 * num2).ToString();
                        break;
                    case "/":
                        rightop = (num1 / num2).ToString();
                        break;
                }
            }

        }
    }
}
