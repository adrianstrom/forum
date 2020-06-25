
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Forum_Team2
{

    public class Authenticator
    {
        

        // Connection string.
        SqlConnection con = new SqlConnection
        (ConfigurationManager.ConnectionStrings["ForumDatabaseConnectionString"].ConnectionString);

        // Outside of Web Forms page class, use HttpContext.Current.
        HttpContext context = HttpContext.Current;

        static string ComputeSha256Hash(string rawData) // https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        {
            //Mahesh Chand, published on sep 16, 2018. Downloaded 16.04.2019.

            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void SuccessAlert(string message)
        {
            context.Response.Write("<div class='alert alert-success'><strong>" + message +"</strong></div>");
        }

        public void FailAlert(string message)
        {
            context.Response.Write("<div class='alert alert-danger'><strong>" + message + "</strong></div>");
        }

        bool IsValidEmail(string email) // Check email, source: https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void CheckSession()
        {
            try
            {
                if (context.Session["username"] == null) // If session username is null.
                {
                    context.Response.Redirect("Default.aspx"); // Redirects to Default page.
                }
            }
            catch (Exception error) // Catches errors.
            {
                context.Response.Write(error.ToString());
            }
        }

        public void LoggedIn()
        {
            try
            {
                if (context.Session["username"] != null) // If session username is not null.
                {
                    context.Response.Redirect("Default.aspx"); // Redirects to Default page.
                }
            }
            catch (Exception error) // Catches errors.
            {
                context.Response.Write(error.ToString());
            }
        }

        public void CheckIfAdmin()
        {

            try
            {
                if (context.Session["username"] != null) // If session username is not null.
                {
                    // Inputs.
                    string userName = context.Session["username"].ToString();
                    string password = context.Session["password"].ToString();

                    // Stored procedure and parameters.
                    SqlCommand cmd = new SqlCommand("uspSelectUserLogin", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", userName); // Parameter username.
                    cmd.Parameters.AddWithValue("@password", ComputeSha256Hash(password)); // Parameter password.

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);  // Creates object of class SqlDataAdapter.
                    DataTable dt = new DataTable(); // Creates object of class DataTable.
                    sda.Fill(dt); // Fills DataTable.
                    con.Open(); // Opens connections.
                    int i = cmd.ExecuteNonQuery(); // Executes.
                    con.Close(); // Closes connection.

                    string role = dt.Rows[0][9].ToString(); // Role in the DataTable.

                    if (role == "2") // If user role is 2. 
                    {
                        // Do something.
                    }
                    else
                    {
                        context.Response.Redirect("/Default.aspx"); // Redirects to Default page.
                    }

                }
                else
                {
                    context.Response.Redirect("/Default.aspx"); // Redirects to Default page.
                }
            }
            catch (Exception error) // Catches errors.
            {
                context.Response.Write(error.ToString());
            }
        }

        public void LogIn(TextBox txtUserName, TextBox txtPassword)
        {
            try
            {
                // Stored procedure and parameters.
                SqlCommand cmd = new SqlCommand("uspSelectUserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", txtUserName.Text); // Parameter username.
                cmd.Parameters.AddWithValue("@password", ComputeSha256Hash(txtPassword.Text)); // Parameter password.

                SqlDataAdapter sda = new SqlDataAdapter(cmd); // Creates object of class SqlDataAdapter.
                DataTable dt = new DataTable(); // Creates object of class DataTable.
                sda.Fill(dt); // Fills DataTable.
                con.Open(); // Opens connection.
                cmd.ExecuteNonQuery(); // Executes.
                con.Close(); // Closes connection.            

                if (dt.Rows.Count > 0) // If there are rows in the database.
                {

                    string activated = dt.Rows[0][12].ToString(); // 0 = not activated, 1 = activated.

                    if ( activated == "0") // User is not activated.
                    {
                        FailAlert("Brukeren din er ikke aktivert");
                    }
                    else // User is activated.
                    {
                        string role = dt.Rows[0][9].ToString(); // Role in the datatable.

                        context.Session["username"] = txtUserName.Text; // Stores username.
                        context.Session["password"] = txtPassword.Text; // Stores password.
                        context.Session["userid"] = dt.Rows[0][0].ToString(); // Stores userid.

                        context.Response.Redirect("Default.aspx");

                        context.Session.RemoveAll(); // Removes all values from session collection.
                    }

                }
                else // Wrong combination of username and password.
                {
                    FailAlert("Feil kombinasjon av brukernavn og passord.");
                }
            }
            catch(System.Web.HttpRequestValidationException)
            {
                FailAlert("Slutt med det du holder på med!");
            }
            catch (Exception error) // Catches errors.
            {
                context.Response.Write(error.ToString());
            }
        }

        public void MassSendEmails (string subject, string body)
        {
            try
            {
                string reciever = ""; // Reciever

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); // Gmail smtp og port.
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("forumteam2@gmail.com", "vdjyleupec53bb32dghyxxzq8"); // Gmail login credentials.

                SqlCommand cmd = new SqlCommand("SELECT * FROM users", con);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);  // Creates object of class SqlDataAdapter.
                DataTable dt = new DataTable(); // Creates object of class DataTable.
                sda.Fill(dt); // Fills DataTable.

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    reciever = dt.Rows[i]["email"].ToString();
                    MailMessage mail = new MailMessage("forumteam2@gmail.com", reciever);
                    mail.Subject = subject; // Subject
                    mail.Body = body;

                    client.Send(mail);
                }
            }
            catch (System.FormatException error)
            {
                FailAlert(error.ToString());
            }

        }

        public void Confirmation(TextBox txtUserName, TextBox txtCertificationCode)
        {
            try
            {

                string userName = txtUserName.Text;
                string certificationCode = txtCertificationCode.Text;


                SqlCommand cmd = new SqlCommand("uspSelectCert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", userName); // Paramater username.

                SqlDataAdapter sda = new SqlDataAdapter(cmd);  // Creates object of class SqlDataAdapter.
                DataTable dt = new DataTable(); // Creates object of class DataTable.
                sda.Fill(dt); // Fills DataTable.
                con.Open(); // Opens connections.
                cmd.ExecuteNonQuery(); // Executes.
                con.Close(); // Closes connection.

                string dtCert = dt.Rows[0][0].ToString(); // Cert

                if (certificationCode == dtCert)
                {
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("uspActivateUser", con); // Activates user.
                    cmd2.CommandType = CommandType.StoredProcedure; // Stored Procedure.
                    cmd2.Parameters.AddWithValue("@username", userName); // Paramater username.

                    cmd2.ExecuteNonQuery(); // Executes Stored Procedure.
                    con.Close(); // Closes connection.

                    SuccessAlert("Brukeren din er nå aktivert, du kan derfor logge inn. ");
                }
                else
                {
                    FailAlert("Feil kode!");
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                FailAlert("Brukernavnet eksisterer ikke.");
            }
            
        }

        public void SendEmail (string reciever, string subject, string body)
        {
            MailMessage mail = new MailMessage("forumteam2@gmail.com", reciever); // Sends email from forumteam2@gmail.com to another email account.
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); // Gmail smtp og port.
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("forumteam2@gmail.com", "vdjyleupec53bb32dghyxxzq8"); // Gmail login credentials.
            mail.Subject = subject; // Subject
            mail.Body = body;

            client.Send(mail);
        }

        public void Register(TextBox txtUserName, TextBox txtPassword, TextBox txtPasswordCheck,
            TextBox txtEmail, DropDownList ddlGender, TextBox txtEducation, CheckBox chkTermsOfService)
        {
            try
            {
                // Inputs.
                Random random = new Random();

                string userName = txtUserName.Text;
                string password = ComputeSha256Hash(txtPassword.Text);
                string passwordCheck = ComputeSha256Hash(txtPasswordCheck.Text);
                string email = txtEmail.Text;
                string gender = ddlGender.SelectedItem.Value;
                string education = txtEducation.Text;

                int userNameCharacters = txtUserName.Text.Length;
                int passwordCharacters = txtPassword.Text.Length;
                int emailCharacters = txtEmail.Text.Length;
                int educationCharacters = txtEducation.Text.Length;
                int certificationCode = random.Next(100000, 100000000); 

                // Stored procedure and parameters.
                SqlCommand cmd = new SqlCommand("uspRegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", userName); // Paramater username.
                cmd.Parameters.AddWithValue("@password", password); // Parameter password.
                cmd.Parameters.AddWithValue("@email", email); // Paramter gender.
                cmd.Parameters.AddWithValue("@gender", gender); // Paramter gender.
                cmd.Parameters.AddWithValue("@education", education); // Paramater education.
                cmd.Parameters.AddWithValue("@date", DateTime.Now); // Parameter date.
                cmd.Parameters.AddWithValue("@certification_id", certificationCode); // Paramater cert.

                SqlCommand cmd2 = new SqlCommand("uspGetUserNames", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@username", userName); // Paramater username.

                SqlDataAdapter sda = new SqlDataAdapter(cmd2);  // Creates object of class SqlDataAdapter.
                DataTable dt = new DataTable(); // Creates object of class DataTable.
                sda.Fill(dt); // Fills DataTable.

                con.Open();
                cmd2.ExecuteNonQuery();
                con.Close();

                int dtRowsCount = dt.Rows.Count;

                if (educationCharacters >= 4) // If email characters is 4 or more.
                {

                    if ((gender == "Mann") || (gender == "Kvinne")) // If gender is man or woman.
                    {

                        if (IsValidEmail(email)) // If email is email.
                        {

                            if (passwordCharacters >= 6) // If password characters i 6 or more.
                            {

                                if (2 <= userNameCharacters && userNameCharacters <= 14) // If topicNameCharacters is between 2 and 14.
                                {

                                    if (password == passwordCheck) // If passwords matches.
                                    {

                                        if (chkTermsOfService.Checked) // If checkbox is checked.
                                        {

                                            if (dtRowsCount == 0) // If there are no users with input userName.
                                            {
                                                con.Open();
                                                cmd.ExecuteNonQuery(); // Executes Stored Procedure.
                                                con.Close(); // Closes connection.

                                                SendEmail(email, "Certification code", certificationCode.ToString()); // Sends email.
                                                
                                                SuccessAlert("Du er registrert som: " + userName + ". " + "Skriv inn koden du mottok på email og aktiver brukeren din.");
                                            }
                                            else // If there exists users with the same username.
                                            {
                                                FailAlert("Det eksisterer allerede en bruker med brukernavnet: " + userName);
                                            }

                                        }
                                        else // If checkbox is not checked.
                                        {
                                            FailAlert("Du godtok ikke brukervilkårene. For å bli registrert, godta brukervilkårene.");
                                        }

                                    }
                                    else // If passwords doesn't match.
                                    {
                                        FailAlert("Passordene samsvarer ikke.");
                                    }
                                }
                                else // If username characters is not between 2 and 14.
                                {
                                    FailAlert("Brukernavnet må ditt må ha mellom 2 og 14 karakterer.");
                                }
                            }
                            else // If password characters is less than 6.
                            {
                                FailAlert("Passordet ditt må ha flere enn seks karakterer!");
                            }
                        }
                        else // If email is not email.
                        {
                            FailAlert("Emailen din er ikke en ordentlig email!");
                        }
                    }
                    else // If gender is not man or woman.
                    {
                        FailAlert("Du kan ikke være en: " + gender + ". " + "Her godtas bare mann eller kvinne!");
                    }
                }
                else // If education character is less than 5.
                {
                    FailAlert("Utdanningen må ha flere enn 4 karakterer.");
                }
            }
            catch (System.FormatException error)
            {
                FailAlert(error.ToString());
            }
            catch (System.Net.Mail.SmtpException)
            {
                FailAlert("Det er noe galt med emailen din. Husk å bruke det engelske alfabetet.");
            }
            catch (System.ArgumentException)
            {
                FailAlert("Stop this sorcery!");
            }
        }
    }
}