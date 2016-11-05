using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobSquad.Services;

namespace BobSquad
{
    class Equipment
    {
        public string EqName { get; }
        public string EqDescription { get; }
        public int EqId { get; }
        public int TypeId { get; }

        public Equipment(DataRow equipment)
        {
            EqId = int.Parse(equipment["Id"].ToString());
            EqName = equipment["Name"].ToString();
            EqDescription = equipment["Description"].ToString();
            TypeId = int.Parse(equipment["Type"].ToString());
        }



    }
}
