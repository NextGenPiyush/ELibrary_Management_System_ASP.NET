using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElibraryManagementSystem
{
    public partial class adminbookissuing : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        // GO BUTTON
        protected void Button1_Click(object sender, EventArgs e)
        {
            getNames();
        }

        // ISSUE BUTTON
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkIfBookExists() && checkIfMemberExists())
            {
                if (checkIfBookIssuedAlready())
                {
                    Response.Write("<script>alert('This member already has this book');</script>");
                }
                else
                {
                    issueBook();
                }
            }
            else
            {
                Response.Write("<script>alert('Wrong book ID or member ID');</script>");
            }
        }

        // RETURN BUTTON
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkIfBookExistsInMaster() && checkIfMemberExists())
            {
                if (checkIfBookIssuedAlready())
                {
                    returnBook();
                }
                else
                {
                    Response.Write("<script>alert('This book was never issued to this member');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Wrong book ID or member ID');</script>");
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
                    if(dt < today)
                    {
                        e.Row.BackColor = System.Drawing.Color.Pink; // Highlight overdue books
                    }
                    else if(dt == today)
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

        void getNames()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // Get Book Name
                    using (SqlCommand cmd1 = new SqlCommand("SELECT book_name FROM book_master_tbl WHERE book_id=@book_id", con))
                    {
                        cmd1.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);

                        if (dt1.Rows.Count > 0)
                        {
                            TextBox2.Text = dt1.Rows[0]["book_name"].ToString();
                        }
                        else
                        {
                            Response.Write("<script>alert('Book with this ID does not exist');</script>");
                        }
                    }

                    // Get Member Name
                    using (SqlCommand cmd2 = new SqlCommand("SELECT full_name FROM member_master_tbl WHERE member_id=@member_id", con))
                    {
                        cmd2.Parameters.AddWithValue("@member_id", TextBox4.Text.Trim());
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);

                        if (dt2.Rows.Count > 0)
                        {
                            TextBox1.Text = dt2.Rows[0]["full_name"].ToString();
                        }
                        else
                        {
                            Response.Write("<script>alert('Member with this ID does not exist');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        bool checkIfBookExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl WHERE book_id=@book_id AND current_stock > 0", con))
                    {
                        cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt.Rows.Count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return false;
            }
        }

        bool checkIfBookExistsInMaster()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl WHERE book_id=@book_id", con))
                    {
                        cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt.Rows.Count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return false;
            }
        }

        bool checkIfMemberExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id=@member_id", con))
                    {
                        cmd.Parameters.AddWithValue("@member_id", TextBox4.Text.Trim());
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt.Rows.Count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return false;
            }
        }

        bool checkIfBookIssuedAlready()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id", con))
                    {
                        cmd.Parameters.AddWithValue("@member_id", TextBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt.Rows.Count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                return false;
            }
        }

        void issueBook()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // Insert issue record
                    using (SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO book_issue_tbl 
                          (member_id, member_name, book_id, book_name, issue_date, due_date) 
                          VALUES (@member_id, @member_name, @book_id, @book_name, @issue_date, @due_date)", con))
                    {
                        cmd.Parameters.AddWithValue("@member_id", TextBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@member_name", TextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@book_name", TextBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@issue_date", TextBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@due_date", TextBox6.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    // Decrease stock
                    using (SqlCommand cmd = new SqlCommand(
                        @"UPDATE book_master_tbl SET current_stock = current_stock - 1 WHERE book_id=@book_id", con))
                    {
                        cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    Response.Write("<script>alert('Book Issued Successfully!');</script>");
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void returnBook()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // Delete issue record
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id", con))
                    {
                        cmd.Parameters.AddWithValue("@member_id", TextBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            // Increase stock
                            using (SqlCommand cmd2 = new SqlCommand(
                                @"UPDATE book_master_tbl SET current_stock = current_stock + 1 WHERE book_id=@book_id", con))
                            {
                                cmd2.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                                cmd2.ExecuteNonQuery();
                            }

                            Response.Write("<script>alert('Book Returned Successfully!');</script>");
                            GridView1.DataBind();
                        }
                        else
                        {
                            Response.Write("<script>alert('This book was never issued to this member');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
