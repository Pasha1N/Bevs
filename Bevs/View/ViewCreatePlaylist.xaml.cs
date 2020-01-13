using Bevs.ViewModel;
using System.Windows;

namespace Bevs.View
{
    public partial class ViewCreatePlaylist : Window
    {
        public ViewCreatePlaylist(ViewModelCreatePlaylist modelCreateAlbum)
        {
            InitializeComponent();
            DataContext = modelCreateAlbum;
        }
    }
}