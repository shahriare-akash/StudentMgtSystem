
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;


namespace StudentMgtSystem
{
    class Student
    {
        private string firstName, middleName, lastName, studentId, joiningBatch;

        private int depart, deg;

        public enum dept { 
            ComputerScience = 1,
            BBA,
            English
        }
        
        public enum degree { 
            BSC = 1,
            BBA,
            BA,
            MSC,
            MBA,
            MA
        }

        Dictionary<string, List<Course>> semestersAttended;


        public static List<Student> allStudents = new List<Student>();

        
        public void addToJson(Student obj)
        {
            string pathString = "students.json";
            string fileName = "students.json";
            List<Student> lst = new List<Student>();
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
                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(read);
                    foreach (Student i in lst)
                    {
                        if (obj.ID == i.ID)
                        {
                            Console.WriteLine("User with the same ID already exists");
                            return;
                        }
                    }
                    toAdd = read.Substring(0 , read.Length - 1)  + "," + x + "]"; // string to be contatenated
                }
                else {
                    toAdd = read.Substring(0 , read.Length - 1) + x + "]"; // string to be contatenated
                }

            }

            //Console.WriteLine("Adding " + toAdd);
            StreamWriter fs = new StreamWriter(pathString);
            fs.Write(toAdd);
            fs.Close();
            
            
        }

        public Student()
        {

        }
        public Student(string id, string first, string middle, string last,int dep, int degree_,string batch)
        {
            Student st = new Student();
            st.studentId = id;
            st.firstName = first;
            st.middleName = middle;
            st.lastName = last;
            st.depart = dep;
            st.deg = degree_;
            st.joiningBatch = batch;
            st.ListofSem = new Dictionary<string, List<Course>>();
            addToJson(st);
            allStudents.Add(st);
        }

        public string ID
        {
            get { return this.studentId; }
            set { this.studentId = value; }
        }
        public string First
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }
        public string Mid
        {
            get { return this.middleName; }
            set { this.middleName = value; }
        }
        public string Last
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }
        public int Dept
        {
            get { return this.depart; }
            set { this.depart = value; }
        }
        public int Degree
        {
            get { return this.deg; }
            set { this.deg = value; }
        }

        public string Batch
        {
            get { return this.joiningBatch; }
            set { this.joiningBatch = value; }
        }
        public degree Deg
        {
            get { return (degree)this.deg; }
        }
        public dept Dep
        {
            get { return (dept)this.depart; }
        }

        public Dictionary<string, List<Course>> ListofSem
        {
            get { return this.semestersAttended; }
            set { this.semestersAttended = value; }
        }

    }
}
