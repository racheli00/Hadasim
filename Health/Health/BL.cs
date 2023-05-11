using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;


namespace Health
{
    public static class BL
    {
        static List<SqlParameter> param = new List<SqlParameter>();
        static SqlParameter p;

        public static void addPatient(string id, string firstName, string lastName, string birthDate, string phone, string cellPhone, string city, string street, int number)
        {
            param.Clear();

            p = new SqlParameter("@ID", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@firstName", firstName);
            p.SqlDbType = SqlDbType.NVarChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@lastName",lastName);
            p.SqlDbType = SqlDbType.NVarChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@birthDate", birthDate);
            p.SqlDbType = SqlDbType.Date;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@phone", phone);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@cellPhone", cellPhone);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@city", city);
            p.SqlDbType = SqlDbType.NVarChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@street", street);
            p.SqlDbType = SqlDbType.NVarChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@houseNumber", number);
            p.SqlDbType = SqlDbType.Int;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            Dal.ExecuteNonQuery("SP_addNewPatient", param);
        }

        public static void addVaccination(string id, string vaccinationDate, string manufacturer)
        {
            param.Clear();

            p = new SqlParameter("@ID", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@myDate", vaccinationDate);
            p.SqlDbType = SqlDbType.Date;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@manufacturer", manufacturer);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            Dal.ExecuteNonQuery("SP_addVaccination", param);
        }

        public static int isPatientExist(string id)
        {
            param.Clear();

            p = new SqlParameter("@id", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            return (int)Dal.Scalar("SP_isPatientExists", param);
        }

        public static bool IsIsraeliIdNumber(string id)
        {
            id = id.Trim();
            if (id.Length > 9 || !int.TryParse(id, out int idNum))
            {
                return false;
            }
            id = id.Length < 9 ? ("00000000" + id).Substring(id.Length) : id;
            int sum = 0;
            for (int i = 0; i < id.Length; i++)
            {
                int digit = int.Parse(id[i].ToString());
                int step = digit * ((i % 2) + 1);
                sum += step > 9 ? step - 9 : step;
            }
            return sum % 10 == 0;
        }

        public static DataTable isAlreadySick(string id)
        {
            param.Clear();

            p = new SqlParameter("@id", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            return Dal.getTable("SP_isSickExist", param);
        }

        public static void insertSick(string id, string positive, string recovery)
        {
            param.Clear();

            p = new SqlParameter("@ID", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@positiveDate", positive);
            p.SqlDbType = SqlDbType.Date;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            if (recovery!="")
            {
                p = new SqlParameter("@recoveryDate", recovery);
                p.SqlDbType = SqlDbType.Date;
                p.Direction = ParameterDirection.Input;
                param.Add(p);

            }

            else
            {
                p = new SqlParameter("@recoveryDate", DBNull.Value);
                p.SqlDbType = SqlDbType.Date;
                p.Direction = ParameterDirection.Input;
                param.Add(p);
            }

            Dal.ExecuteNonQuery("SP_insertSick", param);
        }

        public static void updateRecoveryDate(string id, string recovery)
        {
            param.Clear();

            p = new SqlParameter("@id", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            p = new SqlParameter("@recovery_date", recovery);
            p.SqlDbType = SqlDbType.Date;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            Dal.ExecuteNonQuery("SP_updateRecoveryDate", param);
        }

        public static DataTable getDataById(string id)
        {
            param.Clear();

            p = new SqlParameter("@id", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            return Dal.getTable("SP_getDataByID", param);

        }

        public static int countVaccinations(string id)
        {
            param.Clear();

            p = new SqlParameter("@id", id);
            p.SqlDbType = SqlDbType.NChar;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            string count = Dal.Scalar("SP_countVaccinationById", param).ToString();
            return int.Parse(count);
            
        }

        public static int countSickInDay(string date)
        {
            param.Clear();

            p = new SqlParameter("@date", date);
            p.SqlDbType = SqlDbType.Date;
            p.Direction = ParameterDirection.Input;
            param.Add(p);


            return int.Parse(Dal.Scalar("SP_countSickInDay", param).ToString());
            
        }

        public static DataTable vaccinationsNumber(int num)
        {
            param.Clear();

            p = new SqlParameter("@vaccinations", num);
            p.SqlDbType = SqlDbType.Int;
            p.Direction = ParameterDirection.Input;
            param.Add(p);

            return Dal.getTable("SP_vaccinationsNumber", param);
        }
    }
}
