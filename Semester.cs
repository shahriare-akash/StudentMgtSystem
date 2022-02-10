using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMgtSystem
{   
    class Semester
    {
        private string semCode, year;


        public Semester(string code, string yr) {
            this.semCode = code;
            this.year = yr;
        }

        public string SemCode
        {
            get { return this.semCode; }
            set { this.semCode = value; }
        }

        public string Year
        {
            get { return this.year; }
            set { this.year = value; }
        }
    }
}
