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
    public partial class adminpublishermanagement : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)  // add button  
        {
            if (checkIfPublisherExists())
            {
                Response.Write("<script>alert('Publisher with this ID already exist');</script>");
            }
            else
            {
                addNewPublisher();
            }
        }

        protected void Button3_Click(object sender, EventArgs e)  // update button  
        {
            if (checkIfPublisherExists())
            {
                updatePublisher();
            }
            else
            {
                Response.Write("<script>alert('Publisher with this ID does not exists');</script>");
            }
        }

        protected void Button4_Click(object sender, EventArgs e)  // delete button  
        {
            if (checkIfPublisherExists())
            {
                deletePublisher();
            }
            else
            {
                Response.Write("<script>alert('Publisher with this ID does not exists');</script>");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)  // go button  
        {
            getPublisherByID();
        }

        void addNewPublisher()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"  
                            INSERT INTO publisher_master_tbl (publisher_id,publisher_name) VALUES (@publisher_id,@publisher_name)", con);

                    cmd.Parameters.AddWithValue("@publisher_id", TextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@publisher_name", TextBox4.Text.Trim());

                    cmd.ExecuteNonQuery();

                    con.Close();
                    Response.Write("<script>alert('Publisher added Successful!');</script>");
                    clearFields();
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        bool checkIfPublisherExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM publisher_master_tbl WHERE publisher_id=@publisher_id", con);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@publisher_id", TextBox3.Text.Trim());

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

        void updatePublisher()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"UPDATE publisher_master_tbl SET publisher_name=@publisher_name WHERE publisher_id='" + TextBox3.Text.Trim() + "'", con);

                    cmd.Parameters.AddWithValue("@publisher_name", TextBox4.Text.Trim());

                    cmd.ExecuteNonQuery();

                    con.Close();
                    Response.Write("<script>alert('Publisher updated Successful!');</script>");
                    clearFields();
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void deletePublisher()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM publisher_master_tbl WHERE publisher_id=@publisher_id", con);
                    cmd.Parameters.AddWithValue("@publisher_id", TextBox3.Text.Trim());

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Publisher deleted Successful!');</script>");
                    clearFields();
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void getPublisherByID()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM publisher_master_tbl WHERE publisher_id=@publisher_id", con);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@publisher_id", TextBox3.Text.Trim());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        TextBox4.Text = dt.Rows[0]["publisher_name"].ToString();
                    }
                    else
                    {
                        Response.Write("<script>alert('No Publisher found with this ID');</script>");
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