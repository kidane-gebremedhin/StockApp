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
    public class Receivables
    {
        //item Part
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        public Int64 productId { get; set; }
        public Int64 qty { get; set; }
        public DateTime date { get; set; }

        public Receivables(Int64 id, Int64 productId, Int64 qty, DateTime date)
        {   
            var query = MainActivity.db.Table<Product>().Where(i => i.Id == productId);
            var productObj = query.Count()>0? query.First():null;
            if (id > 0)//for list adapters 
                this.Id = id;
            this.productId = productId;
            this.qty = qty;
            this.date = date;

        }

        public Receivables()
        {
        }

        public override string ToString()
        {
            return "";
        }





        //LOGIC PART
        public static bool store(Receivables item)
        {

            item.date=DateTime.Now;
            //setup a table
            MainActivity.db.CreateTable<Receivables>();
            MainActivity.db.Insert(item);
            //Update Stock Qty
            var query = MainActivity.db.Table<Product>().Where(c => c.Id == item.productId);
            Product prod = query.Count() > 0 ? query.First() : null;
            if (prod != null)
            {
                prod.qty = prod.qty + item.qty;
                Product.update(prod);
            }
            

            return true;
        }
        public static bool update(Receivables item)
        {
            //keep old receivabled qty 
            var query2 = MainActivity.db.Table<Receivables>().Where(c => c.Id == item.Id);
            Receivables old_receivable = query2.Count() > 0 ? query2.First() : null;

            //setup a table
            MainActivity.db.CreateTable<Receivables>();
            //Insert new Program
            MainActivity.db.Update(item);

            //Update Stock Qty
            var query = MainActivity.db.Table<Product>().Where(c => c.Id == item.productId);
            Product prod = query.Count() > 0 ? query.First() : null;
            if (prod != null && old_receivable != null)
            {
                prod.qty = prod.qty + (item.qty - old_receivable.qty);
                Product.update(prod);
            }

            return true;
        }

        public static void delete(long id)
        {
            //setup a table
            MainActivity.db.CreateTable<Receivables>();
            var query = MainActivity.db.Table<Receivables>().Where(u => u.Id == id);

            if (query != null)
            {
                foreach (var item in query.ToList<Receivables>())
                {
                    MainActivity.db.Delete(item);
                }
            }
            MainActivity.db.Commit();
        }

    }
}