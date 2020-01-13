using Bevs.Model;
using Bevs.Wpf.Commands;
using Bevs.Wpf.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Bevs.ViewModel
{
    public class ViewModelCreatePlaylist : EventINotifyPropertyChanged
    {
        private ICommand createPlaylist;
        private string name;
        private Playlist playlist;
        private Window windowCreatePlaylist;

        public ViewModelCreatePlaylist()
        {
            createPlaylist = new DelegateCommand(CreatePlaylist);
        }

        public DateTime DateCreation { get; }
        public Playlist GetPlayList => playlist;
        //  public string ImagePath { get; }
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        //  public string Path { get; set; }
        public ICommand PCreatePlaylist => createPlaylist;
        public Window WindowCreatePlaylist { get; set; }

        public void CreatePlaylist()
        {
            playlist = new Playlist(Name);

            if (!Directory.Exists("..\\Playlists"))
            {
                Directory.CreateDirectory("..\\Playlists");
            }

            Directory.CreateDirectory(@"..\Playlists");

            WindowCreatePlaylist.Close();
        }
    }
}