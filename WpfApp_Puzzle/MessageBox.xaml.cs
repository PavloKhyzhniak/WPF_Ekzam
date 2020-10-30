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
    /// Логика взаимодействия для MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public MessageBox()
        {
            InitializeComponent();

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            this.DataContext = this;
        }

        void AddButtons(MessageBoxButton buttons)
        {
            if (buttons == null)
                return;
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton("OK", MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton("OK", MessageBoxResult.OK);
                    AddButton("Cancel", MessageBoxResult.Cancel, isCancel: true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Yes", MessageBoxResult.Yes);
                    AddButton("No", MessageBoxResult.No);
                    AddButton("Cancel", MessageBoxResult.Cancel, isCancel: true);
                    break;
                default:
                    throw new ArgumentException("Unknown button value", "buttons");
            }
        }

        void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            var button = new Button() { Content = text, IsCancel = isCancel };
            button.Click += (o, args) => { Result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
        }

        MessageBoxResult Result = MessageBoxResult.None;

        public static MessageBoxResult Show(string message, string caption = "Message",
                                            MessageBoxButton buttons = MessageBoxButton.OK)
        {
            var dialog = new MessageBox() { Title = caption };
            dialog.MessageContainer.Text = message;
            dialog.AddButtons(buttons);
            dialog.ShowDialog();
            return dialog.Result;
        }
    }
}
