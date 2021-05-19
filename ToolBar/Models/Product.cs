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
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        public Int64 qty { get; set; }
        public string name { get; set; }


        public Product(Int64 id, string name, Int64 qty=0){
            if (id > 0)//for list adapters 
                this.Id = id;
            this.name= name;
            this.qty= qty;
        }

        public Product()
        {
        }

        public override string ToString()
        {
            return name;
        }





        //LOGIC PART
        public static bool store(Product Product)
        {
            var query = MainActivity.db.Table<Product>().Where(c => c.name == Product.name);
            Product prod = query.Count() > 0 ? query.First() : null;
            if (prod != null)
            {
                Toast.MakeText(MainActivity.mainActivity, "Product already exists!", ToastLength.Short).Show();
                return false;
            }
                //setup a table
                MainActivity.db.CreateTable<Product>();
            //Insert new Program
            MainActivity.db.Insert(Product);
            return true;
        }
        public static bool update(Product Product)
        {
            var query = MainActivity.db.Table<Product>().Where(c => c.name == Product.name && c.Id != Product.Id);
            Product prod = query.Count() > 0 ? query.First() : null;
            if (prod != null)
            {
                Toast.MakeText(MainActivity.mainActivity, "Product already exists!", ToastLength.Short).Show();
                return false;
            }
            //setup a table
            MainActivity.db.CreateTable<Product>();
            //Insert new Program
            MainActivity.db.Update(Product);
            return true;
        }

        public static void delete(long id)
        {
            //setup a table
            MainActivity.db.CreateTable<Product>();
            var query = MainActivity.db.Table<Product>().Where(c => c.Id == id);

            if (query != null)
            {
                foreach (var Product in query.ToList<Product>())
                {
                    MainActivity.db.Delete(Product);
                }
            }
            MainActivity.db.Commit();
        }
        
    }
}