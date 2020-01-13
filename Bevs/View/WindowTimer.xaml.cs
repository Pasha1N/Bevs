using Bevs.ViewModel;
using System.Windows;

namespace Bevs.View
{
    public partial class WindowTimer : Window
    {
        public WindowTimer(ViewModelWindowTimer windowTimer)
        {
            InitializeComponent();
            DataContext = windowTimer;
        }
    }
}