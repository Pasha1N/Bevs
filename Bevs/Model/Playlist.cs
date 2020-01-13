using Bevs.Wpf.ViewModel;
using System;
using System.Collections.Generic;

namespace Bevs.Model
{
    public class Playlist : EventINotifyPropertyChanged
    {
        private DateTime dateCreation;
        private string imagePath;
        private string name;
        private string path;
        private ICollection<Track> trackNames = new List<Track>();


        public Playlist(string name)
        {
            path = name;
            TrimName(name);
        }

        public string Name => name;
        public string Path
        {
            get => path;
            set => SetProperty(ref path, value);
        }
        public ICollection<Track> TrackNames
        {
            get => trackNames;
            set { trackNames = value; }
        }

        private void TrimName(string fullName)
        {
            int ind = fullName.LastIndexOf("\\");

            name = fullName.Substring(ind + 1);
        }
    }
}