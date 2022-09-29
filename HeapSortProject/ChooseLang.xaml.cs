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
using System.Windows.Shapes;

namespace HeapSortProject
{
    /// <summary>
    /// Interaction logic for ChooseLang.xaml
    /// </summary>
    public partial class ChooseLang : Window
    {
        public ChooseLang()
        {
            InitializeComponent();
        }

        private void UKClick(object sender, RoutedEventArgs e)
        {
            HeapSortUK heapSortUK = new HeapSortUK();
            heapSortUK.ShowDialog();
        }

        private void RSClick(object sender, RoutedEventArgs e)
        {
            HeapSortRS heapSortRS = new HeapSortRS();   
            heapSortRS.ShowDialog();
        }
    }
}
