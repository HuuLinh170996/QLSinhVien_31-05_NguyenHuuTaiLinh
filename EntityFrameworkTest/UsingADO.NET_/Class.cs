using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingADO.NET
{
    public class Class
    {
        public int ID { get; set; }
        public string TenLop { get; set; }

        public Class(int id, string tenlop)
        {
            ID = id;
            TenLop = tenlop;
        }

        public Class(DataRow row)
        {
            ID = Convert.ToInt32(row["ID"]);
            TenLop = row["TenLop"].ToString();
        }

    }
}
