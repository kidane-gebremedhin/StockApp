using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SQLite;
using ToolBar.Adapters;
using static Android.Widget.AdapterView;
using ToolBar.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Android.Content;

namespace ToolBar.Resources.Fragments
{
    public class Product_Index : SupportFragment
    {
        private ListView ProductListView;
        private Button showBtn, addBtn, printBtn, editBtn, showDeleteBtn, cancelBtn, deleteBtn;
        private long mProductId;
        private float screenHeight = 400;
        private LinearLayout actionOptions_PupUp, deleteConfirm_PupUp;
        public static List<Product> Products;
        public static Product Product;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.Product_Index, container, false);
            ProductListView = view.FindViewById<ListView>(Resource.Id.ProductListView);
            showBtn = view.FindViewById<Button>(Resource.Id.showBtn);
            cancelBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
            deleteBtn = view.FindViewById<Button>(Resource.Id.deleteBtn);
            addBtn = view.FindViewById<Button>(Resource.Id.addBtn);
            printBtn = view.FindViewById<Button>(Resource.Id.printBtn);
        
            editBtn = view.FindViewById<Button>(Resource.Id.editBtn);
            showDeleteBtn = view.FindViewById<Button>(Resource.Id.showDeleteBtn);
            actionOptions_PupUp = view.FindViewById<LinearLayout>(Resource.Id.actionOptions_PupUp);
            deleteConfirm_PupUp = view.FindViewById<LinearLayout>(Resource.Id.deleteConfirm_PupUp);
            
            ProductListView.ItemClick += ProductListView_ItemClick;
            ProductListView.ItemLongClick += ProductListView_ItemLongClick;
            showBtn.Click += ShowBtn_Click;
            addBtn.Click += AddBtn_Click;
            printBtn.Click += PrintBtn_Click;
            
            editBtn.Click += EditBtn_Click;
            showDeleteBtn.Click += ShowDeleteBtn_Click;
            deleteBtn.Click += DeleteBtn_Click;
            cancelBtn.Click += CancelBtn_Click;
            var db = new SQLiteConnection(MainActivity.dbPath);
            //Retrive Programs
            Products = db.Table<Product>()/*.OrderByDescending(p => p.Id)*/.ToList();
            List<Product> ProductList=new List<Product>();
            foreach (var c in Products)
            {
                Product Product = new Product(c.Id, c.name, c.qty);
                ProductList.Add(Product);
            }
            Product_list_adapter adapter = new Product_list_adapter(this.Context, ProductList);
            ProductListView.Adapter = adapter;
            return view;
        }

        private void ShowBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            MainActivity.mainActivity.replaceFragment(new Product_Show());
        }
        private void ShowDeleteBtn_Click(object sender, EventArgs e)
        {
            deleteConfirm_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;
        }
        
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            TestPrint();
        }
        private void AddBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Product_Create());
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            MainActivity.mainActivity.replaceFragment(new Product_Edit());
            //MainActivity.Product_Edit.Edit_Product(mProductId);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Toast.MakeText(this.Context, "  Product Delete Canceled", ToastLength.Short).Show();
        }

        
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            Product.delete(mProductId);
            MainActivity.mainActivity.replaceFragment(new Product_Index());
            Toast.MakeText(this.Context, " Product deleted successfully", ToastLength.Short).Show();
        }

        private void ProductListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            actionOptions_PupUp.TranslationY = screenHeight;// 250;// screenHeight;
            deleteConfirm_PupUp.TranslationY = screenHeight;//250;// screenHeight;
            Console.WriteLine("" + e.Id.ToString());
        }

        private void ProductListView_ItemLongClick(object sender, ItemLongClickEventArgs e){
            mProductId = Products[e.Position].Id;
            Product = Products[e.Position];
            actionOptions_PupUp.TranslationY = screenHeight - 350;// screenHeight-150;

        }
        
        private void TestPrint()
        {
            string dirPath = "/storage/emulated/0/Documents/GGIndustrialEngineering";
            if (Directory.Exists(dirPath))
            {
                string[] files=Directory.GetFiles(dirPath);
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                        File.Delete(files[i]);
                }
                Directory.Delete(dirPath);
            }
            String savePath = "/storage/emulated/0/Documents/GGIndustrialEngineering/Products-"+DateTime.Now.Year+"-"+ DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + " " + DateTime.Now.Minute + " " + DateTime.Now.Second + ".pdf";//Android.OS.Environment.DirectoryDownloads + /*"/GGIndustrialEngineering/Products.pdf"*/;
//            String savePath2 = "/storage/emulated/0/Documents/GGIndustrialEngineering/Products-" + DateTime.Now.Year+"-"+ DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + " " + DateTime.Now.Minute + " " + DateTime.Now.Second + "-copy.pdf";//Android.OS.Environment.DirectoryDownloads + /*"/GGIndustrialEngineering/Products.pdf"*/;
            //Checks Directory exists
            if (File.Exists(savePath) == false)
            {
                Directory.CreateDirectory(dirPath);
                //File.Create(savePath);
            }
            
            FileStream fs = new FileStream(savePath, FileMode.Create);
            //FileStream fs = new FileStream(savePath, FileMode.Create, System.Security.AccessControl.FileSystemRights.FullControl, FileShare.ReadWrite, 1000, FileOptions.SequentialScan);
            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            // Create an instance to the PDF file by creating an instance of the PDF Writer class, using the document and the filestrem in the constructor.

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            //Before we can write to the document, we need to open it.

            // Open the document to enable you to write to the document

            document.Open();
            /*Paragraph p = new Paragraph("");
            p.Add(Resource.Drawable.homeImage);*/
            // Add content
            document.Add(new Paragraph("\n\n"+GetString(Resource.String.AppName) + "\n\n"));
            document.Add(new Paragraph("\t\t\tProduct/Stock Quantity\n"));
            document.Add(new Paragraph("\t\t\t\t\t\tProduct \t\t\t\t\t\tQuantity"));
            for (int i = 0; i < Products.Count; i++)
            {
                document.Add(new Paragraph("\t\t\t\t\t\t" + Products[i].name+ ": \t\t\t" + Products[i].qty.ToString()));
            }

            // Close the document

            document.Close();
            // Close the writer instance

            writer.Close();
            // Always close open file handles explicitly
            fs.Close();

            string message = "File saved to " + savePath;
//            Toast.MakeText(this.Context, message, ToastLength.Long).Show();
            MainActivity.mainActivity.showAlertDialog(message);
            //viewPDF(savePath);
        }

        private void viewPDF2(string savePath6)
        {
            String savePath = "/storage/emulated/0/Documents/GGIndustrialEngineering/Products.pdf";//Android.OS.Environment.DirectoryDownloads + /*"/GGIndustrialEngineering/Products.pdf"*/;
            //var file = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString(), "Products.pdf");
            var uri = Android.Net.Uri.Parse("http://localhost:81/EDMS/documents-play/36");
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetDataAndType(uri, "application/pdf");
            try
            {
                StartActivity(intent);
            }catch(ActivityNotFoundException e)
            {
                Toast.MakeText(this.Context, "Please install Pdf viewer.", ToastLength.Long).Show();
            }
        }


        private void viewPDF(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            //Copy the private file's data to the EXTERNAL PUBLIC location
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            string application = "";
            string extension = System.IO.Path.GetExtension(filePath);
            switch (extension.ToLower())
            {
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }
//            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/report";
            String externalPath = "/storage/emulated/0/Documents/GGIndustrialEngineering/Products-copy.pdf";//Android.OS.Environment.DirectoryDownloads + /*"/GGIndustrialEngineering/Products.pdf"*/;
            File.WriteAllBytes(externalPath, bytes);
            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);
            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            try
            {
               this.Context.StartActivity(intent);
            }
            catch (Exception e)
            {
                Toast.MakeText(this.Context, "No Application Available to View PDF message: "+e.Message+"\n"+e.StackTrace, ToastLength.Long).Show();
            }
        }
    }

    }
