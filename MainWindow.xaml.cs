using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LFS.View;
using LFS.ViewModel;
using LFS.Base;

namespace LFS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SubtitlesView SubView { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Debug_AutoSelectSub()
        {
            SubView = new SubtitlesView(@"E:\from ubuntu\videos\The Wire 1-5\The Wire Season 4\The Wire [4x02] Soft Eyes.srt");
            this.Frame.Content = SubView;
        }
    }
}
