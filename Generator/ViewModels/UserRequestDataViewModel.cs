using Core.Export;
using System;
using System.ComponentModel;

namespace Generator.ViewModels
{
    public class UserRequestDataViewModel : INotifyPropertyChanged
    {
        private UserRequestData _userRequestData;

        public UserRequestDataViewModel()
        {
            _userRequestData = new UserRequestData();
            _userRequestData.ReportStartDate = DateTime.Now;
            _userRequestData.ReportEndDate = DateTime.Now;
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


        public DateTime StartDate {
            get 
            {
                return _userRequestData.ReportStartDate.Value;
            }

            set
            {
                if (_userRequestData.ReportStartDate != value)
                {
                    _userRequestData.ReportStartDate = value;
                    RaisePropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _userRequestData.ReportEndDate.Value;
            }

            set
            {
                if (_userRequestData.ReportEndDate != value)
                {
                    _userRequestData.ReportEndDate = value;
                    RaisePropertyChanged("EndDate");
                }
            }
        }

        public string ReportName
        {
            get 
            {
                return _userRequestData.ReportName;
            }

            set
            {
                if (_userRequestData.ReportName != value)
                {
                    _userRequestData.ReportName = value;
                    RaisePropertyChanged("ReportName");
                }
            }
        }


        public string ReportMaterials
        {
            get
            {
                return _userRequestData.ReportMaterials;
            }

            set
            {
                if (_userRequestData.ReportMaterials != value)
                {
                    _userRequestData.ReportMaterials = value;
                    RaisePropertyChanged("ReportMaterials");
                }
            }
        }

        public UserRequestData UserRequestData
        {
            get { return _userRequestData; }
        }
    }
}
