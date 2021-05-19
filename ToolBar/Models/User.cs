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
using System.IO;
using SQLite;

namespace ToolBar.Models
{
    public class User
    {
        //User Part
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        public string role { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string registrationDate { get; set; }
        public string status { get; set; }

        
        public User(Int64 id, string role, string userName, string password, string firstName, string lastName, string phoneNumber, string email, string address){
            if (id > 0)//for list adapters 
                this.Id = id;
            this.role = role;
            this.userName= userName;
            this.password= password;
            this.firstName= firstName;
            this.lastName= lastName;
            this.phoneNumber= phoneNumber;
            this.email= email;
            this.address= address;
        }

        public User()
        {
        }

        public override string ToString()
        {
            return role + " " + userName + " " + password + " " + firstName + " " + lastName + " " + phoneNumber + " " + email + " " + address;
        }





        //LOGIC PART
        public static bool store(User user)
        {
            //setup a table
            MainActivity.db.CreateTable<User>();
            //Insert new Program
            MainActivity.db.Insert(user);
            return true;
        }
        public static bool update(User user)
        {
            //setup a table
            MainActivity.db.CreateTable<User>();
            //Insert new Program
            MainActivity.db.Update(user);
            return true;
        }

        public static void delete(long id)
        {
            var query = MainActivity.db.Table<User>().Where(u => u.Id == id);

            if (query != null)
            {
                foreach (var user in query.ToList<User>())
                {
                    MainActivity.db.Delete(user);
                }
            }
            MainActivity.db.Commit();
        }

    }
}