using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

using LFS.Base;
using LFS.Services;
using LFS.View;

namespace LFS.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private ICommand _showWordsFromDatabase;
        private ICommand _openSubtitles;

        private UserControl _frame;
        #endregion

        #region Private methods
        private void OpenSubtitles()
        {
            var fileOpen = new Microsoft.Win32.OpenFileDialog();
            fileOpen.Filter = "Subtitles (.srt)|*.srt";

            bool? userClickedOk = fileOpen.ShowDialog();
            if (userClickedOk == true)
            {
                //SubView = new SubtitlesView(fileOpen.FileName);
                //this.Frame.Content = SubView;
                //var content = this.Frame.Content;
                Frame = new SubtitlesView(fileOpen.FileName);
            }
        }

        private void ShowWordsFromDatabase()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties
        public UserControl Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand ShowWordsFromDatabaseCmd
        {
            get
            {
                if (_showWordsFromDatabase == null)
                {
                    _showWordsFromDatabase = new RelayCommand(p => ShowWordsFromDatabase());
                }
                return _showWordsFromDatabase;
            }
        }

        public ICommand OpenSubtitlesCmd
        {
            get
            {
                if (_openSubtitles == null)
                {
                    _openSubtitles = new RelayCommand(p => OpenSubtitles());
                }
                return _openSubtitles;
            }
        }

        #endregion
    }
}
