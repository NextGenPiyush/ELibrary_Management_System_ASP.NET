using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElibraryManagementSystem
{
    public partial class userprofile : System.Web.UI.Page
    {

        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(Session["username"].ToString() == null || Session["username"].ToString() == "")
                {
                    Response.Write("<script>alert('Session Expired Login again');</script>");
                    Response.Redirect("userlogin.aspx");
                }
                else
                {
                    getUserBookData();

                    if(!Page.IsPostBack)
                    {
                        getUserPersonalData();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)  // Update Button
        {
            try
            {
                if (Session["username"].ToString() == null || Session["username"].ToString() == "")
                {
                    Response.Write("<script>alert('Session Expired Login again');</script>");
                    Response.Redirect("userlogin.aspx");
                }
                else
                {
                    updateUserPersonalDetails();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Check your condition here
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[5].Text);
                    DateTime today = DateTime.Today;
                    if (dt < today)
                    {
                        e.Row.BackColor = System.Drawing.Color.Pink; // Highlight overdue books
                    }
                    else if (dt == today)
                    {
                        e.Row.BackColor = System.Drawing.Color.LightYellow; // Highlight books that need to return today
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.LightGreen; // Highlight books that are not overdue
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void getUserBookData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM book_issue_tbl WHERE member_id='" + Session["username"].ToString() +"';", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void getUserPersonalData()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id='" + Session["username"].ToString() + "';", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if(dt.Rows.Count < 1)
                    {
                        Response.Write("<script>alert('No user data found');</script>");
                        return;
                    }
                    TextBox3.Text = dt.Rows[0]["full_name"].ToString();
                    TextBox4.Text = dt.Rows[0]["dob"].ToString();
                    TextBox1.Text = dt.Rows[0]["contact_no"].ToString();
                    TextBox2.Text = dt.Rows[0]["email"].ToString();
                    DropDownList1.SelectedValue = dt.Rows[0]["state"].ToString();
                    TextBox5.Text = dt.Rows[0]["city"].ToString();
                    TextBox7.Text = dt.Rows[0]["pincode"].ToString();
                    TextBox6.Text = dt.Rows[0]["full_address"].ToString();
                    TextBox8.Text = dt.Rows[0]["member_id"].ToString();

                    Label1.Text = dt.Rows[0]["account_status"].ToString();
                    if(Label1.Text == "active")
                    {
                        //Label1.ForeColor = System.Drawing.Color.Green;
                        Label1.Attributes.Add("class", "badge bagde-pill badge-success");
                    }
                    else if(Label1.Text == "pending")
                    {
                        //Label1.ForeColor = System.Drawing.Color.Yellow;
                        Label1.Attributes.Add("class", "badge bagde-pill badge-warning");
                    }
                    else if(Label1.Text == "deactive")
                    {
                        //Label1.ForeColor = System.Drawing.Color.Red;
                        Label1.Attributes.Add("class", "badge bagde-pill badge-danger");
                    }
                    else
                    {
                        Label1.Attributes.Add("class", "badge bagde-pill badge-info");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void updateUserPersonalDetails()
        {
            string password = "";
            if(TextBox10.Text.Trim() == "")
            {
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id='" + Session["username"].ToString() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                password = dt.Rows[0]["password"].ToString();
            }
            else
            {
                password = TextBox10.Text.Trim();
            }

            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if(con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand(@"UPDATE member_master_tbl SET full_name=@full_name, dob=@dob, contact_no=@contact_no, email=@email, state=@state, city=@city, pincode=@pincode, full_address=@full_address, password=@password, account_status=@account_status WHERE member_id='" + Session["username"].ToString().Trim() +"'", con);

                cmd.Parameters.AddWithValue("@full_name", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@contact_no", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@email", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@state", DropDownList1.SelectedValue);
                cmd.Parameters.AddWithValue("@city", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@pincode", TextBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@full_address", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@account_status", "pending");
                
                int result = cmd.ExecuteNonQuery();
                con.Close();
                if(result > 0)
                {
                    Response.Write("<script>alert('User profile updated successfully!');</script>");
                    getUserPersonalData();
                    getUserBookData();
                }
                else
                {
                    Response.Write("<script>alert('Invalid Entry');</script>");
                }
                
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}