using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebGameService.Models
{
    public class DataBaseManager
    {
        private SqlConnection SqlConnection { get; set; }

        public DataBaseManager()
        {
            SqlConnection = new SqlConnection(@"Data Source = CO - YAR - WS100\SQLEXPRESS; Initial Catalog = GameStatisticDb; User ID = sa; Password = firm;");
        }

        public void GetWholeInformation()
        {
            using (SqlConnection)
            {
                SqlCommand command = new SqlCommand(
                    "SELECT All FROM GameStatisticDb",SqlConnection);
                SqlDataReader dataReader = command.ExecuteReader();
                List<object> dataList = new List<object>();
                while (dataReader.Read())
                {
                    dataList.Add(dataReader.GetString(0));

                }
                dataReader.Close();
            }
        }
    }
}