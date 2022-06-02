using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingADO.NET
{
    
    public class Student
    {
        public int ID { get; set; }
        public string HoTen { get; set; }
        
        public int MaLop { get; set; }

        public Student(int id, string hoten, int malop)
        {
            ID = id;
            HoTen = hoten;
            MaLop = malop;
        }

        public Student(DataRow row)
        {
            ID = Convert.ToInt32(row["ID"]);
            HoTen = row["HoTen"].ToString();
            MaLop = Convert.ToInt32(row["MaLop"]);
        }
        public Student()
        { }
    }
}
