using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Lab5
{
    public partial class PhotosAdmin : System.Web.UI.Page
    {
        int SleepTime = 2000;
        List<PhotoField> images;
        string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
        int getMaxNumber() => (images.Count>0)?images[images.Count - 1].Number:0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Default.auth)
            {
                Response.Redirect("~/Default.aspx");
            }

            string GalID = Request.QueryString["ID"];

            Label1.Text = galleryName(int.Parse(GalID));

            images = new List<PhotoField>();
            string QueryText = $"select * from Picture_List where Gallery_ID={int.Parse(GalID)}";
            
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(QueryText, Conn);
                SqlDataReader R = Comm.ExecuteReader();
                while (R.Read())
                {
                    images.Add(new PhotoField(R.GetInt32(0),R.GetInt32(1),R.GetInt32(2),R[3].ToString(),R[4].ToString(),R[5].ToString()));
                }
                images.Sort((x,y)=>PhotoField.Compare(x,y));
                Table1.Width = 200 + 3*PhotoField.ButtonsWidth;

                for (int i = 0; i < images.Count; i++)
                {
                    TableRow tr = new TableRow();
                    tr.Width = Table1.Width;

                    TableCell tc1 = new TableCell();
                    images[i].pic.Command += new CommandEventHandler(redirect);
                    tc1.Controls.Add(images[i].pic);
                  
                    tr.Cells.Add(tc1);

                    TableCell td2 = new TableCell();
                    images[i].Up.Command += new CommandEventHandler(upRecord);

                    td2.Controls.Add(images[i].Up);
                    tr.Cells.Add(td2);

                    TableCell td3 = new TableCell();
                    images[i].Down.Command += new CommandEventHandler(downRecord);
                    td3.Controls.Add(images[i].Down);
                    tr.Cells.Add(td3);

                    TableCell td4 = new TableCell();
                    images[i].Delete.Command += new CommandEventHandler(deleteRecord);
                    td4.Controls.Add(images[i].Delete);
                    tr.Cells.Add(td4);
                    Table1.Rows.Add(tr);
                }
                
            }
        }


        void FixPictureNumber(int Number)
        {
            int galID = int.Parse(Request.QueryString["ID"]);
            for (int i = Number; i <= getMaxNumber(); i++)
            {
                string queryUp = $"update Picture_List set Picture_Number=Picture_Number-1 where Picture_Number={i} AND Gallery_ID={galID}";

                using (SqlConnection Conn = new SqlConnection(DB))
                {
                    Conn.Open();
                    SqlCommand Comm = new SqlCommand(queryUp, Conn);
                    Comm.ExecuteNonQuery();
                }
            }
        }

        void redirect(object sender, CommandEventArgs e)
        {
            Response.Redirect("~/" + HttpUtility.UrlEncode(e.CommandArgument.ToString().Split('|')[5]));
        }

        void deleteRecord(object sender, CommandEventArgs e)
        {
            Thread.Sleep(SleepTime);
            //string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
            int id = int.Parse(e.CommandArgument.ToString().Split('|')[0]);
            string queryUp = $"delete from Picture_List where ID={id}";

            string full_size_name = e.CommandArgument.ToString().Split('|')[5];
            string small_size = e.CommandArgument.ToString().Split('|')[4];

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                Comm.ExecuteNonQuery();

                File.Delete(Server.MapPath(Path.Combine("~/", small_size)));
                File.Delete(Server.MapPath(Path.Combine("~/", full_size_name)));
            }
            FixPictureNumber(int.Parse(e.CommandArgument.ToString().Split('|')[2]));
            //DrawTable();
            Response.Redirect(Request.RawUrl);
        }

        void upRecord(object sender, CommandEventArgs e)
        {
            Thread.Sleep(SleepTime);
            //DrawTable();

            int galID = int.Parse(e.CommandArgument.ToString().Split('|')[1]);

            int id = int.Parse(e.CommandArgument.ToString().Split('|')[0]);
            int pictureNumber = int.Parse(e.CommandArgument.ToString().Split('|')[2]);
            string queryUp = $"update Picture_List set Picture_Number=Picture_Number+1 where Picture_Number={pictureNumber - 1} AND Gallery_ID={galID}; update Picture_List set Picture_Number=Picture_Number-1 where ID={id} AND Gallery_ID={galID}";

            if (pictureNumber == 1)
                return;
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                Comm.ExecuteNonQuery();
            }
            //DrawTable();
            Response.Redirect(Request.RawUrl);
        }

        void downRecord(object sender, CommandEventArgs e)
        {
            Thread.Sleep(SleepTime);
            //DrawTable();

            int galID = int.Parse(e.CommandArgument.ToString().Split('|')[1]);

            int id = int.Parse(e.CommandArgument.ToString().Split('|')[0]);
            int pictureNumber = int.Parse(e.CommandArgument.ToString().Split('|')[2]);
            string queryUp = $"update Picture_List set Picture_Number=Picture_Number-1 where Picture_Number={pictureNumber + 1} AND Gallery_ID={galID}; update Picture_List set Picture_Number=Picture_Number+1 where ID={id} AND Gallery_ID={galID}";

            if (pictureNumber == getMaxNumber())
                return;
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                Comm.ExecuteNonQuery();
            }
            //DrawTable();
            Response.Redirect(Request.RawUrl);
        }

        string galleryName(int id)
        {
            string name = "";

            string queryUp = $"select Gallery_Name from Gallery_List where ID={id}";

            
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                SqlDataReader reader = Comm.ExecuteReader();
                reader.Read();
                return reader[0].ToString();
            }

            return name;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int gal_id = int.Parse(Request.QueryString["ID"]);
            string dir_name = galleryName(gal_id);
            string full_dir_Name = $@"{dir_name}/";

            int number = getMaxNumber()+1;

            string fileName = FileUpload1.FileName;

            FileUpload1.SaveAs(Server.MapPath(Path.Combine(full_dir_Name, fileName)));

            Bitmap bmp=new Bitmap(System.Drawing.Image.FromFile(Server.MapPath(Path.Combine(full_dir_Name, fileName))),200,150);              

            string Mini_version_path=fileName.Split('.')[0]+"m."+ fileName.Split('.')[1];
            bmp.Save(Server.MapPath(Path.Combine(full_dir_Name, Mini_version_path)));

            string queryUp = $"insert into Picture_List(Gallery_ID,Picture_Number,Description,Mini_version,Full_version) values ({gal_id},{number},N'{TextBox1.Text}',N'{full_dir_Name + Mini_version_path}',N'{full_dir_Name + fileName}')";

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                Comm.ExecuteNonQuery();
                //Directory.CreateDirectory(@"D:\triod315\2 курс\SysProg\SysProgLabs\Lab5\Lab5\Lab5\" + TextBox1.Text);
            }
            //DrawTable();
            Response.Redirect(Request.RawUrl);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Default.auth = false;
            Response.Redirect("~/Default.aspx");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminPage.aspx");
        }
    }
}