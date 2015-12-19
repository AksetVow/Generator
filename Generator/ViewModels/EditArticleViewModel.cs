using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.ViewModels
{
    public class EditArticleViewModel : INotifyPropertyChanged
    {
        private Article _article;
        private DateTime _publicDate;

        public EditArticleViewModel(Article article)
        {
            _article = article;
            if (_article.PublicDate == null)
            {
                _article.PublicDate = DateTime.Now.ToShortDateString();
            }
            _publicDate = DateTime.Parse(_article.PublicDate);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ArticleText {
            get { return _article.ArticleText; }

            set {
                if (_article.ArticleText != value)
                {
                    _article.ArticleText = value;
                    RaisePropertyChanged("ArticleText");
                }
            
            }
        }


        public string Author
        {
            get { return _article.Author; }

            set
            {
                if (_article.Author != value)
                {
                    _article.Author = value;
                    RaisePropertyChanged("Author");
                }

            }
        }


        public string Category
        {
            get { return _article.Category; }

            set
            {
                if (_article.Category != value)
                {
                    _article.Category = value;
                    RaisePropertyChanged("Category");
                }

            }
        }

        public string CategoryEmpty
        {
            get { return _article.CategoryEmpty; }

            set
            {
                if (_article.CategoryEmpty != value)
                {
                    _article.CategoryEmpty = value;
                    RaisePropertyChanged("CategoryEmpty");
                }

            }
        }

        public string Id
        {
            get { return _article.Id.ToString(); }
            set { }
        }

        public string IdMain
        {
            get { return _article.IdMain.ToString(); }

            set
            {
                var intValue = string.IsNullOrEmpty(value) ? 0 : Int32.Parse(value);
                if (_article.IdMain != intValue)
                {
                    _article.IdMain = intValue;
                    RaisePropertyChanged("IdMain");
                }

            }
        }

        public string KeyWords
        {
            get { return _article.KeyWords; }

            set
            {
                if (_article.KeyWords != value)
                {
                    _article.KeyWords = value;
                    RaisePropertyChanged("KeyWords");
                }

            }
        }

        public string Mark
        {
            get { return _article.Mark.ToString(); }

            set
            {
                var intValue = string.IsNullOrEmpty(value) ? 0 : Int32.Parse(value);
                if (_article.Mark != intValue)
                {
                    _article.Mark = intValue;
                    RaisePropertyChanged("Mark");
                }

            }
        }

        public DateTime PublicDate
        {
            get { return _publicDate; }

            set
            {
                if (_publicDate != value)
                {
                    _publicDate = value;
                    _article.PublicDate = _publicDate.ToShortDateString();
                    RaisePropertyChanged("PublicDate");
                }

            }
        }

        public string Region
        {
            get { return _article.Region; }

            set 
            {
                if (_article.Region != value)
                {
                    _article.Region = value;
                    RaisePropertyChanged("Region");
                }
                
            }
        }

        public string Source
        {
            get { return _article.Source; }

            set
            {
                if (_article.Source != value)
                {
                    _article.Source = value;
                    RaisePropertyChanged("Source");
                }

            }
        }

        public string SourceNumber
        {
            get { return _article.SourceNumber; }

            set
            {
                if (_article.SourceNumber != value)
                {
                    _article.SourceNumber = value;
                    RaisePropertyChanged("SourceNumber");
                }

            }
        }

        public string SubjectCategory
        {
            get { return _article.SubjectCategory; }

            set
            {
                if (_article.SubjectCategory != value)
                {
                    _article.SubjectCategory = value;
                    RaisePropertyChanged("SubjectCategory");
                }
            }
        }

        public string Title
        {
            get { return _article.Title; }

            set
            {
                if (_article.Title != value)
                {
                    _article.Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        public Article Article {
            get { return _article; }
        }
    }
}
