using Bevs.Wpf.ViewModel;
using Bevs.Wpf.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using TagLib.Mpeg;
using TagLib;
using Bevs.View;
using System.ComponentModel;
using Bevs.ViewModel;
using Bevs.Model;
using System.Windows;
using System.Timers;
using System.Windows.Threading;
using System.Threading;

namespace Player.ViewModel
{
    public class MainViewModel : EventINotifyPropertyChanged
    {
        private string btnOpenCloseTrackBoxContent = "<";
        private bool isMute = false;
        private bool isPause = false;
        private const int maxSizeTreckBox = 220;
        private ViewModelCreatePlaylist modelCreatePlalist;
        private ViewModelWindowTimer modelWindowTimer;
        private string pathPlaylists;
        private MediaPlayer player;
        private ICollection<Playlist> playlists = new ObservableCollection<Playlist>();
        private double savedVolume;
        private Playlist selectPlaylist;
        private TrackViewModel selectTrackViewModel = null;
        private int sizeTreckBox = 220;
        private IList<TrackViewModel> tracks = new ObservableCollection<TrackViewModel>();

        private ICommand addTreck;
        private ICommand mute;
        private ICommand nextTrack;
        private ICommand pause;
        private ICommand playTrack;
        private ICommand previousTrack;
        private ICommand openCloseTreckBox;
        private ICommand openWindowTimer;
        private ICommand windowCreateAlbum;

        // private System.Timers.Timer timer = new System.Timers.Timer();

        public MainViewModel()
        {
            addTreck = new DelegateCommand(AddTrack);
            mute = new DelegateCommand(ChangeMute);
            modelCreatePlalist = new ViewModelCreatePlaylist();
            nextTrack = new DelegateCommand(NextTrack);
            openCloseTreckBox = new DelegateCommand(OpenCloseTreckBox);
            openWindowTimer = new DelegateCommand(CreateWindowTimer);
            pause = new DelegateCommand(PouseTrack);
            player = new MediaPlayer();
            player.MediaOpened += TrackOpened;
            playTrack = new DelegateCommand(Play);
            previousTrack = new DelegateCommand(PreviousTrack);
            windowCreateAlbum = new DelegateCommand(CreateAlbum);

            StartLoad();

            //  timer.Interval = 1040;
            //  timer.Elapsed += Progress; //как синхронизировать?

        }

        public ICommand ChageSizeTreckBox => openCloseTreckBox;
        public ICommand GetNextTrack => nextTrack;
        public ICommand GetPreviousTrack => previousTrack;
        public ICommand Mute => mute;
        public ICommand Pause => pause;
        public ICommand PlayTrack => playTrack;
        public ICommand NewTrack => addTreck;
        public ICommand OpenWindowTimer => openWindowTimer;
        public ICommand WindowCreateAlbum => windowCreateAlbum;

        public string BtnOpenCloseTrackBoxContent
        {
            get => btnOpenCloseTrackBoxContent;
            set => SetProperty(ref btnOpenCloseTrackBoxContent, value);
        }
        public ICollection<Playlist> Playlists
        {
            get => playlists;
            set => SetProperty(ref playlists, value);
        }
        public TrackViewModel SelectTrack
        {
            get => selectTrackViewModel;
            set => SetProperty(ref selectTrackViewModel, value);
        }
        public Playlist SelectPlaylist
        {
            get => selectPlaylist;
            set
            {
                SetProperty(ref selectPlaylist, value);
                GetAllTrecks();
                SelectTrack = tracks.First();
            }
        }
        public int SizeTreckBox
        {
            get => sizeTreckBox;
            set => SetProperty(ref sizeTreckBox, value);
        }
        public IList<TrackViewModel> Tracks
        {
            get => tracks;
            set => SetProperty(ref tracks, value);
        }
        public double Volume
        {
            get => Math.Round(player.Volume * 100);
            set
            {
                player.Volume = value / 100;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Volume)));
            }
        }

        //-------------------------------------------------------------------------------

        private double trackDuretion;
        private double positionInDuretion;
        private double currendPosition = 0;

        public double PositionInDuretion
        {
            get => player.Position.TotalSeconds;//на какое событие подписаться для постоянного считывания Position?
            set
            {
                SetProperty(ref positionInDuretion, value);
                player.Position = new TimeSpan(0, 0, 0, int.Parse(positionInDuretion.ToString()));
            }
        }


        //public void Progress (object sender, EventArgs e)
        //{
        //    Application.Current.Dispatcher.Invoke(() => { 

        //    PositionInDuretion = player.Position.Seconds;
        //    });
        //}

        public double TrackDuretion
        {
            get => trackDuretion;
            set => SetProperty(ref trackDuretion, value);
        }
        public double CurrendPosition
        {
            get => currendPosition;
            set => SetProperty(ref currendPosition, value);
        }
        public void PouseTrack()
        {
            if (!isPause)
            {
                player.Pause();
                isPause = true;
                bool f = player.IsMuted;
            }
            else
            {
                player.Play();
                isPause = false;
            }
        }

  //-------------------------------------------------------------------------------
  

        public void AddTrack()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.DefaultExt = ".mp3";
            dlg.CheckFileExists = true;
            dlg.ShowDialog();
            SelectPlaylist.TrackNames.Add(new Track(dlg.FileName));

            try
            {
                System.IO.File.Copy(dlg.FileName, SelectPlaylist.Path + "\\" + dlg.FileName.Substring(dlg.FileName.LastIndexOf("\\") + 1));
            }
            catch
            {

            }
            GetAllTrecks();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Tracks)));
        }

        public void ChangeMute()
        {
            if (isMute)
            {
                isMute = false;
                Volume = savedVolume * 100;
            }
            else if (!isMute)
            {
                savedVolume = player.Volume;
                isMute = true;
                Volume = 0.0;
            }
        }

        public void CreateAlbum()
        {
            ViewCreatePlaylist windowCreateAlbum = new ViewCreatePlaylist(modelCreatePlalist);
            modelCreatePlalist.WindowCreatePlaylist = windowCreateAlbum;
            windowCreateAlbum.ShowDialog();
            playlists.Add(modelCreatePlalist.GetPlayList);
        }

        public void CreateWindowTimer()
        {
            if (modelWindowTimer == null)
            {
                modelWindowTimer = new ViewModelWindowTimer();
            }

            WindowTimer windowTimer = new WindowTimer(modelWindowTimer);
            modelWindowTimer.WindowTimer = windowTimer;
            modelWindowTimer.UpdateDataTimer();
            windowTimer.ShowDialog();

            if(!modelWindowTimer.IsTimerSet)
            {
                modelWindowTimer = null;
            }
        }

        public void GetAllTrecks()
        {
            Tracks.Clear();
            string[] playList = Directory.GetFiles(SelectPlaylist.Path, "*.mp3");

            foreach (string trackName in playList)
            {
                Tracks.Add(new TrackViewModel(new Track(trackName)));
            }
        }

        public void GetPathPlaylists()
        {
            DirectoryInfo dirPlaylists = Directory.GetParent(@"..\Playlists..\");
            pathPlaylists = dirPlaylists.FullName;
        }

        public void LoadPlaylists()
        {
            string[] namesplaylists = Directory.GetDirectories(pathPlaylists);

            foreach (string playlistName in namesplaylists)
            {
                playlists.Add(new Playlist(playlistName));
            }
        }

        public void NextTrack()
        {
            int indexCurrentTreck = tracks.IndexOf(SelectTrack);
            if (indexCurrentTreck + 1 == tracks.Count)
            {
                SelectTrack = tracks.ElementAt(0);
            }
            else
            {
                SelectTrack = tracks.ElementAt(indexCurrentTreck + 1);
            }

            Play();
        }

        public void OpenDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.DefaultExt = ".mp3";
            dlg.CheckFileExists = true;
            dlg.ShowDialog();
        }

        public void OpenCloseTreckBox()
        {
            if (sizeTreckBox == 0)
            {
                SizeTreckBox = maxSizeTreckBox;
                BtnOpenCloseTrackBoxContent = "<";
            }
            else if (sizeTreckBox == maxSizeTreckBox)
            {
                SizeTreckBox = 0;
                BtnOpenCloseTrackBoxContent = ">";
            }
        }

        public void Play()
        {
            if (SelectTrack != null)
            {
                player.Open(new Uri(selectTrackViewModel.Path));
                //timer.Start();
            }
        }

        public void PreviousTrack()
        {
            int indexCurrentTreck = tracks.IndexOf(SelectTrack);

            if (indexCurrentTreck == 0)
            {
                SelectTrack = tracks.ElementAt(tracks.Count - 1);
            }
            else
            {
                SelectTrack = tracks.ElementAt(indexCurrentTreck - 1);
            }

            Play();
        }

        private void StartLoad()
        {
            GetPathPlaylists();
            LoadPlaylists();
            SelectPlaylist = playlists.First();
            GetAllTrecks();

        }

        public void TrackOpened(object sander, EventArgs e)
        {
            Play();
            if (player.NaturalDuration.HasTimeSpan)
            {
                TrackDuretion = player.NaturalDuration.TimeSpan.TotalSeconds;
            }

            player.Volume = 100;
            player.Play();
        }
    }
}

//Mp3File mp3 = new Mp3File(@"C:\06-project_pat-bloodhound.mp3");
//Console.WriteLine("Альбом: " + mp3.TagHandler.Album);
//        Console.WriteLine("Артист: " + mp3.TagHandler.Artist);
//        Console.WriteLine("Год:    " + mp3.TagHandler.Year);
//        Console.WriteLine("Жанр:   " + mp3.TagHandler.Genre);
//        Console.WriteLine("Композитор: " + mp3.TagHandler.Composer);
//        Console.WriteLine("Диск: " + mp3.TagHandler.Disc);
//        Console.WriteLine("Стихи: " + mp3.TagHandler.Lyrics);
//        Console.WriteLine("Название песни: " + mp3.TagHandler.Song);
//        Console.WriteLine("Title: " + mp3.TagHandler.Title);
//        Console.WriteLine("Дорожка: " + mp3.TagHandler.Track);
//        Console.ReadKey(true);