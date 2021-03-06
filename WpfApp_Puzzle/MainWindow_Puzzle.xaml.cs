﻿using DP_Memento;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
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

namespace WpfApp_Puzzle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow_Puzzle : Window
    {
        //Timer timer_game;
        System.Windows.Threading.DispatcherTimer timer_game = new System.Windows.Threading.DispatcherTimer();

        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public DelegateCommand<object> NewGameCommand { get; }
        public DelegateCommand<object> LoadGameCommand { get; }
        public DelegateCommand<object> SoundOnOffCommand { get; }
        public DelegateCommand<object> MusicOnOffCommand { get; }
        public DelegateCommand<object> HighScoreShowCommand { get; }

        public DelegateCommand<object> ExitCommand { get; }

        public MainWindow_Puzzle()
        {
            InitializeComponent();

            timer_game.Tick += timer_game_Elapsed;
            timer_game.Interval = new TimeSpan(0, 0, 0, 0, 100);

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            NewGameCommand = new DelegateCommand<object>(obj => { if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); NewGame(); });
            LoadGameCommand = new DelegateCommand<object>(obj => { if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); LoadGame(); });
            SoundOnOffCommand = new DelegateCommand<object>(obj => { if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); SoundOnOff = !SoundOnOff; });
            MusicOnOffCommand = new DelegateCommand<object>(obj => { if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); MusicOnOff = !MusicOnOff; if (MusicOnOff) mp.Play(); else mp.Pause(); });
            HighScoreShowCommand = new DelegateCommand<object>(obj => { if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); HighScoreShow(); });

            ExitCommand = new DelegateCommand<object>(obj => { if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); PageModel_BaseFunction.ClosePage(obj); });

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
                    MessageBox.Show("Media Failed!!!\n" + args.ErrorException.Message);
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

            if (flag_checkPuzzle)
            {
                flag_checkPuzzle = false;
                CheckPuzzle();
            }
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

        private void NewGame()
        {
            uniformGridGame.Children.Clear();
            uniformGridGamePieces.Children.Clear();

            OpenPicture();
            time = 0;

            flag_borderShow = false;
            BorderGridShow();

            //запустим безопасное перемешивание - перемешивание элементов в обратном порядке 
            ShufflePuzzle();

            timer_game.Start();
            labelGameEnd.Visibility = Visibility.Hidden;
        }

        bool flag_borderShow = false;
        private void BorderGridShow()
        {
            flag_borderShow = true;
            int rows = base_rows;// uniformGridGame.Rows;
            int columns = base_columns;// uniformGridGame.Columns;

         //   uniformGridGame.ShowGridLines = true;

            StringBuilder str = new StringBuilder(1000);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    str.Append(" " + list_pathstring[i * columns + j].GetPathString());
                }
            ScaleTransform scale = new ScaleTransform(ScaleX,ScaleY);
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1,
                //  RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = scale,
                        Data = Geometry.Parse(str.ToString()),
                        Fill= new ImageBrush(bSource) { Stretch = Stretch.Fill, Opacity = 0.2 },
                        Width = uniformGridGame.ActualWidth,
                        Height = uniformGridGame.ActualHeight,                        
            };

            //Grid.SetColumnSpan(path, base_columns);
            //Grid.SetRowSpan(path, base_rows);

            //Border border = new Border()
            //        {
            //            Background = Brushes.Transparent,
            //            BorderBrush=Brushes.Green,
            //            BorderThickness = new Thickness(1),
            //            Child = path,
            //            Width = uniformGridGame.ActualWidth,
            //            Height = uniformGridGame.ActualHeight
            //        };

        //            Binding scaleX;
        //            Binding scaleY;
        //
        //            scaleX = new Binding("ScaleX") { Source = this, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
        //            BindingOperations.SetBinding(scale, ScaleTransform.ScaleXProperty, scaleX);
        //
        //            scaleY = new Binding("ScaleY") { Source = this, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
        //            BindingOperations.SetBinding(scale, ScaleTransform.ScaleYProperty, scaleY);

                    uniformGridGame.Children.Add(path);
            //                    Grid.SetZIndex(border, 1000);
            //    Grid.SetColumn(border, j);
            //    Grid.SetRow(border, i);

            uniformGridGame.UpdateLayout();
            path.Width = path.ActualWidth;
            path.Height = path.ActualHeight;
            path.UpdateLayout();
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            NewGame();
        }

        private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            //запустим перемешивание
            ShufflePuzzle();
            time = 0;//обнулим таймер

            timer_game.Start();
            labelGameEnd.Visibility = Visibility.Hidden;
        }             
               
        private void OpenPicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Фильтры файлов в диалоге
            openFileDialog.Filter = "Image|*.jpg;*.bmp;*.png;*.ico;*.gif" + "|All Files|*.*";

            // Номер выбранного по умолчанию фильтра
            openFileDialog.FilterIndex = 1;

            // Проверка существования выбранного файла
            openFileDialog.CheckFileExists = true;

            // Разрешить выбор нескольких файлов
            openFileDialog.Multiselect = false;

            // Открытие диалога
            if (openFileDialog.ShowDialog() == true)
            {
                ImageFilename = openFileDialog.FileName;

                // Короткое имя выбранного файла
                MessageBox.Show(openFileDialog.SafeFileName, "Файл открыт");

                PrepareUniformGrid();
                //NewGame();
            }
        }

        string ImageFilename = "\\Resources\\yellow_breakfast.jpg";
        int time;

        List<string> highscoreList { get; set; }
               
        class playerScore
        {
            public int Pos { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
        }

        public string PlayerName { get; set; }

        private void HighScoreShow(playerScore newPlayer = null)
        {
            highscoreList = new List<string>();

            string scorefilename = ".\\highscore.txt";
            FileInfo info = new FileInfo(scorefilename);
            FileStream filestream = null;
            if (!info.Exists)
            {
                filestream = File.Create(scorefilename);
            }
            else
                filestream = File.Open(scorefilename, FileMode.Open);
            info = null;

            List<playerScore> players = new List<playerScore>();

            //Read High Score Table From File
            using (StreamReader reader = new StreamReader(filestream))
            {
                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    string[] str = currentLine.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    playerScore player = new playerScore();

                    if (str.Length > 0)
                        if (int.TryParse(str[0], out int pos))
                            player.Pos = pos;
                    if (str.Length > 1)
                        player.Name = str[1].Length > 20 ? str[1].Substring(0, 20) : str[1];
                    if (str.Length > 2)
                    {
                        if (int.TryParse(str[2], out int score))
                            player.Score = score;
                    }
                    else
                        player.Score = 99999;

                    players.Add(player);
                }
            }

            filestream.Close();

            //Add New Player in High Score Table
            if (newPlayer != null)
                players.Add(newPlayer);

            //Sorted Score By Ascending
            var playersSorted = players.OrderBy(p => p.Score);

            //Create High Score Table List
            int i = 0;
            foreach (var item in playersSorted)
            {
                highscoreList.Add((i + 1).ToString("0#") + ". " + String.Format("{0,-20}", item.Name) + " " + String.Format("{0,5}", item.Score));
                i++;
                if (i > 10)
                    break;
            }
            for (i = (int)playersSorted.LongCount(); i < 10; i++)
            {
                highscoreList.Add((i + 1).ToString("0#") + ". " + String.Format("{0,-20}", "noname") + " " + String.Format("{0,5}", " "));
            }

            //Save New High Score Table
            using (StreamWriter writer = new StreamWriter(scorefilename))
            {
                foreach (var item in highscoreList)
                    writer.WriteLine(item);
            }

            //Show High Score Table
            HighScoreTable newHighScoreTable = new HighScoreTable(highscoreList);
            newHighScoreTable.ShowDialog();
        }

        private void WorkWithHighScoreTable()
        {
            //Add New Player in High Score Table
            playerScore newPlayer = new playerScore()
            {
                Score = time,
                Name = PlayerName == null ? "noname" : PlayerName
            };

            HighScoreShow(newPlayer);

            labelGameEnd.Visibility = Visibility.Visible;
        }
                
        Random rand = new Random();
        private void ShufflePuzzle()
        {
            //            uniformGridGamePieces.Children.Clear();
            while (uniformGridGamePieces.Children.Count > 0)
            {
                var element = uniformGridGamePieces.Children[0];

                uniformGridGamePieces.Children.Remove(element);
                uniformGridGame.Children.Add(element);
            }

            int cnt = uniformGridGame.Children.Count;

            //    uniformGridGamePieces.Rows = (int)Math.Sqrt(cnt) + 1;
            //    uniformGridGamePieces.Columns = uniformGridGamePieces.Rows;

            int tmp_cnt = 0;
            if (flag_borderShow)
                tmp_cnt = 1;

            while ((cnt = uniformGridGame.Children.Count) > tmp_cnt)
            {
                var element = uniformGridGame.Children[rand.Next(cnt)];

                if (element is System.Windows.Shapes.Path)
                    continue;

                uniformGridGame.Children.Remove(element);
                uniformGridGamePieces.Children.Add(element);

                Canvas.SetLeft(element, rand.Next(50, (int)uniformGridGamePieces.ActualWidth-50));
                Canvas.SetTop(element, rand.Next(50, (int)uniformGridGamePieces.ActualHeight-50));
            }
        }
                

        private void PrepareUniformGrid()
        {                          
            //очистка игрового поля перед новой игрой
            uniformGridGame.Children.Clear();

            PrepareCanvasImagePuzzle();

            Binding bndWidth;
            Binding bndHeight;
            if (mainWindowPuzzle.ActualWidth > mainWindowPuzzle.ActualHeight)
                bndWidth = new Binding("Height") { ElementName = "borderGame" };
            //uniformGridGame.Width = mainWindowBarleyBreak.ActualHeight -100;
            else
                bndWidth = new Binding("Width") { ElementName = "borderGame" };
            //uniformGridGame.Width = mainWindowBarleyBreak.ActualWidth -100;

            //BindingOperations.ClearAllBindings(uniformGridGame);
            BindingOperations.ClearBinding(uniformGridGame, Canvas.WidthProperty);
            BindingOperations.SetBinding(uniformGridGame, Canvas.WidthProperty, bndWidth);

            bndHeight = new Binding("ActualWidth") { ElementName = "uniformGridGame" };
            BindingOperations.ClearBinding(uniformGridGame, Canvas.HeightProperty);
            BindingOperations.SetBinding(uniformGridGame, Canvas.HeightProperty, bndHeight);
            //uniformGridGame.Height = uniformGridGame.Width;

            uniformGridGame.UpdateLayout();
         }
               
        private void mainWindowPuzzle_Loaded(object sender, RoutedEventArgs e)
        {           
            MusicMP3(".\\..\\..\\Resources\\PurplePlanetMusic-Awakening(1_50)120bpm(L).mp3", true);
            if (MusicOnOff)
                mp.Play();
            else
                mp.Stop();

            MessageBox.Show(
"Игра \"Паззлы\" (7 баллов)" +
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
        
        class PathString
        {
            public string Top { get; set; }
            public string Bottom { get; set; }
            public string Left { get; set; }
            public string Right { get; set; }
            public Point Center { get; set; }

            public double Width { get; set; }
            public double Height { get; set; }

            public string GetPathString()
            {
                return Top + Right + Bottom + Left + " Z";
            }
        }


        public int base_rows { get; set; } = 5;
        public int base_columns { get; set; } = 5;

        BitmapSource bSource;
        List<PathString> list_pathstring;
        private void PrepareCanvasImagePuzzle()
        {
            int rows = base_rows;// uniformGridGame.Rows;
            int columns = base_columns;// uniformGridGame.Columns;
            int cnt = rows * columns;

            int width = (int)uniformGridGame.ActualWidth;
            int height = (int)uniformGridGame.ActualHeight;
            
            //исходная картинка
            bSource = new BitmapImage(new Uri(ImageFilename))
            {
                SourceRect = new Int32Rect(0, 0, width, height)               
            };
                       
            RecalculatedScale();

            uniformGridGame.Background = new ImageBrush(bSource) { Stretch = Stretch.Fill, Opacity = 0.2 };

            //            int width_delta = (int)bSource.PixelWidth / rows;
            //            int height_delta = (int)bSource.PixelHeight / columns;
            int width_delta = (int)bSource.Width / columns;
            int height_delta = (int)bSource.Height / rows;

            //разобъем картинку на 16 частей

            /*
             *  A-------B-------*-------*-------*-----
             *  |       |       |       |       |               
             *  |       |       |       |       |
             *  |       |       |       |       |
             *  D-------C-------*-------*-------*-----
             *  |       |       |       |       |               
             *  |       |       |       |       |
             *  |       |       |       |       |
             *  *-------*-------*-------*-------*-----
             *  |       |       |       |       |               
             *  |       |       |       |       |
             *  |       |       |       |       |
             * 
            */

            list_pathstring = new List<PathString>();

            Point A;
            Point B;
            Point C;
            Point D;

            Random rand = new Random();

            for (int i = 0; i < cnt; i++)
            {
                //сделаем все элементы

                A = new Point(i % columns * width_delta, i / columns * height_delta);
                B = new Point(i % columns * width_delta + width_delta, i / columns * height_delta);
                C = new Point(i % columns * width_delta + width_delta, i / columns * height_delta + height_delta);
                D = new Point(i % columns * width_delta, i / columns * height_delta + height_delta);

                int cnt_point = 8;
                int deltaX = width_delta / cnt_point;
                int deltaY = height_delta / cnt_point;

                //////////////////////////////////////////////////
                ///
                //////////////////////////////////////////////////

                PathString new_path = null;

                //first left top piece
                if (i == 0) //(i / columns == 0 && i % columns == 0)
                    new_path = new PathString()
                    {
                        Top = CreatePathMoveX(rand, A, B, cnt_point, 0, 0, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, -deltaX, deltaX),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, -deltaY, deltaY),
                        Left = CreatePathMoveY(rand, D, A, cnt_point, 0, 0)
                    };

                //last right top piece
                else if (i == columns-1 ) //(i / columns == 0 && i % columns == 0)
                    new_path = new PathString()
                    {
                        Top = CreatePathMoveX(rand, A, B, cnt_point, 0, 0, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, 0, 0),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, -deltaY, deltaY),
                        Left = RevertPathString(list_pathstring[i - 1].Right)
                    };

                //top piece
                else if (i / columns == 0 && i % (columns - 1) != 0)
                    new_path = new PathString()
                    {
                        Top = CreatePathMoveX(rand, A, B, cnt_point, 0, 0, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, -deltaX, deltaX),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, -deltaY, deltaY),
                        Left = RevertPathString(list_pathstring[i - 1].Right)
                    };


                //first bottom piece
                else if (i / columns == rows - 1 && i % columns == 0)
                    new_path = new PathString()
                    {
                        Top = RevertPathString(list_pathstring[i - columns].Bottom, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, -deltaX, deltaX),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, 0, 0),
                        Left = CreatePathMoveY(rand, D, A, cnt_point, 0, 0)
                    };

                //last bottom piece
                else if (i / columns == rows -1 && i % columns == columns - 1)
                    new_path = new PathString()
                    {
                        Top = RevertPathString(list_pathstring[i - columns].Bottom, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, 0, 0),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, 0, 0),
                        Left = RevertPathString(list_pathstring[i - 1].Right)
                    };

                //bottom piece
                else if (i / columns == rows -1 && i % columns != 0)
                    new_path = new PathString()
                    {
                        Top = RevertPathString(list_pathstring[i - columns].Bottom, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, -deltaX, deltaX),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, 0, 0),
                        Left = RevertPathString(list_pathstring[i - 1].Right)
                    };

                //first middle piece
                else if (i / columns != 0 && i % columns == 0)
                    new_path = new PathString()
                    {
                        Top = RevertPathString(list_pathstring[i - columns].Bottom, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, -deltaX, deltaX),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, -deltaY, deltaY),
                        Left = CreatePathMoveY(rand, D, A, cnt_point, 0, 0)
                    };

                //last middle piece
                else if (i / columns != 0 && i % columns == columns - 1)
                    new_path = new PathString()
                    {
                        Top = RevertPathString(list_pathstring[i - columns].Bottom, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, 0, 0),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, -deltaY, deltaY),
                        Left = RevertPathString(list_pathstring[i - 1].Right)
                    };

                //middle piece
                else if (i / columns != 0 && i % columns != 0)
                    new_path = new PathString()
                    {
                        Top = RevertPathString(list_pathstring[i - columns].Bottom, true),
                        Right = CreatePathMoveY(rand, B, C, cnt_point, -deltaX, deltaX),
                        Bottom = CreatePathMoveX(rand, C, D, cnt_point, -deltaY, deltaY),
                        Left = RevertPathString(list_pathstring[i - 1].Right)
                    };


                new_path.Center=new Point(i % columns * width_delta + width_delta/2, i / columns * height_delta + height_delta/2);
                new_path.Width = width_delta;
                new_path.Height = height_delta;

                list_pathstring.Add(new_path);

                //////////////////////////////////////////////////
                ///
                //////////////////////////////////////////////////

                //разместим фигуру на холсте, его и будем анимировать
                Image current_image = CreateImageClipToPathPuzzle(new_path, bSource);
                //current_image.RenderTransformOrigin = new Point(0.5, 0.5);
                current_image.Tag = i + 1;//укажем номер в стандартном расположении
             
                current_image.PreviewMouseDown += element_PreviewMouseDown;
                uniformGridGame.Children.Add(current_image);
                Canvas.SetLeft(current_image, 0);
                Canvas.SetTop(current_image, 0);
            }

        }

        public double ScaleX { get; set; } = 2;
        public double ScaleY { get; set; } = 2;

        private string CreatePathMoveX(Random rand,Point Start, Point End, int cnt_point, int deltaYmin, int deltaYmax, bool flag_start = false)
        {
            string currentPath;

            int deltaX = (int)(End.X - Start.X) / cnt_point;

            if (flag_start)
                currentPath = " M ";
            else
                currentPath = " L ";

            currentPath += (int)Start.X + "," + (int)Start.Y;

            for (int k = 1; k < cnt_point; k++)
                currentPath += " L " + (int)(Start.X + k * deltaX) + "," + (int)(Start.Y + rand.Next(deltaYmin, deltaYmax));
            currentPath += " L " + (int)End.X + "," + (int)End.Y;

            return currentPath;
        }

        private string CreatePathMoveY(Random rand, Point Start, Point End, int cnt_point, int deltaXmin, int deltaXmax,bool flag_start = false)
        {
            string currentPath;

            int deltaY = (int)(End.Y - Start.Y) / cnt_point;
            
            if (flag_start)
                currentPath = " M ";
            else 
                currentPath = " L ";
    
            currentPath += (int)Start.X + "," + (int)Start.Y;
            
            for (int k = 1; k < cnt_point; k++)
                currentPath += " L " + (int)(Start.X + rand.Next(deltaXmin, deltaXmax)) + "," + (int)(Start.Y + k * deltaY);
            currentPath += " L " + (int)End.X + "," + (int)End.Y;

            return currentPath;
        }

        private string RevertPathString(string path, bool flag_start = false)
        {
            StringBuilder new_str = new StringBuilder(1000);
            string[] str = path.Split(new string[] { "L", "M","Z" },StringSplitOptions.RemoveEmptyEntries);

            if (flag_start)
                new_str.Append(" M ");
            else
                new_str.Append(" L ");

            new_str.Append(str[str.Length - 1]);
            for (int i = str.Length - 2; i > 0; i--)
                new_str.Append(" L " + str[i]);

            return new_str.ToString();
        }
        
        private System.Windows.Shapes.Path CreateFigureFromPathPuzzle(PathString pathString, BitmapSource bSource)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                RenderTransformOrigin = new Point(0.5, 0.5),               
                Data = Geometry.Parse(pathString.GetPathString()),
                Fill = new ImageBrush(bSource) { TileMode = TileMode.None},
            };

            return path;
        }

        private Image CreateImageClipToPathPuzzle(PathString pathString, BitmapSource bSource)
        {
            ScaleTransform scale = new ScaleTransform(ScaleX,ScaleY);
            TranslateTransform translate = new TranslateTransform(-pathString.Center.X, -pathString.Center.Y);
            TransformGroup group = new TransformGroup();
            group.Children.Add(translate);
            group.Children.Add(scale);

            Image new_image = new Image()
            {
                Source = bSource,
                Clip = Geometry.Parse(pathString.GetPathString()),
                Stretch = Stretch.Uniform,
//Style = (Style)FindResource("imageScaleTransformStyle")
                RenderTransform = group//scale
            };

        //    Binding scaleX;
        //    Binding scaleY;
        //
        //    scaleX = new Binding("ScaleX") { Source = this, Mode = BindingMode.TwoWay,UpdateSourceTrigger=UpdateSourceTrigger.Explicit };
        //    BindingOperations.SetBinding(scale, ScaleTransform.ScaleXProperty, scaleX);
        //
        //    scaleY = new Binding("ScaleY") { Source = this, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.Explicit };
        //    BindingOperations.SetBinding(scale, ScaleTransform.ScaleYProperty, scaleY);

            return new_image;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
                if (textBox.Text.Length > 20)
                    textBox.Text = textBox.Text.Substring(0, 20);
        }

        private void buttonLoadGame_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            LoadGame();
        }

        private void buttonSaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            SaveGame();
        }

        private void LoadGame()
        {
        //    LoadLastState(".\\last_state.xml");
        }

        private void SaveGame()
        {
        //    SaveLastState(".\\last_state.xml");
        }

        private void mainWindowPuzzle_Unloaded(object sender, RoutedEventArgs e)
        {
            mp.Close();
        }

        private void mainWindowPuzzle_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            mp.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void LoadLastState(string filename)
        {
            //// Инициализация хранилища резервных копий
            //MementoBackUp memory = new MementoBackUp();
            //Object_BarleyBreak last_state = new Object_BarleyBreak();

            //last_state.ID = (last_state.ID.Item1, last_state.ID.Item2, 1);

            ////если словарь уже создавался, загрузим его
            //FileInfo info;
            //info = new FileInfo(filename);
            //if (info.Exists)
            //{
            //    memory = XML.Load(filename);
            //}

            //// Восстановление данных основного объекта из резервной копии
            //last_state.Get(memory[last_state.ID]);

            //// Восстановление состояния программы
            //RecoveryBarleyBreakState(last_state);
        }

        private void SaveLastState(string filename)
        {
            //// Инициализация хранилища резервных копий
            //MementoBackUp memory = new MementoBackUp();
            //Object_BarleyBreak last_state = new Object_BarleyBreak()
            //{
            //    time = time,
            //    SoundOnOff = SoundOnOff,
            //    MusicOnOff = MusicOnOff,
            //    array = array,//(int[][])array.Clone(),
            //    ImageFilename = ImageFilename,
            //    Count = Count,
            //    rows = uniformGridGame.Rows,
            //    columns = uniformGridGame.Columns,
            //    PlayerName = PlayerName
            //};

            //last_state.ID = (last_state.ID.Item1, last_state.ID.Item2, 1);

            //// Выполнение back-up
            //memory[last_state.ID] = last_state.Put();

            //XML.Save(memory, filename);
        }

        private void RecoveryBarleyBreakState(Object_BarleyBreak last_state)
        {
            ////восстановим параметры игры
            //SoundOnOff = last_state.SoundOnOff;
            //MusicOnOff = last_state.MusicOnOff;
            //ImageFilename = last_state.ImageFilename;
            //Count = last_state.Count;
            //uniformGridGame.Rows = last_state.rows;
            //uniformGridGame.Columns = last_state.columns;
            //PlayerName = last_state.PlayerName;

            ////подготовим новую игру по умолчанию
            //PrepareUniformGrid();

            ////приведем игру к сохраненному состоянию
            //int rows = uniformGridGame.Rows;
            //int columns = uniformGridGame.Columns;
            //int cnt = rows * columns;

            //for (int i = 0; i < cnt; i++)
            //{
            //    int index = last_state.array[i / columns][i % columns];
            //    for (int j = i; j < cnt; j++)
            //    {
            //        Button button = (Button)uniformGridGame.Children[j];
            //        if ((int)button.Tag == index)
            //        {
            //            uniformGridGame.Children.RemoveAt(j);
            //            uniformGridGame.Children.Insert(i, button);
            //        }
            //    }
            //}
            //array = last_state.array;

            ////запустим таймер
            //timer_game.Start();
            ////начнем игру
            //labelGameEnd.Visibility = Visibility.Hidden;
            ////установим время на момент сохранения сотояния
            //time = last_state.time;
        }

        private void mainWindowPuzzle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculatedScale();            
        }

        private void RecalculatedScale()
        {
            if (bSource != null)
            {
                ScaleX = uniformGridGame.ActualWidth / bSource.Width;
                ScaleY = uniformGridGame.ActualHeight / bSource.Height;

                uniformGridGame.UpdateLayout();
                foreach (var item in uniformGridGame.Children)
                    if (item is Image image)
                    {
                        if (image.RenderTransform is TransformGroup group)
                        {
                            group.Children[1].SetCurrentValue(ScaleTransform.ScaleXProperty, ScaleX);
                            group.Children[1].SetCurrentValue(ScaleTransform.ScaleYProperty, ScaleY);
                        }
                    }
                    else if (item is System.Windows.Shapes.Path path)
                    {
                        path.RenderTransform.SetCurrentValue(ScaleTransform.ScaleXProperty, ScaleX);
                        path.RenderTransform.SetCurrentValue(ScaleTransform.ScaleYProperty, ScaleY);
                    }
                foreach (var item in uniformGridGamePieces.Children)
                    if (item is Image image)
                    {
                        if (image.RenderTransform is TransformGroup group)
                        {
                            group.Children[1].SetCurrentValue(ScaleTransform.ScaleXProperty, ScaleX);
                            group.Children[1].SetCurrentValue(ScaleTransform.ScaleYProperty, ScaleY);
                        }
                    }

            }
        }

        private void uniformGridGamePieces_DragEnter(object sender, DragEventArgs e)
        {
            // Если пользователь копирует объект перетаскиванием и это список файлов и это не перетаскивание из listBox в него же
            if (e.Data.GetDataPresent("uniformGridGame") &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("uniformGridGamePieces"))
            {
                // Разрешить копирование
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void uniformGridGamePieces_Drop(object sender, DragEventArgs e)
        {
            // Если перетаскивается список файлов
            if (e.Data.GetDataPresent("uniformGridGame") &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("uniformGridGamePieces"))
            {
                int index_data = (int)e.Data.GetData("uniformGridGame");
                // Получить
                var element = uniformGridGame.Children[index_data];

                uniformGridGame.Children.Remove(element);
                uniformGridGamePieces.Children.Add(element);                
            }
        }

        private void uniformGridGame_DragEnter(object sender, DragEventArgs e)
        {
            // Если пользователь копирует объект перетаскиванием и это список файлов и это не перетаскивание из listBox в него же
            if (e.Data.GetDataPresent("uniformGridGamePieces") &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("uniformGridGame"))
            {
                // Разрешить копирование
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void uniformGridGame_Drop(object sender, DragEventArgs e)
        {            
            // Если перетаскивается список файлов
            if (e.Data.GetDataPresent("uniformGridGamePieces") &&
                (e.AllowedEffects & DragDropEffects.Copy) != 0 &&
                !e.Data.GetDataPresent("uniformGridGame"))
            {
                int index_data = (int)e.Data.GetData("uniformGridGamePieces");
                // Получить
                var element = uniformGridGamePieces.Children[index_data];

                uniformGridGamePieces.Children.Remove(element);
                uniformGridGame.Children.Add(element);  
            }
        }

        private void element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //    // получить координаты мыши 
            //    System.Windows.Point pt = e.GetPosition(this);
            //
            //    // выяснить, над каким контролом находится курсор мыши
            //    HitTestResult res = System.Windows.Media.VisualTreeHelper.HitTest(this, pt);

            if (sender is Image image)
            {
                draggedImage = image;

                // Создать контейнер для хранения данных
                DataObject data = new DataObject();

                //определим его индекс
                int index_element;
                if (image.Parent is UniformGrid)
                {
                    index_element = uniformGridGamePieces.Children.IndexOf(image);
                    // Добавить признак пользовательского формата в контейнер
                    data.SetData("uniformGridGamePieces", index_element);
                    // НАЧАТЬ перетаскивание программно
                    DragDropEffects dde = DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
                }
                else if (image.Parent is Grid)
                {
                    index_element = uniformGridGame.Children.IndexOf(image);
                    // Добавить признак пользовательского формата в контейнер
                    data.SetData("uniformGridGame", index_element);
                    // НАЧАТЬ перетаскивание программно
                    DragDropEffects dde = DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
                }
                else
                    return;

            }

            //if (sender is Canvas canvas)
            //{
            //    // Создать контейнер для хранения данных
            //    DataObject data = new DataObject();

            //    //определим его индекс
            //    int index_element;
            //    if (canvas.Parent is UniformGrid)
            //    {
            //        index_element = uniformGridGamePieces.Children.IndexOf(canvas);
            //        // Добавить признак пользовательского формата в контейнер
            //        data.SetData("uniformGridGamePieces", index_element);
            //    }
            //    else if (canvas.Parent is Grid)
            //    {
            //        index_element = uniformGridGame.Children.IndexOf(canvas);
            //        // Добавить признак пользовательского формата в контейнер
            //        data.SetData("uniformGridGame", index_element);
            //    }
            //    else
            //        return;

            //    // НАЧАТЬ перетаскивание программно
            //    DragDropEffects dde = DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            //}

           
//            if (res.VisualHit is UniformGrid uniformgrid)
//            {
//                //получим элемент, над которым находится курсор мыши
//                var element = (UIElement)res.VisualHit;
//                //определим его индекс
//                int index_element = uniformGridGame.Children.IndexOf(element);
//                // Создать контейнер для хранения данных
//                DataObject data = new DataObject();
//                // Добавить признак пользовательского формата в контейнер
//                data.SetData("uniformGridGamePieces",index_element);
//
//                // НАЧАТЬ перетаскивание программно
//                DragDropEffects dde = DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
//            }

        }

        private Image draggedImage;
        private Point mousePosition;

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;
            if (image == null)
                return;

            var grid = image.Parent as Canvas;

            if (image != null && grid.CaptureMouse())
            {
                mousePosition = e.GetPosition(grid);
                draggedImage = image;
                Panel.SetZIndex(draggedImage, 1); // in case of multiple images
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedImage != null)
            {
                // получить координаты мыши 
                System.Windows.Point pt1 = e.GetPosition(uniformGridGame);

                // выяснить, над каким контролом находится курсор мыши
                HitTestResult res1 = System.Windows.Media.VisualTreeHelper.HitTest(uniformGridGame, pt1);
                if (res1 != null && draggedImage.Parent.Equals(uniformGridGamePieces))
                {
                    uniformGridGamePieces.Children.Remove(draggedImage);
                    uniformGridGame.Children.Add(draggedImage);
                }

                // получить координаты мыши 
                System.Windows.Point pt2 = e.GetPosition(uniformGridGamePieces);

                // выяснить, над каким контролом находится курсор мыши
                HitTestResult res2 = System.Windows.Media.VisualTreeHelper.HitTest(uniformGridGamePieces, pt2);
                if (res2 != null && draggedImage.Parent.Equals(uniformGridGame))
                {
                    uniformGridGame.Children.Remove(draggedImage);
                    uniformGridGamePieces.Children.Add(draggedImage);
                }


            //    if (res1 != null)
            //        uniformGridGame.ReleaseMouseCapture();
            //    else if (res2 != null)
            //        uniformGridGamePieces.ReleaseMouseCapture();

                uniformGridGame.ReleaseMouseCapture();
                uniformGridGamePieces.ReleaseMouseCapture();


                //
                //
                Point position = e.GetPosition(uniformGridGame);
                position.X /= ScaleX;
                position.Y /= ScaleY;

                foreach (var path in list_pathstring)
                {
                    if (
                        (position.X > path.Center.X - (path.Width*2 / 3))
                        && (position.X < path.Center.X + (path.Width*2 / 3))
                            &&
                        (position.Y > path.Center.Y - (path.Height*2 / 3))
                        && (position.Y < path.Center.Y + (path.Height*2 / 3))
                        )
                    {
                        position = path.Center;
                        Canvas.SetLeft(draggedImage, (position.X) * ScaleX);
                        Canvas.SetTop(draggedImage, (position.Y) * ScaleY);

                        flag_checkPuzzle = true;
                    }
                }
                //
                //


                Panel.SetZIndex(draggedImage, 0);
                draggedImage = null;
            }
        }

        bool flag_checkPuzzle = false;
        private void CheckPuzzle()
        {
            int cnt = 0;
            int index = 1;
            foreach (var path in list_pathstring)
            {
                Point currentCenter = new Point(path.Center.X * ScaleX, path.Center.Y * ScaleY);
                foreach (var element in uniformGridGame.Children)
                {
                    if (element is Image image)
                    {
                        double posX = Canvas.GetLeft(image);
                        double posY = Canvas.GetTop(image);

                        int current_index = (int)image.Tag;
                        if (posX == currentCenter.X && posY == currentCenter.Y
                            && index == current_index)
                            cnt++;
                    }
                }
                index++;
            }

            if (cnt == (base_columns * base_rows))
            {
                MessageBox.Show("Your are Winner!!!", "End Game");
                timer_game.Stop();
                labelGameEnd.Visibility = Visibility.Visible;
            }
        }


        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (draggedImage != null)
            {
                // получить координаты мыши 
                System.Windows.Point pt1 = e.GetPosition(uniformGridGame);

                // выяснить, над каким контролом находится курсор мыши
                HitTestResult res1 = System.Windows.Media.VisualTreeHelper.HitTest(uniformGridGame, pt1);
                if (res1 != null && draggedImage.Parent.Equals(uniformGridGamePieces))
                {
                    uniformGridGamePieces.Children.Remove(draggedImage);
                    uniformGridGame.Children.Add(draggedImage);
                }

                // получить координаты мыши 
                System.Windows.Point pt2 = e.GetPosition(uniformGridGamePieces);

                // выяснить, над каким контролом находится курсор мыши
                HitTestResult res2 = System.Windows.Media.VisualTreeHelper.HitTest(uniformGridGamePieces, pt2);
                if (res2 != null && draggedImage.Parent.Equals(uniformGridGame))
                {
                    uniformGridGame.Children.Remove(draggedImage);
                    uniformGridGamePieces.Children.Add(draggedImage);
                }

                Point position = e.GetPosition(uniformGridGame);

                if (res2 != null)
                    position = e.GetPosition(uniformGridGamePieces);
                else if (res1 == null)
                    return;

                var offset = position - mousePosition;
                mousePosition = position;
                Canvas.SetLeft(draggedImage, position.X);
                Canvas.SetTop(draggedImage, position.Y);
                //                Canvas.SetLeft(draggedImage, Canvas.GetLeft(draggedImage) + offset.X);
                //                Canvas.SetTop(draggedImage, Canvas.GetTop(draggedImage) + offset.Y);
            }
        }

        private void textBox_Rows_TextChanged(object sender, TextChangedEventArgs e)
        {
            //считаем введенные данные - количество перемешиваний пятнашек
            int.TryParse(textBox_Rows.Text, out int res);
            if (res >= 4)
                base_rows = res;//обновим значение переменной, если введенные данные корректны
            textBox_Rows.Text = base_rows.ToString();//отобразим обновленное значение

        }

        private void textBox_Columns_TextChanged(object sender, TextChangedEventArgs e)
        {
            //считаем введенные данные - количество перемешиваний пятнашек
            int.TryParse(textBox_Columns.Text, out int res);
            if (res >= 4)
                base_columns = res;//обновим значение переменной, если введенные данные корректны
            textBox_Columns.Text = base_columns.ToString();//отобразим обновленное значение

        }
    }
}
