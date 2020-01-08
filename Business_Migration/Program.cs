using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Business_Migration
{
    class Program
    {

        static String contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts;Integrated Security=True";

        static void Main()
        {
            Globals.counter = 0;
            //String contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts;Integrated Security=True";
            List<EnerGovSource> contacts = new List<EnerGovSource>();

            using (SqlConnection contact_connection = new SqlConnection(contact_connectionString))
            {
                SqlCommand contact_command = new SqlCommand("GetContactCompany", contact_connection);
                contact_command.CommandType = CommandType.StoredProcedure;

                contact_connection.Open();

                SqlDataReader con_rdr = contact_command.ExecuteReader();

                while (con_rdr.Read())
                {
                    EnerGovSource source = new EnerGovSource();
                    source.Contact_ID = con_rdr["contact_id"].ToString();
                    source.Company_Name = con_rdr["company_name"].ToString();
                    source.BTR_IDs = con_rdr["btr_ids"].ToString();
                    source.ENG_IDs = con_rdr["eng_ids"].ToString();
                    source.CODE_IDs = con_rdr["code_ids"].ToString();
                    contacts.Add(source);
                }

                contact_connection.Close();
                
            }

            for(int i = 0; i <= contacts.Count; i++)
            {

            }
        }

    }
}
