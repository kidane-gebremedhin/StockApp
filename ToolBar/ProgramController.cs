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
namespace ToolBar
{
    class ProgramController
    {
       private static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dbTest_.db3");

        public static bool storeProgram(Program program)
        {
            //initialize db
            var db = new SQLiteConnection(dbPath);
            //setup a table
            db.CreateTable<Program>();
            //Insert new Program
            db.Insert(program);
            return true;
        }
        public static bool updateProgram(Program program)
        {
            //initialize db
            var db = new SQLiteConnection(dbPath);
            //setup a table
            db.CreateTable<Program>();
            //Insert new Program
            db.Update(program);
            return true;
        }

        public static void delete(long id)
        {
            //DateTime expireDate = DateTime.Now.AddDays(-3);
            var db = new SQLiteConnection(dbPath);
            var query = db.Table<Program>().Where(program => program.Id==id);

            if (query != null)
            {
                foreach (var prog in query.ToList<Program>()) {
                    Console.WriteLine("deleted: "+prog.name + " - "+ prog.Id.ToString());
                    db.Delete(prog);
                    //db.Delete<Program>(prog.Id);
                }
            }
            db.Commit();
        }
    }
}