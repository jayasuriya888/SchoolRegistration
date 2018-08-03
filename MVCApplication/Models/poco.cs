using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCApplication.Models
{
    public class poco
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public DateTime dob { get; set; }
        public int age { get; set; }
        public int branch_Id { get; set; }
        public int class_Id { get; set; }

    }
}