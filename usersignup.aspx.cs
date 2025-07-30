using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace ElibraryManagementSystem
{
    public partial class usersignup : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Run validations before DB interaction
            string validationMessage = ValidateInputs();
            if (validationMessage != "")
            {
                Response.Write("<script>alert('" + validationMessage + "');</script>");
                return;
            }

            if (checkMemberExists())
            {
                Response.Write("<script>alert('Member already exists with this Member Id, try other ID');</script>");
            }
            else
            {
                signUpNewUser();
            }
        }

        // Server-side validation
        private string ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(TextBox3.Text))
                return "Full Name is required";

            if (string.IsNullOrWhiteSpace(TextBox4.Text))
                return "Date of Birth is required";
            DateTime dob;
            if (!DateTime.TryParse(TextBox4.Text.Trim(), out dob))
                return "Invalid Date of Birth";

            if (string.IsNullOrWhiteSpace(TextBox1.Text))
                return "Contact Number is required";
            if (!Regex.IsMatch(TextBox1.Text.Trim(), @"^\d{10}$"))
                return "Contact Number must be 10 digits";

            if (string.IsNullOrWhiteSpace(TextBox2.Text))
                return "Email is required";
            if (!Regex.IsMatch(TextBox2.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Invalid Email Address";

            if (DropDownList1.SelectedIndex == 0)
                return "Please select a State";

            if (string.IsNullOrWhiteSpace(TextBox5.Text))
                return "City is required";

            if (string.IsNullOrWhiteSpace(TextBox7.Text))
                return "Pincode is required";
            if (!Regex.IsMatch(TextBox7.Text.Trim(), @"^\d{6}$"))
                return "Pincode must be 6 digits";

            if (string.IsNullOrWhiteSpace(TextBox6.Text))
                return "Address is required";

            if (string.IsNullOrWhiteSpace(TextBox8.Text))
                return "Member ID is required";

            if (string.IsNullOrWhiteSpace(TextBox9.Text))
                return "Password is required";

            return "";  // all input are correct
        }

        bool checkMemberExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id=@member_id", con);
                    cmd.Parameters.AddWithValue("@member_id", TextBox8.Text.Trim());

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

        void signUpNewUser()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO member_master_tbl
                        (full_name, dob, contact_no, email, state, city, pincode, full_address, member_id, password, account_status)
                        VALUES (@full_name, @dob, @contact_no, @email, @state, @city, @pincode, @full_address, @member_id, @password, @account_status)", con);

                    cmd.Parameters.AddWithValue("@full_name", TextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@dob", TextBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@contact_no", TextBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", TextBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@state", DropDownList1.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@city", TextBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@pincode", TextBox7.Text.Trim());
                    cmd.Parameters.AddWithValue("@full_address", TextBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@member_id", TextBox8.Text.Trim());

                    // Hashing the password
                    //string hashedPassword = GetSha256Hash(TextBox9.Text.Trim());
                    //cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@password", TextBox9.Text.Trim());

                    cmd.Parameters.AddWithValue("@account_status", "pending");

                    cmd.ExecuteNonQuery();

                    con.Close();
                    Response.Write("<script>alert('User Registration Successful! Go to user login');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        // Password hashing function
        //private string GetSha256Hash(string input)
        //{
        //    using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
        //    {
        //        byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        //        var builder = new System.Text.StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}
    }
}
