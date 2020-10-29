using DP_Memento;
using Microsoft.Win32;
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

namespace WpfApp_BarleyBreak
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow_BarleyBreak : Window
    {
        //Timer timer_game;
        System.Windows.Threading.DispatcherTimer timer_game = new System.Windows.Threading.DispatcherTimer();

        public DelegateCommand<object> HideCommand { get; }
        public DelegateCommand<object> MinimizedCommand { get; }
        public DelegateCommand<object> CloseCommand { get; }
        public DelegateCommand<object> DragCommand { get; }

        public DelegateCommand<object> NewGameCommand { get; }
        public DelegateCommand<object> LoadGameCommand { get; }
        public DelegateCommand<object> SetImageCommand { get; }
        public DelegateCommand<object> SoundOnOffCommand { get; }
        public DelegateCommand<object> MusicOnOffCommand { get; }
        public DelegateCommand<object> HighScoreShowCommand { get; }

        public DelegateCommand<object> ExitCommand { get; }

        public MainWindow_BarleyBreak()
        {
            InitializeComponent();

            timer_game.Tick += timer_game_Elapsed;
            timer_game.Interval = new TimeSpan(0, 0, 0, 0, 100);

            HideCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.HidePage(obj); });
            MinimizedCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.MinimizedPage(obj); });
            CloseCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.ClosePage(obj); });
            DragCommand = new DelegateCommand<object>(obj => { PageModel_BaseFunction.DragPage(obj); });

            NewGameCommand       = new DelegateCommand<object>(obj => {if(SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); NewGame(); });
            LoadGameCommand      = new DelegateCommand<object>(obj => {if(SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); LoadGame(); });
            SetImageCommand      = new DelegateCommand<object>(obj => {if(SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); SetImage(); });
            SoundOnOffCommand  = new DelegateCommand<object>(obj => {if(SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); SoundOnOff = !SoundOnOff; });
            MusicOnOffCommand = new DelegateCommand<object>(obj => {if(SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav"); MusicOnOff = !MusicOnOff; if (MusicOnOff) mp.Play(); else mp.Pause(); });
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
            if(info.Exists)
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

        private void SoundWAV(string sound_filename,bool repeat = false)
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
            PrepareUniformGrid();
            time = 0;

            //запустим безопасное перемешивание - перемешивание элементов в обратном порядке 
            ShuffleSafe_Barley_Break(Count);

            timer_game.Start();
            labelGameEnd.Visibility = Visibility.Hidden;
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            NewGame();
        }

        private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");
            
            //запустим безопасное перемешивание - перемешивание элементов в обратном порядке 
            ShuffleSafe_Barley_Break(Count);
            time = 0;//обнулим таймер
         
            timer_game.Start();
            labelGameEnd.Visibility = Visibility.Hidden;
        }

        private void SetImage()
        {
            flag_image = true;
            OpenPicture();
        }

        private void checkBoxPicture_Checked(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            SetImage();
        }

        private void checkBoxPicture_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\MenuSelectionClick.wav");

            flag_image = false;
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

                NewGame();
            }
        }

        int[][] array;//массив номеров элементов на игровом поле
        bool flag_image = false;
        string ImageFilename = "\\Resources\\yellow_breakfast.jpg";
        int Count = 10;//установим количество перемешиваний
        int time;

        List<string> highscoreList { get; set; }

        //работа с каждым элементом на игровом поле
        private void element_Click(object sender, EventArgs e)
        {
            if (!timer_game.IsEnabled)
                return;


            if (sender is Button button)
            {
                int rows = uniformGridGame.Rows;
                int columns = uniformGridGame.Columns;
                
                //получение позиции
                int current_pos = uniformGridGame.Children.IndexOf(button);
                int current_posRow = current_pos / columns;
                int current_posColumn = current_pos % columns;

                int index = 0;
                index = (int)(button.Tag);//получили номер элемента

                int new_pos;//определим новую позицию

                //проверим все элементы и найдя пустой, проверим можно ли в него переместиться
                //помним, перемещения только по вертикали или горизонтали, диагонали запрещены
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        if (array[i][j] == 0)
                        {
                            //проверим достижимость позиции из исходной - текущей позиции
                            bool flag = false;
                            if ((current_posColumn - 1 == j) && (current_posRow == i)) flag = true;
                            if ((current_posColumn + 1 == j) && (current_posRow == i)) flag = true;
                            if ((current_posRow - 1 == i) && (current_posColumn == j)) flag = true;
                            if ((current_posRow + 1 == i) && (current_posColumn == j)) flag = true;

                            if (!flag)
                                return;

                            //осуществляем обмен элементов
                            array[i][j] = index;//запишем номер элемента в новую позицию
                            array[current_posRow][current_posColumn] = 0;//в старой позиции затираем его размещение
                            new_pos = i * columns + j;//готовим новую позицию

                            //установим новую позицию элементу
                            if(new_pos>current_pos)
                            {
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\leave.wav");

                                uniformGridGame.Children.RemoveAt(new_pos);
                                uniformGridGame.Children.RemoveAt(current_pos);
                                uniformGridGame.Children.Insert(current_pos, CreateLastPiece());
                                uniformGridGame.Children.Insert(new_pos, button);
                            }
                            else
                            {
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\arrive.wav");

                                uniformGridGame.Children.RemoveAt(current_pos);
                                uniformGridGame.Children.RemoveAt(new_pos);
                                uniformGridGame.Children.Insert(new_pos, button);
                                uniformGridGame.Children.Insert(current_pos, CreateLastPiece());
                            }

                            //проверим размещение элементов на игровом поле - а вдруг уже победа?
                            if (CheckCorrectBarleyBreak())
                            {
                                mp.Pause();
                                if (SoundOnOff) SoundWAV(".\\..\\..\\Resources\\interlude.wav");

                                MessageBox.Show("Congratulation!!!\nYour Time: " + textBlock_Time.Text);
                                WorkWithHighScoreTable();
                                mp.Play();
                            }
                            return;
                        }
            }
        }

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
                filestream = File.Open(scorefilename,FileMode.Open);
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
            if(newPlayer!=null)
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

        private bool CheckCorrectBarleyBreak()
        {
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;
            int cnt = rows * columns;

            //проверка корректности размещения пятнашек - номера по порядку от верхнего левого к нижнему правому игровому полю
            for (int i = 0; i < cnt - 1; i++)
                if (array[i / columns][i % columns] != i + 1)
                    return false;

            timer_game.Stop();

            return true;
        }

        private void ShuffleSafe_Barley_Break(int count)
        {
            int cnt = uniformGridGame.Rows * uniformGridGame.Columns;
            if (uniformGridGame.Children.Count != cnt)
                return;

            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;

            Random rand = new Random();
            for (int k = 0; k < count; k++)
            {
                int current_pos = rand.Next(cnt);
                int current_posRow = current_pos / columns;
                int current_posColumn = current_pos % columns;

                var button = uniformGridGame.Children[current_pos];

                int index = (int)(((Button)button).Tag);
    
                int new_pos;
    
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        if (array[i][j] == 0)
                        {
                            bool flag = false;
                            if ((current_posColumn - 1 == j) && (current_posRow == i)) flag = true;
                            if ((current_posColumn + 1 == j) && (current_posRow == i)) flag = true;
                            if ((current_posRow - 1 == i) && (current_posColumn == j)) flag = true;
                            if ((current_posRow + 1 == i) && (current_posColumn == j)) flag = true;

                            if (!flag)
                            {
                                k--;
                                goto repeat_shuffle;
                            }

                            //осуществляем обмен элементов
                            array[i][j] = index;//запишем номер элемента в новую позицию
                            array[current_posRow][current_posColumn] = 0;//в старой позиции затираем его размещение
                            new_pos = i * columns + j;//готовим новую позицию

                            //установим новую позицию элементу
                            if (new_pos > current_pos)
                            {
                                uniformGridGame.Children.RemoveAt(new_pos);
                                uniformGridGame.Children.RemoveAt(current_pos);
                                uniformGridGame.Children.Insert(current_pos, CreateLastPiece());
                                uniformGridGame.Children.Insert(new_pos, button);
                            }
                            else
                            {
                                uniformGridGame.Children.RemoveAt(current_pos);
                                uniformGridGame.Children.RemoveAt(new_pos);
                                uniformGridGame.Children.Insert(new_pos, button);
                                uniformGridGame.Children.Insert(current_pos, CreateLastPiece());
                            }

                        }
                    repeat_shuffle:;
            }
        }

        private Button CreateLastPiece()
        {
            return new Button() { Visibility = Visibility.Hidden, Tag = 0 };
        }

        private void PrepareUniformGrid()
        {
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;

            //создадим и заполним(проинициализируем) массив
            array = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                    array[i][j] = 1 + i + i * (columns-1) + j;
            }
            array[rows - 1][columns - 1] = 0;

            //очистка игрового поля перед новой игрой
            uniformGridGame.Children.Clear();

            if (flag_image)
                PrepareButtonImage();
            else
                PrepareButtonNummer();     

            Binding bndWidth;
            Binding bndHeight;
            if (mainWindowBarleyBreak.ActualWidth > mainWindowBarleyBreak.ActualHeight)
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

            //            //Shuffle
            //            Shuffle_Barley_Break(10);
        }

        private void textBoxCountShuffle_TextChanged(object sender, TextChangedEventArgs e)
        {
            //считаем введенные данные - количество перемешиваний пятнашек
            int.TryParse(textBoxCountShuffle.Text, out int res);
            if (res != null && res > 0)
                Count = res;//обновим значение переменной, если введенные данные корректны
            textBoxCountShuffle.Text = Count.ToString();//отобразим обновленное значение
        }

        private void mainWindowBarleyBreak_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxCountShuffle.Text = Count.ToString();
            
            if (MusicOnOff) MusicMP3(".\\..\\..\\Resources\\PurplePlanetMusic-Awakening(1_50)120bpm(L).mp3",true);

            MessageBox.Show(
"Игра \"Пятнашки\" (7 баллов)" +
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

        private void PrepareButtonNummer()
        {
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;
            int cnt = rows * columns;

            Button button;
            for (int i = 0; i < cnt - 1; i++)
            {
                button = CreateButtonNummer((i + 1).ToString());
                button.Tag = i + 1;
                uniformGridGame.Children.Add(button);
            }
            uniformGridGame.Children.Add(CreateLastPiece());
        }

        Image source;//исходная картинка
        private void PrepareButtonImage()
        {             
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;
            int cnt = rows * columns;
            
            int width = (int)uniformGridGame.ActualWidth;
            int height = (int)uniformGridGame.ActualHeight;
           
            //исходная картинка
            BitmapSource bSource = new BitmapImage(new Uri(ImageFilename))
            {                
                SourceRect = new Int32Rect(0,0,width,height)                
            };

            uniformGridGame.Background = new ImageBrush(bSource) { Stretch=Stretch.Fill,Opacity=0.2 };

            int width_delta = (int)bSource.PixelWidth / rows;
            int height_delta = (int)bSource.PixelHeight / columns;

            //разобъем картинку на 15 частей(16 пустая - отсутствует)
            Button buttonImage;
            for (int i = 0; i < cnt - 1; i++)
            {
                //сделаем все элементы

                //сформируем прямоугольник - квадрат в нужной части исходной промасштабированной картинки
                // Create a CroppedBitmap based off of a xaml defined resource.
                CroppedBitmap cb = new CroppedBitmap(
                   bSource,
                   new Int32Rect(i % columns * width_delta, i / columns * height_delta, width_delta, height_delta));       //select region rect
                Image imagePiece = new Image
                {
                    Source = cb,
                    Stretch = Stretch.Fill
                };

                //получим эту часть - используя операцию ВЫРЕЗКА
                buttonImage = CreateButtonImage((i + 1).ToString(), imagePiece);
                buttonImage.Tag = i + 1;//укажем номер в стандартном расположении пятнашек
                uniformGridGame.Children.Add(buttonImage);
            }
            uniformGridGame.Children.Add(CreateLastPiece());
        }

        private Button CreateButtonNummer(string text)
        {
            Button button = new Button
            {
                FontSize = 26,
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

        private Button CreateButtonImage(string text, Image image)
        {
            Button button = new Button
            {
                FontSize = 26,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Normal,
                Name = "button_" + text,
                Content = image
            };
            button.Click += new RoutedEventHandler(this.element_Click);

            button.Style = (Style)TryFindResource("PiecesAnimation");

            return button;
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
            LoadLastState(".\\last_state.xml");
        }

        private void SaveGame()
        {
            SaveLastState(".\\last_state.xml");
        }

        private void mainWindowBarleyBreak_Unloaded(object sender, RoutedEventArgs e)
        {
            mp.Close();
        }

        private void mainWindowBarleyBreak_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            mp.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void LoadLastState(string filename)
        {
            // Инициализация хранилища резервных копий
            MementoBackUp memory = new MementoBackUp();
            Object_BarleyBreak last_state = new Object_BarleyBreak();
      
            last_state.ID = (last_state.ID.Item1, last_state.ID.Item2, 1);

            //если словарь уже создавался, загрузим его
            FileInfo info;
            info = new FileInfo(filename);
            if (info.Exists)
            {
                memory = XML.Load(filename);
            }

            // Восстановление данных основного объекта из резервной копии
            last_state.Get(memory[last_state.ID]);

            // Восстановление состояния программы
            RecoveryBarleyBreakState(last_state);
        }

        private void SaveLastState(string filename)
        {
            // Инициализация хранилища резервных копий
            MementoBackUp memory = new MementoBackUp();
            Object_BarleyBreak last_state = new Object_BarleyBreak()
            {
                time = time,
                SoundOnOff = SoundOnOff,
                MusicOnOff = MusicOnOff,
                array = array,//(int[][])array.Clone(),
                flag_image = flag_image,
                ImageFilename = ImageFilename,
                Count = Count,
                rows = uniformGridGame.Rows,
                columns = uniformGridGame.Columns,
                PlayerName = PlayerName
            };

            last_state.ID = (last_state.ID.Item1, last_state.ID.Item2, 1);

            // Выполнение back-up
            memory[last_state.ID] = last_state.Put();

            XML.Save(memory, filename);
        }

        private void RecoveryBarleyBreakState(Object_BarleyBreak last_state)
        {
            //восстановим параметры игры
            SoundOnOff = last_state.SoundOnOff;
            MusicOnOff = last_state.MusicOnOff;
            flag_image = last_state.flag_image;
            ImageFilename = last_state.ImageFilename;
            Count = last_state.Count;
            uniformGridGame.Rows = last_state.rows;
            uniformGridGame.Columns = last_state.columns;
            PlayerName = last_state.PlayerName;

            //подготовим новую игру по умолчанию
            PrepareUniformGrid();

            //приведем игру к сохраненному состоянию
            int rows = uniformGridGame.Rows;
            int columns = uniformGridGame.Columns;
            int cnt = rows * columns;

            for(int i=0;i<cnt;i++)
            {
                int index = last_state.array[i / columns][i % columns];
                for (int j = i; j < cnt; j++)
                {
                    Button button = (Button)uniformGridGame.Children[j];
                    if ((int)button.Tag == index)
                    {
                        uniformGridGame.Children.RemoveAt(j);
                        uniformGridGame.Children.Insert(i, button);
                    }
                }
            }
            array = last_state.array;

            //запустим таймер
            timer_game.Start();
            //начнем игру
            labelGameEnd.Visibility = Visibility.Hidden;
            //установим время на момент сохранения сотояния
            time = last_state.time;
        }
    }
}
