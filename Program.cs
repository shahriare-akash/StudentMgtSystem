using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;


namespace StudentMgtSystem
{   
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Select a level");
            //string abc = Console.ReadLine();
            //Level myVar = (Level)Convert.ToInt32(abc);
            //Console.WriteLine(myVar);


            LoadJson();

            Console.WriteLine("----------------------------------");
            Student s1 = new Student("S01", "Mohammad", "Saqibul", "Alam", 1, 1, "Summer 2017");
            LoadJson();
            Student s2 = new Student("S02", "Mohammad", "Ajmain", "Alam", 2, 1, "Spring 2017");
            LoadJson();
            Student s3 = new Student("S03", "Kawser", "Ibna", "Raihan", 3, 1, "Summer 2017");

            LoadJson();

            Console.WriteLine("\nWhat would you like to do \n1)Add new Student \n2)View Student details \n3)Delete Student \n4)Add Semester");
            string firstChoice = Console.ReadLine();

            if (firstChoice == "1")
            {
                addStudent();
            }
            else if (firstChoice == "2")
            {
                Console.WriteLine("Please enter the ID of the user you would like to see");
                string id = Console.ReadLine();
                getUser(id);
            }
            else if (firstChoice == "3")
            {
                Console.WriteLine("Please enter the ID of the user you would like to see");
                string id = Console.ReadLine();
                deleteUser(id);
                LoadJson();
            }
            else if (firstChoice == "4")
            {
                Console.WriteLine("Please select the ID of the student you would like add a semester to");
                string id = Console.ReadLine();
                addSemester(id);
            }
            else
            {
                Console.WriteLine("Not a valid option!");
            }

        }

        public static void addSemester(string id) {
            Student s = getUserSemester(id);//get student by id

            if (s == null) {
                return;
            }

            Console.WriteLine("Please enter the semester code");
            string semCode = Console.ReadLine();

            Console.WriteLine("Please enter the semester year");
            string semYear = Console.ReadLine();

            Semester newSem = new Semester(semCode,semYear);//create semester object using the user inputs
            Console.WriteLine(newSem.SemCode + newSem.Year);
            string semName = newSem.SemCode + " " + newSem.Year;


            Dictionary<string, List<Course>> semCourseDict = s.ListofSem; // get the semester information of the student as a dictionary

            List<Course> currentSemCourse = new List<Course>();// create an empty list for current semester course

            if (semCourseDict.ContainsKey(semName))
            {
                currentSemCourse = semCourseDict[semName];//if there are already existing courses in the semester, retrieve it
            }

            showCourses();

            Console.WriteLine("Please write the Course Code that you would like to add to the student");
            string code = Console.ReadLine();

            Course courseToAdd = getCourse(code);//get course to add to dictionary

            foreach (Course c in currentSemCourse) { 
                if (c.CID == courseToAdd.CID)//check if the course already exists in the list
                {
                    Console.WriteLine("The course is already enrolled for " + newSem);
                    return;
                }
            }
            
            

            
            currentSemCourse.Add(courseToAdd);
            //semCourseDict[newSem] = currentSemCourse;
            semCourseDict.Add(semName,currentSemCourse);
            s.ListofSem = semCourseDict;
            //update the student in JSON

            updateStudent(s);

            foreach (KeyValuePair<string, List<Course>> entry in semCourseDict) {
                Console.WriteLine("Courses for " + entry.Key + " are");
                foreach (Course c in entry.Value) {
                    Console.WriteLine(c.CID + "(" + c.CName + ")");
                } 
            }
            //Console.WriteLine("The courses that " + s.First + " " + s.Mid + " " + s.Last + " is enrolled in " +semCode +" are: ");
            //showCoursesinSemester(s);


            

        }

        public static void updateStudent(Student obj) {
            string pathString = "students.json";
            string read = null;
            List<Student> lst = new List<Student>();
            string x = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            string currentData = "";
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
                            //update
                            //rewrite existing json obj with x
                            currentData = Newtonsoft.Json.JsonConvert.SerializeObject(i);
                            break;
                        }
                    }
                }

            }
            string newView = read.Replace(currentData,x);

            StreamWriter fs = new StreamWriter(pathString);
            fs.Write(newView);
            fs.Close();

        }
        public static Course? getCourse(string code)
        {
            string fileName = "courses.json";
            List<Course> ls = new List<Course>();
            string read = null;
            if (System.IO.File.Exists(fileName))
            {
                StreamReader r = new StreamReader(fileName);
                read = r.ReadToEnd();
                r.Close();
                if (read.Length > 5)
                {
                    ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Course>>(read);
                    foreach (Course c in ls)
                    {
                        if (c.CID == code) {
                            return c;
                        }
                    }
                    return null;
                }
                else
                {
                    Console.WriteLine("No courses at the moment");
                    StreamReader r_ = new StreamReader(fileName);
                    read = r_.ReadToEnd();
                    Console.WriteLine(read.Length);
                    r_.Close();
                    return null;
                }
            }
            else {
                Console.WriteLine("No such course");
                return null;
            }
        }

        public static void addStudent() {
            string fName, mName, lName, sID, jBatch;
            int dept, degree;
            Console.WriteLine("Please write your first name: ");
            fName = Console.ReadLine();
            Console.WriteLine("Please write your middle name: ");
            mName = Console.ReadLine();
            Console.WriteLine("Please write your last name: ");
            lName = Console.ReadLine();
            Console.WriteLine("Please write your student ID: ");
            sID = Console.ReadLine();
            Console.WriteLine("Please write your joining batch: ");
            jBatch = Console.ReadLine();
            Console.WriteLine("Please select the number that corresponds to your department:\n1) Computer Science\n2) BBA\n3) English ");
            dept = int.Parse(Console.ReadLine());
            if (dept != 1 | dept != 2 | dept != 3) {
                Console.WriteLine("Not a valid Input!");
                return;
            }
            Console.WriteLine("Please select the number that corresponds to your degree:\n1) BSC\n2) BBA\n3) BS\n4) MSC\n5) MBA\n6) MA ");
            degree = int.Parse(Console.ReadLine());
            if (degree != 1 | degree != 2 | degree != 3 | degree != 4 | degree != 5 | degree != 6)
            {
                Console.WriteLine("Not a valid Input!");
                return;
            }

            Student st = new Student(sID,fName,mName,lName,dept,degree,jBatch);
        }

        public static void getUser(string id) {


            string pathString = "students.json";
            string fileName = "students.json";
            List<Student> lst = new List<Student>();
            string read = null;

            if (System.IO.File.Exists(pathString)) // check if there is a students.json file.
            {
                StreamReader r = new StreamReader(pathString);
                read = r.ReadToEnd();
                //Console.WriteLine("Reading :  " + read);
                r.Close();
                if (read.Length > 2)
                {
                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(read);
                    foreach (Student i in lst)
                    {
                        if (id == i.ID)
                        {
                            Console.WriteLine(i.First + " " + i.Mid + " " + i.Last + " is pursuing " + i.Deg + " in " + i.Dep + ". They joined in " + i.Batch); 
                            return;
                        }
                    }
                    Console.WriteLine("No student with the given ID exists");
                }
            }
            else {
                Console.WriteLine("There are no students at the moment");
            }
        }


        public static Student? getUserSemester(string id)
        {


            string pathString = "students.json";
            string fileName = "students.json";
            List<Student> lst = new List<Student>();
            string read = null;

            if (System.IO.File.Exists(pathString)) // check if there is a students.json file.
            {
                StreamReader r = new StreamReader(pathString);
                read = r.ReadToEnd();
                //Console.WriteLine("Reading :  " + read);
                r.Close();
                if (read.Length > 2)
                {
                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(read);
                    foreach (Student i in lst)
                    {
                        if (id == i.ID)
                        {
                            //Console.WriteLine(i.First + " " + i.Mid + " " + i.Last + " is pursuing " + i.Deg + " in " + i.Dep + ". They joined in " + i.Batch);
                            return i;
                        }
                    }
                    Console.WriteLine("No student with the given ID exists");
                    return null;
                }
                return null;
            }
            else
            {
                Console.WriteLine("There are no students at the moment");
                return null;
            }
        }


        public static void deleteUser(string id) {

            Console.WriteLine("User to be deleted is " + id);

            string pathString = "students.json";
            string fileName = "students.json";
            List<Student> lst = new List<Student>();
            string read = null;

            var updated = "";
            var x = "";
            if (System.IO.File.Exists(pathString)) // check if there is a students.json file.
            {
                StreamReader r = new StreamReader(pathString);
                read = r.ReadToEnd();
                r.Close();
                int flag = 0;
                if (read.Length > 2)
                {
                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(read);
                    foreach (Student i in lst)
                    {
                        if (id == i.ID)
                        {
                            x = Newtonsoft.Json.JsonConvert.SerializeObject(i);

                            if ((x.Length + 2) == read.Length)
                            {
                                updated = read.Replace(read.Substring(read.IndexOf(x), x.Length), "");
                            }
                            else if (read[read.IndexOf(x) - 1] == ',')
                            {
                                updated = read.Replace(read.Substring(read.IndexOf(x) - 1, x.Length + 1), "");
                            }
                            else {
                                updated = read.Replace(read.Substring(read.IndexOf(x) , x.Length + 1), "");
                            }
                            flag = 1;
                        }
                    }

                }

                if (flag == 0)
                {
                    Console.WriteLine("No such user");
                }
                else {
                    Console.WriteLine(updated);
                    Console.WriteLine("Removing  " + x);
                    StreamWriter fs = new StreamWriter(pathString);
                    fs.Write(updated);
                    fs.Close();
                }               
            }
            Console.WriteLine("----------------End of Delete User------------------------");
        }

        public static void LoadJson() {
            Console.WriteLine("---------------------STUDENTS----------------------");
            string fileName = "students.json";
            List<Student> ls = new List<Student>();
            string read = null;
            if (System.IO.File.Exists(fileName))
            {
                StreamReader r = new StreamReader(fileName);
                read = r.ReadToEnd();
                r.Close();
                if (read.Length > 5)
                {
                    //Console.WriteLine("There is already a file");
                    ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Student>>(read);
                    Console.WriteLine("The students currently enrolled are ");
                    foreach (Student st in ls)
                    {
                        Console.WriteLine(st.First + " " + st.Mid + " " + st.Last + " is pursuing " + st.Deg + " in " + st.Dep);
                    }
                }
                else
                {
                    Console.WriteLine("No students at the moment");
                    StreamReader r_ = new StreamReader(fileName);
                    read = r_.ReadToEnd();
                    Console.WriteLine(read.Length);
                    r_.Close();
                }
                
            }
            else {
                Console.WriteLine("Created Json File");
                FileStream _r = File.Create(fileName);
                byte[] bdata = Encoding.Default.GetBytes("[]");
                _r.Write(bdata, 0, bdata.Length);
                _r.Close();
                Console.WriteLine("No students at the moment");
                StreamReader r_ = new StreamReader(fileName);
                read = r_.ReadToEnd();
                Console.WriteLine(read);
                r_.Close();
                
            }
            Console.WriteLine("----------------End of LoadJson------------------------");
        }

        public static void showCourses() {
            Console.WriteLine("---------------------COURSES----------------------");
            string fileName = "courses.json";
            List<Course> ls = new List<Course>();
            string read = null;
            if (System.IO.File.Exists(fileName))
            {
                StreamReader r = new StreamReader(fileName);
                read = r.ReadToEnd();
                r.Close();
                if (read.Length > 5)
                {
                    //Console.WriteLine("There is already a file");
                    ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Course>>(read);
                    Console.WriteLine("The available courses are ");
                    foreach (Course c in ls)
                    {
                        Console.WriteLine(c.CID + " (" + c.CName + ") is taught by " + c.InstrName + ". It has " + c.Creds + " credits");
                    }
                }
                else
                {
                    Console.WriteLine("No courses at the moment");
                    StreamReader r_ = new StreamReader(fileName);
                    read = r_.ReadToEnd();
                    Console.WriteLine(read.Length);
                    r_.Close();
                }

            }
            else
            {
                Console.WriteLine("Created Json File");
                FileStream _r = File.Create(fileName);
                byte[] bdata = Encoding.Default.GetBytes("[]");
                _r.Write(bdata, 0, bdata.Length);
                _r.Close();
                Console.WriteLine("No courses at the moment");
                StreamReader r_ = new StreamReader(fileName);
                read = r_.ReadToEnd();
                Console.WriteLine(read);
                r_.Close();

            }
            Console.WriteLine("----------------End of ShowCourse------------------------");
        }

        public static void showCoursesinSemester(Student s)
        {
            Dictionary<string, List<Course>> c = s.ListofSem;


            foreach (KeyValuePair<string, List<Course>> entry in c)
            {
                Console.WriteLine("The courses for " + entry.Key +  " are: ");
                if(entry.Value != null)
                {
                    foreach (Course ls in entry.Value) {
                        Console.WriteLine(ls.CID + "(" + ls.CName + ")");
                    }
                }
                
            }
        }
    }
}
