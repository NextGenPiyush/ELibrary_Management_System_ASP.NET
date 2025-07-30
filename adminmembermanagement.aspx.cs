using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElibraryManagementSystem
{
    public partial class adminmembermanagement : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void LinkButton4_Click(object sender, EventArgs e)  // Go Button
        {
            getMemberById();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)  // Active
        {
            updateMemberStatusByID("active");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)  // Pending
        {
            updateMemberStatusByID("pending");
        }

        protected void LinkButton3_Click(object sender, EventArgs e) // Deactivate
        {
            updateMemberStatusByID("deactive");
        }

        protected void Button2_Click(object sender, EventArgs e)  // Delete Button
        {
            deleteMemberById();
        }

        void getMemberById()
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id='" + TextBox3.Text.Trim() + "'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TextBox4.Text = dr.GetValue(0).ToString();
                        TextBox7.Text = dr.GetValue(10).ToString();
                        TextBox1.Text = dr.GetValue(1).ToString();
                        TextBox2.Text = dr.GetValue(2).ToString();
                        TextBox8.Text = dr.GetValue(3).ToString();
                        TextBox5.Text = dr.GetValue(4).ToString();
                        TextBox6.Text = dr.GetValue(5).ToString();
                        TextBox9.Text = dr.GetValue(6).ToString();
                        TextBox10.Text = dr.GetValue(7).ToString();
                    }
                }
                else
                {
                    Response.Write("<script>alert('Invalid Member ID');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void updateMemberStatusByID(string status)
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl SET account_status='"+status+"' WHERE member_id='" + TextBox3.Text.Trim() + "'", con);

                cmd.ExecuteReader();
                con.Close();
                clearForm();
                GridView1.DataBind();
                Response.Write("<script>alert('Member status updated');</script>");

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void deleteMemberById()
        {
            if (checkIfMemberExists())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM member_master_tbl WHERE member_id=@member_id", con);
                        cmd.Parameters.AddWithValue("@member_id", TextBox3.Text.Trim());

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Response.Write("<script>alert('Member deleted Successful!');</script>");
                        clearForm();
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid Member_ID');</script>");
            }
        }

        bool checkIfMemberExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id=@member_id", con);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@member_id", TextBox3.Text.Trim());

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

        void clearForm()
        {
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox7.Text = "";
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox8.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
        }
    }
}