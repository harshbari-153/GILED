using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.IO;
using System.Net.Http;
using System.Runtime;
using System.Runtime.InteropServices;


namespace Giled
{
    public partial class Giled : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Discription, int ReservedValue);
        public Giled()
        {
            InitializeComponent();
        }

        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "IfbQIWnn0aMhLQ8LN56BJsCBfmvePwf7ENbz9NLV",
            BasePath = "https://giled-730b5-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        Boolean check_conn()
        {
            int des;
            return InternetGetConnectedState(out des, 0);
        }

        int login()
        {
            DirectoryInfo dir = new DirectoryInfo(@"c:\Giled");
            DirectoryInfo dir2 = new DirectoryInfo(@"c:\Giled\userinfo");
            String email, password, id, name, number;
            

            if (!dir.Exists || !dir2.Exists)
            {
                return -1;
            }
            else if(!File.Exists(@"c:\Giled\userinfo\user.txt"))
            {
                return -2;
            }
            else
            {
                try
                {
                    StreamReader sr = new StreamReader(@"c:\Giled\userinfo\user.txt");
                    id = sr.ReadLine();
                    name = sr.ReadLine();
                    password = sr.ReadLine();
                    number = sr.ReadLine();
                    email = sr.ReadLine();
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return -3;
                }

                if (textBox1.Text == email && textBox3.Text == password)
                {
                    StreamWriter sw = new StreamWriter(@"c:\Giled\userinfo\user.txt");
                    sw.WriteLine(id);
                    sw.WriteLine(name);
                    sw.WriteLine(password);
                    sw.WriteLine(number);
                    sw.WriteLine(email);
                    sw.WriteLine("1");
                    sw.Close();
                    return 1;
                }
                else if (textBox1.Text == "")
                {
                    return -4;
                }
                else if (textBox3.Text == "")
                {
                    return -5;
                }
                else
                    return -6;

            }
        }

        Boolean show_error_message(int code)
        {
            if (code == -1)
            {
                MessageBox.Show("You have not registered");
            }

            else if (code == -2)
            {
                MessageBox.Show("Your application is corrupted");
            }

            else if (code == -3)
            {
                MessageBox.Show("Your login credintial is corrupted");
            }

            else if (code == -4)
            {
                MessageBox.Show("Enter your email");
                textBox1.Focus();
            }

            else if (code == -5)
            {
                MessageBox.Show("Enter your password");
                textBox3.Focus();
            }

            else if (code == -6)
            {
                MessageBox.Show("Invalid credintial");
                textBox3.Focus();
                textBox1.Focus();
            }

            else if (code == 1)
            {
                return true;
            }

            else
            {
                MessageBox.Show("Something went wrong in the application");
            }
            return false;
        }

        void switch_page()
        {
            //go to home page
            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible = false;
            this.Hide();
            HomePage homePage = new HomePage();
            homePage.Show();
        }
        //login button
        private void button1_Click(object sender, EventArgs e)
        {
                int login_status = login();
                if (show_error_message(login_status))
                {
                    switch_page();
                }
        }

        Boolean logged_in()
        {
            int login_status = login();

            if(login_status >= -3)
            {
                show_error_message(login_status);
                return false;
            }
            else
            {
                string log;
                StreamReader sr = new StreamReader(@"c:\Giled\userinfo\user.txt");
                log = sr.ReadLine();
                log = sr.ReadLine();
                log = sr.ReadLine();
                log = sr.ReadLine();
                log = sr.ReadLine();
                log = sr.ReadLine();
                sr.Close();

                if (log == "1")
                    return true;
                else
                    return false;
            }
        }

        int change = 0;

        private void Giled_Load(object sender, EventArgs e)
        {
            
            if (logged_in())
            {
                change = 1;
            }
            try
            {
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel1.Dock = DockStyle.Fill;
                client = new FireSharp.FirebaseClient(fcon);
                if (!(client != null))
                    MessageBox.Show("Not connected, error occured");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet Connectivity"+ex);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
           // panel3.Visible = false;
            panel2.Visible = false;
            panel4.Visible = true;
            panel4.Dock = DockStyle.Fill;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            //panel3.Visible = false;
            panel4.Visible = false;
            panel2.Visible = true;
            panel2.Dock = DockStyle.Fill;
        }

        Boolean validation()
        {
            if (String.IsNullOrEmpty(textBox4.Text))
            {
                panel3.Visible = false;
                MessageBox.Show("Enter Name");
                textBox4.Focus();
            }

            else if (String.IsNullOrEmpty(textBox5.Text))
            {
                panel3.Visible = false;
                MessageBox.Show("Enter Password");
                textBox5.Focus();
            }

            else if (String.IsNullOrEmpty(textBox6.Text) || textBox5.Text.Trim() != textBox6.Text.Trim())
            {
                panel3.Visible = false;
                MessageBox.Show("Password not matching");
                textBox6.Focus();
            }

            else if (String.IsNullOrEmpty(textBox7.Text) || textBox7.Text.Length != 10)
            {
                panel3.Visible = false;
                MessageBox.Show("Enter Mobile Number");
                textBox7.Focus();
            }

            else if (String.IsNullOrEmpty(textBox8.Text))
            {
                panel3.Visible = false;
                MessageBox.Show("Enter Email");
                textBox8.Focus();
            }

            else
            { return true; }
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                panel3.Visible = true;
                progressBar1.Value = 0;

                int ids;

                //validation of the form
                if (validation())
                {
                    progressBar1.Increment(20);
                    //establish connection
                    try
                    {
                        progressBar1.Increment(20);
                        var result = client.Get("Registries/" + "meta_data/" + "total_count");
                        ids = result.ResultAs<int>();
                        register std = new register()
                        {
                            username = textBox4.Text,
                            password = textBox5.Text,
                            phoneNo = textBox7.Text,
                            email = textBox8.Text,
                            total_org = "0",
                            id = ids.ToString(),
                        };
                        ids = ids + 1;
                        var setter = client.Set("Registries/" + std.id, std);
                        var setter2 = client.Set("Registries/" + "meta_data/" + "total_count", ids.ToString());
                        progressBar1.Increment(20);
                        if (setter2 != null && setter != null)
                        {
                            DirectoryInfo dir = new DirectoryInfo(@"c:\Giled");
                            try
                            {
                                if (!(dir.Exists))
                                {
                                    dir.Create();
                                    dir = new DirectoryInfo(@"C:\Giled\userinfo");
                                    dir.Create();
                                }
                                progressBar1.Increment(10);
                                StreamWriter sw = new StreamWriter(@"C:\Giled\userinfo\user.txt");
                                sw.WriteLine(ids - 1);
                                sw.WriteLine(textBox4.Text);
                                sw.WriteLine(textBox5.Text);
                                sw.WriteLine(textBox7.Text);
                                sw.WriteLine(textBox8.Text);
                                sw.WriteLine("0");
                                sw.Close();
                                progressBar1.Increment(10);
                            }
                            catch (Exception ex)
                            {
                                panel3.Visible = false;
                                MessageBox.Show(ex.Message);
                            }
                            progressBar1.Increment(20);
                            panel3.Visible = false;
                            MessageBox.Show("Registered Sucessfully");
                            MessageBox.Show("Enter your credentials to login");

                        }
                        else
                        {
                            panel3.Visible = false;
                            MessageBox.Show("Error Occured, Not Registered");
                        }
                        panel1.Visible = true;
                        panel2.Visible = false;
                        panel4.Visible = false;
                        panel3.Visible = false;
                        panel1.Dock = DockStyle.Fill;
                        // panel3.Visible = true;
                    }
                    catch (HttpRequestException ex)
                    {
                        panel3.Visible = false;
                        MessageBox.Show(ex.Message, "Error");
                    }
                    catch (Exception ex)
                    {
                        panel3.Visible = false;
                        MessageBox.Show("No Internet Connective " + ex.Message);
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            //panel3.Visible = false;
            panel4.Visible = false;
            panel1.Visible = true;
            panel1.Dock = DockStyle.Fill;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
           // panel3.Visible = false;
            panel4.Visible = false;
            panel1.Visible = true;
            panel1.Dock = DockStyle.Fill;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
           // panel3.Visible = false;
            panel4.Visible = false;
            panel1.Visible = true;
            panel1.Dock = DockStyle.Fill;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                panel1.Visible = false;
                //panel3.Visible = false;
                panel4.Visible = false;
                panel2.Visible = true;
                panel2.Dock = DockStyle.Fill;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if(!Char.IsDigit(c) && c!=8 && c!=46)
                e.Handled = true;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            // panel3.Visible = false;
            panel4.Visible = false;
            panel2.Visible = true;
            panel2.Dock = DockStyle.Fill;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            button7.BackColor = Color.FromArgb(212, 23, 23);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(4, 80, 169);
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            button8.BackColor = Color.FromArgb(4, 169, 20);
        }

        private void button7_MouseHover(object sender, EventArgs e)
        {
            button7.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button8_MouseHover(object sender, EventArgs e)
        {
            button8.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(4, 169, 20);
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(235, 116, 31);
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(211, 231, 228);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(235, 116, 31);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(212, 23, 23);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(4, 169, 20);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (change == 1)
            {
                timer1.Enabled = false;
                //change = 0;
                switch_page();
            }

            timer1.Enabled = false;


        }
    }
}
