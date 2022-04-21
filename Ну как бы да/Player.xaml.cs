using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TagLib;
using System.Drawing;
using System.Windows.Media.Animation;

namespace Ну_как_бы_да
{
    /// <summary>
    /// Логика взаимодействия для Player.xaml
    /// </summary>
    public partial class Player : Window
    {
        public Player()
        {
            InitializeComponent();
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}");
            string path = Directory.GetCurrentDirectory() + "\\Drunk.wav";
            ss.Source = new Uri(path);
            ss.LoadedBehavior = MediaState.Manual;
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(300);
            dispatcher.Interval = TimeSpan.FromSeconds(1);
            dispatcher.Tick += Dispatcher_Tick;
            RoutedEventArgs e = new RoutedEventArgs();

            using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{MainWindow.accountData}.txt"))
            {
                reader.ReadLine();
                txtUser.Text = reader.ReadLine();
                if(reader.ReadLine() == "0")
                {
                    btnCreateNewPlaylist.Visibility = Visibility.Collapsed;
                    btnRepeat.Visibility = Visibility.Collapsed;
                    btnRepeatOne.Visibility = Visibility.Collapsed;
                    btnBackwardTen.Visibility = Visibility.Collapsed;
                    btnForwardTen.Visibility = Visibility.Collapsed;
                    cmbPlaylist.Visibility = Visibility.Collapsed;
                    txtSearch.Visibility = Visibility.Collapsed;
                }
                else
                {
                    i1.Visibility = Visibility.Collapsed;
                    i2.Visibility = Visibility.Collapsed;
                    i3.Visibility = Visibility.Collapsed;
                    i4.Visibility = Visibility.Collapsed;
                    i5.Visibility = Visibility.Collapsed;
                    btnDonation.Visibility = Visibility.Collapsed;
                }
                reader.ReadLine();
                string img = reader.ReadLine();
                if (img != @"Resources\avva.png" && img != "none")
                    imgUser.ImageSource = BitmapFrame.Create(new Uri(img, UriKind.Relative));
            }

            if(!System.IO.File.Exists(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\All songs.txt"))
                using (FileStream file = System.IO.File.Create(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\All songs.txt"))
                    file.Close();

            btnAllSongs_Click(btnAllSongs, e);
            AddBtn();
        }

        public DispatcherTimer timer = new DispatcherTimer();
        public DispatcherTimer dispatcher = new DispatcherTimer();

        private void Timer_Tick(object sender, EventArgs e)
        {
            ss.Position = TimeSpan.FromSeconds(sliderMusicDuration.Value);
            blet = Convert.ToInt32(sliderMusicDuration.Value); 
            timer.Stop();
        }

        public bool play = false;

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
                    
            if (play_pause.Kind == MaterialDesignThemes.Wpf.PackIconKind.Play)
            {
                if (mediaEnded)
                {
                    play_pause.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                    ss.Position = TimeSpan.FromSeconds(0);
                    ss.Play();
                    dispatcher.Start();
                    mediaEnded = false;
                }
                else
                {
                    play_pause.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                    ss.Play();
                    dispatcher.Start();
                }
            }
            else
            {
                if (mediaEnded)
                {
                    play_pause.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    ss.Position = TimeSpan.FromSeconds(0);
                    ss.Pause();
                    dispatcher.Stop();
                    mediaEnded = false;
                }
                else
                {
                    play_pause.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    ss.Pause();
                    dispatcher.Stop();
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ss.NaturalDuration.HasTimeSpan && blet + 10 < ss.NaturalDuration.TimeSpan.TotalSeconds)
            {
                ss.Position += TimeSpan.FromSeconds(10);
                sliderMusicDuration.Value += 10;
                blet += 10;
            }
            else
            {
                ss.Position = TimeSpan.FromSeconds(0);
                sliderMusicDuration.Value = 0;
                blet = 0;
            }
        }

        private void sliderMusicDuration_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Thread.Sleep(10);
                timer.Start();
            }
            if (!ss.CanPause) sliderMusicDuration.Value = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (blet - 10 > 10)
            {
                blet -= 10;
                ss.Position -= TimeSpan.FromSeconds(10);
                sliderMusicDuration.Value -= 10;
            }
            else
            {
                blet -= blet;
                ss.Position -= ss.Position;
                sliderMusicDuration.Value -= sliderMusicDuration.Value;
            }
        }
        public int blet = 0;
        private void Dispatcher_Tick(object sender, EventArgs e)
        {
            blet += 1;
            sliderMusicDuration.Value += 1;
            if (ss.NaturalDuration.HasTimeSpan)
            {
                sliderMusicDuration.Maximum = ss.NaturalDuration.TimeSpan.TotalSeconds;
                duration.Text = ss.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + ss.NaturalDuration.TimeSpan.Seconds.ToString();
                curent.Text = new TimeSpan(0, 0, blet).Minutes.ToString() + ":" + new TimeSpan(0, 0, blet).Seconds.ToString();
            }
        }

        private void PackIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                volume.Value = 0;
        }

        private void Slider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            balance.Value = 0;
        }

        private void btnAllSongs_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as Button) == btnAllSongs)
            {
                btnCreateNewPlaylist.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                btnCreateNewPlaylist.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                btnAllSongs.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
                btnAllSongs.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
            }
            else
            {
                btnAllSongs.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                btnAllSongs.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                btnCreateNewPlaylist.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
                btnCreateNewPlaylist.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
            }
        }

        private void btnAddSongs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select song or directory with songs";
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Аудио формат (*.wav, *.mp3, *.mp4)|*.wav; *.mp3; *.mp4";
            if (fileDialog.ShowDialog() == true)
            {
                string path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}";
                using (StreamWriter writer = new StreamWriter(path + "\\All songs.txt", true))
                {
                    foreach (string filepath in fileDialog.FileNames)
                        writer.WriteLine(filepath);
                }
            }
            musicData.ItemsSource = null;
            curentMusicList.Clear();
            filepathlist.Clear();
            btnAllSongs_Click(btnAllSongs, e);
        }

        public List<Music> curentMusicList = new List<Music>();
        public List<string> filepathlist = new List<string>();
        public List<string> selectedMusic = new List<string>();

        private void btnAllSongs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnChangeName.IsEnabled = false;
                btnAddSongsToPlaylist.IsEnabled = false;
                musicData.ItemsSource = null;
                musicData.Items.Refresh();
                curentMusicList.Clear();
                filepathlist.Clear();
                txtPlaylistName.Text = "All songs";
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}.txt"))
                {
                    while (reader.Peek() != -1)
                    {
                        filepathlist.Add(reader.ReadLine());
                    }
                }
                int j = 1;
                char[] separator = { '\\', '.' };
                foreach (string file in filepathlist)
                {
                    var audioFile = TagLib.File.Create(file);
                    TagLib.File file_TAG = audioFile;
                    if (audioFile.Tag.Title != null)
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = audioFile.Tag.Album,
                            Artist = String.Join(", ", audioFile.Tag.Performers),
                            Name = audioFile.Tag.Title
                        });
                    }
                    else
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = "",
                            Artist = "",
                            Name = file.Split(separator, StringSplitOptions.RemoveEmptyEntries)[file.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length - 2]
                        });
                    }
                    j++;
                }
                musicData.ItemsSource = curentMusicList;
                musicData.Items.Refresh();
                cmbPlaylist.SelectedIndex = -1;
            }
            catch { MessageBox.Show("Мы рассмотрим данную ошибку в ближайшее время.\nОтправить отчет об ошибке?", "Иди на*** по причине дол***б", MessageBoxButton.YesNo, MessageBoxImage.Question); }
        }

        private void musicData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!helper && musicData.SelectedIndex != -1 && !deleteSong)
                {
                    blet = 0;
                    sliderMusicDuration.Value = 0;
                    ss.Source = new Uri(filepathlist[musicData.SelectedIndex]);
                    curent.Text = "0:0";
                    var audioFile = TagLib.File.Create(ss.Source.OriginalString);
                    TagLib.File file_TAG = audioFile;
                    if (file_TAG.Tag.Pictures.Length > 0)
                    {
                        var bin = (file_TAG.Tag.Pictures[0].Data.Data);
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        MemoryStream stream = new MemoryStream(bin);
                        BitmapImage bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.StreamSource = stream;
                        bmp.EndInit();
                        stream.Close();
                        Oblojka.Source = bmp;
                    }
                    else Oblojka.Source = BitmapFrame.Create(new Uri($"Resources/camera.jpg", UriKind.Relative));
                    txbCurentSong.Text = curentMusicList[musicData.SelectedIndex].Name;
                }
                else if (deleteSong)
                {
                    if (musicData.SelectedIndex != -1)
                    {
                        curentMusicList[musicData.SelectedIndex].IsSelected = "✗";
                        selectedMusic.Add(filepathlist[musicData.SelectedIndex]);
                    }
                }
                else
                {
                    if (musicData.SelectedIndex != -1)
                    {
                        curentMusicList[musicData.SelectedIndex].IsSelected = "✓";
                        selectedMusic.Add(filepathlist[musicData.SelectedIndex]);
                    }
                }
                musicData.ItemsSource = curentMusicList;
                musicData.Items.Refresh();
            }
            catch { MessageBox.Show("Мы рассмотрим данную ошибку в ближайшее время.\nОтправить отчет об ошибке?", "Иди на*** по причине дол***б", MessageBoxButton.YesNo, MessageBoxImage.Question); }
        }

        public bool helper = false;

        private void btnCreateNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnChangeName.IsEnabled = false;
                btnAddSongsToPlaylist.IsEnabled = false;
                musicData.ItemsSource = null;
                musicData.Items.Refresh();
                curentMusicList.Clear();
                filepathlist.Clear();
                txtPlaylistName.Text = "All songs";
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}.txt"))
                {
                    while (reader.Peek() != -1)
                    {
                        filepathlist.Add(reader.ReadLine());
                    }
                }
                int j = 1;
                char[] separator = { '\\', '.' };
                foreach (string file in filepathlist)
                {
                    var audioFile = TagLib.File.Create(file);
                    TagLib.File file_TAG = audioFile;
                    if (audioFile.Tag.Title != null)
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = audioFile.Tag.Album,
                            Artist = String.Join(", ", audioFile.Tag.Performers),
                            Name = audioFile.Tag.Title
                        });
                    }
                    else
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = "",
                            Artist = "",
                            Name = file.Split(separator, StringSplitOptions.RemoveEmptyEntries)[file.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length - 2]
                        });
                    }
                    j++;
                }
                musicData.ItemsSource = curentMusicList;
                musicData.Items.Refresh();
                cmbPlaylist.SelectedIndex = -1;
                helper = true;
                btnAllSongs.Visibility = Visibility.Collapsed;
                btnAddSongs.Visibility = Visibility.Collapsed;
                cmbPlaylist.Visibility = Visibility.Collapsed;
                btnSavePlaylist.Visibility = Visibility.Visible;
                btnDiscardPlaylist.Visibility = Visibility.Visible;
                txtPlaylistName.Text = "New playlist";
                txtPlaylistName.IsReadOnly = false;
                stkPanel.IsEnabled = false;
                grdGrid.IsEnabled = false;
                tlbBar.IsEnabled = false;
            }
            catch { MessageBox.Show("Мы рассмотрим данную ошибку в ближайшее время.\nОтправить отчет об ошибке?", "Иди на*** по причине дол***б", MessageBoxButton.YesNo, MessageBoxImage.Question); }
            AddBtn();
        }

        private void btnSavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            cmbPlaylist.ItemsSource = null;
            List<string> btns = new List<string>();
            bool origin = true;
            btns.Add("New playlist");
            btns.Add("All songs");
            foreach (string playlist in Directory.GetFileSystemEntries(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}"))
            {
                if (playlist.Contains("✓"))
                {
                    btns.Add(System.IO.Path.GetFileNameWithoutExtension(playlist).Replace("✓", ""));
                }
            }
            for (int i = 0; i < btns.Count; i++)
                if (txtPlaylistName.Text == btns[i]) origin = false;
            if (origin)
            {
                string path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}✓.txt";
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    foreach (string file in selectedMusic)
                        writer.WriteLine(file);
                }
                foreach (Music music in curentMusicList)
                    music.IsSelected = "";
                btnAllSongs.Visibility = Visibility.Visible;
                btnAddSongs.Visibility = Visibility.Visible;
                cmbPlaylist.Visibility = Visibility.Visible;
                btnSavePlaylist.Visibility = Visibility.Collapsed;
                btnDiscardPlaylist.Visibility = Visibility.Collapsed;
                cmbPlaylist.SelectedIndex = -1;
                btnAllSongs.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(63, 81, 181));
                btnAllSongs.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(63, 81, 181));
                btnCreateNewPlaylist.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                btnCreateNewPlaylist.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                txtPlaylistName.Text = "All songs";
                txtPlaylistName.IsReadOnly = true;
                helper = false;
                selectedMusic.Clear();
                stkPanel.IsEnabled = true;
                grdGrid.IsEnabled = true;
                tlbBar.IsEnabled = true;
                musicData.ItemsSource = null;
                musicData.ItemsSource = curentMusicList;
                AddBtn();
            } else MessageBox.Show("Create av exlusive name for your playlist\nyou have never used before", "Atentoin", MessageBoxButton.OK, MessageBoxImage.Information);
            AddBtn();
        }

        private void AddBtn()
        {
            List<string> YaNeHochuBolysheBityProgrammistom = new List<string>();
            cmbPlaylist.ItemsSource = null;
            List<string> btns = new List<string>();
            foreach (string playlist in Directory.GetFileSystemEntries(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}"))
            {
                if (playlist.Contains("✓"))
                {
                    YaNeHochuBolysheBityProgrammistom.Add(System.IO.Path.GetFileNameWithoutExtension(playlist).Replace("✓", ""));
                }
            }
            cmbPlaylist.Items.Refresh();
            cmbPlaylist.ItemsSource = YaNeHochuBolysheBityProgrammistom;
        }

        private void btnDiscardPlaylist_Click(object sender, RoutedEventArgs e)
        {
            foreach (Music music in curentMusicList)
                music.IsSelected = "";
            btnAllSongs.Visibility = Visibility.Visible;
            btnAddSongs.Visibility = Visibility.Visible;
            cmbPlaylist.Visibility = Visibility.Visible;
            btnSavePlaylist.Visibility = Visibility.Collapsed;
            btnDiscardPlaylist.Visibility = Visibility.Collapsed;
            btnCreateNewPlaylist.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
            btnCreateNewPlaylist.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
            btnAllSongs.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(63, 81, 181));
            btnAllSongs.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(63, 81, 181));
            stkPanel.IsEnabled = true;
            grdGrid.IsEnabled = true;
            tlbBar.IsEnabled = true;
            cmbPlaylist.SelectedIndex = -1;
            txtPlaylistName.Text = "All songs";
            txtPlaylistName.IsReadOnly = true;
            helper = false;
            selectedMusic.Clear();
        }

        private void cmbPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPlaylist.SelectedIndex != - 1)
            {
                btnChangeName.IsEnabled = true;
                btnAddSongsToPlaylist.IsEnabled = true;
                string path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{cmbPlaylist.SelectedItem.ToString()}✓.txt";
                musicData.ItemsSource = null;
                musicData.Items.Refresh();
                curentMusicList.Clear();
                filepathlist.Clear();
                using (StreamReader reader = new StreamReader(path))
                {
                    while (reader.Peek() != -1)
                    {
                        filepathlist.Add(reader.ReadLine());
                    }
                }
                int j = 1;
                char[] separator = { '\\', '.' };
                foreach (string file in filepathlist)
                {
                    var audioFile = TagLib.File.Create(file);
                    TagLib.File file_TAG = audioFile;
                    if (audioFile.Tag.Title != null)
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = audioFile.Tag.Album,
                            Artist = String.Join(", ", audioFile.Tag.Performers),
                            Name = audioFile.Tag.Title
                        });
                    }
                    else
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = "",
                            Artist = "",
                            Name = file.Split(separator, StringSplitOptions.RemoveEmptyEntries)[file.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length - 2]
                        });
                    }
                    j++;
                }
                musicData.ItemsSource = curentMusicList;
                musicData.Items.Refresh();
                txtPlaylistName.Text = cmbPlaylist.SelectedItem.ToString();
                btnAllSongs.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
                btnAllSongs.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 13, 13));
            }
        }

        private void btnNextSong_Click(object sender, RoutedEventArgs e)
        {
            if (musicData.SelectedIndex == musicData.Items.Count - 1) musicData.SelectedIndex = 0;
            else musicData.SelectedIndex++;
            if (ss.NaturalDuration.HasTimeSpan)
            {
                sliderMusicDuration.Maximum = ss.NaturalDuration.TimeSpan.TotalSeconds;
                duration.Text = ss.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + ss.NaturalDuration.TimeSpan.Seconds.ToString();
                curent.Text = new TimeSpan(0, 0, blet).Minutes.ToString() + ":" + new TimeSpan(0, 0, blet).Seconds.ToString();
            }
        }

        private void btnPriviousSong_Click(object sender, RoutedEventArgs e)
        {
            if (musicData.SelectedIndex == 0) musicData.SelectedIndex = musicData.Items.Count - 1;
            else if (musicData.SelectedIndex == -1) musicData.SelectedIndex = 0;
            else musicData.SelectedIndex--;
            if (ss.NaturalDuration.HasTimeSpan)
            {
                sliderMusicDuration.Maximum = ss.NaturalDuration.TimeSpan.TotalSeconds;
                duration.Text = ss.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + ss.NaturalDuration.TimeSpan.Seconds.ToString();
                curent.Text = new TimeSpan(0, 0, blet).Minutes.ToString() + ":" + new TimeSpan(0, 0, blet).Seconds.ToString();
            }
        }

        public bool mediaEnded = false;

        private void ss_MediaEnded(object sender, RoutedEventArgs e)
        {
            blet = 0;
            sliderMusicDuration.Value = 0;
            //dispatcher.Stop();
            mediaEnded = true;         
            if (musicData.SelectedIndex == musicData.Items.Count - 1 && repeat == 2) musicData.SelectedIndex = 0;
            else if (repeat != 1 && musicData.SelectedIndex != musicData.Items.Count - 1) musicData.SelectedIndex++;
            else if (repeat == 1) ss.Position = TimeSpan.FromSeconds(0);
            else if (repeat == 0 && musicData.SelectedIndex == musicData.Items.Count - 1) btnPlayPause_Click(btnPlayPause, e);
            if (ss.NaturalDuration.HasTimeSpan)
            {
                sliderMusicDuration.Maximum = ss.NaturalDuration.TimeSpan.TotalSeconds;
                duration.Text = ss.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + ss.NaturalDuration.TimeSpan.Seconds.ToString();
                curent.Text = new TimeSpan(0, 0, blet).Minutes.ToString() + ":" + new TimeSpan(0, 0, blet).Seconds.ToString();
            }
        }

        List<int> rnd = new List<int>();
        public int repeat = 0;

        private void btnRepeatOne_Click(object sender, RoutedEventArgs e)
        {
            if(repeat != 1)
            {
                repeat = 1;
                kndRepeatOne.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(216, 27, 96));
                kndRepeat.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
            else if (repeat == 1)
            {
                repeat = 0;
                kndRepeatOne.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            if (repeat != 2)
            {
                repeat = 2;
                kndRepeat.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(216, 27, 96));
                kndRepeatOne.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
            else if(repeat == 2)
            {
                repeat = 0;
                kndRepeat.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (cheDelaem == "удаляем")
            {
                int j = 1;
                char[] separator = { '\\', '.'};
                string path = "";
                if (txtPlaylistName.Text == "All songs")
                    path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}.txt";
                else
                    path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}✓.txt";
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    foreach (string file in selectedMusic)
                        filepathlist.Remove(file);//////////////////////////////////////////////////////////////////////////////////////////
                    foreach (string filepath in filepathlist)
                        writer.WriteLine(filepath);
                }
                curentMusicList.Clear();
                foreach (string file in filepathlist)
                {
                    var audioFile = TagLib.File.Create(file);
                    TagLib.File file_TAG = audioFile;
                    if (audioFile.Tag.Title != null)
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = audioFile.Tag.Album,
                            Artist = String.Join(", ", audioFile.Tag.Performers),
                            Name = audioFile.Tag.Title
                        });
                    }
                    else
                    {
                        curentMusicList.Add(new Music
                        {
                            Num = j.ToString(),
                            Album = "",
                            Artist = "",
                            Name = file.Split(separator, StringSplitOptions.RemoveEmptyEntries)[file.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length - 2]
                        });
                    }
                    j++;
                }
                btnSaveChanges.Visibility = Visibility.Collapsed;
                selectedMusic.Clear();
                musicData.ItemsSource = null;
                musicData.ItemsSource = curentMusicList;
                musicData.Items.Refresh();
                deleteSong = false;
                tlbBar.IsEnabled = true;
                stkPanel.IsEnabled = true;
                grdGrid.IsEnabled = true;
                cmbPlaylist.IsEnabled = true;
                cheDelaem = "";
            }
            else if (cheDelaem == "меняем название")
            {
                System.IO.File.Move(path, Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}✓.txt");
                cheDelaem = "меняем название";
                btnSaveChanges.Visibility = Visibility.Visible;
                tlbBar.IsEnabled = true;
                stkPanel.IsEnabled = true;
                grdGrid.IsEnabled = true;
                cmbPlaylist.IsEnabled = true;
                musicData.IsEnabled = true;
                txtPlaylistName.IsReadOnly = true;
                btnSaveChanges.Visibility = Visibility.Collapsed;
                cheDelaem = "";
                path = "";
                AddBtn();
                cmbPlaylist.SelectedItem = txtPlaylistName.Text;
            }
            else if (cheDelaem == "добавляем")
            {
                
            }
        }

        public bool deleteSong = false;
        public string cheDelaem = "";
        public string path = "";

        private void DeleteSongs_Click(object sender, RoutedEventArgs e)
        {
            cheDelaem = "удаляем";
            btnSaveChanges.Visibility = Visibility.Visible;
            deleteSong = true;
            tlbBar.IsEnabled = false;
            stkPanel.IsEnabled = false;
            grdGrid.IsEnabled = false;
            cmbPlaylist.IsEnabled = false;

        }

        private void btnChangeName_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text != "All songs")
            {
                cheDelaem = "меняем название";
                btnSaveChanges.Visibility = Visibility.Visible;
                tlbBar.IsEnabled = false;
                stkPanel.IsEnabled = false;
                grdGrid.IsEnabled = false;
                cmbPlaylist.IsEnabled = false;
                musicData.IsEnabled = false;
                txtPlaylistName.IsReadOnly = false;
                path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}✓.txt";
            }
            else MessageBox.Show("You can not rename this playlist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnAddSongsToPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text != "All songs")
            {
                cheDelaem = "добавляем";
                int selectedIndex = cmbPlaylist.SelectedIndex;
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Select song or directory with songs";
                fileDialog.Multiselect = true;
                fileDialog.Filter = "Аудио формат (*.wav, *.mp3, *.mp4)|*.wav; *.mp3; *.mp4";
                if (fileDialog.ShowDialog() == true)
                {
                    string path = Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{txtPlaylistName.Text}✓.txt";
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        foreach (string filepath in fileDialog.FileNames)
                            writer.WriteLine(filepath);
                    }
                }
                musicData.ItemsSource = null;
                curentMusicList.Clear();
                filepathlist.Clear();
                cmbPlaylist.SelectedIndex = -1;
                cmbPlaylist.SelectedIndex = selectedIndex;
            }
            else MessageBox.Show("You can not add songs to this playlist\nthis way", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtSearch.Text != "")
            {
                musicData.IsEnabled = false;
                var filter = curentMusicList.Where(x => x.Name.ToLower().Contains(txtSearch.Text));
                musicData.ItemsSource = null;
                musicData.ItemsSource = filter;
            }
            else
            {
                musicData.IsEnabled = true;
                musicData.ItemsSource = null;
                musicData.ItemsSource = curentMusicList;
            }
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Аудио формат (.png, .jpg, jpeg)|*.png; *.jpg; *.jpeg";

                if (fileDialog.ShowDialog() == true)
                {
                    imgUser.ImageSource = BitmapFrame.Create(new Uri(fileDialog.FileName));
                }

                string[] blyaaa = new string[4]; 
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{MainWindow.accountData}.txt"))
                {
                    blyaaa[0] = reader.ReadLine();
                    blyaaa[1] = reader.ReadLine();
                    blyaaa[2] = reader.ReadLine();
                    blyaaa[3] = reader.ReadLine();

                }

                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{MainWindow.accountData}.txt", false))
                {
                    writer.WriteLine(blyaaa[0]);
                    writer.WriteLine(blyaaa[1]);
                    writer.WriteLine(blyaaa[2]);
                    writer.WriteLine(blyaaa[3]);
                    if (fileDialog.FileName != "")
                    {
                        writer.WriteLine(fileDialog.FileName);
                        imgUser.ImageSource = BitmapFrame.Create(new Uri(fileDialog.FileName));
                    }
                    else 
                    {
                        writer.WriteLine(@"C:\Users\petra\OneDrive\Рабочий стол\Ну как бы да\Ну как бы да\Ну как бы да\Resources\avva.png");
                        imgUser.ImageSource = BitmapFrame.Create(new Uri(@"C:\Users\petra\OneDrive\Рабочий стол\Ну как бы да\Ну как бы да\Ну как бы да\Resources\avva.png"));
                    }
       
                }

            }
        }

        private void btnDonation_Click(object sender, RoutedEventArgs e)
        {
            qr.Visibility = Visibility.Visible;
            string[] blyaaa = new string[5];
            using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{MainWindow.accountData}.txt"))
            {
                blyaaa[0] = reader.ReadLine();
                blyaaa[1] = reader.ReadLine();
                blyaaa[2] = reader.ReadLine();
                blyaaa[3] = reader.ReadLine();
                blyaaa[4] = reader.ReadLine(); 
            }

            using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + $"\\AccInfo\\{MainWindow.accountData}\\{MainWindow.accountData}.txt", false))
            {
                writer.WriteLine(blyaaa[0]);
                writer.WriteLine(blyaaa[1]);
                writer.WriteLine("1");
                writer.WriteLine(blyaaa[3]);
                writer.WriteLine(blyaaa[4]);
            }
        }
    }
    public class Music
    {
        public string Num { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string IsSelected { get; set; }
    }
}
