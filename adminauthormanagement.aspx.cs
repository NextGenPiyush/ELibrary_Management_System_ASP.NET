using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElibraryManagementSystem
{
    public partial class adminauthormanagement : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)  // add button  
        {
            if (checkIfAuthorExists())
            {
                Response.Write("<script>alert('Author with this ID already exist');</script>");
            }
            else
            {
                addNewAuthor();
            }
        }

        protected void Button3_Click(object sender, EventArgs e)  // update button  
        {
            if (checkIfAuthorExists())
            {
                updateAuthor();
            }
            else
            {
                Response.Write("<script>alert('Author with this ID does not exists');</script>");
            }
        }

        protected void Button4_Click(object sender, EventArgs e)  // delete button  
        {
            if (checkIfAuthorExists())
            {
                deleteAuthor();
            }
            else
            {
                Response.Write("<script>alert('Author with this ID does not exists');</script>");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)  // go button  
        {
            getAuthorByID();
        }

        void addNewAuthor()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"  
                            INSERT INTO author_master_tbl (author_id,author_name) VALUES (@author_id,@author_name)", con);

                    cmd.Parameters.AddWithValue("@author_id", TextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@author_name", TextBox4.Text.Trim());

                    cmd.ExecuteNonQuery();

                    con.Close();
                    Response.Write("<script>alert('Author added Successful!');</script>");
                    clearFields();
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        bool checkIfAuthorExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM author_master_tbl WHERE author_id=@author_id", con);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@author_id", TextBox3.Text.Trim());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt.Rows.Count >= 1;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return false;
            }
        }

        void updateAuthor()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE author_master_tbl SET author_name=@author_name WHERE author_id='" + TextBox3.Text.Trim() + "'", con);

                    cmd.Parameters.AddWithValue("@author_name", TextBox4.Text.Trim());

                    cmd.ExecuteNonQuery();

                    con.Close();
                    Response.Write("<script>alert('Author updated Successful!');</script>");
                    clearFields();
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void deleteAuthor()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM author_master_tbl WHERE author_id=@author_id", con);
                    cmd.Parameters.AddWithValue("@author_id", TextBox3.Text.Trim());

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Author deleted Successful!');</script>");
                    clearFields();
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void getAuthorByID()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM author_master_tbl WHERE author_id=@author_id", con);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@author_id", TextBox3.Text.Trim());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if(dt.Rows.Count >= 1)
                    {
                        TextBox4.Text = dt.Rows[0]["author_name"].ToString();
                    }
                    else
                    {
                        Response.Write("<script>alert('No Author found with this ID');</script>");
                        clearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void clearFields()
        {
            TextBox3.Text = "";
            TextBox4.Text = "";
        }

    }
}