using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace Health
{
    public static class data
    {
        public static DataTable patientsDT = Dal.getTable("SP_getPatients", null);
        public static DataTable vaccinationsDT = Dal.getTable("SP_getVaccinations", null);
        public static DataTable sickDT = Dal.getTable("SP_getSick", null);

    }
}
