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

namespace WpfApp_Puzzle
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class HighScoreTable : Window
    {
        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public HighScoreTable()
        {
            InitializeComponent();

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            this.DataContext = this;
        }


        public HighScoreTable(List<string> list)
            : this()
        {
            listView.ItemsSource = list;
        }

    }
}
