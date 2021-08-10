using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace idm_link_generator
{
    public partial class MainWindow : Window
    {
        string Address = "";
        readonly string HintItem = "use < for season# and > for episod#";
        public MainWindow()
        {
            InitializeComponent();
            listbox.Items.Add(HintItem);
        }

        private void AddSeries_click(object sender, RoutedEventArgs e)
        {
            if (inputRegex.Text.Length > 0)
            {
                string txt = inputRegex.Text;
                try
                {
                    for (int i = int.Parse(SfomINT.Text); i <= int.Parse(StoINT.Text); i++)
                    {
                        for (int j = int.Parse(fomINT.Text); j <= int.Parse(toINT.Text); j++)
                        {
                            string rowString = txt
                                    .Replace("<", WildCast(i, int.Parse(SwcSize.Text)))
                                    .Replace(">", WildCast(j, int.Parse(wcSize.Text)));
                            listbox.Items.Add(rowString);
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {

            }
        }
        string WildCast(int input, int wc)
        {
            string s = input.ToString();
            if (s.Length == wc)
            {
                return s;
            }
            string tmp = "";
            for (int i = s.Length; i < wc; i++)
            {
                tmp += "0";
            }
            return tmp + s;
        }




        private void NewFile_click(object sender, RoutedEventArgs e)
        {
            listbox.Items.Clear();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = string.Format("{0}Text files (*.txt)|*.txt|All files (*.*)|*.*", "Idm download "),
                RestoreDirectory = true,
                CheckFileExists = false
            };
            if (saveFileDialog.ShowDialog().HasValue)
            {
                Address = saveFileDialog.FileName;
                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Address);
                streamWriter.Write("");
                streamWriter.Dispose();
                AddSession.Visibility = AddSeries.Visibility = AddEpisod.Visibility = Visibility.Visible;
                save.IsEnabled = true;
            }
        }

        private void SaveFile_click(object sender, RoutedEventArgs e)
        {
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Address);
            for (int i = 0; i < listbox.Items.Count; i++)
            {
                streamWriter.WriteLine(listbox.Items[i]);
            }
            streamWriter.Dispose();
            MessageBox.Show("Saved ");
        }

        private void Listbox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var content = (sender as ListBox).SelectedItem;
            if (content.ToString() != HintItem)
            {
                MessageBoxResult deleteResult = MessageBox.Show(messageBoxText: $"Do you want to delete?\n{content}", caption: "", button: MessageBoxButton.YesNo);
                if (deleteResult == MessageBoxResult.Yes)
                {
                    // (sender as ListBox).Items.Remove(content);
                    //listbox.Items.Remove(content);
                    //https://www.codeproject.com/Questions/1205489/How-to-delete-seleted-item-from-a-listbox-using-MV

                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = string.Format("{0}Text files (*.txt)|*.txt|All files (*.*)|*.*", "Idm download "),
                RestoreDirectory = true,
                CheckFileExists = false
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Address = openFileDialog.FileName;
                save.IsEnabled = true;
                AddSession.Visibility = AddSeries.Visibility = AddEpisod.Visibility = Visibility.Visible;
                listbox.Items.Clear();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(Address);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    listbox.Items.Add(line);
                }  //TODO fix lines between
                streamReader.Dispose();
                MessageBox.Show("Loaded ");
            }
        }

        private void AddEpisod_click(object sender, RoutedEventArgs e)
        {
            listbox.Items.Add(inputRegex.Text);
        }

        private void AddSession_click(object sender, RoutedEventArgs e)
        {
            if (inputRegex.Text.Length > 0)
            {
                string txt = inputRegex.Text;

                try
                {
                    for (int j = int.Parse(fomINT.Text); j <= int.Parse(toINT.Text); j++)
                    {
                        string rowString = txt
                                .Replace(">", WildCast(j, int.Parse(wcSize.Text)));
                        listbox.Items.Add(rowString);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {

            }
        }
    }
}
