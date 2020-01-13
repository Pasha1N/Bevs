using Bevs.Model;
using System.Collections.Generic;

namespace Bevs.ViewModel
{
    public class PlaylistViewModel
    {
        private Playlist playlist;

        public PlaylistViewModel(Playlist playlist)
        {
            this.playlist = playlist;
        }

        //// public DateTime DateCreation { get; set; }
        public string Name => playlist.Name;
        public string Path => playlist.Path;
        //// public string ImagePath { get; set; }
        public ICollection<Track> TrackNames => playlist.TrackNames;
    }
}