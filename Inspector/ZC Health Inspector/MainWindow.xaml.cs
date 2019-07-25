using System;
using System.Collections.ObjectModel;
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
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace ZC_Health_Inspector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        struct foodItemEng
        {
            public byte type { get; set; }
            public byte type2 { get; set; }
            public string name { get; set; }
            public short cost { get; set; }
            public byte level { get; set; }
            public short minutes { get; set; }
            public short servings { get; set; }
            public short pricePerServing { get; set; }
            public short xpEarned { get; set; }
            public short ID { get; set; }
            public byte idk4 { get; set; }
            public byte idk5 { get; set; }
            public short idk6 { get; set; }
            public short idk7 { get; set; }
        }

        struct foodItemJap
        {
            public byte type { get; set; }
            public byte type2 { get; set; }
            public string name { get; set; }
            public short cost { get; set; }
            public byte level { get; set; }
            public short minutes { get; set; }
            public short servings { get; set; }
            public short pricePerServing { get; set; }
            public short xpEarned { get; set; }
            public short ID { get; set; }
            public byte idk4 { get; set; }
        }

        struct characterEng
        {
            public byte one { get; set; }
            public byte two { get; set; }
            public short three { get; set; }
            public short four { get; set; }
            public short five { get; set; }
            public string six { get; set; }
            public short seven { get; set; }
        }

        Type currentFileType;    

        public MainWindow()
        {
            InitializeComponent();

            //byte[] data = File.ReadAllBytes(@"C:\Users\Dylan\Desktop\zce\out\assets\data\foodData.bin.mid");         //English
            //byte[] data = File.ReadAllBytes(@"C:\Users\Dylan\Desktop\zc\out\assets\data\foodData.bin.mid");             //Japanese
            //readFood(data);
        }



        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Binary Files (*.bin.mid)|*.bin.mid|All files (*.*)|*.*";
            of.ShowDialog();
            if(of.FileName.Length > 1)
            {
                byte[] data = File.ReadAllBytes(of.FileName);

                BinaryReader br = new BinaryReader(data);
                if (br.ReadStruct<foodItemEng>().servings == 12 && br.ReadStruct<foodItemEng>().servings == 20)
                {
                    br.index = 0;
                    List<foodItemEng> items = br.ReadStructArray<foodItemEng>();

                    DisplayView.Items.Clear();
                    foreach (foodItemEng item in items)
                    {
                        DisplayView.Items.Add(item);
                    }
                    DisplayView.Items.Refresh();


                    currentFileType = typeof(foodItemEng);
                    Console.WriteLine("CurrentFileType: " + currentFileType);
                    return;
                }

                br.index = 0;
                if (br.ReadStruct<foodItemJap>().servings == 12 && br.ReadStruct<foodItemJap>().servings == 20)
                {
                    br.index = 0;
                    List<foodItemJap> items = br.ReadStructArray<foodItemJap>();

                    DisplayView.Items.Clear();
                    foreach(foodItemJap item in items)
                    {
                        DisplayView.Items.Add(item);
                    }
                    DisplayView.Items.Refresh();
                    currentFileType = typeof(foodItemJap);
                    return;
                }

            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Binary Files (*.bin.mid)|*.bin.mid|All files (*.*)|*.*";
            sf.ShowDialog();
            if(sf.FileName.Length > 1)
            {
                BinaryWriter bw = new BinaryWriter();
                foreach(var item in DisplayView.Items)
                {
                    bw.writeStruct(Convert.ChangeType(item, currentFileType), currentFileType);
                }

                if(currentFileType == typeof(foodItemJap))
                {
                    bw.writeByte((byte)0x04);
                }
                else if(currentFileType == typeof(foodItemEng))
                {
                    bw.removeByte();
                }

                bw.writeToFile(sf.FileName);
            }
        }

        private void DisplayView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (currentFileType == typeof(foodItemJap))
                {
                    NewNameTextBox.Text = ((foodItemJap)(DisplayView.SelectedItem)).name;
                }
                else if (currentFileType == typeof(foodItemEng))
                {
                    NewNameTextBox.Text = ((foodItemEng)(DisplayView.SelectedItem)).name;
                }
            }
            catch { }
        }

        private void RenameItem_Click(object sender, RoutedEventArgs e)
        {
            if(currentFileType == typeof(foodItemJap))
            {
                foodItemJap food = (foodItemJap)(DisplayView.SelectedItem);
                food.name = NewNameTextBox.Text;
                DisplayView.Items[DisplayView.SelectedIndex] = food;
                DisplayView.Items.Refresh();
            }
        }
    }
}
