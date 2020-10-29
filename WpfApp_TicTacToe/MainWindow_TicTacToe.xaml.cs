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

namespace WpfApp_TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow_TicTacToe : Window
    {
        //Timer timer_game;
        System.Windows.Threading.DispatcherTimer timer_game = new System.Windows.Threading.DispatcherTimer();

        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public MainWindow_TicTacToe()
        {
            InitializeComponent();

            timer_game.Tick += timer_game_Elapsed;
            timer_game.Interval = new TimeSpan(0, 0, 0, 0, 100);

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            this.DataContext = this;
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

        int time;
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

        int[][] array;
        int step;

        int computer_win = 0;
        int user_win = 0;

        private void PrepareGame()
        {
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;

            //создадим и заполним(проинициализируем) массив
            array = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                    array[i][j] = 9;
            }

            //очистка игрового поля перед новой игрой
            uniformGridGame.Children.Clear();

            PrepareButton();

            Binding bndWidth;
            Binding bndHeight;
            if (mainWindowTicTacToe.ActualWidth > mainWindowTicTacToe.ActualHeight)
                bndWidth = new Binding("Height") { ElementName = "borderGame" };
            //uniformGridGame.Width = mainWindowBarleyBreak.ActualHeight -100;
            else
                bndWidth = new Binding("Width") { ElementName = "borderGame" };
            //uniformGridGame.Width = mainWindowBarleyBreak.ActualWidth -100;

            //BindingOperations.ClearAllBindings(uniformGridGame);
            BindingOperations.ClearBinding(uniformGridGame, UniformGrid.WidthProperty);
            BindingOperations.SetBinding(uniformGridGame, UniformGrid.WidthProperty, bndWidth);

            bndHeight = new Binding("ActualWidth") { ElementName = "uniformGridGame" };
            BindingOperations.ClearBinding(uniformGridGame, UniformGrid.HeightProperty);
            BindingOperations.SetBinding(uniformGridGame, UniformGrid.HeightProperty, bndHeight);
            //uniformGridGame.Height = uniformGridGame.Width;

            uniformGridGame.UpdateLayout();
            step = 0;
        }

        private void PrepareButton()
        {
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;
            int cnt = rows * columns;

            Button button;
            for (int i = 0; i < cnt ; i++)
            {
                button = CreateButton("");
                button.Tag = i;
                uniformGridGame.Children.Add(button);
            }
        }

        private Button CreateButton(string text)
        {
            Button button = new Button
            {                
                FontSize = 100,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Normal,
                Name = "button_" + text,
                Content = text
            };
            button.Click += new RoutedEventHandler(this.element_Click);

            button.Style = (Style)TryFindResource("PiecesAnimation");
            button.Background = (GradientBrush)TryFindResource("linearGradientBrush");

            return button;
        }

        //работа с каждым элементом на игровом поле
        private void element_Click(object sender, EventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\Win.wav");

            if (sender is Button button)
            {
                int index = (int)button.Tag;

                if (array[index / 3][index % 3] == 9)
                    array[index / 3][index % 3] = 1;
                else
                    return;
                                
                button.Content = "X";
                step++;

                int cnt = 0;
                switch (step)
                {
                    case 1:
                        if (index != 4)
                            cnt = 4;
                        else
                            cnt = 0;
                        break;
                    case 3:
                    case 5:
                    case 7:
                        cnt = CheckNextStep(0);//для выигрыша
                        if (cnt == 99)
                            cnt = CheckNextStep(1);//для защиты
                        if (cnt == 99)
                            cnt = CheckNextStepForWin(0);//для нападения
                        if (cnt == 99)//УПРОЩЕНИЕ
                        {
                            if (array[0][0] == 9)
                                cnt = 0;
                            else if (array[0][2] == 9)
                                cnt = 2;
                            else if (array[2][0] == 9)
                                cnt = 6;
                            else if (array[2][2] == 9)
                                cnt = 8;
                            else
                                for (int i = 0; i < 3; i++)
                                    for (int j = 0; j < 3; j++)
                                        if (array[i][j] == 9)
                                            cnt = i * 3 + j;
                        }
                        break;
                    case 9:
                        break;
                }

                if (step != 9)
                {
                    (uniformGridGame.Children[cnt] as Button).Content = "0";
                    array[cnt / 3][cnt % 3] = 0;
                    step++;
                }

                int winner = CheckWinner();
                if (winner == 1)
                {
                    WpfApp_BarleyBreak.MessageBox.Show("User is Win!!!", "End of Game");
                    user_win++;

                    textBlock_Player.Text = user_win.ToString();
                }
                if (winner == 0)
                {
                    WpfApp_BarleyBreak.MessageBox.Show("Computer is Win!!!", "End of Game");
                    computer_win++;

                    textBlock_Computer.Text = computer_win.ToString();
                }

                if (winner == 99 && step == 9)
                    WpfApp_BarleyBreak.MessageBox.Show("No Winner!!!", "End of Game");

                if (winner < 99 || step == 9)
                {
                    PrepareGame();
                    timer_game.Stop();
                    time = 0;
                    labelGameEnd.Visibility = Visibility.Visible;

                }
            }
        }

        int CheckNextStep(int player = 0)
        {
            int answer = 99;

            int i = 0;
            int j = 0;
            //проверка не допустить проигрыша
            for (i = 0; i < 3; i++)
            {
                if (array[i][0] == player && array[i][1] == player && array[i][2] == 9) return 2 + (i * 3);
                if (array[i][1] == player && array[i][2] == player && array[i][0] == 9) return 0 + (i * 3);
                if (array[i][0] == player && array[i][2] == player && array[i][1] == 9) return 1 + (i * 3);
            }
            for (j = 0; j < 3; j++)
            {
                if (array[0][j] == player && array[1][j] == player && array[2][j] == 9) return j + (2 * 3);
                if (array[1][j] == player && array[2][j] == player && array[0][j] == 9) return j + (0 * 3);
                if (array[0][j] == player && array[2][j] == player && array[1][j] == 9) return j + (1 * 3);
            }

            if (array[0][0] == player && array[1][1] == player && array[2][2] == 9) return 8;
            if (array[0][0] == player && array[2][2] == player && array[1][1] == 9) return 4;
            if (array[1][1] == player && array[2][2] == player && array[0][0] == 9) return 0;

            if (array[0][2] == player && array[1][1] == player && array[2][0] == 9) return 6;
            if (array[1][1] == player && array[2][0] == player && array[0][2] == 9) return 2;
            if (array[0][2] == player && array[2][0] == player && array[1][1] == 9) return 4;
            //проверка делаем ход
            //!!!!!!!!! УПРОСТИМ
            return answer;
        }

        int CheckNextStepForWin(int player = 0)
        {
            int answer = 99;

            int i = 0;
            int j = 0;
            //проверка не допустить проигрыша
            for (i = 0; i < 3; i++)
            {
                if (array[i][0] == 9 && array[i][1] == 9 && array[i][2] == player) return 0 + (i * 3);
                if (array[i][1] == 9 && array[i][2] == 9 && array[i][0] == player) return 2 + (i * 3);
                //		if (array[i][0] == 9 && array[i][2] == 9 && array[i][1] == player) return 1 + (i * 3);
            }
            for (j = 0; j < 3; j++)
            {
                if (array[0][j] == 9 && array[1][j] == 9 && array[2][j] == player) return j + (0 * 3);
                if (array[1][j] == 9 && array[2][j] == 9 && array[0][j] == player) return j + (2 * 3);
                //		if (array[0][j] == 9 && array[2][j] == 9 && array[1][j] == player) return j + (1 * 3);
            }

            if (array[0][0] == 9 && array[1][1] == 9 && array[2][2] == player) return 0;
            //	if (array[0][0] == 9 && array[2][2] == 9 && array[1][1] == player) return 4;
            if (array[1][1] == 9 && array[2][2] == 9 && array[0][0] == player) return 8;

            if (array[0][2] == 9 && array[1][1] == 9 && array[2][0] == player) return 2;
            if (array[1][1] == 9 && array[2][0] == 9 && array[0][2] == player) return 6;
            //	if (array[0][2] == 9 && array[2][0] == 9 && array[1][1] == player) return 4;
            //проверка делаем ход
            //!!!!!!!!! УПРОСТИМ
            return answer;
        }

        int CheckWinner()
        {
            int answer = 99;

            int i = 0;
            int j = 0;
            int k = 0;
            for (k = 1; k >= 0; k--)
            {
                //проверка не допустить проигрыша
                for (i = 0; i < 3; i++)
                {
                    if (array[i][0] == k && array[i][1] == k && array[i][2] == k) return k;
                    if (array[i][1] == k && array[i][2] == k && array[i][0] == k) return k;
                    if (array[i][0] == k && array[i][2] == k && array[i][1] == k) return k;
                }
                for (j = 0; j < 3; j++)
                {
                    if (array[0][j] == k && array[1][j] == k && array[2][j] == k) return k;
                    if (array[1][j] == k && array[2][j] == k && array[0][j] == k) return k;
                    if (array[0][j] == k && array[2][j] == k && array[1][j] == k) return k;
                }

                if (array[0][0] == k && array[1][1] == k && array[2][2] == k) return k;
                if (array[0][0] == k && array[2][2] == k && array[1][1] == k) return k;
                if (array[1][1] == k && array[2][2] == k && array[0][0] == k) return k;

                if (array[0][2] == k && array[1][1] == k && array[2][0] == k) return k;
                if (array[1][1] == k && array[2][0] == k && array[0][2] == k) return k;
                if (array[0][2] == k && array[2][0] == k && array[1][1] == k) return k;
            }

            return answer;
        }

        private void menuItem_NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            PrepareGame();

            timer_game.Start();
            time = 0;
            labelGameEnd.Visibility = Visibility.Hidden;
        }

        private void menuItem_SoundOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            SoundOnOff = !SoundOnOff;
        }

        private void menuItem_MusicOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            MusicOnOff = !MusicOnOff;
            if (MusicOnOff) 
                mp.Play();
            else 
                mp.Pause();
        }

        private void menuItem_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            if (timer_game.IsEnabled)
            {
                timer_game.IsEnabled = false;
                timer_game.Stop();
                labelPause.Visibility = Visibility.Visible;
            }
            else
            {
                timer_game.IsEnabled = true;
                timer_game.Start();
                labelPause.Visibility = Visibility.Hidden;
            }
        }

        private void menuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            PageModel_BaseFunction.ClosePage(this);
        }

        private void mainWindowTicTacToe_Loaded(object sender, RoutedEventArgs e)
        {
            if (MusicOnOff) MusicMP3(".\\..\\..\\Resources\\PurplePlanetMusic-FloatingInSpace(2_14)95bpm.mp3", true);

           WpfApp_BarleyBreak.MessageBox.Show(
"Игра \"Крестики-Нолики\" (5 баллов)" +
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

        private void mainWindowTicTacToe_Unloaded(object sender, RoutedEventArgs e)
        {
            mp.Close();
        }
    }
}
