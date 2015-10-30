using System;

namespace Core.Export
{
    public class UserRequestData
    {
        #region Constants
        public const string ReportNameRegex = @"%USERREQUEST_Название отчета_s%";
        public const string ReportStartDateRegex = @"%USERREQUEST_Начальная_дата отчета_d%";
        public const string ReportEndDateRegex = @"%USERREQUEST_Конечная дата отчета_d%";
        public const string ReportMaterialsRegex = @"%USERREQUEST_По материалам_s%";
        #endregion

        public string ReportStartDateString 
        {
            get
            {
                if (ReportStartDate == null)
                {
                    return string.Empty;
                }

                return ReportStartDate.Value.ToShortDateString();
            }
        }
        
        public string ReportEndDateString 
        {
            get
            {
                if (ReportEndDate == null)
                {
                    return string.Empty;
                }

                return ReportEndDate.Value.ToShortDateString();
            }
        }

        public DateTime? ReportStartDate { get; set; }
        public DateTime? ReportEndDate { get; set; }
        public string ReportName { get; set; }
        public string ReportMaterials { get; set; }




        public UserRequestData()
        {
            ReportName = string.Empty;
            ReportMaterials = string.Empty;
        }

    }
}
