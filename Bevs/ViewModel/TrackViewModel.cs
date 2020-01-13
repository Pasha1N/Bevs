using Bevs.Model;

namespace Bevs.ViewModel
{
    public class TrackViewModel
    {
        private Track track;

        public TrackViewModel(Track track)
        {
            this.track = track;
        }

        public string Name => track.Name;
        public string Path => track.Path;
    }
}