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

namespace HeapSortProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TextBlock> elementi = new List<TextBlock>(); 
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary> ///
        /// 
        /// </summary>
        /// <para
        private void AddClick(object sender, RoutedEventArgs e)
        {
            TextBlock temp = new TextBlock();
            int tempnum = 0;
            if(int.TryParse(MainIn.Text, out tempnum))
            {
                temp.Text = MainIn.Text;
                
                temp.Background = Brushes.Black;
                PanelS.Children.Add(temp);
                temp.Width = 50;
                temp.Height = 50;
                temp.HorizontalAlignment = HorizontalAlignment.Left;
                elementi.Add(temp);
                temp.FontSize = 40;
                temp.VerticalAlignment = VerticalAlignment.Center;
                temp.TextAlignment = TextAlignment.Center;
                temp.Margin = new Thickness(5);
                temp.Foreground = Brushes.White;
            }
            else
            {
                MessageBox.Show("Molim vas unesite broj");
            }
            ClearInput();
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            TextBlock temp = elementi.Where(item => item.Text == MainIn.Text).FirstOrDefault();
            PanelS.Children.Remove(temp);
            elementi.Remove(temp);
            ClearInput();
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            PanelS.Children.Clear();
            elementi.Clear();
        }

        void ClearInput()
        {
            MainIn.Text = string.Empty;
        }

        void Heapify(int index)
        {

        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {

        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {

        }
        private void InfoClick(object sender, RoutedEventArgs e)
        {
            ChooseLang chooseLang = new ChooseLang();
            chooseLang.ShowDialog();

        }
        private void SortClick(object sender, RoutedEventArgs e)
        {

        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
