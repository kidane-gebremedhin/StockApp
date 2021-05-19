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
    public class Issuables
    {
        //item Part
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        public Int64 productId { get; set; }
        public Int64 qty { get; set; }
        public DateTime date { get; set; }

        public Issuables(Int64 id, Int64 productId, Int64 qty, DateTime date)
        {   
            var query = MainActivity.db.Table<Product>().Where(i => i.Id == productId);
            var productObj = query.Count()>0? query.First():null;
            if (id > 0)//for list adapters 
                this.Id = id;
            this.productId = productId;
            this.qty = qty;
            this.date = date;

            if (this.date == null)
                this.date = DateTime.Now;
        }

        public Issuables()
        {
        }

        public override string ToString()
        {
            return "";
        }





        //LOGIC PART
        public static bool store(Issuables item)
        {
            item.date = DateTime.Now;

            //Update Stock Qty
            var query = MainActivity.db.Table<Product>().Where(c => c.Id == item.productId);
            Product prod = query.Count() > 0 ? query.First() : null;
            if (prod != null)
            {
                if(prod.qty - item.qty < 0)
                {
                    Toast.MakeText(MainActivity.mainActivity, "Quantity is out of stock!", ToastLength.Short).Show();
                    return false;
                }
                //setup a table
                MainActivity.db.CreateTable<Issuables>();
                MainActivity.db.Insert(item);

                prod.qty = prod.qty - item.qty;
                Product.update(prod);
            }
            

            return true;
        }
        public static bool update(Issuables item)
        {
            //keep old Issuabled qty 
            var query2 = MainActivity.db.Table<Issuables>().Where(c => c.Id == item.Id);
            Issuables old_Issuable = query2.Count() > 0 ? query2.First() : null;

            //Update Stock Qty
            var query = MainActivity.db.Table<Product>().Where(c => c.Id == item.productId);
            Product prod = query.Count() > 0 ? query.First() : null;
            if (prod != null && old_Issuable != null)
            {
                if (prod.qty - (item.qty - old_Issuable.qty) < 0)
                {
                    Toast.MakeText(MainActivity.mainActivity, "Quantity is out of stock!", ToastLength.Short).Show();
                    return false;
                }

                //setup a table
                MainActivity.db.CreateTable<Issuables>();
                //Insert new Program
                MainActivity.db.Update(item);

                prod.qty = prod.qty - (item.qty - old_Issuable.qty);
                Product.update(prod);
            }

            return true;
        }

        public static void delete(long id)
        {
            //setup a table
            MainActivity.db.CreateTable<Issuables>();
            var query = MainActivity.db.Table<Issuables>().Where(u => u.Id == id);

            if (query != null)
            {
                foreach (var item in query.ToList<Issuables>())
                {
                    MainActivity.db.Delete(item);
                }
            }
            MainActivity.db.Commit();
        }

    }
}