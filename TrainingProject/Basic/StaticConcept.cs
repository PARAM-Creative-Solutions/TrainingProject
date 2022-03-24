using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Basic
{
    public static class StaticConcept
    {

        public static MyClass Obj { get; set; }

        public static int Add(int a,int b)
        {
            int c = a + b;
            return c;
        }

        public static int Sub(int a, int b)
        {
            int c = a - b;
            return c;
        }
    }


    public class MyClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }




    class StartPoint
    {
        public StartPoint()
        {
            MyClass Obj1 = new MyClass();
            Obj1.Id = 2;
            Obj1.Name = "Pooja";

            MyClass Obj2 = new MyClass();
            Obj2.Id = 3;
            Obj2.Name = "Rishi";

            MyClass Obj3 = new MyClass();
            Obj3.Id = 4;
            Obj3.Name = "Mangesh";

            StaticConcept.Obj = new MyClass();
            StaticConcept.Obj.Id = 4;
            StaticConcept.Obj.Name = "Vikrant";
        }

    }


    class EnumDemo
    {

        public enum Names
        {
            Mangesh=5,
            Neha,
            Rishi=8,
            Revati,
            Vikrant,
            Pooja

        }


        public void EnumTetsIF()
        {

            int DbId = 6;

            Names Username = (Names)DbId; // Neha

            
            if(Username==Names.Neha)
            {
                //Neha
            }
            else if(Username==Names.Mangesh)
            {

            }
            else if(Username==Names.Pooja)
            {

            }
            else
            {
                //
            }


        }

        public void EnumTetsSwitch()
        {
            int DbId = 6;

            Names Username = (Names)DbId; // Neha

            switch (Username)
            {
                case Names.Mangesh:
                    //Mangesh
                 
                case Names.Neha:


                    //Neha
                    break;
                case Names.Rishi:
                    break;
                case Names.Revati:
                    break;
                case Names.Vikrant:
                    break;
                case Names.Pooja:
                    break;
                default:
                    break;
            }

        }

    }

}