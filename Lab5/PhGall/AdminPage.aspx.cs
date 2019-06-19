using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace Lab5
{
    public partial class AdminPage : System.Web.UI.Page
    {
        int SleepTime = 2000;
        List<GalTableField> rows;
        string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Default.auth)
                Response.Redirect("~/Default.aspx");
            DrawTable();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        private void DrawTable()
        {
            //string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
            int TableWidth = 400;
            
            
            Table1.Width = TableWidth + 3 * GalTableField.ButtonsWidth;
            Table1.Font.Size = 18;

            Table1.Rows.Clear();
            TableRow tr1 = new TableRow();
            TableCell td11 = new TableCell();
            TableCell td12 = new TableCell();
            TableCell td13 = new TableCell();

            Label l11 = new Label();
            Label l12 = new Label();
            Label l13 = new Label();

            l11.Text = "ID"; td11.Controls.Add(l11); tr1.Cells.Add(td11);
            l12.Text = "Номер галереї"; td12.Controls.Add(l12); tr1.Cells.Add(td12);
            l13.Text = "Галерея"; td13.Controls.Add(l13); tr1.Cells.Add(td13);



            tr1.HorizontalAlign = HorizontalAlign.Center;
            tr1.Width = TableWidth + 3 * GalTableField.ButtonsWidth;
            Table1.Rows.Add(tr1);


            rows = new List<GalTableField>();

            string QueryText = "select ID,Gallery_Number,Gallery_Name from Gallery_List";

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(QueryText, Conn);
                SqlDataReader R = Comm.ExecuteReader();

                while (R.Read())
                {

                    rows.Add(new GalTableField(R.GetInt32(0), R.GetInt32(1), R[2].ToString()));

                }

                rows.Sort((x, y) => GalTableField.Compare(x, y));

                for (int i = 0; i < rows.Count; i++)
                {
                    TableRow tr2 = new TableRow();

                    TableCell td02 = new TableCell();
                    td02.Controls.Add(rows[i].IDLabel);
                    tr2.Cells.Add(td02);

                    TableCell td01 = new TableCell();
                    td01.Controls.Add(rows[i].NumberLabel);
                    tr2.Cells.Add(td01);



                    TableCell td1 = new TableCell();
                    rows[i].NameButton.Command += new CommandEventHandler(b4_Command);
                    td1.Controls.Add(rows[i].NameButton);
                    tr2.Cells.Add(td1);

                    TableCell td2 = new TableCell();
                    rows[i].Up.Command += new CommandEventHandler(upRecord);
                    rows[i].Up.CssClass = "DynamicButtons";
                    rows[i].Up.OnClientClick = "hide_all()";
                    td2.Controls.Add(rows[i].Up);
                    tr2.Cells.Add(td2);

                    TableCell td3 = new TableCell();
                    rows[i].Down.Command += new CommandEventHandler(downRecord);
                    rows[i].Down.CssClass = "DynamicButtons";
                    rows[i].Down.OnClientClick = "hide_all()";
                    td3.Controls.Add(rows[i].Down);
                    tr2.Cells.Add(td3);

                    TableCell td4 = new TableCell();
                    rows[i].Delete.Command += new CommandEventHandler(deleteRecord);
                    rows[i].Delete.CssClass = "DynamicButtons";
                    rows[i].Delete.OnClientClick = "hide_all()";
                    td4.Controls.Add(rows[i].Delete);
                    tr2.Cells.Add(td4);

                    tr2.Width = TableWidth + 3 * GalTableField.ButtonsWidth;
                    tr2.HorizontalAlign = HorizontalAlign.Center;
                    Table1.Rows.Add(tr2);
                }

            }
        }

        void deleteRecord(object sender, CommandEventArgs e)
        {
            Thread.Sleep(SleepTime);
            //string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
            int id = int.Parse(e.CommandArgument.ToString().Split('|')[0]);
            string queryUp = $"delete from Gallery_List where ID={id}";

            string name = e.CommandArgument.ToString().Split('|')[2];

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                Comm.ExecuteNonQuery();


                Directory.Delete(Server.MapPath(Path.Combine("~/", name)),true);
            }
            //DrawTable();
            Response.Redirect(Request.RawUrl);
        }

        void upRecord(object sender, CommandEventArgs e)
        {
            Thread.Sleep(SleepTime);
            //DrawTable();
            //string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
            int id = int.Parse(e.CommandArgument.ToString().Split('|')[0]);
            int galeryNumber = int.Parse(e.CommandArgument.ToString().Split('|')[1]);
            if (galeryNumber == 1)
                return;
            string queryUp = $"update Gallery_List set Gallery_Number=Gallery_Number+1 where Gallery_Number={galeryNumber-1} update Gallery_List set Gallery_Number=Gallery_Number-1 where ID={id}";
       
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
            //DrawTable();
            Thread.Sleep(SleepTime);
            int id = int.Parse(e.CommandArgument.ToString().Split('|')[0]);
            int galeryNumber = int.Parse(e.CommandArgument.ToString().Split('|')[1]);
            string queryUp = $"update Gallery_List set Gallery_Number=Gallery_Number-1 where Gallery_Number={galeryNumber + 1}; update Gallery_List set Gallery_Number=Gallery_Number+1 where ID={id}";

            if (galeryNumber == getMaxNumber())
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

        protected void b4_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("PhotosAdmin.aspx?ID=" + HttpUtility.UrlEncode(e.CommandArgument.ToString().Split('|')[0]));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Default.auth = false;
            Response.Redirect("~/Default.aspx");
        }

        int getMaxNumber() => (rows.Count>0)?rows[rows.Count - 1].Number:0;
        

        protected void Button2_Click(object sender, EventArgs e)
        {
           
            string queryUp = $"insert into Gallery_List(Gallery_Number,Gallery_Name) values ({getMaxNumber()+1},N'{TextBox1.Text}')";

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(queryUp, Conn);
                Comm.ExecuteNonQuery();
                Directory.CreateDirectory(Server.MapPath(Path.Combine("~/", TextBox1.Text)));
            }
            //DrawTable();
            Response.Redirect(Request.RawUrl);
        }
    }
}