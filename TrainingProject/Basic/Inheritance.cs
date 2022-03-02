using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Basic
{
    class Animal
    {
        public string Name { get; set; }

        #region Access Specifiers
        private void TestPrivateMethod()
        {

        }
        public void TestPublicMethod()
        {

        }
        protected void TestProtectedMethod()
        {

        }

        #endregion

        public virtual void Display()
        {
            Name = "I am an animal";
        }
    }

    class Dog : Animal
    {
        public void getName()
        {
            Name="My name is" + Name;
        }
    }

    class Cat : Animal
    {
        public override void Display()
        {
            Name="I am an small animal";
        }
    }
}