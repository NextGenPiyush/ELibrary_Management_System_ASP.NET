using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElibraryManagementSystem
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["role"] == null || Session["role"].ToString() == "")  // when nobody loges in default
                {
                    LinkButton1.Visible = true; // User Login
                    LinkButton2.Visible = true; // sign up
                    LinkButton3.Visible = false; // logout
                    LinkButton7.Visible = false; // hello user 

                    LinkButton6.Visible = true; // admin login
                    LinkButton11.Visible = false; // author management
                    LinkButton12.Visible = false; // publisher management
                    LinkButton8.Visible = false; // book inventory
                    LinkButton9.Visible = false; // book issuing
                    LinkButton10.Visible = false; // member management
                }
                else if(Session["role"].Equals("user")) // when user loges in
                {
                    LinkButton1.Visible = false; // User Login
                    LinkButton2.Visible = false; // sign up
                    LinkButton3.Visible = true; // logout
                    LinkButton7.Visible = true; // hello user 
                    LinkButton7.Text = "Helllo" + Session["username"].ToString();

                    LinkButton6.Visible = true; // admin login
                    LinkButton11.Visible = false; // author management
                    LinkButton12.Visible = false; // publisher management
                    LinkButton8.Visible = false; // book inventory
                    LinkButton9.Visible = false; // book issuing
                    LinkButton10.Visible = false; // member management
                }
                else if (Session["role"].Equals("admin")) // when admin loges in
                {
                    LinkButton1.Visible = false; // User Login
                    LinkButton2.Visible = false; // sign up

                    LinkButton3.Visible = true; // logout
                    LinkButton7.Visible = true; // hello user 
                    LinkButton7.Text = "Helllo Admin";

                    LinkButton6.Visible = false; // admin login
                    LinkButton11.Visible = true; // author management
                    LinkButton12.Visible = true; // publisher management
                    LinkButton8.Visible = true; // book inventory
                    LinkButton9.Visible = true; // book issuing
                    LinkButton10.Visible = true; // member management
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminlogin.aspx");
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminauthormanagement.aspx");
        }

        protected void LinkButton12_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminpublishermanagement.aspx");
        }

        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminbookinventory.aspx");
        }

        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminbookissuing.aspx");
        }

        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminmembermanagement.aspx");
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Response.Redirect("viewbooks.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlogin.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("usersignup.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Session["username"] = "";
            Session["full_name"] = "";
            Session["role"] = "";
            Session["status"] = "";

            // everything is set to default
            LinkButton1.Visible = true; // User Login
            LinkButton2.Visible = true; // sign up
            LinkButton3.Visible = false; // logout
            LinkButton7.Visible = false; // hello user 

            LinkButton6.Visible = true; // admin login
            LinkButton11.Visible = false; // author management
            LinkButton12.Visible = false; // publisher management
            LinkButton8.Visible = false; // book inventory
            LinkButton9.Visible = false; // book issuing
            LinkButton10.Visible = false; // member management

            Response.Redirect("homepage.aspx");
        }

        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            if (Session["role"] != null && Session["role"].ToString() == "user")
            {
                Response.Redirect("userprofile.aspx");
            }
        }
    }
}