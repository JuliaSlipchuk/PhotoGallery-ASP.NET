using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;

namespace Lab5
{
   
    class GalTableField 
    {
        public static int NameWidth = 400;
        public static int ButtonsWidth = 100;
        public int GalID;
        public int Number;
        public Button NameButton;
        public Button Up;
        public Button Down;
        public Button Delete;
        public Label IDLabel;
        public Label NumberLabel;

        public static int Compare(GalTableField arg1, GalTableField arg2)
        {
            if (arg1.Number >= arg2.Number)
                return 1;
            if (arg1.Number <= arg2.Number)
                return -1;
            return 0;
        }

        public GalTableField(int ID, int Number, string name) 
        {
            this.GalID = ID;
            this.Number = Number;

            NameButton = new Button();

            Button b4 = new Button();

            NameButton.Text = name; NameButton.Font.Bold = true; NameButton.Font.Size = 20;
            NameButton.Width = NameWidth;
            NameButton.CommandArgument = GalID.ToString();
            Up = initButton("Up");
            Down = initButton("Down");
            Delete = initButton("Delete");

            IDLabel = new Label();
            IDLabel.Text = ID.ToString();
            IDLabel.Font.Bold = true;
            IDLabel.Font.Size = 16;
            IDLabel.Width = ButtonsWidth;
            

            NumberLabel = new Label();
            NumberLabel.Text = Number.ToString();
            NumberLabel.Font.Bold = true;
            NumberLabel.Font.Size = 16;
            NumberLabel.Width = ButtonsWidth;

        }

        Button initButton(string name)
        {
            Button b4 = new Button();

            b4.Text = name; b4.Font.Bold = true; b4.Font.Size = 20;
            b4.Width = ButtonsWidth;
            b4.CommandArgument = GalID.ToString() + "|" + Number.ToString() +"|"+NameButton.Text;
            return b4;
        }

    }


    public partial class Default : System.Web.UI.Page
    {
        //true if user is admin
        public static bool auth = false;

        //check login/password combination
        bool CheckAuth(string login, string password)
        {
            bool flag = false;

            string QueryText = $"select password from Users where login=N'{login}'";
            string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(QueryText, Conn);
                SqlDataReader R = Comm.ExecuteReader();
                
                return (R.Read() && R[0].ToString() == password);
            }
            return flag;
        }

        int TableWidth = 400;
        protected void Page_Load(object sender, EventArgs e)
        {

            

            LoginErrorLabel.ForeColor = Color.Red;
            LoginErrorLabel.Font.Size = 18;
           
            LoginErrorLabel.Visible = false;
            Table1.Width = TableWidth;
            Table1.Font.Size = 24;

            TableRow tr1 = new TableRow();
            TableCell td11 = new TableCell(); Label l11 = new Label();

            l11.Text = "Галерея"; td11.Controls.Add(l11); tr1.Cells.Add(td11);
            tr1.HorizontalAlign = HorizontalAlign.Center;
            tr1.Width = TableWidth;
            Table1.Rows.Add(tr1);


            List<GalTableField> rows = new List<GalTableField>();

            string QueryText = "select ID,Gallery_Number,Gallery_Name from Gallery_List";
            string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";
            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(QueryText, Conn);
                SqlDataReader R = Comm.ExecuteReader();

                while (R.Read())
                {

                    rows.Add(new GalTableField(R.GetInt32(0), R.GetInt32(1), R[2].ToString()));
                   
                }

                rows.Sort((x,y) => GalTableField.Compare(x,y));

                for (int i = 0; i < rows.Count; i++)
                {
                    TableRow tr2 = new TableRow();
                    TableCell td1 = new TableCell();
                    rows[i].NameButton.Command += new CommandEventHandler(b4_Command);
                    td1.Controls.Add(rows[i].NameButton);
                    tr2.Cells.Add(td1);
                    tr2.Width = TableWidth;
                    
                    Table1.Rows.Add(tr2);
                }

            }

        }
        protected void b4_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("Photos.aspx?ID=" + HttpUtility.UrlEncode(e.CommandArgument.ToString().Split('|')[0]));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (CheckAuth(TextBox1.Text, TextBox2.Text))
            {
                auth = true;
                Response.Redirect("~/AdminPage.aspx");
                
            }
            else
            {
                LoginErrorLabel.Visible = true;         
                Thread.Sleep(10000);
                Button1.Visible = true;
                auth = false;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            
        }
    }
}