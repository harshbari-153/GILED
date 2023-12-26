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
using Microsoft.Office.Interop.Excel;
using System.Runtime;
using System.Runtime.InteropServices;
namespace Giled
{
    public partial class HomePage : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Discription, int ReservedValue);
        public HomePage()
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {

                label11.Text = "Creating Organisation.....";
                /*
                if (String.IsNullOrEmpty(textBox4.Text))
                {
                    label15.Text = "Enter Name";
                    label19.Text = "";
                    label16.Text = "";
                    label18.Text = "";
                    label20.Text = "";
                    label17.Text = "";
                    textBox4.Focus();
                }
                else if (String.IsNullOrEmpty(textBox5.Text))
                {
                    label19.Text = "Enter Password";
                    label15.Text = "";
                    label16.Text = "";
                    label18.Text = "";
                    label20.Text = "";
                    label17.Text = "";
                    textBox5.Focus();
                }
                else if (String.IsNullOrEmpty(textBox6.Text) || textBox5.Text.Trim() != textBox6.Text.Trim())
                {
                    if (String.IsNullOrEmpty(textBox6.Text))
                        label16.Text = "Enter Password";
                    else
                        label16.Text = "Password Not Matching";
                    label19.Text = "";
                    label15.Text = "";
                    label18.Text = "";
                    label20.Text = "";
                    label17.Text = "";
                    textBox6.Focus();
                }

                else if (String.IsNullOrEmpty(textBox7.Text) || textBox7.Text.Length != 10)
                {
                    label18.Text = "Enter 10 digit Mobile No";
                    label19.Text = "";
                    label16.Text = "";
                    label15.Text = "";
                    label20.Text = "";
                    label17.Text = "";
                    textBox7.Focus();
                }
                else if (String.IsNullOrEmpty(textBox8.Text))
                {
                    label20.Text = "Enter Email";
                    label19.Text = "";
                    label16.Text = "";
                    label18.Text = "";
                    label15.Text = "";
                    label17.Text = "";
                    textBox8.Focus();
                }
                else if (!(radioButton1.Checked || radioButton2.Checked))
                {
                    label17.Text = "Select Gender";
                    label19.Text = "";
                    label16.Text = "";
                    label18.Text = "";
                    label20.Text = "";
                    label15.Text = "";
                }*/
                // else
                //  {
                panel2.Visible = true;

                progressBar1.Value = 0;
                progressBar1.Increment(1);
                int ids;
                string ida;
                try
                {
                    StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                    ida = sr.ReadLine();
                    sr.Close();
                    progressBar1.Increment(1);
                    var result = client.Get("Organisation/meta_data/total_count");
                    ids = result.ResultAs<int>();
                    int total_org;

                    result = client.Get("Registries/" + ida + "/total_org");
                    total_org = result.ResultAs<int>();

                    client.Set("Registries/" + ida + "/organisation" + total_org.ToString(), ids.ToString());
                    total_org += 1;
                    client.Set("Registries/" + ida + "/total_org", total_org.ToString());

                    organisation std = new organisation()
                    {
                        name = textBox1.Text,
                        type = textBox2.Text,
                        owner = textBox3.Text,
                        place = textBox4.Text,
                        state = textBox5.Text,
                        country = textBox6.Text,
                        miner_id = ida,
                        total_leger = "0",
                        total_miner = "1",
                        id = ids.ToString(),
                    };
                    var setter = client.Set("Organisation/" + ids.ToString(), std);
                    progressBar1.Increment(1);
                    ids = ids + 1;
                    var setter2 = client.Set("Organisation/" + "meta_data/" + "total_count", ids.ToString());




                    progressBar1.Increment(1);

                    if ((setter != null) && (setter2 != null))
                    {
                        progressBar1.Increment(1);

                        DirectoryInfo dir = new DirectoryInfo(@"c:\Giled\organisations");
                        try
                        {
                            if (!(dir.Exists))
                            {
                                dir.Create();
                            }
                            progressBar1.Increment(1);
                            StreamWriter sw;
                            if (!(File.Exists(@"C:\Giled\organisations\meta_data.txt")))
                            {
                                sw = new StreamWriter(@"C:\Giled\organisations\meta_data.txt");
                                sw.WriteLine("1");
                                sw.WriteLine(--ids);
                                sw.WriteLine(textBox1.Text);
                                sw.Close();
                                dir = new DirectoryInfo(@"c:\Giled\organisations\" + textBox1.Text);
                                dir.Create();
                                progressBar1.Increment(1);
                            }
                            else
                            {
                                int i;
                                string[] org_id = new string[50];
                                string[] org_name = new string[50];
                                sr = new StreamReader(@"C:\Giled\organisations\meta_data.txt");
                                ida = sr.ReadLine();

                                //read line
                                for (i = 0; i < int.Parse(ida); i++)
                                {
                                    org_id[i] = sr.ReadLine();
                                    org_name[i] = sr.ReadLine();
                                    progressBar1.Increment(1);
                                }
                                sr.Close();
                                progressBar1.Increment(1);

                                sw = new StreamWriter(@"C:\Giled\organisations\meta_data.txt");
                                sw.WriteLine((int.Parse(ida) + 1).ToString());
                                //write line
                                for (i = 0; i < int.Parse(ida); i++)
                                {
                                    sw.WriteLine(org_id[i]);
                                    sw.WriteLine(org_name[i]);
                                    progressBar1.Increment(1);
                                }
                                sw.WriteLine(--ids);
                                sw.WriteLine(textBox1.Text);
                                sw.Close();
                                dir = new DirectoryInfo(@"C:\Giled\organisations\" + textBox1.Text);
                                dir.Create();

                            }
                            sw = new StreamWriter(@"C:\Giled\organisations\" + textBox1.Text + "\\meta_data.txt");
                            sw.WriteLine("0");
                            sw.Close();
                            panel2.Visible = false;
                            MessageBox.Show("Registered Sucessfully");
                            Ledger lg = new Ledger(textBox1.Text, ids.ToString());


                            Hide();
                            lg.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            panel2.Visible = false;
                            MessageBox.Show(ex.Message);
                            //MessageBox.Show("hello");
                        }
                    }
                    else
                    {
                        panel2.Visible = false;
                        MessageBox.Show("Error Occured, Not Registered");
                    }
                    panel1.Visible = false;
                    panel3.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            int i;
            string[] org_id = new string[50];
            string[] org_name = new string[50];
            string ida;
            try
            {
                panel8.Visible = false;
                panel1.Visible = false;
                panel3.Visible = true;
                panel3.Dock = DockStyle.Fill;
                panel2.Visible = false;
                client = new FireSharp.FirebaseClient(fcon);
                //load the organisations
                DirectoryInfo dir = new DirectoryInfo(@"c:\Giled\organisations");
                if (dir.Exists)
                {
                    label7.Text = "My Organization";
                    //read lines
                    StreamReader sr = new StreamReader(@"C:\Giled\organisations\meta_data.txt");
                    ida = sr.ReadLine();
                    //read line
                    for (i = 0; i < int.Parse(ida); i++)
                    {
                        org_id[i] = sr.ReadLine();
                        org_name[i] = sr.ReadLine();
                    }
                    sr.Close();
                    //file read
                    //setting into list view
                    ListViewItem item;
                    for (i = 0; i < int.Parse(ida); i++)
                    {
                        item = new ListViewItem(org_name[i]);
                        item.SubItems.Add(org_id[i]);
                        listView1.Items.Add(item);
                    }
                    //setted into list view
                }
                else
                {
                    label7.Text = "No Organisations Added";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet Connectivity" + ex);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i;
            panel2.Visible = true;
            label11.Text = "Joining Organisation.....";
            progressBar1.Value = 0;
            progressBar1.Increment(1);

            //reading self id
            string ida;
            StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
            ida = sr.ReadLine();
            sr.Close();

            progressBar1.Increment(1);

            //finding the organisation
            var result = client.Get("Organisation/" + textBox7.Text);
            organisation org = result.ResultAs<organisation>();

            //if org found
            if (org != null)
            {
                int total_org, total_leg, leg_id, total_fields, j;
                String leg_name;

                //adding org in self registries
                result = client.Get("Registries/" + ida + "/total_org");
                total_org = result.ResultAs<int>();
                client.Set("Registries/" + ida + "/organisation" + total_org.ToString(), textBox7.Text);
                total_org += 1;
                client.Set("Registries/" + ida + "/total_org", total_org.ToString());


                //adding user info to selected organisation
                int ids = int.Parse(org.total_miner);
                string s = "/miner" + ids.ToString();
                var v1 = client.Set("Organisation/" + textBox7.Text + s, ida);
                ids = ids + 1;
                var v2 = client.Set("Organisation/" + textBox7.Text + "/total_miner", ids.ToString());


                //setting org  info to local machine
                //create directory
                DirectoryInfo dir = new DirectoryInfo(@"c:\Giled\organisations");
                progressBar1.Increment(1);

                try
                {
                    if (!(dir.Exists))
                    {
                        dir.Create();
                    }
                    StreamWriter sw;
                    //for first entry
                    if (!(File.Exists(@"C:\Giled\organisations\meta_data.txt")))
                    {
                        sw = new StreamWriter(@"C:\Giled\organisations\meta_data.txt");
                        sw.WriteLine("1");
                        sw.WriteLine(org.id);
                        sw.WriteLine(org.name);
                        sw.Close();
                        dir = new DirectoryInfo(@"C:\Giled\organisations\" + org.name);
                        dir.Create();
                        progressBar1.Increment(1);
                    }
                    //already has some data
                    else
                    {
                        
                        string[] org_id = new string[50];
                        string[] org_name = new string[50];
                        sr = new StreamReader(@"C:\Giled\organisations\meta_data.txt");
                        ida = sr.ReadLine();


                        //read line
                        for (i = 0; i < int.Parse(ida); i++)
                        {
                            org_id[i] = sr.ReadLine();
                            org_name[i] = sr.ReadLine();
                            progressBar1.Increment(1);
                        }
                        sr.Close();
                        sw = new StreamWriter(@"C:\Giled\organisations\meta_data.txt");
                        sw.WriteLine((int.Parse(ida) + 1).ToString());


                        //write line
                        for (i = 0; i < int.Parse(ida); i++)
                        {
                            sw.WriteLine(org_id[i]);
                            sw.WriteLine(org_name[i]);
                            progressBar1.Increment(1);
                        }
                        sw.WriteLine(org.id);
                        sw.WriteLine(org.name);
                        sw.Close();
                        dir = new DirectoryInfo(@"c:\Giled\organisations\" + org.name);
                        dir.Create();
                    }


                    //collect the total ledgers
                    progressBar1.Increment(1);
                    result = client.Get("Organisation/" + textBox7.Text + "/total_leger");
                    total_leg = result.ResultAs<int>();


                    //set the meta data
                    progressBar1.Increment(1);
                    sw = new StreamWriter(@"C:\Giled\organisations\" + org.name + "\\meta_data.txt");
                    sw.WriteLine(total_leg);
                    for (i = 0; i < total_leg; i++)
                    {
                        progressBar1.Increment(1);
                        result = client.Get("Organisation/" + textBox7.Text + "/ledger_" + i + "_id");
                        leg_id = result.ResultAs<int>();
                        sw.WriteLine(leg_id);
                        result = client.Get("Ledgers/" + leg_id + "/ledger_name");
                        leg_name = result.ResultAs<String>();
                        sw.WriteLine(leg_name);
                    }
                    sw.Close ();



                    //now set each ledger to local system
                    //this is done by adding the structure and the ledger simultaneously
                    String ck_box = null, field_id, field;
                    int reduce = 0, cell;
                    for (i = 0; i < total_leg; i++)
                    {
                        progressBar1.Increment(1);

                        //take ledger id
                        result = client.Get("Organisation/" + textBox7.Text + "/ledger_" + i + "_id");
                        leg_id = result.ResultAs<int>();
                        //take ledger name
                        result = client.Get("Ledgers/" + leg_id + "/ledger_name");
                        leg_name = result.ResultAs<String>();

                        //now add the ledger to local machine
                        //create new folder for ledger
                        dir = new DirectoryInfo(@"c:\Giled\organisations\" + org.name + "\\" + leg_name);
                        dir.Create();

                        //get total fields
                        result = client.Get("Ledgers/" + leg_id + "/total_fields");
                        total_fields = result.ResultAs<int>();



                        //create actual ledger

                        //file object
                        Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                        //workbook object
                        Workbook wb;

                        //worksheet object
                        Worksheet ws;
                        wb = excel.Workbooks.Add();
                        ws = wb.Worksheets[1];

                        //set first row
                        ws.Cells[1, 1].Value = "0";
                        ws.Cells[1, 2].Value = "0";
                        
                        ws.Cells[1, 4].Value = org.name;
                        ws.Cells[1, 5].Value = textBox7.Text;
                        ws.Cells[1, 6].Value = leg_name;
                        ws.Cells[1, 7].Value = leg_id;
                        ws.Cells[1, 8].Value = 0;

                        //set second row
                        ws.Cells[2, 2].Value = "Entry No";
                        ws.Cells[2, 3].Value = "Previous Hash";
                        ws.Cells[2, 4].Value = "Current Hash";
                        ws.Cells[2, 5].Value = "Miner Id";
                        ws.Cells[2, 6].Value = "Nonce";
                        ws.Cells[2, 7].Value = "Id";
                        ws.Cells[2, 8].Value = "Lock";

                        //create ledger structure file
                        sw = new StreamWriter(@"C:\Giled\organisations\" + org.name + "\\" + leg_name + "\\ledger_structure.txt");
                        sw.WriteLine(total_fields);

                        for (j=0, cell=0, reduce=0; j< total_fields; j++)
                        {
                            progressBar1.Increment(1);

                            result = client.Get("Ledgers/" + leg_id + "//" + j);
                            field = result.ResultAs<String>();

                            sw.WriteLine(field);

                            field_id = field.Substring(0, 2);
                            field = field.Substring(3);

                            if(field_id == "00" || field_id == "01")
                            {
                                ws.Cells[2, 9 + cell].Value = field;
                                    cell++;
                            }
                            else if(field_id == "02")
                            {
                                reduce++;
                            }
                            else if(field_id == "03")
                            {
                                ck_box = field;
                                reduce++;
                            }
                            else if(field_id == "04")
                            {
                                ws.Cells[2, 9 + cell].Value = ck_box + ":-" + field;
                                cell++;
                            }
                            else
                            {
                                MessageBox.Show("Something Error in the ledger structure");
                            }

                        }
                        ws.Cells[1, 3].Value = total_fields - reduce;
                        //close the process

                        //close structure
                        sw.Close();
                        //close the ledger
                        wb.SaveAs(@"C:\Giled\organisations\" + org.name + "\\" + leg_name + "\\" + leg_name + ".xlsx");
                        wb.Close(0);
                        excel.Quit();

                    }




                    //finally success
                    MessageBox.Show("Organisation Added");
                    Ledger lg = new Ledger(org.name, org.id);
                    progressBar1.Increment(1);
                    panel2.Visible = false;
                    Hide();
                    lg.ShowDialog();

                }
                catch (Exception ex)
                {
                    panel2.Visible = false;
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                panel2.Visible = false;
                MessageBox.Show("No Such Organisation with id: " + textBox7.Text + " Found");
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Ledger lg = new Ledger(listView1.SelectedItems[0].Text, listView1.SelectedItems[0].SubItems[1].Text);
            Hide();
            lg.ShowDialog();
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel3.Visible = true;
        }

        private void label15_Click(object sender, EventArgs e)
        {
            string id = "0", name = "0", password = "0", number = "0", email = "0";
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
            }

                StreamWriter sw = new StreamWriter(@"c:\Giled\userinfo\user.txt");
                sw.WriteLine(id);
                sw.WriteLine(name);
                sw.WriteLine(password);
                sw.WriteLine(number);
                sw.WriteLine(email);
                sw.WriteLine("0");
                sw.Close();
                //
                Giled giled = new Giled();
            giled.Show();
            Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel1.Visible = false;
            panel3.Visible = true;
            panel3.Dock = DockStyle.Fill;
        }

        private void label22_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel3.Visible = false;
            panel1.Visible = true;
            panel1.Dock = DockStyle.Fill;
        }

        private void label23_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel1.Visible = false;
            panel8.Visible = true;
            panel8.Dock = DockStyle.Fill;
        }

        private void label21_MouseHover(object sender, EventArgs e)
        {
            panel16.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label21_MouseLeave(object sender, EventArgs e)
        {
            panel16.BackColor = Color.FromArgb(3, 150, 159);
        }

        private void label22_MouseHover(object sender, EventArgs e)
        {
            panel17.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label22_MouseLeave(object sender, EventArgs e)
        {
            panel17.BackColor = Color.FromArgb(3, 150, 159);
        }

        private void label23_MouseHover(object sender, EventArgs e)
        {
            panel18.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label23_MouseLeave(object sender, EventArgs e)
        {
            panel18.BackColor = Color.FromArgb(3, 150, 159);
        }

        private void label15_MouseHover(object sender, EventArgs e)
        {
            panel6.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label15_MouseLeave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.FromArgb(19, 103, 130);
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                int i;
                panel2.Visible = true;
                label11.Text = "Joining Organisation.....";
                progressBar1.Value = 0;
                progressBar1.Increment(1);

                //reading self id
                string ida;
                StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                ida = sr.ReadLine();
                sr.Close();

                progressBar1.Increment(1);

                //finding the organisation
                var result = client.Get("Organisation/" + textBox7.Text);
                organisation org = result.ResultAs<organisation>();

                //if org found
                if (org != null)
                {
                    int total_org, total_leg, leg_id, total_fields, j;
                    String leg_name;

                    //adding org in self registries
                    result = client.Get("Registries/" + ida + "/total_org");
                    total_org = result.ResultAs<int>();
                    client.Set("Registries/" + ida + "/organisation" + total_org.ToString(), textBox7.Text);
                    total_org += 1;
                    client.Set("Registries/" + ida + "/total_org", total_org.ToString());


                    //adding user info to selected organisation
                    int ids = int.Parse(org.total_miner);
                    string s = "/miner" + ids.ToString();
                    var v1 = client.Set("Organisation/" + textBox7.Text + s, ida);
                    ids = ids + 1;
                    var v2 = client.Set("Organisation/" + textBox7.Text + "/total_miner", ids.ToString());


                    //setting org  info to local machine
                    //create directory
                    DirectoryInfo dir = new DirectoryInfo(@"c:\Giled\organisations");
                    progressBar1.Increment(1);

                    try
                    {
                        if (!(dir.Exists))
                        {
                            dir.Create();
                        }
                        StreamWriter sw;
                        //for first entry
                        if (!(File.Exists(@"C:\Giled\organisations\meta_data.txt")))
                        {
                            sw = new StreamWriter(@"C:\Giled\organisations\meta_data.txt");
                            sw.WriteLine("1");
                            sw.WriteLine(org.id);
                            sw.WriteLine(org.name);
                            sw.Close();
                            dir = new DirectoryInfo(@"C:\Giled\organisations\" + org.name);
                            dir.Create();
                            progressBar1.Increment(1);
                        }
                        //already has some data
                        else
                        {

                            string[] org_id = new string[50];
                            string[] org_name = new string[50];
                            sr = new StreamReader(@"C:\Giled\organisations\meta_data.txt");
                            ida = sr.ReadLine();


                            //read line
                            for (i = 0; i < int.Parse(ida); i++)
                            {
                                org_id[i] = sr.ReadLine();
                                org_name[i] = sr.ReadLine();
                                progressBar1.Increment(1);
                            }
                            sr.Close();
                            sw = new StreamWriter(@"C:\Giled\organisations\meta_data.txt");
                            sw.WriteLine((int.Parse(ida) + 1).ToString());


                            //write line
                            for (i = 0; i < int.Parse(ida); i++)
                            {
                                sw.WriteLine(org_id[i]);
                                sw.WriteLine(org_name[i]);
                                progressBar1.Increment(1);
                            }
                            sw.WriteLine(org.id);
                            sw.WriteLine(org.name);
                            sw.Close();
                            dir = new DirectoryInfo(@"c:\Giled\organisations\" + org.name);
                            dir.Create();
                        }


                        //collect the total ledgers
                        progressBar1.Increment(1);
                        result = client.Get("Organisation/" + textBox7.Text + "/total_leger");
                        total_leg = result.ResultAs<int>();


                        //set the meta data
                        progressBar1.Increment(1);
                        sw = new StreamWriter(@"C:\Giled\organisations\" + org.name + "\\meta_data.txt");
                        sw.WriteLine(total_leg);
                        for (i = 0; i < total_leg; i++)
                        {
                            progressBar1.Increment(1);
                            result = client.Get("Organisation/" + textBox7.Text + "/ledger_" + i + "_id");
                            leg_id = result.ResultAs<int>();
                            sw.WriteLine(leg_id);
                            result = client.Get("Ledgers/" + leg_id + "/ledger_name");
                            leg_name = result.ResultAs<String>();
                            sw.WriteLine(leg_name);
                        }
                        sw.Close();



                        //now set each ledger to local system
                        //this is done by adding the structure and the ledger simultaneously
                        String ck_box = null, field_id, field;
                        int reduce = 0, cell;
                        for (i = 0; i < total_leg; i++)
                        {
                            progressBar1.Increment(1);

                            //take ledger id
                            result = client.Get("Organisation/" + textBox7.Text + "/ledger_" + i + "_id");
                            leg_id = result.ResultAs<int>();
                            //take ledger name
                            result = client.Get("Ledgers/" + leg_id + "/ledger_name");
                            leg_name = result.ResultAs<String>();

                            //now add the ledger to local machine
                            //create new folder for ledger
                            dir = new DirectoryInfo(@"c:\Giled\organisations\" + org.name + "\\" + leg_name);
                            dir.Create();

                            //get total fields
                            result = client.Get("Ledgers/" + leg_id + "/total_fields");
                            total_fields = result.ResultAs<int>();



                            //create actual ledger

                            //file object
                            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                            //workbook object
                            Workbook wb;

                            //worksheet object
                            Worksheet ws;
                            wb = excel.Workbooks.Add();
                            ws = wb.Worksheets[1];

                            //set first row
                            ws.Cells[1, 1].Value = "0";
                            ws.Cells[1, 2].Value = "0";

                            ws.Cells[1, 4].Value = org.name;
                            ws.Cells[1, 5].Value = textBox7.Text;
                            ws.Cells[1, 6].Value = leg_name;
                            ws.Cells[1, 7].Value = leg_id;
                            ws.Cells[1, 8].Value = 0;

                            //set second row
                            ws.Cells[2, 2].Value = "Entry No";
                            ws.Cells[2, 3].Value = "Previous Hash";
                            ws.Cells[2, 4].Value = "Current Hash";
                            ws.Cells[2, 5].Value = "Miner Id";
                            ws.Cells[2, 6].Value = "Nonce";
                            ws.Cells[2, 7].Value = "Id";
                            ws.Cells[2, 8].Value = "Lock";

                            //create ledger structure file
                            sw = new StreamWriter(@"C:\Giled\organisations\" + org.name + "\\" + leg_name + "\\ledger_structure.txt");
                            sw.WriteLine(total_fields);

                            for (j = 0, cell = 0, reduce = 0; j < total_fields; j++)
                            {
                                progressBar1.Increment(1);

                                result = client.Get("Ledgers/" + leg_id + "//" + j);
                                field = result.ResultAs<String>();

                                sw.WriteLine(field);

                                field_id = field.Substring(0, 2);
                                field = field.Substring(3);

                                if (field_id == "00" || field_id == "01")
                                {
                                    ws.Cells[2, 9 + cell].Value = field;
                                    cell++;
                                }
                                else if (field_id == "02")
                                {
                                    reduce++;
                                }
                                else if (field_id == "03")
                                {
                                    ck_box = field;
                                    reduce++;
                                }
                                else if (field_id == "04")
                                {
                                    ws.Cells[2, 9 + cell].Value = ck_box + ":-" + field;
                                    cell++;
                                }
                                else
                                {
                                    MessageBox.Show("Something Error in the ledger structure");
                                }

                            }
                            ws.Cells[1, 3].Value = total_fields - reduce;
                            //close the process

                            //close structure
                            sw.Close();
                            //close the ledger
                            wb.SaveAs(@"C:\Giled\organisations\" + org.name + "\\" + leg_name + "\\" + leg_name + ".xlsx");
                            wb.Close(0);
                            excel.Quit();

                        }




                        //finally success
                        MessageBox.Show("Organisation Added");
                        Ledger lg = new Ledger(org.name, org.id);
                        progressBar1.Increment(1);
                        panel2.Visible = false;
                        Hide();
                        lg.ShowDialog();

                    }
                    catch (Exception ex)
                    {
                        panel2.Visible = false;
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    panel2.Visible = false;
                    MessageBox.Show("No Such Organisation with id: " + textBox7.Text + " Found");
                }
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}