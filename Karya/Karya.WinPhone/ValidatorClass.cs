using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// this class holds various methods to validate the user fields
/// </summary>
namespace Karya.WinPhone
{
    class ValidatorClass
    {
        //static variables to be used
        public static bool isempty;
        public static bool isemail;


        //function to check if the field is empty
        public static bool checkemp(string s)
        {
            if ((s == "")||(s.Length == 0)||(s == " "))
                return false;

            else
                return true;
        }
    }
}
