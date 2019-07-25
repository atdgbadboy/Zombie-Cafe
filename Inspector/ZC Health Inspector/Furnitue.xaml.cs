using System;
using System.Collections.Generic;
using System.IO;
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

namespace ZC_Health_Inspector
{
    /// <summary>
    /// Interaction logic for Furnitue.xaml
    /// </summary>
    public partial class Furnitue : Window
    {
        enum types
        {
            deco = 0,
            stove = 1,
            table = 3,
            chair = 4,
            wall = 5,
            
        }


        struct furniture{
            public byte idk1 { get; set; }
            public byte idk29 { get; set; }
            public byte idk2 { get; set; }
            public byte idk30 { get; set; }
            public byte level { get; set; }
            public byte idk28 { get; set; }
            public string name { get; set; }
            public int price { get; set; }
            public bool usesToxin { get; set; }
            public byte idk27 { get; set; }
            public byte idk6 { get; set; }
            public byte idk31 { get; set; }
            public byte idk7 { get; set; }
            
            public short idk8 { get; set; }
           
            public short idk9 { get; set; }
            public short idk10 { get; set; }
            public byte type { get; set; }
            public byte idk33 { get; set; }
            public byte idk32 { get; set; }
            public short idk12 { get; set; }
            public short idk13 { get; set; }
            public short idk14 { get; set; }
            public short idk15 { get; set; }
            public short idk16 { get; set; }
            public short idk17 { get; set; }
            public short idk18 { get; set; }
            public short idk19 { get; set; }
            
            public byte idk20 { get; set; }
            public byte idk34 { get; set; }
            public short idk21 { get; set; }
           
            public byte idk22 { get; set; }
            public string description { get; set; }
            public short idk24 { get; set; }
            public byte idk25 { get; set; }
            public short idk23 { get; set; }
        }

        public Furnitue()
        {
            InitializeComponent();

            byte[] file = File.ReadAllBytes(@"C:\Users\Dylan\Desktop\zce\out\assets\data\furnitureData.bin.mid");
            BinaryReader br = new BinaryReader(file);
            List<furniture> items = br.ReadStructArray<furniture>();

            foreach(furniture item in items)
            {
                DisplayView.Items.Add(item);
            }
        }
    }
}
