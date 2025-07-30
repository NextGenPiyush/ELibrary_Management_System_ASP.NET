using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElibraryManagementSystem
{
    public partial class adminbookinventory : System.Web.UI.Page
    {

        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        static string global_filepath;
        static int global_actual_stock, global_current_stock, global_issued_books;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillAuthorPublisherValues();
            }
            GridView1.DataBind();
        }

        protected void LinkButton4_Click(object sender, EventArgs e)  // Go Button
        {
            getBookByID();
        }

        protected void Button2_Click(object sender, EventArgs e)  // add button
        {
            if(checkIfBookExists())
            {
                Response.Write("<script>alert('Book with this ID already exists.');</script>");
            }
            else
            {
                addBook();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)  // update button
        {
            updateBookByID();
        }

        protected void Button3_Click(object sender, EventArgs e) // delete button
        {
            if(checkIfBookExists())
            {
                deleteBookByID();
            }
            else
            {
                Response.Write("<script>alert('Invalid Book ID');</script>");
            }
        }

        void fillAuthorPublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT author_name FROM author_master_tbl;", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DropDownList3.DataSource = dt;
                DropDownList3.DataValueField = "author_name";
                DropDownList3.DataBind();

                cmd = new SqlCommand("SELECT publisher_name FROM publisher_master_tbl;", con);

                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                DropDownList2.DataSource = dt;
                DropDownList2.DataValueField = "publisher_name";
                DropDownList2.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        bool checkIfBookExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl WHERE book_id=@book_id OR book_name=@book_name;" , con);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());

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

        void addBook()
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand(@"INSERT INTO book_master_tbl 
                (book_id, book_name, genre, author_name, publisher_name, publish_date, language, edition, book_cost, no_of_page, book_description, actual_stock, current_stock, book_img_link) 
                VALUES  
                (@book_id, @book_name, @genre, @author_name, @publisher_name, @publish_date, @language, @edition, @book_cost, @no_of_page, @book_description, @actual_stock, @current_stock, @book_img_link);", con);

                cmd.Parameters.AddWithValue("@book_id", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@publisher_name", DropDownList2.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@author_name", DropDownList3.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@publish_date", TextBox1.Text.Trim());

                string genre = "";
                foreach (ListItem item in ListBox1.Items)
                {
                    if (item.Selected)
                    {
                        genre += item.Text + ", ";
                    }
                }
                genre = genre.TrimEnd(',', ' ');
                cmd.Parameters.AddWithValue("@genre", genre);

                cmd.Parameters.AddWithValue("@edition", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@book_cost", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@no_of_page", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@actual_stock", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@current_stock", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@book_description", TextBox10.Text.Trim());

                // Image link handling
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (!string.IsNullOrEmpty(filename))
                {
                    // Save the file in the folder bookFrontPage
                    FileUpload1.SaveAs(Server.MapPath("~/bookFrontPage/" + filename));

                    // Store the relative path in database
                    string filepath = "~/bookFrontPage/" + filename;
                    cmd.Parameters.AddWithValue("@book_img_link", filepath);
                }
                else
                {
                    // If no file uploaded, save default image
                    cmd.Parameters.AddWithValue("@book_img_link", "~/imgs/book.jpeg");
                }

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Book Added Successfully');</script>");
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        void getBookByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl WHERE book_id='"+TextBox3.Text.Trim()+"';", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if(dt.Rows.Count >= 1)
                {
                    TextBox4.Text = dt.Rows[0]["book_name"].ToString();
                    TextBox6.Text = dt.Rows[0]["edition"].ToString();
                    TextBox5.Text = dt.Rows[0]["no_of_page"].ToString().Trim();
                    TextBox9.Text = dt.Rows[0]["book_cost"].ToString().Trim();
                    TextBox2.Text = dt.Rows[0]["actual_stock"].ToString().Trim();
                    TextBox7.Text = dt.Rows[0]["current_stock"].ToString().Trim();
                    TextBox8.Text = (Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString()) - Convert.ToInt32(dt.Rows[0]["current_stock"].ToString())).ToString().Trim();
                    TextBox10.Text = dt.Rows[0]["book_description"].ToString();
                    DropDownList1.SelectedValue = dt.Rows[0]["language"].ToString();
                    DropDownList2.SelectedValue = dt.Rows[0]["publisher_name"].ToString();
                    DropDownList3.SelectedValue = dt.Rows[0]["author_name"].ToString();
                    TextBox1.Text = dt.Rows[0]["publish_date"].ToString();

                    ListBox1.ClearSelection();
                    string[] genre = dt.Rows[0]["genre"].ToString().Trim().Split(',');
                    for(int i = 0; i < genre.Length; i++)
                    {
                        for(int j = 0; j < ListBox1.Items.Count; j++)
                        {
                            if (ListBox1.Items[j].ToString() == genre[i])
                            {
                                ListBox1.Items[j].Selected = true;
                            }
                        }
                    }

                    global_actual_stock = Convert.ToInt32(dt.Rows[0]["actual_stock"].ToString().Trim());
                    global_current_stock = Convert.ToInt32(dt.Rows[0]["current_stock"].ToString().Trim());
                    global_issued_books = global_actual_stock - global_current_stock;
                    global_filepath = dt.Rows[0]["book_img_link"].ToString();

                }
                else
                {
                    Response.Write("<script>alert('Invalid Book Id');</script>");
                }

                
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void updateBookByID()
        {
            if (checkIfBookExists())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE book_master_tbl SET book_name=@book_name, genre=@genre, author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, language=@language, edition=@edition, book_cost=@book_cost, no_of_page=@no_of_page, book_description=@book_description, actual_stock=@actual_stock, current_stock=@current_stock, book_img_link=@book_img_link WHERE book_id='"+TextBox3.Text.Trim()+"'", con);

                        cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@publisher_name", DropDownList2.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@author_name", DropDownList3.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@publish_date", TextBox1.Text.Trim());

                        string genre = "";
                        foreach (ListItem item in ListBox1.Items)
                        {
                            if (item.Selected)
                            {
                                genre += item.Text + ", ";
                            }
                        }
                        genre = genre.TrimEnd(',', ' ');
                        cmd.Parameters.AddWithValue("@genre", genre);

                        cmd.Parameters.AddWithValue("@edition", TextBox6.Text.Trim());
                        cmd.Parameters.AddWithValue("@book_cost", TextBox9.Text.Trim());
                        cmd.Parameters.AddWithValue("@no_of_page", TextBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@book_description", TextBox10.Text.Trim());

                        int actual_stock = Convert.ToInt32(TextBox2.Text.Trim());
                        int current_stock = Convert.ToInt32(TextBox7.Text.Trim());
                        if(global_actual_stock == actual_stock)
                        {

                        }
                        else
                        {
                            if(actual_stock < global_issued_books)
                            {
                                Response.Write("<script>alert('Actual Stock Value Cannot be less than the Issued Books');</script>");
                            }
                            else
                            {
                                current_stock = actual_stock - global_issued_books;
                                TextBox7.Text = "" + current_stock;
                            }
                        }
                        cmd.Parameters.AddWithValue("@actual_stock", actual_stock.ToString());
                        cmd.Parameters.AddWithValue("@current_stock", current_stock.ToString());

                        string filepath = "~/bookFrontPage/book";
                        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                        if(filename == "" || filename == null)
                        {
                            filepath = global_filepath;
                        }
                        else
                        {
                            FileUpload1.SaveAs(Server.MapPath("bookFrontPage/" + filename));
                            filepath = "~/bookFrontPage/" + filename;
                        }
                        cmd.Parameters.AddWithValue("@book_img_link", filepath);

                        cmd.ExecuteNonQuery();
                        con.Close();

                        Response.Write("<script>alert('Book Ststus Updated Successful!');</script>");
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
                Response.Write("<script>alert('Invalid Book_ID');</script>");
            }
        }

        void deleteBookByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(conStr);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("DELETE FROM book_master_tbl WHERE book_id='" + TextBox3.Text.Trim() + "';", con);

                cmd.ExecuteNonQuery();
                con.Close();

                Response.Write("<script>alert('Book Deleted Successfully');</script>");
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}