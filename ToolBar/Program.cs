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
using SQLite;

namespace ToolBar
{
    public class Program
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        public string name { get; set; }
        public string day{ get;set; }
        public string startTime_Hrs { get; set; }
        public string startTime_Mins { get; set; }
        public string startTime_amPm { get; set; }
        public string endTime_Hrs { get; set; }
        public string endTime_Mins { get; set; }
        public string endTime_amPm { get; set; }
        

        public Program(string name, string day, string startTime_Hrs, string startTime_Mins, string startTime_amPm, string endTime_Hrs, string endTime_Mins, string endTime_amPm)
        {
            this.name = name;
            this.day = day;
            this.startTime_Hrs = startTime_Hrs;
            this.startTime_Mins = startTime_Mins;
            this.startTime_amPm = startTime_amPm;
            this.endTime_Hrs = endTime_Hrs;
            this.endTime_Mins = endTime_Mins;
            this.endTime_amPm = endTime_amPm;
        }

        public Program()
        {

        }

        public override string ToString()
        {
            return name + " " + day + " (" + startTime_Hrs.ToString()+":"+ startTime_Mins.ToString()+""+startTime_amPm + " - " + endTime_Hrs.ToString() + ":" + endTime_Mins.ToString() + "" + endTime_amPm + ")";
        }
        
    }
}