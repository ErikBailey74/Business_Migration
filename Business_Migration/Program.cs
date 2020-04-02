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


        static string contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts_SecondPass;User ID=sa;Password=M3lb0urn3";

        static void Main()
        {
            /*
            FileStream filestream = new FileStream("output.txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            Globals.counter = 0;*/
            //String contact_connectionString = "Data Source=EBAILEY;Initial Catalog=Contacts;Integrated Security=True";
            List<EnerGovSource> contacts = new List<EnerGovSource>();

            using (SqlConnection contact_connection = new SqlConnection(contact_connectionString))
            {
                SqlCommand contact_command = new SqlCommand("GetContactCompanyBusiness", contact_connection);
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
                    source.CODE_IDs = con_rdr["cont_ids"].ToString();
                    source.building_id = con_rdr["building_ids"].ToString();
                    contacts.Add(source);
                }

                contact_connection.Close();
                
            }
            List<Business> businesses = new List<Business>();
            List<Business_Address> business_addresses = new List<Business_Address>();
            //List<Business_Parcel> business_parcels = new List<Business_Parcel>();
            List<Business_Note> business_notes = new List<Business_Note>();



            for (int i = 0; i < contacts.Count; i++)
            {

                ProcessBusiness(contacts[i]);

                /*
                using (SqlConnection bus_con = new SqlConnection(contact_connectionString))
                {
                    SqlCommand bus_command = new SqlCommand("GetBusinessInfo", bus_con);
                    bus_command.CommandType = CommandType.StoredProcedure;

                    bus_command.Parameters.AddWithValue("@refnum", split[0]);

                    bus_con.Open();

                    SqlDataReader bus_rdr = bus_command.ExecuteReader();

                    while (bus_rdr.Read())
                    {
                        Globals.counter = Globals.counter + 1;
                        Business business = new Business()
                        {
                            Business_ID = ReturnFormattedID(Globals.counter),
                            Contact_ID = contacts[i].Contact_ID,
                            Ownership_Type = bus_rdr["Ownership"].ToString(),
                            Location_Type = "Commercial",
                            Business_Status = "Active",
                            District = bus_rdr["district"].ToString(),
                            Open_Date = bus_rdr["open_date"].ToString(),
                            Business_Description = "",
                            Closed_Date = "",
                            Federal_ID_Number = bus_rdr["federal_id_number"].ToString(),
                            State_ID_Number = "",
                            DBA = "",
                            Legacy_Data_Source_Name = "BTR"
                        };
                        businesses.Add(business);
                        Console.WriteLine("New Business Object created with ID: " + ReturnFormattedID(Globals.counter));

                        Business_Address business_address = new Business_Address()
                        {
                            Business_ID = ReturnFormattedID(Globals.counter),
                            Main_Address = true,
                            Address_Type = "Location",
                            Street_Number = bus_rdr["street_number"].ToString(),
                            Pre_Direction = bus_rdr["pre_direction"].ToString(),
                            Street_Name = bus_rdr["street_name"].ToString(),
                            Street_Type = bus_rdr["street_type"].ToString(),
                            Post_Direction = "",
                            Unit_Suite_Number = bus_rdr["unit_suite_number"].ToString(),
                            Address_Line_3 = "",
                            PO_Box = "",
                            City = bus_rdr["city"].ToString(),
                            State_Code = "FL",
                            Zip = bus_rdr["zip"].ToString(),
                            County_Code = "",
                            Country_Code = "",
                            Country_Type = "US",
                            Last_Update_Date = "",
                            Last_Update_User = ""
                        };

                        business_addresses.Add(business_address);

                        Business_Note business_note = new Business_Note()
                        {
                            Business_ID = ReturnFormattedID(Globals.counter),
                            Note_Text = bus_rdr["note_text"].ToString(),
                            Note_User = "",
                            Note_Date = ""
                        };

                        business_notes.Add(business_note);

                        Business_Parcel business_parcel = new Business_Parcel()
                        {
                            Business_ID = ReturnFormattedID(Globals.counter),
                            Parcel_Number = bus_rdr["note_text"].ToString(),
                            Main_Parcel = true
                        };

                        business_parcels.Add(business_parcel);

                        
                    }
                }*/
            }

        }

        static void ProcessBusiness(EnerGovSource source)
        {
            string[] split = source.BTR_IDs.Split(";");
            Console.WriteLine("Processing Business Object with Contact ID of " + source.Contact_ID + " and " + split[0]);

            using (SqlConnection bus_con = new SqlConnection(contact_connectionString))
            {
                SqlCommand bus_command = new SqlCommand("GetBusinessInfo", bus_con);
                bus_command.CommandType = CommandType.StoredProcedure;

                bus_command.Parameters.AddWithValue("@refnum", split[0]);

                bus_con.Open();

                SqlDataReader bus_rdr = bus_command.ExecuteReader();

                while (bus_rdr.Read())
                {
                    Globals.counter = Globals.counter + 1;
                    Business business = new Business()
                    {
                        Business_ID = ReturnFormattedID(Globals.counter),
                        Contact_ID = source.Contact_ID,
                        Ownership_Type = bus_rdr["Ownership"].ToString(),
                        Location_Type = "Commercial",
                        Business_Status = "Active",
                        District = bus_rdr["district"].ToString(),
                        Open_Date = bus_rdr["open_date"].ToString(),
                        Business_Description = "",
                        Closed_Date = "",
                        Federal_ID_Number = bus_rdr["federal_id_number"].ToString(),
                        State_ID_Number = "",
                        DBA = "",
                        Legacy_Data_Source_Name = "BTR"
                    };
                    
                    Console.WriteLine("New Business Object created with ID: " + ReturnFormattedID(Globals.counter));

                    Business_Address business_address = new Business_Address()
                    {
                        Business_ID = ReturnFormattedID(Globals.counter),
                        Main_Address = true,
                        Address_Type = "Location",
                        Street_Number = bus_rdr["street_number"].ToString(),
                        Pre_Direction = bus_rdr["pre_direction"].ToString(),
                        Street_Name = bus_rdr["street_name"].ToString(),
                        Street_Type = bus_rdr["street_type"].ToString(),
                        Post_Direction = "",
                        Unit_Suite_Number = bus_rdr["unit_suite_number"].ToString(),
                        Address_Line_3 = "",
                        PO_Box = "",
                        City = bus_rdr["city"].ToString(),
                        State_Code = "FL",
                        Zip = bus_rdr["zip"].ToString(),
                        County_Code = "",
                        Country_Code = "",
                        Country_Type = "US",
                        Last_Update_Date = "",
                        Last_Update_User = ""
                    };

                   

                    Business_Note business_note = new Business_Note()
                    {
                        Business_ID = ReturnFormattedID(Globals.counter),
                        Note_Text = bus_rdr["note_text"].ToString(),
                        Note_User = "",
                        Note_Date = ""
                    };



                    /*
                    Business_Parcel business_parcel = new Business_Parcel()
                    {
                        Business_ID = ReturnFormattedID(Globals.counter),
                        Parcel_Number = bus_rdr["note_text"].ToString(),
                        Main_Parcel = true
                    };*/

                    /*
                    //Business Additional Fields
                    Business_Additional_Fields business_additional_fields = new Business_Additional_Fields
                    {
                        Business_ID = ReturnFormattedID(Globals.counter),
                        Exemptions = bus_rdr["Exemption"].ToString(),
                        NewHomeBasedBusiness  = bus_rdr["NewHomeBasedBusiness"].ToString(),
                        NotForProfitBusiness = bus_rdr["NotForProfitBusiness"].ToString(),
                        OfAdditionalClassifications = Convert.ToDecimal(bus_rdr["OfAdditionalClassifications"])
                    };*/

                    InsertBusiness(business);
                    InsertBusinessAddress(business_address);
                    // InsertBusinessParcel(business_parcels);
                    InsertBusinessNote(business_note);
                   // InsertBusinessAdditionalFields(business_additional_fields);
                }
            } 
        }

        static void InsertBusiness(Business business)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {
                
                SqlCommand cmd = new SqlCommand("AddBusiness", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@business_id", business.Business_ID);
                cmd.Parameters.AddWithValue("@contact_id", business.Contact_ID);
                cmd.Parameters.AddWithValue("@ownership_type", ReplaceEmpty(business.Ownership_Type));
                cmd.Parameters.AddWithValue("@location_type", ReplaceEmpty(business.Location_Type));
                cmd.Parameters.AddWithValue("@business_status", ReplaceEmpty(business.Business_Status));
                cmd.Parameters.AddWithValue("@district", ReplaceEmpty(business.District));
                cmd.Parameters.AddWithValue("@open_date", ReplaceEmpty(business.Open_Date));
                cmd.Parameters.AddWithValue("@business_description", ReplaceEmpty(business.Business_Description));
                cmd.Parameters.AddWithValue("@closed_date", ReplaceEmpty(business.Closed_Date));
                cmd.Parameters.AddWithValue("@federal_id_number", ReplaceEmpty(business.Federal_ID_Number));
                cmd.Parameters.AddWithValue("@state_id_number", ReplaceEmpty(business.State_ID_Number));
                cmd.Parameters.AddWithValue("@dba", ReplaceEmpty(business.DBA));
                cmd.Parameters.AddWithValue("@legacy_data_source_name", ReplaceEmpty(business.Legacy_Data_Source_Name));

                con.Open();
                int j = cmd.ExecuteNonQuery();
                con.Close();

                if (j >= 1)
                {
                    Console.WriteLine("Sucessfully wrote Business with ID: " + business.Business_ID);
                }
                else
                {
                    Console.WriteLine("Error writing Business with ID: " + business.Business_ID);
                }
                
            }
        }

        static void InsertBusinessAddress(Business_Address business_address)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddBusinessAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@business_id", business_address.Business_ID);
                cmd.Parameters.AddWithValue("@main_address", business_address.Main_Address);
                cmd.Parameters.AddWithValue("@address_type", ReplaceEmpty(business_address.Address_Type));
                cmd.Parameters.AddWithValue("@street_number", ReplaceEmpty(business_address.Street_Number));
                cmd.Parameters.AddWithValue("@pre_direction", ReplaceEmpty(business_address.Pre_Direction));
                cmd.Parameters.AddWithValue("@street_name", ReplaceEmpty(business_address.Street_Name));
                cmd.Parameters.AddWithValue("@street_type", ReplaceEmpty(business_address.Street_Type));
                cmd.Parameters.AddWithValue("@post_direction", ReplaceEmpty(business_address.Post_Direction));
                cmd.Parameters.AddWithValue("@unit_suite_number", ReplaceEmpty(business_address.Unit_Suite_Number));
                cmd.Parameters.AddWithValue("@address_line_3", ReplaceEmpty(business_address.Address_Line_3));
                cmd.Parameters.AddWithValue("@po_box", ReplaceEmpty(business_address.PO_Box));
                cmd.Parameters.AddWithValue("@city", ReplaceEmpty(business_address.City));
                cmd.Parameters.AddWithValue("@state_code", ReplaceEmpty(business_address.State_Code));
                cmd.Parameters.AddWithValue("@zip", ReplaceEmpty(business_address.Zip));
                cmd.Parameters.AddWithValue("@county_code", ReplaceEmpty(business_address.County_Code));
                cmd.Parameters.AddWithValue("@country_code", ReplaceEmpty(business_address.Country_Code));
                cmd.Parameters.AddWithValue("@country_type", ReplaceEmpty(business_address.Country_Type));
                cmd.Parameters.AddWithValue("@last_update_date", ReplaceEmpty(business_address.Last_Update_Date));
                cmd.Parameters.AddWithValue("@last_update_user", ReplaceEmpty(business_address.Last_Update_User));

                con.Open();
                int j = cmd.ExecuteNonQuery();
                con.Close();

                if (j >= 1)
                {
                    Console.WriteLine("Sucessfully wrote Business Address with ID: " + business_address.Business_ID);
                }
                else
                {
                    Console.WriteLine("Error writing Business Address with ID: " + business_address.Business_ID);
                }
                
            }
        }

        static void InsertBusinessParcel(Business_Parcel business_parcel)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {

                SqlCommand cmd = new SqlCommand("AddBusinessParcel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@business_id", business_parcel.Business_ID);
                cmd.Parameters.AddWithValue("@parcel_number", business_parcel.Parcel_Number);
                cmd.Parameters.AddWithValue("@main_parcel", business_parcel.Main_Parcel);


                con.Open();
                int j = cmd.ExecuteNonQuery();
                con.Close();

                if (j >= 1)
                {
                    Console.WriteLine("Sucessfully wrote Business Parcel with ID: " + business_parcel.Business_ID);
                }
                else
                {
                    Console.WriteLine("Error writing Business Parcel with ID: " + business_parcel.Business_ID);
                }
                
            }
        }

        static void InsertBusinessNote(Business_Note business_note)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {

                SqlCommand cmd = new SqlCommand("AddBusinessNote", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@business_id", business_note.Business_ID);
                cmd.Parameters.AddWithValue("@note_text", business_note.Note_Text);
                cmd.Parameters.AddWithValue("@note_user", ReplaceEmpty(business_note.Note_User));
                cmd.Parameters.AddWithValue("@note_date", ReplaceEmpty(business_note.Note_Date));


                con.Open();
                int j = cmd.ExecuteNonQuery();
                con.Close();

                if (j >= 1)
                {
                    Console.WriteLine("Sucessfully wrote Business Note with ID: " + business_note.Business_ID);
                }
                else
                {
                    Console.WriteLine("Error writing Business Note with ID: " + business_note.Business_ID);
                }
                
            }
        }

        static void InsertBusinessAdditionalFields(Business_Additional_Fields business_additional_field)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {

                SqlCommand cmd = new SqlCommand("AddBusinessAdditionalFields", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@business_id", business_additional_field.Business_ID);
                cmd.Parameters.AddWithValue("@Exemptions", business_additional_field.Exemptions);
                cmd.Parameters.AddWithValue("@NewHomeBasedBusiness", business_additional_field.NewHomeBasedBusiness);
                cmd.Parameters.AddWithValue("@NotForProfitBusiness", business_additional_field.NotForProfitBusiness);
                cmd.Parameters.AddWithValue("@OfAdditionalClassifications", business_additional_field.OfAdditionalClassifications);
                

                con.Open();
                int j = cmd.ExecuteNonQuery();
                con.Close();

                if (j >= 1)
                {
                    Console.WriteLine("Sucessfully wrote Business Additional Field with ID: " + business_additional_field.Business_ID);
                }
                else
                {
                    Console.WriteLine("Error writing Business Additional Field with ID: " + business_additional_field.Business_ID);
                }

            }
        }

        /*
        static void InsertBusiness(List<Business> business)
        {

           
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {
                for (int i = 0; i < business.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("AddBusiness", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@business_id", business[i].Business_ID);
                    cmd.Parameters.AddWithValue("@contact_id", business[i].Contact_ID);
                    cmd.Parameters.AddWithValue("@ownership_type", ReplaceEmpty(business[i].Ownership_Type));
                    cmd.Parameters.AddWithValue("@location_type", ReplaceEmpty(business[i].Location_Type));
                    cmd.Parameters.AddWithValue("@business_status", ReplaceEmpty(business[i].Business_Status));
                    cmd.Parameters.AddWithValue("@district", ReplaceEmpty(business[i].District));
                    cmd.Parameters.AddWithValue("@open_date", ReplaceEmpty(business[i].Open_Date));
                    cmd.Parameters.AddWithValue("@business_description", ReplaceEmpty(business[i].Business_Description));
                    cmd.Parameters.AddWithValue("@closed_date", ReplaceEmpty(business[i].Closed_Date));
                    cmd.Parameters.AddWithValue("@federal_id_number", ReplaceEmpty(business[i].Federal_ID_Number));
                    cmd.Parameters.AddWithValue("@state_id_number", ReplaceEmpty(business[i].State_ID_Number));
                    cmd.Parameters.AddWithValue("@dba", ReplaceEmpty(business[i].DBA));
                    cmd.Parameters.AddWithValue("@legacy_data_source_name", ReplaceEmpty(business[i].Legacy_Data_Source_Name));

                    con.Open();
                    int j = cmd.ExecuteNonQuery();
                    con.Close();

                    if (j >= 1)
                    {
                        Console.WriteLine("Sucessfully wrote Business with ID: " + business[i].Business_ID);
                    }
                    else
                    {
                        Console.WriteLine("Error writing Business with ID: " + business[i].Business_ID);
                    }
                }
            }
        }

        static void InsertBusinessAddress(List<Business_Address> business_address)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {
                for (int i = 0; i < business_address.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("AddBusinessAddress", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@business_id", business_address[i].Business_ID);
                    cmd.Parameters.AddWithValue("@main_address", business_address[i].Main_Address);
                    cmd.Parameters.AddWithValue("@address_type", ReplaceEmpty(business_address[i].Address_Type));
                    cmd.Parameters.AddWithValue("@street_number", ReplaceEmpty(business_address[i].Street_Number));
                    cmd.Parameters.AddWithValue("@pre_direction", ReplaceEmpty(business_address[i].Pre_Direction));
                    cmd.Parameters.AddWithValue("@street_name", ReplaceEmpty(business_address[i].Street_Name));
                    cmd.Parameters.AddWithValue("@street_type", ReplaceEmpty(business_address[i].Street_Type));
                    cmd.Parameters.AddWithValue("@post_direction", ReplaceEmpty(business_address[i].Post_Direction));
                    cmd.Parameters.AddWithValue("@unit_suite_number", ReplaceEmpty(business_address[i].Unit_Suite_Number));
                    cmd.Parameters.AddWithValue("@address_line_3", ReplaceEmpty(business_address[i].Address_Line_3));
                    cmd.Parameters.AddWithValue("@po_box", ReplaceEmpty(business_address[i].PO_Box));
                    cmd.Parameters.AddWithValue("@city", ReplaceEmpty(business_address[i].City));
                    cmd.Parameters.AddWithValue("@state_code", ReplaceEmpty(business_address[i].State_Code));
                    cmd.Parameters.AddWithValue("@zip", ReplaceEmpty(business_address[i].Zip));
                    cmd.Parameters.AddWithValue("@county_code", ReplaceEmpty(business_address[i].County_Code));
                    cmd.Parameters.AddWithValue("@country_code", ReplaceEmpty(business_address[i].Country_Code));
                    cmd.Parameters.AddWithValue("@country_type", ReplaceEmpty(business_address[i].Country_Type));
                    cmd.Parameters.AddWithValue("@last_update_date", ReplaceEmpty(business_address[i].Last_Update_Date));
                    cmd.Parameters.AddWithValue("@last_update_user", ReplaceEmpty(business_address[i].Last_Update_User));

                    con.Open();
                    int j = cmd.ExecuteNonQuery();
                    con.Close();

                    if (j >= 1)
                    {
                        Console.WriteLine("Sucessfully wrote Business Address with ID: " + business_address[i].Business_ID);
                    }
                    else
                    {
                        Console.WriteLine("Error writing Business Address with ID: " + business_address[i].Business_ID);
                    }
                }
            }
        }

        static void InsertBusinessParcel(List<Business_Parcel> business_parcel)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {
                for (int i = 0; i < business_parcel.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("AddBusinessParcel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@business_id", business_parcel[i].Business_ID);
                    cmd.Parameters.AddWithValue("@parcel_number", business_parcel[i].Parcel_Number);
                    cmd.Parameters.AddWithValue("@main_parcel", business_parcel[i].Main_Parcel);


                    con.Open();
                    int j = cmd.ExecuteNonQuery();
                    con.Close();

                    if (j >= 1)
                    {
                        Console.WriteLine("Sucessfully wrote Business Parcel with ID: " + business_parcel[i].Business_ID);
                    }
                    else
                    {
                        Console.WriteLine("Error writing Business Parcel with ID: " + business_parcel[i].Business_ID);
                    }
                }
            }
        }

        static void InsertBusinessNote(List<Business_Note> business_note)
        {
            using (SqlConnection con = new SqlConnection(contact_connectionString))
            {
                for (int i = 0; i < business_note.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("AddBusinessNote", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@business_id", business_note[i].Business_ID);
                    cmd.Parameters.AddWithValue("@note_text", ReplaceEmpty(business_note[i].Note_Text));
                    cmd.Parameters.AddWithValue("@note_user", ReplaceEmpty(business_note[i].Note_User));
                    cmd.Parameters.AddWithValue("@note_date", ReplaceEmpty(business_note[i].Note_Date));


                    con.Open();
                    int j = cmd.ExecuteNonQuery();
                    con.Close();

                    if (j >= 1)
                    {
                        Console.WriteLine("Sucessfully wrote Business Note with ID: " + business_note[i].Business_ID);
                    }
                    else
                    {
                        Console.WriteLine("Error writing Business Note with ID: " + business_note[i].Business_ID);
                    }
                }
            }
        }*/

        static string ReturnFormattedID(int id)
        {
            string output = "";
            int zeros = 9 - id.ToString().Length;

            for (int i = 0; i < zeros; i++)
            {
                output = output + "0";
            }

            return "ID-" + output + id.ToString();
        }

        static object ReplaceEmpty(string input)
        {
            if (input == " ")
            {
                return (object)DBNull.Value;
            }
            else
            {
                return !string.IsNullOrEmpty(input) ? input : (object)DBNull.Value;
            }

        }
    }

}
