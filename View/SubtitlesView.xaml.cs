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

using LFS.ViewModel;

namespace LFS.View
{
    /// <summary>
    /// Interaction logic for SubtitlesView.xaml
    /// </summary>
    public partial class SubtitlesView : UserControl
    {
        private SubtitlesViewModel viewModel;

        public SubtitlesView(string fileName)
        {
            InitializeComponent();
            viewModel = new SubtitlesViewModel(fileName);
            this.DataContext = viewModel;

            // Binding FlowDocument :(
            translate.Document = viewModel.translate;

            Loaded += (o, e) => this.Focus();
            //favoriteIcon.Source = new BitmapImage(new Uri("/Resources/Images/cl.png", UriKind.Relative));
        } 
    }
}
