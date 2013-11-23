using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Documents;

using LFS.Model;
using LFS.Base;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using LFS.Services;

namespace LFS.ViewModel
{
    public class SubtitlesViewModel : ViewModelBase
    {
        #region Fields
        private ICommand _buttonClickNext;
        private ICommand _buttonClickPrevious;
        private ICommand _mostRareWords;
        private ICommand _mostOftenWords;
        private ICommand _matchDb;
        private ICommand _translate;
        private ICommand _pronouncing;
        private ICommand _favorite;
        private int _index;
        private string _word;
        private string _fileName;
        private int _count;
        private string _favoriteIconSource;

        public FlowDocument translate = new FlowDocument();
        private Translater translater = new Translater();
        #endregion

        public SubtitlesViewModel(string fileName)
        {
            this.Model = new SubtitlesModel(fileName);
            FileName = fileName;

            // Events handlers
            translater.GotTranslate += ShowTranslate;

            // Default behaivor
            Model.MostRareWords();

            //Reset values
            ResetValues();
        }

        private void ShowTranslate(object sender, TranslateArgs args)
        {
            //translate.Blocks.Add( args.GetTranslate() );
            foreach(var block in args.GetTranslate().Blocks.ToList())
            {
                translate.Blocks.Add(block);
            }
        }

        private void ResetValues()
        {
            Index = Convert.ToString(0);
            Word = Model.words[0];
            Count = Model.words.Count.ToString();
            UpdateInfo();
        }

        private void NextWord()
        {
            Index = (_index + 1).ToString();
            Word = this.Model.words[_index];
            UpdateInfo();
        }

        private void PreviousWord()
        {
            Index = (_index - 1).ToString();
            Word = this.Model.words[_index];
            UpdateInfo();
        }

        private void MostOftenWords()
        {
            Model.MostOftenWords();
            ResetValues();
        }

        private void MostRareWords()
        {
            Model.MostRareWords();
            ResetValues();
        }

        private void MatchDb()
        {
            Model.MatchDb();
            ResetValues();
        }
        
        private void Translate()
        {
            // Clear old text
            translate.Blocks.Clear();
            translater.Translate(Word);
        }

        private void Pronouncing()
        {
            translater.Pronouncing(Word);
        }

        private void Favorite()
        {
            if (Model.HasWord(Word))
            {
                Model.DeleteWord(Word);
            }
            else
            {
                Model.AddWord(Word);
            }

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (Model.HasWord(Word))
            {
                FavoriteIconSource = "/Resources/Images/cl.png";
                Model.UpdateWordInfo(Word);
            }
            else
            {
                FavoriteIconSource = "/Resources/Images/add.gif";
            }
        }

        #region Properties

        public SubtitlesModel Model { get; private set; }

        /// <summary>
        /// Return value+1, because of human readable
        /// </summary>
        public string Index
        {
            get
            {
                return (_index + 1).ToString();
            }
            set
            {
                Int32.TryParse(value, out this._index);
                OnPropertyChanged();
            }
        }

        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                _word = value;
                OnPropertyChanged();
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = Path.GetFileName(value);
                OnPropertyChanged();
            }
        }

        public string Count
        {
            get
            {
                return _count.ToString();
            }
            set
            {
                Int32.TryParse(value, out this._count);
                OnPropertyChanged();
            }
        }

        public string FavoriteIconSource
        {
            get
            {
                return _favoriteIconSource;
            }
            set
            {
                _favoriteIconSource = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand ButtonClickNext
        {
            get
            {
                if (_buttonClickNext == null)
                {
                    _buttonClickNext = new RelayCommand(
                        param => NextWord(),
                        param => (this.Model.words.Count - 1 > _index)
                    );
                }
                return _buttonClickNext;
            }
        }
        
        public ICommand ButtonClickPrevious
        {
            get
            {
                if (_buttonClickPrevious == null)
                {
                    _buttonClickPrevious = new RelayCommand(
                        param => PreviousWord(),
                        param => (_index > 0)
                    );
                }
                return _buttonClickPrevious;
            }
        }

        public ICommand MostRareWordsCmd
        {
            get
            {
                if (_mostRareWords == null)
                {
                    _mostRareWords = new RelayCommand(
                        p => MostRareWords()
                        );
                }
                return _mostRareWords;
            }
        }

        public ICommand MostOftenWordsCmd
        {
            get
            {
                if (_mostOftenWords == null)
                {
                    _mostOftenWords = new RelayCommand(p => MostOftenWords());
                }
                return _mostOftenWords;
            }
        }

        public ICommand TranslateCmd
        {
            get
            {
                if (_translate == null)
                {
                    _translate = new RelayCommand(p => Translate());
                }
                return _translate;
            }
        }

        public ICommand PronouncingCmd
        {
            get
            {
                if (_pronouncing == null)
                {
                    _pronouncing = new RelayCommand(p => Pronouncing());
                }
                return _pronouncing;
            }
        }

        public ICommand FavoriteCmd
        {
            get
            {
                if (_favorite == null)
                {
                    _favorite = new RelayCommand(p => Favorite());
                }
                return _favorite;
            }
        }

        public ICommand MatchDbCmd
        {
            get
            {
                if (_matchDb == null)
                {
                    _matchDb = new RelayCommand(
                        p => MatchDb(),
                        p => Model.HasAnyMatchedWordsFromDb
                    );
                }
                return _matchDb;
            }
        }

        #endregion

    }
}
