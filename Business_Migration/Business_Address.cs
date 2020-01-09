using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Migration
{
    class Business_Address
    {
        public string Business_ID { get; set; }
        public bool Main_Address { get; set; }
        public string Address_Type { get; set; }
        public string Street_Number { get; set; }
        public string Pre_Direction { get; set; }
        public string Street_Name { get; set; }
        public string Street_Type { get; set; }
        public string Post_Direction { get; set; }
        public string Unit_Suite_Number { get; set; }
        public string Address_Line_3 { get; set; }
        public string PO_Box { get; set; }
        public string City { get; set; }
        public string State_Code { get; set; }
        public string Zip { get; set; }
        public string County_Code { get; set; }
        public string Country_Code { get; set; }
        public string Country_Type { get; set; }
        public string Last_Update_Date { get; set; }
        public string Last_Update_User { get; set; }
    }
}
