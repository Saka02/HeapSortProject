using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        List<int> logicList = new List<int>();
        int execTimeMS = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary> ///
        /// 
        /// </summary>
        /// <para
        void clearBlocks()
        {
            foreach (TextBlock item in PanelS.Children)
            {
                item.Background = Brushes.Black;
            }
        }
        void AddNew(int num)
        {
            TextBlock temp = new TextBlock();

            temp.Text = num.ToString();
            temp.Background = Brushes.Black;
            PanelS.Children.Add(temp);
            temp.Width = 50;
            temp.Height = 50;
            temp.HorizontalAlignment = HorizontalAlignment.Left;
            temp.FontSize = 40;
            temp.VerticalAlignment = VerticalAlignment.Center;
            temp.TextAlignment = TextAlignment.Center;
            temp.Margin = new Thickness(5);
            temp.Foreground = Brushes.White;
            elementi.Add(temp);
            logicList.Add(int.Parse(temp.Text));
            MainIn.Focus();
            ClearInput();
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            int num = 0;
            if(int.TryParse(MainIn.Text, out num)){
                AddNew(num);
            }
            else
            {
                MessageBox.Show("Molim vas unesite broj");
            }
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            TextBlock temp = elementi.Where(item => item.Text == MainIn.Text).FirstOrDefault();
            PanelS.Children.Remove(temp);
            elementi.Remove(temp);
            logicList.Remove(int.Parse(temp.Text));
            ClearInput();
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            PanelS.Children.Clear();
            elementi.Clear();
            logicList.Clear();
        }

        void ClearInput()
        {
            MainIn.Text = string.Empty;
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
        public async void SortClick(object sender, RoutedEventArgs e)
        {
            DateTime startTime = DateTime.Now;
            await HeapSort();
            execTimeMS += (int)(DateTime.Now - startTime).TotalMilliseconds;
            MessageBox.Show($"Sorting done. Time taken: {execTimeMS.ToString()}ms");
            Status.Text = "Status: STANDBY";
        }


        async Task hipify(int INX)
        {
            int left= INX*2+1;
            int right= INX*2+2;
            int largest= INX;
            await Task.Delay(1000);
            execTimeMS -= 1000;
            clearBlocks();
            //Postavljanje boje trenutnom itemu
            if (INX > 0)
            ((TextBlock)PanelS.Children[INX]).Background = Brushes.Yellow;

            if (left < logicList.Count)
            {
                if(logicList[left] > logicList[largest])
                largest = left;
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[left]).Background= Brushes.Yellow;
            }

            if (right<logicList.Count)
            {
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[right]).Background = Brushes.Yellow;
                if (logicList[right] > logicList[largest])
                largest = right;
            }

            if(largest!= INX)
            {   
                //Postavljanje sa cime treba da se zameni u crveno
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[largest]).Background = Brushes.Red;
                ((TextBlock)PanelS.Children[INX]).Background = Brushes.Red;

                await Task.Delay(1000);
                execTimeMS -= 1000;
                int temp = logicList[largest];
                ((TextBlock)PanelS.Children[largest]).Text = ((TextBlock)PanelS.Children[INX]).Text;
                ((TextBlock)PanelS.Children[INX]).Text = temp.ToString();

                logicList[largest] = logicList[INX];
                logicList[INX] = temp;
                await hipify(largest);
            }

            return;
        }

        async Task buildHeap()
        {
            Status.Text = "Status: BUILDING HEAP";
            for(int i=logicList.Count/2-1; i>=0; i--)
            {
                await hipify(i);
            }
        }

        async Task HeapSort()
        {
            List<int> helper = new List<int>();
            await buildHeap();
            while (logicList.Count > 0)
            {
                await Task.Delay(1000);
                execTimeMS -= 1000;
                clearBlocks();
                Status.Text = "Status: SORTING";
                TextBlock tempBlock = new TextBlock();
                helper.Add(logicList.First());
                int temp = logicList.First();
                logicList[0] = logicList.Last();
                logicList[logicList.Count-1] = temp;
                logicList.Remove(logicList.Last());

                tempBlock.Width = 50;
                tempBlock.Height = 50;
                tempBlock.HorizontalAlignment = HorizontalAlignment.Left;
                tempBlock.FontSize = 40;
                tempBlock.VerticalAlignment = VerticalAlignment.Center;
                tempBlock.TextAlignment = TextAlignment.Center;
                tempBlock.Margin = new Thickness(5);
                tempBlock.Foreground = Brushes.White;
                tempBlock.Background = Brushes.Orange;

                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[0]).Background = Brushes.Orange;
                await Task.Delay(1000);
                execTimeMS -= 1000;
                tempBlock.Text = temp.ToString();
                Sorted.Children.Add(tempBlock);
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[0]).Text= " ";
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[0]).Text = ((TextBlock)PanelS.Children[PanelS.Children.Count-1]).Text;
                PanelS.Children.Remove(PanelS.Children[PanelS.Children.Count-1]);
                if(PanelS.Children.Count>0)
                Status.Text = $"Status: HEAPIFYING {((TextBlock)PanelS.Children[0]).Text}";
                await hipify(0);            
            }

            //Converting sorted back to main panel
            for(int i=0; i< helper.Count; i++) {
                await Task.Delay(1000);
                execTimeMS -= 1000;
                clearBlocks();
                Status.Text = "Status: SORTED, TRANSFERING ARRAYS";
                ((TextBlock)Sorted.Children[0]).Background = Brushes.Purple;
                await Task.Delay(1000);
                execTimeMS -= 1000;
                AddNew(helper[i]);
                Sorted.Children.Remove(((TextBlock)Sorted.Children[0]));
            }

            logicList = helper;
        }
    }
}
