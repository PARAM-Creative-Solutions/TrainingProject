using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Basic
{
    public class Collections
    {


        public static void Test()
        {
            List<Users> Users = MyData.Users;

            Users user = Users.Where(x => x.UserId == (int)EnumDemo.Names.Mangesh && x.Amount==7000).FirstOrDefault();

            Double totalAmountOfAllUsers = 0;

            foreach (Users item in Users)
            {
                totalAmountOfAllUsers = totalAmountOfAllUsers + item.Amount;

            }

            totalAmountOfAllUsers = Users.Sum(x => x.Amount);

            Users= Users.OrderBy(x => x.UserId).ToList();

            Users = Users.OrderByDescending(x => x.UserId).ToList();

            #region Non-Generic

            ArrayList list = new ArrayList();

            list.Add("abc");
            list.Add(7);
            list.Add(DateTime.Now);

            Hashtable ht = new Hashtable();
            ht.Add("1dfsdf", "Pooja");
            ht.Add("Length", 20);
            ht.Add("Width", 10);

            SortedList sortedList = new SortedList();
            sortedList.Add("C#", "C#.net");
            sortedList.Add("vb", "vb.net");
            sortedList.Add("mvc", "Mvc");
            sortedList.Add("asp", user);

            Stack stk = new Stack(); //LIFO
            stk.Push(2);
            stk.Push("vb.net");
            stk.Push("asp.net");
            stk.Push(user);

            Queue q = new Queue(); //FIFO
            q.Enqueue("cs.net");
            q.Enqueue("vb.net");
            q.Enqueue("asp.net");
            q.Enqueue(user);

            #endregion

            #region Generic

            #region List

            List<int> ListOfint = new List<int>();
            ListOfint.Add(20);
            ListOfint.Add(10);

            List<string> ListOfNames = new List<string>();
            ListOfNames.Add("Vik");
            ListOfNames.Add("XYZ");
            ListOfNames.Add("PQR");

            List<Emp> AllEmp = new List<Emp>();

            Emp emp1 = new Emp();
            emp1.EmpId = 2;
            emp1.EmpName = "xyz";


            Emp emp2 = new Emp()
            {
                EmpId = 3,
                EmpName = "pqr"
            };

            AllEmp.Add(emp1);
            AllEmp.Add(emp2);
            AllEmp.Add(new Emp() { EmpId = 1, EmpName="Vikrant" });

            #endregion

            #region Dictonary

            Dictionary<char, string> MyDic = new Dictionary<char, string>();
            MyDic.Add('A', "Akash");
            MyDic.Add('A', "ABC");

            Dictionary<string, string> MyStringDic = new Dictionary<string, string>();
            MyStringDic.Add("A", "ABC");



            Dictionary<char, MyDictionary> Words = new Dictionary<char, MyDictionary>();

            MyDictionary word1 = new MyDictionary()
            {
                EngWord = "Pen",
                MarWord = "Nib"
            };

            MyDictionary word2 = new MyDictionary();
            word2.EngWord = "Distance";
            word2.MarWord = "Antara";

            Words.Add('P', word1);
            Words.Add('A', word2);

            List<KeyValuePair<char, MyDictionary>> AllWordswithA = Words.Where(w => w.Key == 'A').ToList();


            #endregion

            #region HashSet

            HashSet<string> has = new HashSet<string>();

            has.Add("vik");
            has.Add("Mangesh");
            has.Add("vik");
            has.Add("Rishi");

            #endregion

            #region SortedList
            SortedList<string, string> sortedlist = new SortedList<string, string>();


            #endregion

            #region Stack

            Stack<int> intStack = new Stack<int>();
            intStack.Push(10);

            Stack<string> StringStack = new Stack<string>();

            Stack<MyDictionary> MyDictionaryStack = new Stack<MyDictionary>();

            #endregion

            #region Queue

            Queue<int> QuInt = new Queue<int>();

            Queue<string> Qustring = new Queue<string>();

            Queue<MyDictionary> QuMydata = new Queue<MyDictionary>();

            #endregion

            #endregion

        }

    }


    class MyDictionary
    {
        public string EngWord { get; set; }
        public string MarWord { get; set; }

    }



    class Emp
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }

    }



    class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public double Amount { get; set; }
    }


    class MyData
    {
        private static List<Users> _users { get; set; }
        public static List<Users> Users
        {
            get
            {
                _users = new List<Users>();
                _users.Add(new Users() { UserId = 1, UserName = "Mangesh", Amount = 2000 });

                _users.Add(new Users() { UserId = 2, UserName = "Rishi", Amount = 3000 });

                _users.Add(new Users() { UserId = 3, UserName = "Neha", Amount = 5000 });

                _users.Add(new Users() { UserId = 4, UserName = "Revati", Amount = 6000 });
                _users.Add(new Users() { UserId = 5, UserName = "Pooja", Amount = 7000 });
                _users.Add(new Users() { UserId = 6, UserName = "Vikrant", Amount = 8000 });
                return _users;
            }
        }

    }
}