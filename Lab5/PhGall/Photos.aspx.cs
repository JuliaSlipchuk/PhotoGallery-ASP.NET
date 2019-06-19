using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Lab5
{

    class PhotoField
    {
        public static int ButtonsWidth = 100;
        public int GalID;
        public int ID;
        public int Number;
        public ImageButton pic;
        public Button Up;
        public Button Down;
        public Button Delete;
        public string mini_version;
        public string full_version;
        public string description;
        public Label IDLabel;
        public Label NumberLabel;

        public static int Compare(PhotoField arg1, PhotoField arg2)
        {
            if (arg1.Number >= arg2.Number)
                return 1;
            if (arg1.Number <= arg2.Number)
                return -1;
            return 0;
        }

        public PhotoField(int ID,int GalID, int Number, string description,string mini_version,string full_version)
        {
            this.ID = ID;
            this.mini_version = mini_version;
            this.full_version = full_version;
            this.description = description;
            this.GalID = GalID;
            this.Number = Number;

            pic = new ImageButton();

            Button b4 = new Button();

            pic.Width = 200;
            pic.Height = 150;
            pic.CommandArgument = ID.ToString()+"|"+GalID.ToString()+"|"+Number.ToString()+"|"+description+"|"+mini_version+"|"+full_version;
            pic.ImageUrl = mini_version;
            pic.ToolTip = description;

            Up = initButton("Up");
            Down = initButton("Down");
            Delete = initButton("Delete");


            IDLabel = new Label();
            IDLabel.Text = ID.ToString();
            IDLabel.Font.Bold = true;
            IDLabel.Font.Size = 16;
            IDLabel.Width = ButtonsWidth;


            NumberLabel = new Label();
            NumberLabel.Text = ID.ToString();
            NumberLabel.Font.Bold = true;
            NumberLabel.Font.Size = 16;
            NumberLabel.Width = ButtonsWidth;

        }

        Button initButton(string name)
        {
            Button b4 = new Button();
            b4.CssClass = "DynamicButtons";
            b4.OnClientClick = "hide_all()";
            b4.Text = name; b4.Font.Bold = true; b4.Font.Size = 20;
            b4.Width = ButtonsWidth;
            b4.CommandArgument = ID.ToString() + "|" + GalID.ToString() + "|" + Number.ToString() + "|" + description + "|" + mini_version + "|" + full_version;
            return b4;
        }
    }

    public partial class Photos : System.Web.UI.Page
    {
        string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";





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

        protected void Page_Load(object sender, EventArgs e)
        {
            string GalID = Request.QueryString["ID"];
            Label1.Text = galleryName(int.Parse(GalID));
            List<ImageButton> images = new List<ImageButton>();
            //string QueryText = $"select * from Picture_List where Gallery_ID={int.Parse(GalID)}";
            string DB = @"Data Source=DESKTOP-5582JAK\SQLEXPRESS; Initial Catalog=Galery; Integrated Security=true";

            List<PhotoField> imagesPh = new List<PhotoField>();
            string QueryText = $"select * from Picture_List where Gallery_ID={int.Parse(GalID)}";

            using (SqlConnection Conn = new SqlConnection(DB))
            {
                Conn.Open();
                SqlCommand Comm = new SqlCommand(QueryText, Conn);
                SqlDataReader R = Comm.ExecuteReader();
                while (R.Read())
                {
                    imagesPh.Add(new PhotoField(R.GetInt32(0), R.GetInt32(1), R.GetInt32(2), R[3].ToString(), R[4].ToString(), R[5].ToString()));
                }
                imagesPh.Sort((x, y) => PhotoField.Compare(x, y));

                for (int i = 0; i < imagesPh.Count; i++)
                {
                    images.Add(createImage(imagesPh[i].mini_version, imagesPh[i].full_version, imagesPh[i].description));
                }
            }

               
        }
        ImageButton createImage(string Url, string BigUrl,string Description)
        {
            ImageButton tmpImg = new ImageButton();
            tmpImg.CssClass = "imageButtons";
            tmpImg.ImageUrl = Url;
            tmpImg.Width = 200;
            tmpImg.Height = 150;
            tmpImg.CommandArgument = BigUrl;
            tmpImg.Command += new CommandEventHandler(comm);
            tmpImg.ToolTip = Description;
            Panel1.Controls.Add(tmpImg);
            return tmpImg;
        }

        void comm(object sender, CommandEventArgs e)
        {
            Response.Redirect("~/" + HttpUtility.UrlEncode(e.CommandArgument.ToString()));
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}