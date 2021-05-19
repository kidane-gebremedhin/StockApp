using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ToolBar
{
   
    // Create a class DatePickerFragment  
    public class Date_class
    {
        public static string getToday()
        {
            return DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
        }
        public static int getThisYear()
        {
            return DateTime.Today.Year;
        }
        public static int getThisMonth()
        {
            return DateTime.Today.Month;
        }
        public static int getThisDay()
        {
            return DateTime.Today.Day;
        }
        public static bool first_isAfter(string date1, string date2)
        {
            Console.WriteLine(date1 + "  " + date2);
            Console.WriteLine(date1.Substring(0, date1.IndexOf("/")) + " " + date1.Substring(date1.IndexOf("/") + 1, date1.LastIndexOf("/") - date1.IndexOf("/") - 1) + " " + date1.Substring(date1.LastIndexOf("/") + 1) + "  " + date2.Substring(0, date2.IndexOf("/")) + " " + date2.Substring(date2.IndexOf("/") + 1, date2.LastIndexOf("/") - date2.IndexOf("/") - 1) + " " + date2.Substring(date2.LastIndexOf("/") + 1));
            // /*
            int year1 = int.Parse(date1.Substring(0, date1.IndexOf("/")));
            int month1 = int.Parse(date1.Substring(date1.IndexOf("/") + 1, date1.LastIndexOf("/") - date1.IndexOf("/") -1));
            int day1 = int.Parse(date1.Substring(date1.LastIndexOf("/") + 1));

            int year2 = int.Parse(date2.Substring(0, date2.IndexOf("/")));
            int month2 = int.Parse(date2.Substring(date2.IndexOf("/") + 1, date2.LastIndexOf("/") - date2.IndexOf("/") -1));
            int day2 = int.Parse(date2.Substring(date2.LastIndexOf("/") + 1));
            return year1 >= year2|| year1 >= year2 && month1 >= month2 || year1 >= year2 && month1 >= month2 && day1>=day2;
            //*/
        }

    }

}