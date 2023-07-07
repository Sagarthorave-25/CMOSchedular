using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMORefMailSchedular
{
    class BussLayer
    {
        Commfunc objComm = new Commfunc();
        DataSet _ds = new DataSet();
        public string CheckApplicationNoCMO(string ApplicationNo) {
            string result = string.Empty;
            result=objComm.CheckApplicationNoCMO(ApplicationNo);
            return result;
        }
        public void UpdateUCNNumber(string Application_Number)
        {
            objComm.UpdateUCNNumber(Application_Number);
        }
    }
}
