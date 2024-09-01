using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;

namespace SP_02_Hometask
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int maxValue;
        private int _value = 0;

        public bool IsBtn1Clicked { get; set; }  //from

        public bool IsBtn2Clicked { get; set; }  //to

        public string firstText { get; set; }
        public string secondText { get; set; }

        public int MaxValue { get => maxValue; set { maxValue = value; OnPropertyChanged(); } }

        public int ProgressValue { get => _value; set { _value = value; OnPropertyChanged(); } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            IsBtn1Clicked = false;
            IsBtn2Clicked = false;
            ProgressValue = 0;
            MaxValue = 1;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Copy_btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (IsBtn1Clicked is not true)
                firstText = txt1.Text;

            if (IsBtn2Clicked is not true)
                secondText = txt2.Text;

            if (File.Exists(firstText))
            {

                if (!File.Exists(secondText))
                    File.Create(secondText).Close();

                byte[] byteText = File.ReadAllBytes(firstText);
                int len = byteText.Length;
                MaxValue = len;

                new Thread(() =>
                {
                    using (FileStream fs = new FileStream(secondText, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        foreach (var item in byteText)
                        {
                            fs.WriteByte(item);
                            ProgressValue++;
                            Thread.Sleep(30);
                        }

                    }

                }).Start();

            }

            else
                MessageBox.Show("File to read data does NOT exist!!!");


        }

        private void btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            openFileDialog.InitialDirectory = @"C:\";

            Button? btn = sender as Button;
            string btnName = string.Empty;

            if (btn is not null)
            {
                btnName = btn.Name;

                if (btnName == "btn1")
                {
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filePath = openFileDialog.FileName;
                        firstText = filePath;
                        txt1.Text = firstText;
                        IsBtn1Clicked = true;
                    }
                }

                else if (btnName == "btn2")
                {
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filePath = openFileDialog.FileName;
                        secondText = filePath;
                        txt2.Text = secondText;
                        IsBtn2Clicked = true;
                    }
                }
            }

        }
    }
}