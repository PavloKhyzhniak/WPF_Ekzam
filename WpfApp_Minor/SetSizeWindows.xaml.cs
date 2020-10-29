using Prism.Commands;
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

namespace WpfApp_Minor
{
    /// <summary>
    /// Логика взаимодействия для SetSizeWindows.xaml
    /// </summary>
    public partial class SetSizeWindows : Window
    {
        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public SetSizeWindows()
        {
            InitializeComponent();

            textBox_Colums.Text = colums.ToString();
            textBox_Rows.Text = rows.ToString();
            switch (level)
            {
                case MainWindow.Level.Easy:
                    radioButton_Easy.IsChecked = true;
                    break;
                case MainWindow.Level.Normal:
                    radioButton_Normal.IsChecked = true;
                    break;
                case MainWindow.Level.Hard:
                    radioButton_Hard.IsChecked = true;
                    break;
            }

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            this.DataContext = this;
        }
        public static int rows { get; set; }
        public static int colums { get; set; }
        public static MainWindow.Level level { get; set; } = MainWindow.Level.Easy;

        int min_cnt_rows = 10;
        int min_cnt_colums = 10;
        int max_cnt_rows = 100;
        int max_cnt_colums = 100;

        private void textBox_Colums_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(((TextBox)sender).Text, out int tmp_int);
            if (tmp_int != null && tmp_int > 0)
            {
                if (tmp_int >= max_cnt_colums)
                    colums = max_cnt_colums;
                else if (tmp_int <= min_cnt_colums)
                    colums = min_cnt_colums;
                else
                    colums = tmp_int;
                ((TextBox)sender).Text = colums.ToString();
            }
            else
                ((TextBox)sender).Text = min_cnt_colums.ToString();
        }

        private void textBox_Rows_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(((TextBox)sender).Text, out int tmp_int);
            if (tmp_int != null && tmp_int > 0)
            {
                if (tmp_int >= max_cnt_rows)
                    rows = max_cnt_rows;
                else if (tmp_int <= min_cnt_rows)
                    rows = min_cnt_rows;
                else
                    rows = tmp_int;
                ((TextBox)sender).Text = rows.ToString();
            }
            else
                ((TextBox)sender).Text = min_cnt_rows.ToString();
        }

        private void radioButton_Click(object sender, EventArgs e)
        {
            if (sender is RadioButton button)
                switch (button.Content)
                {
                    case "Easy":
                        level = MainWindow.Level.Easy;
                        break;
                    case "Normal":
                        level = MainWindow.Level.Normal;
                        break;
                    case "Hard":
                        level = MainWindow.Level.Hard;
                        break;
                }
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
