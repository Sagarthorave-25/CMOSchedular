using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMORefMailSchedular
{
    class DataLayer
    {
        
        public int InsertRecord(string spName, SqlParameter[] sqlParam)
        {
            int result=0;
            try
            {
                string cnStr = ConfigurationSettings.AppSettings["transactiondbLF"];
                result = SqlHelper.ExecuteNonQuery(cnStr, CommandType.StoredProcedure, spName, sqlParam);
              
            }
            catch (Exception ex) {
                new Commfunc().SaveErrorLogs("", "InsertRecord", "Datalayer", "error", ex.Message);
            }
            return result;
        }
        public DataSet RetrieveDataset(string spName, SqlParameter[] sqlParam)
        {
            DataSet _ds=null;
            try
            {
             
                string strCon = ConfigurationSettings.AppSettings["transactiondbLF"];
                _ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, spName, sqlParam);
                
            }
            catch (Exception ex) {

                new Commfunc().SaveErrorLogs("", "RetrieveDataset", "Datalayer", "error", ex.Message);
            }
            return _ds;
        }
       
    }
}
