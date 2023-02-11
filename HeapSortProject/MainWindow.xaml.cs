using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HeapSortProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = "C:\\Users\\Samir123\\Desktop\\HeapSortProject\\HeapSortProject\\"; 
        SqlConnector SqlConnector;
        List<TextBlock> elementi = new List<TextBlock>();
        string arrayName;
        string time; 
        List<string> nizoviIzBaze = new List<string>(); 
        List<int> logicList = new List<int>();
        int execTimeMS = 0;
        
        public MainWindow()
        {
            SqlConnector = new SqlConnector(path);
            InitializeComponent();
            LoadArrayNames();
            cbNizovi.ItemsSource = nizoviIzBaze;
            // ovde nizoviIzBaze popuni imenima svih nizova sta imas u bazi posle listu povezi na dropbox il kako vec se zove, za populaciju ove liste ima sql komanda u sqlconnector <------------------------------------------
        }
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
            if (int.TryParse(MainIn.Text, out num))
            {
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
            if(Status.Text == "Status: STANDBY")
            {
                string nodeString = "";
                time = execTimeMS.ToString();
                arrayName = tbImeNiza.Text;// ovde dodeli teksst iz textboxa za ime <------------------------------------------
                if (nizoviIzBaze.Contains(arrayName) && MessageBox.Show($"Are you sure you want to update the graph: {arrayName}", "Graph Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes) // promeni tekst ove poruke pls <------------------------------------------------------------
                {
                    foreach (int n in logicList)
                    {
                        nodeString += n.ToString() + ",";
                    }

                    SqlConnector.Edit(nodeString, arrayName, time);
                }
                else if (arrayName != "" && elementi.Count > 0)
                {
                    foreach (int n in logicList)
                    {
                        nodeString += n.ToString() + ",";
                    }
                    SqlConnector.Write(nodeString, arrayName, time);
                    nizoviIzBaze.Clear();
                    LoadArrayNames();
                    cbNizovi.ItemsSource = null;
                    cbNizovi.ItemsSource = nizoviIzBaze;
                }
            }
        }

        private void LoadArrayNames()
        {
            nizoviIzBaze.Clear();
            DataTable dt = SqlConnector.ReadArrays();

            foreach (DataRow dr in dt.Rows)
            {
                nizoviIzBaze.Add(dr["ArrayName"].ToString());
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            // kad ovo kliknes ovaj brat ucita iz sql string ime iza baze i stavi u textbox, a posle uscita niz noda npr 1,2,4,54,57, i posle taj string .split() i doda u ovu logicList, i vidi kako ces posel ovu elementi listu popunit <------------------------------------------
            
            if(cbNizovi.SelectedItem != null)
            {
                string selected = cbNizovi.SelectedItem.ToString();
                arrayName = selected;
                tbImeNiza.Text = selected;

                DataTable dt = SqlConnector.ReadArrayElements(selected);

                string ele = null;

                foreach (DataRow dr in dt.Rows)
                {
                    ele = dr["Nodes"].ToString();
                }

                string[] Nodes = ele.Split(',');
                Nodes = Nodes.Reverse().Skip(1).Reverse().ToArray();
               
                foreach(TextBlock Node in elementi.ToList())
                {
                    TextBlock temp = elementi.Where(item => item.Text == Node.Text).FirstOrDefault();
                    PanelS.Children.Remove(temp);
                    elementi.Remove(temp);
                    logicList.Remove(int.Parse(temp.Text));
                }

                foreach(string Node in Nodes)
                {
                    AddNew(int.Parse(Node));
                }

            }
            
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
            int left = INX * 2 + 1;
            int right = INX * 2 + 2;
            int largest = INX;
            await Task.Delay(1000);
            execTimeMS -= 1000;
            clearBlocks();
            //Postavljanje boje trenutnom itemu
            if (INX > 0)
                ((TextBlock)PanelS.Children[INX]).Background = Brushes.Yellow;

            if (left < logicList.Count)
            {
                if (logicList[left] > logicList[largest])
                    largest = left;
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[left]).Background = Brushes.Yellow;
            }

            if (right < logicList.Count)
            {
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[right]).Background = Brushes.Yellow;
                if (logicList[right] > logicList[largest])
                    largest = right;
            }

            if (largest != INX)
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
            for (int i = logicList.Count / 2 - 1; i >= 0; i--)
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
                logicList[logicList.Count - 1] = temp;
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
                ((TextBlock)PanelS.Children[0]).Text = " ";
                await Task.Delay(1000);
                execTimeMS -= 1000;
                ((TextBlock)PanelS.Children[0]).Text = ((TextBlock)PanelS.Children[PanelS.Children.Count - 1]).Text;
                PanelS.Children.Remove(PanelS.Children[PanelS.Children.Count - 1]);
                if (PanelS.Children.Count > 0)
                    Status.Text = $"Status: HEAPIFYING {((TextBlock)PanelS.Children[0]).Text}";
                await hipify(0);
            }

            //Converting sorted back to main panel
            for (int i = 0; i < helper.Count; i++)
            {
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

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            arrayName = tbImeNiza.Text;
            if (nizoviIzBaze.Contains(arrayName) && MessageBox.Show($"Are you sure you want to delete the graph: {arrayName}", "Graph Deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SqlConnector.Delete(arrayName);

                LoadArrayNames();

                cbNizovi.ItemsSource = null;
                cbNizovi.ItemsSource = nizoviIzBaze;

                foreach (TextBlock Node in elementi.ToList())
                {
                    TextBlock temp = elementi.Where(item => item.Text == Node.Text).FirstOrDefault();
                    PanelS.Children.Remove(temp);
                    elementi.Remove(temp);
                    logicList.Remove(int.Parse(temp.Text));
                }

                tbImeNiza.Text = "";
            }
        }
    }
}
