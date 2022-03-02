using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingProject.Basic
{
    class MethodOverloading
    {
        public MethodOverloading()
        {

        }

        public string DispalyName()
        {
            return "Vikrant";
        }

        public string DispalyName(string FirstChar)
        {
            string Result = string.Empty;
            switch (FirstChar)
            {
                case "R":
                    Result = "Rishi";
                    break;
                case "V":
                    Result = "Vikrant";
                    break;
                case "P":
                    Result = "Pooja";
                    break;
                default:
                    Result = "Default";
                    break;
            }
            return Result;

        }

        public string DispalyName(string FirstChar, int Age)
        {
            string Result = string.Empty;
            switch (FirstChar)
            {
                case "R":
                    Result = "Rishi";
                    break;
                case "V":
                    Result = "Vikrant";
                    break;
                case "P":
                    Result = "Pooja";
                    break;
                default:
                    Result = "Default";
                    break;
            }
            Result = Result + "-" + Age;
            return Result;
        }
    }
}