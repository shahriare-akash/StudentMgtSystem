using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMgtSystem
{
    class Course
    {
        string courseID, courseName, instructorName;
        int noOfCredits;

        public Course() { 
        
        }

        public Course(string cID, string cName, string intsName, int creds) {
            Course c = new Course();
            c.courseID = cID;
            c.courseName = cName;
            c.instructorName = intsName;
            c.noOfCredits = creds;
            addToJson(c);
        }

        public void addToJson(Course obj)
        {
            string pathString = "courses.json";
            List<Course> lst = new List<Course>();
            string read = null;
            string x = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            var toAdd = "";

            if (System.IO.File.Exists(pathString)) // check if there is a students.json file.
            {
                StreamReader r = new StreamReader(pathString);
                read = r.ReadToEnd();
                //Console.WriteLine("Reading :  "+read);
                r.Close();
                if (read.Length > 2)
                {
                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Course>>(read);
                    foreach (Course c in lst)
                    {
                        if (obj.CID == c.CID)
                        {
                            Console.WriteLine("Course with the same ID already exists");
                            return;
                        }
                    }
                    toAdd = read.Substring(0, read.Length - 1) + "," + x + "]"; // string to be contatenated
                }
                else
                {
                    toAdd = read.Substring(0, read.Length - 1) + x + "]"; // string to be contatenated
                }

            }

            //Console.WriteLine("Adding " + toAdd);
            StreamWriter fs = new StreamWriter(pathString);
            fs.Write(toAdd);
            fs.Close();


        }


        public string CID
        {
            get { return this.courseID; }
            set { this.courseID = value; }
        }        
        public string CName
        {
            get { return this.courseName; }
            set { this.courseName = value; }
        }        
        public string InstrName
        {
            get { return this.instructorName; }
            set { this.instructorName = value; }
        }
        public int Creds
        {
            get { return this.noOfCredits; }
            set { this.noOfCredits = value; }
        }
    }
}
