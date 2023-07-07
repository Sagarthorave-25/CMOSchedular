using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMORefMailSchedular
{
    
    class Commfunc
    {
        DataLayer objSql = new DataLayer();
        public string CheckApplicationNoCMO(string Application_Number)
        {
            string result = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] _sqlparam = new SqlParameter[1];
                _sqlparam[0] = new SqlParameter("@ApplicationNo", Application_Number);
                 ds= objSql.RetrieveDataset("USP_CheckCMOValidApplicationNo", _sqlparam);
                 result =ds.Tables[0].Rows[0]["ApplicationNo"].ToString();
            }
            catch (Exception ex) {
                SaveErrorLogs(Application_Number, "CheckApplicationNoCMO", "CommFun", "error", ex.Message);
            }
            return result;
        }
        public void UpdateUCNNumber(string Application_Number)
        {
            try
            {
                SqlParameter[] _sqlparam = new SqlParameter[3];
                _sqlparam[0] = new SqlParameter("@Application_Number", Application_Number);
                _sqlparam[1] = new SqlParameter("@Code", "UCN");
                _sqlparam[2] = new SqlParameter("@Mode", "CMOref");
                objSql.InsertRecord("USP_UpdateUCNNumber", _sqlparam);
            }
            catch (Exception ex) {
                SaveErrorLogs(Application_Number, "UpdateUCNNumber", "CommFun", "error", ex.Message);
            }
        }
        public void SaveErrorLogs(string ApplicationNo,string Method, string Class, string Error_Status, string Error_Desc)
        {
            SqlParameter[] _sqlparam = new SqlParameter[6];
            _sqlparam[0] = new SqlParameter("@Method", Method);
            _sqlparam[1] = new SqlParameter("@Class", Class);
            _sqlparam[2] = new SqlParameter("@Error_Status", Error_Status);
            _sqlparam[3] = new SqlParameter("@Error_Desc", Error_Desc);
            _sqlparam[4] = new SqlParameter("@Project","CMORef");
            _sqlparam[5] = new SqlParameter("@ApplicationNo", ApplicationNo);
            objSql.InsertRecord("USP_CMOReferalLogs", _sqlparam);
        }
    }
}
