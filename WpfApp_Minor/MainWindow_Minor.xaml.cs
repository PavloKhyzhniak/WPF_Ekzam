using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_Minor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public MainWindow()
        {
            InitializeComponent();

            timer_game.Tick += timer_game_Elapsed;

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            this.DataContext = this;
        }

        //Timer timer_game;
        System.Windows.Threading.DispatcherTimer timer_game = new System.Windows.Threading.DispatcherTimer();

        public enum Level
        {
            Easy,
            Normal,
            Hard
        }

        Brush brush = new SolidColorBrush(Colors.White);
        int[][] array;
        int[][] array_visited;
        int cnt_rows = 10;
        int cnt_colums = 10;
        int current_cnt_bomb = 0;
        private void button_NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            NewGame();
        }

        private void PrepareNewGame()
        {
            SetSizeWindows setSizeWindow = new SetSizeWindows();

            Level level = 0;

            if (setSizeWindow.ShowDialog() == true)
            {
                cnt_rows = SetSizeWindows.rows;
                cnt_colums = SetSizeWindows.colums;
                level = SetSizeWindows.level;
            }
            else
                return;

            current_cnt_bomb = cnt_rows * cnt_colums;
            switch (level)
            {
                case MainWindow.Level.Normal:
                    current_cnt_bomb = (int)(current_cnt_bomb * 0.20);
                    break;
                case MainWindow.Level.Hard:
                    current_cnt_bomb = (int)(current_cnt_bomb * 0.3);
                    break;
                case MainWindow.Level.Easy:
                default:
                    current_cnt_bomb = (int)(current_cnt_bomb * 0.1);
                    break;
            }

            array = new int[cnt_rows][];
            for (int i = 0; i < cnt_rows; i++)
            {
                array[i] = new int[cnt_colums];
                for (int j = 0; j < cnt_colums; j++)
                    array[i][j] = 0;//empty place
            }

            array_visited = new int[cnt_rows][];
            for (int i = 0; i < cnt_rows; i++)
            {
                array_visited[i] = new int[cnt_colums];
                for (int j = 0; j < cnt_colums; j++)
                    array_visited[i][j] = 0;//empty place
            }

            Random rand = new Random();
            int k = current_cnt_bomb;
            while (k > 0)
            {
                int x = rand.Next(cnt_rows);
                int y = rand.Next(cnt_colums);

                if (array[x][y] == 0)
                {
                    array[x][y] = 9;//bomb set
                    k--;
                }
            }

            uniformGrid_Place.Children.Clear();

            uniformGrid_Place.Rows = cnt_rows;
            uniformGrid_Place.Columns = cnt_colums;
                
            Brush brush = new SolidColorBrush(Colors.DarkGray);
            for (int i = 0; i < cnt_rows; i++)
                for (int j = 0; j < cnt_colums; j++)
                {
                    Button new_button = new Button()
                    {
                        Name = "button_" + i.ToString("0#") + "_" + j.ToString("0#"),
                        Tag = new Point(i, j),
                        Background = brush,
                        Margin = new Thickness(0),
                        Padding = new Thickness(0),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };

                    new_button.PreviewMouseLeftButtonDown += button_Down;
                    new_button.PreviewMouseRightButtonDown += button_Down;

                    uniformGrid_Place.Children.Add(new_button);
                }

            Binding bndWidth;
            Binding bndHeight;
            if (mainWindow_Minor.ActualWidth > mainWindow_Minor.ActualHeight)
                bndWidth = new Binding("Height") { ElementName = "borderGame" };
            //uniformGridGame.Width = mainWindowBarleyBreak.ActualHeight -100;
            else
                bndWidth = new Binding("Width") { ElementName = "borderGame" };
            //uniformGridGame.Width = mainWindowBarleyBreak.ActualWidth -100;

            //BindingOperations.ClearAllBindings(uniformGridGame);
            BindingOperations.ClearBinding(uniformGrid_Place, UniformGrid.WidthProperty);
            BindingOperations.SetBinding(uniformGrid_Place, UniformGrid.WidthProperty, bndWidth);

            bndHeight = new Binding("ActualWidth") { ElementName = "uniformGrid_Place" };
            BindingOperations.ClearBinding(uniformGrid_Place, UniformGrid.HeightProperty);
            BindingOperations.SetBinding(uniformGrid_Place, UniformGrid.HeightProperty, bndHeight);
            //uniformGridGame.Height = uniformGridGame.Width;

            uniformGrid_Place.UpdateLayout();
        }
        private void NewGame()
        {
            PrepareNewGame();

            flag_end_game = true;
            time = 0;

            timer_game.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer_game.Start();
       }

        int time = 0;
        private void timer_game_Elapsed(object sender, EventArgs e)
        {
            time++;

            int sec = time / 10;
            int min = sec / 60;
            int hour = min / 60;
            min %= 60;
            sec %= 60;

            string message = hour.ToString("0#") + " : " + min.ToString("0#") + " : " + sec.ToString("0#");
            textBlock_Time.Text = message;

            //            SetTime(message);
        }

        void SetTime(string message)
        {
            if (!textBlock_Time.CheckAccess())
            {
                textBlock_Time.Dispatcher.Invoke(new Action<string>(SetTime), message);
            }
            else
            {
                textBlock_Time.Text = message;
            }
        }

        private int CheckCountNoVisitedPoint()
        {
            int cnt = 0;
            for (int i = 0; i < cnt_rows; i++)
            {
                for (int j = 0; j < cnt_colums; j++)
                    if (array_visited[i][j] != 1)
                        cnt++;
            }
            return cnt;
        }
        bool flag_end_game = false;
        private void CheckPoint(Point point)
        {
            int x = (int)point.X;
            int y = (int)point.Y;

            if (array_visited[x][y] == 1)
                return;
            array_visited[x][y] = 1;

            if (current_cnt_bomb == CheckCountNoVisitedPoint())
            {
                mp.Pause();
                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\interlude.wav");

                timer_game.Stop();
              WpfApp_BarleyBreak.MessageBox.Show("Your are Winner!!!", "End game");
                flag_end_game = false;
                mp.Play();
            }

            int cnt_bomb = CountBombs(point);

            if (cnt_bomb == 0)
            {
                if (x + 1 < cnt_rows && array[x + 1][y] == 0)
                    CheckPoint(new Point(x + 1, y));
                if (y + 1 < cnt_colums && x + 1 < cnt_rows && array[x + 1][y + 1] == 0)
                    CheckPoint(new Point(x + 1, y + 1));
                if (y - 1 >= 0 && x + 1 < cnt_rows && array[x + 1][y - 1] == 0)
                    CheckPoint(new Point(x + 1, y - 1));


                if (x - 1 >= 0 && array[x - 1][y] == 0)
                    CheckPoint(new Point(x - 1, y));
                if (y + 1 < cnt_colums && x - 1 >= 0 && array[x - 1][y + 1] == 0)
                    CheckPoint(new Point(x - 1, y + 1));
                if (y - 1 >= 0 && x - 1 >= 0 && array[x - 1][y - 1] == 0)
                    CheckPoint(new Point(x - 1, y - 1));


                if (y + 1 < cnt_colums && array[x][y + 1] == 0)
                    CheckPoint(new Point(x, y + 1));
                if (y - 1 >= 0 && array[x][y - 1] == 0)
                    CheckPoint(new Point(x, y - 1));
            }

            string name = "button_" + x.ToString("0#") + "_" + y.ToString("0#");
            object obj = uniformGrid_Place.FindName(name);
            object obj_tmp = uniformGrid_Place.Children[x * cnt_colums + y];
            if (obj_tmp is Button button)
            {
                button.Background = brush;
                button.Content = cnt_bomb;
            }
        }

        private int CountBombs(Point point)
        {
            int x = (int)point.X;
            int y = (int)point.Y;
            int cnt_bomb = 0;


            if (x + 1 < cnt_rows && array[x + 1][y] == 9)
                cnt_bomb++;
            if (y + 1 < cnt_colums && x + 1 < cnt_rows && array[x + 1][y + 1] == 9)
                cnt_bomb++;
            if (y - 1 >= 0 && x + 1 < cnt_rows && array[x + 1][y - 1] == 9)
                cnt_bomb++;


            if (x - 1 >= 0 && array[x - 1][y] == 9)
                cnt_bomb++;
            if (y + 1 < cnt_colums && x - 1 >= 0 && array[x - 1][y + 1] == 9)
                cnt_bomb++;
            if (y - 1 >= 0 && x - 1 >= 0 && array[x - 1][y - 1] == 9)
                cnt_bomb++;


            if (y + 1 < cnt_colums && array[x][y + 1] == 9)
                cnt_bomb++;
            if (y - 1 >= 0 && array[x][y - 1] == 9)
                cnt_bomb++;

            return cnt_bomb;
        }

        private void button_Down(object sender, MouseEventArgs e)
        {
            if (flag_end_game)
                if (sender is Button button)
                {
                    if (button.Tag is Point point)
                    {
                        int x = (int)point.X;
                        int y = (int)point.Y;

                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            if (array[x][y] == 0)
                            {
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\win.wav");

                                CheckPoint(point);
                            }
                            else
                            {
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\explode.wav");

                                button.Background = new SolidColorBrush(Colors.IndianRed);
                                button.Content = "x";
                                timer_game.Stop();
                                WpfApp_BarleyBreak.MessageBox.Show("GAME OVER", "End game");
                                flag_end_game = false;
                            }
                        }
                        if (e.RightButton == MouseButtonState.Pressed)
                        {
                            Brush brushMarker = new SolidColorBrush(Colors.Red);
                            if (array_visited[x][y] == 0 && !EqualsSolidColorBrush(button.Background,brushMarker))
                            {
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\arrive.wav");

                                button.Background = brushMarker;
                            }
                            else
                            {
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\leave.wav");

                                if (array_visited[x][y] == 1)
                                    button.Background = brush;
                                else
                                    button.Background = new SolidColorBrush(Colors.DarkGray);
                            }
                        }
                    }
                }
        }
        public static bool EqualsSolidColorBrush(Brush brush1IN, Brush brush2IN)
        {
            if (brush1IN is SolidColorBrush brush1 && brush2IN is SolidColorBrush brush2)

                return brush1.Opacity == brush2.Opacity &&
                    brush1.Color.A == brush2.Color.A &&
                    brush1.Color.R == brush2.Color.R &&
                    brush1.Color.B == brush2.Color.B &&
                    brush1.Color.G == brush2.Color.G;
            else return false;
        }
        private void mainWindow_Minor_Loaded(object sender, RoutedEventArgs e)
        {
            if (MusicOnOff) MusicMP3(".\\..\\..\\Resources\\PurplePlanetMusic-FloatingInSpace(2_14)95bpm.mp3", true);

           WpfApp_BarleyBreak.MessageBox.Show(
"Игра \"Сапер\" (8 баллов)" +
"\n\t1. Общие требования к играм:" +
"\n - наличие игрового меню" +
"\n - наличие статусной строки" +
"\n - возможность сохр / загр игры" +
"\n - наличие привлекательного дизайна" +
"\n - наличие звуков в программе" +
"\n - наличие нескольких окон" +
"\n - таблица рекордов" +
"\n\t2. Разработать шаблон какого - либо элемента управления с использованием " +
"\nтриггеров и анимации.Нужно добавить в игру(2 балла)." +
"\n\t3. Разработать и использовать в игре стили элементов управления, помещённые" +
"\nв ресурсы(2 балла)."
, "Экзамен по WPF");
        }

        private void mainWindow_Minor_Unloaded(object sender, RoutedEventArgs e)
        {
            mp.Close();
        }

        private void checkBox_SoundOnOff_Click(object sender, RoutedEventArgs e)
        {
            if(sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked==true)
                    SoundOnOff = true;
                else
                    SoundOnOff = false;
            }
        }

        private void checkBox_MusicOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked == true)
                    MusicOnOff = true;
                else
                    MusicOnOff = false;

                if (MusicOnOff)
                    mp.Play();
                else
                    mp.Pause();
            }

        }

        private bool SoundOnOff = true;
        private bool MusicOnOff = true;
        MediaPlayer mp = new MediaPlayer();
        private void MusicMP3(string sound_filename, bool repeat = false)
        {
            var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            FileInfo info = new FileInfo(System.IO.Path.Combine(basePath, sound_filename));
            if (info.Exists)
            {

                mp.Open(new Uri(System.IO.Path.Combine(basePath, sound_filename), UriKind.Absolute));
                mp.Volume = 1;
                mp.Balance = 0;
                mp.Position = new TimeSpan(0, 0, 0);
                mp.SpeedRatio = 1;

                if (repeat)
                    mp.MediaEnded += MusicReplay;

                mp.MediaFailed += (o, args) =>
                {
                    WpfApp_BarleyBreak.MessageBox.Show("Media Failed!!!\n" + args.ErrorException.Message);
                };

                mp.Play();
            }

        }

        private void MusicReplay(object sender, EventArgs e)
        {
            mp.Stop();
            mp.Play();
        }

        private void SoundWAV(string sound_filename, bool repeat = false)
        {
            var basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = System.IO.Path.Combine(basePath, sound_filename);

            try
            {
                player.Load();

                if (repeat)
                    player.PlayLooping();
                else
                    player.Play();
            }
            catch (System.IO.FileNotFoundException err)
            {
            }
            catch (FormatException err)
            {
            }
        }
    }
}
