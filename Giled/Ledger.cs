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
using System.Security.Cryptography;
using Microsoft.Office.Interop.Excel;
using System.Runtime;
using System.Runtime.InteropServices;
namespace Giled
{
    public partial class Ledger : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Discription, int ReservedValue);

        int i = 1;
        int[] element_code = new int[50];
        String[] element_name = new String[50];
        int total_element = 0;

        int total_textBoxes = 0;
        int total_radioButtons = 0;
        int total_checkBoxes = 0;

        System.Windows.Forms.TextBox[] textBoxes = new System.Windows.Forms.TextBox[50];
        RadioButton[] radioButtons = new RadioButton[50];
        System.Windows.Forms.CheckBox[] checkBoxes = new System.Windows.Forms.CheckBox[50];


        private System.Windows.Forms.TextBox addTextBox(String field_name)
        {
            System.Windows.Forms.TextBox t_box = new System.Windows.Forms.TextBox();
            this.Controls.Add(t_box);

            //add pannel
            Panel panel = new Panel();
            flowLayoutPanel1.Controls.Add(panel);
            panel.Height = 20;
            panel.Width = 500;

            //add label
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.Text = field_name;
            flowLayoutPanel1.Controls.Add(label);
            label.Height = 20;
            label.Width = 450;

            //add textbox
            flowLayoutPanel1.Controls.Add(t_box);
            t_box.Height = 20;
            t_box.Width = 450;
            return t_box;
        }

        private System.Windows.Forms.RadioButton addRadioButton(System.Windows.Forms.GroupBox gb, String name, String field_name, int i)
        {
            RadioButton r_btn = new RadioButton();
            //this.Controls.Add(r_btn);

            //add radio button
            gb.Controls.Add(r_btn);
            r_btn.Name = name;
            r_btn.Text = field_name;
            r_btn.TextAlign = ContentAlignment.MiddleLeft;
            r_btn.Location = new System.Drawing.Point((i % 2) * 360, ((i + 2) / 2) * 20);
            return r_btn;
        }


        private System.Windows.Forms.CheckBox addCheckBox(System.Windows.Forms.GroupBox gb, String name, String field_name, int i)
        {
            System.Windows.Forms.CheckBox c_box = new System.Windows.Forms.CheckBox();
            //this.Controls.Add(c_box);

            //add check box
            gb.Controls.Add(c_box);
            c_box.Name = name;
            c_box.Text = field_name;
            c_box.TextAlign = ContentAlignment.MiddleLeft;
            c_box.Location = new System.Drawing.Point((i % 2) * 360, ((i + 2) / 2) * 20);
            return c_box;
        }



        public Ledger(string name, string ids)
        {
            InitializeComponent();
            label1.Text = name;
            label2.Text = ids;
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
        private void Ledger_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel1.Dock = DockStyle.Fill;
                client = new FireSharp.FirebaseClient(fcon);
                if (client == null)
                    MessageBox.Show("Not connected, error occured");
                int i;
                string[] org_id = new string[50];
                string[] org_name = new string[50];
                string ida;

                DirectoryInfo dir = new DirectoryInfo(@"C:\Giled\organisations\" + label1.Text);
                if (dir.Exists)
                {
                    label9.Text = "";
                    //read lines
                    StreamReader sr = new StreamReader(@"C:\Giled\organisations\" + label1.Text + "\\meta_data.txt");
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
                    label9.Text = "No Ledgers Added";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet Connectivity" + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            button9.Visible = true;
            button11.Visible = false;
            ListViewItem item;
            item = new ListViewItem("radio_field");
            item.SubItems.Add(textBox1.Text);
            listView3.Items.Add(item);
            i = (i + 1) % 2;
            item.BackColor = i % 2 == 0 ? Color.LightGray : Color.White;
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            item = new ListViewItem("radio_btn");
            item.SubItems.Add(textBox1.Text);
            listView3.Items.Add(item);
            item.BackColor = i % 2 == 0 ? Color.LightGray : Color.White;
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            ListViewItem item;
            item = new ListViewItem("check_box");
            item.SubItems.Add(textBox1.Text);
            listView3.Items.Add(item);
            item.BackColor = i % 2 == 0 ? Color.LightGray : Color.White;
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button9.Visible = false;
            button11.Visible = false;
            ListViewItem item;
            item = new ListViewItem("text");
            item.SubItems.Add(textBox1.Text);
            listView3.Items.Add(item);
            i = (i + 1) % 2;
            item.BackColor = i % 2 == 0 ? Color.LightGray : Color.White;
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            button9.Visible = false;
            button11.Visible = true;
            ListViewItem item;
            item = new ListViewItem("checkbox_field");
            item.SubItems.Add(textBox1.Text);
            listView3.Items.Add(item);
            i = (i + 1) % 2;
            item.BackColor = i % 2 == 0 ? Color.LightGray : Color.White;
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int j;
            for (j = 0; j < listView3.Items.Count; j++)
                if (listView3.Items[j].Checked != false)
                {
                    if (listView3.Items[j].SubItems[0].Text == "text" || listView3.Items[j].SubItems[0].Text == "radio_field" || listView3.Items[j].SubItems[0].Text == "checkbox_field")
                        i = (i + 1) % 2;
                    listView3.Items.RemoveAt(j);
                    j--;
                }
            listView3.Refresh();
            if (listView3.Items.Count >= 1)
            {
                if (listView3.Items[listView3.Items.Count - 1].SubItems[0].Text == "checkbox_field" || listView3.Items[listView3.Items.Count - 1].SubItems[0].Text == "check_box")
                {
                    button11.Visible = true;
                }
                if (listView3.Items[listView3.Items.Count - 1].SubItems[0].Text == "radio_field" || listView3.Items[listView3.Items.Count - 1].SubItems[0].Text == "radio_btn")
                {
                    button9.Visible = true;
                }
            }
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(comboBox1.Text);
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel1.Dock= DockStyle.Fill;
        }

        Boolean check_validity()
        {
            //ledger name must not be empty
            if (textBox2.Text == "")
            {
                panel4.Visible = false;
                MessageBox.Show("Please enter name of ledger");
                textBox2.Focus();
                return false;
            }
            //atleast one input data must be there
            else if (listView3.Items.Count == 0)
            {
                panel4.Visible = false;
                MessageBox.Show("There must be minimum 1 data field");
                textBox1.Focus();
                return false;
            }
            else if (comboBox1.Text == "Select Level")
            {
                panel4.Visible = false;
                MessageBox.Show("Select your level of hashing");
                comboBox1.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        Boolean ledger_structure_validity()
        {
            int i;

            //verify the ledger structure
            for (i = 0; i < listView3.Items.Count; i++)
            {
                if (listView3.Items[i].SubItems[0].Text == "checkbox_field")
                {
                    if ((i == (listView3.Items.Count - 1)) || (listView3.Items[i + 1].SubItems[0].Text != "check_box"))
                    {
                        panel4.Visible = false;
                        MessageBox.Show("Invalid Form Structure");
                        return false;
                    }
                }

                if (listView3.Items[i].SubItems[0].Text == "radio_field")
                {
                    if ((i == (listView3.Items.Count - 1)) || (listView3.Items[i + 1].SubItems[0].Text != "radio_btn"))
                    {
                        panel4.Visible = false;
                        MessageBox.Show("Invalid Form Structure");
                        return false;
                    }
                }
                progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
            }
            return true;
        }

        void create_actual_ledger(int ids, int a, int b)
        {
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
            ws.Cells[1, 3].Value = listView3.Items.Count - a - b;
            ws.Cells[1, 4].Value = label1.Text;
            ws.Cells[1, 5].Value = label2.Text;
            ws.Cells[1, 6].Value = textBox2.Text;
            ws.Cells[1, 7].Value = ids;
            ws.Cells[1, 8].Value = 0;

            //set second row
            ws.Cells[2, 2].Value = "Entry No";
            ws.Cells[2, 3].Value = "Previous Hash";
            ws.Cells[2, 4].Value = "Current Hash";
            ws.Cells[2, 5].Value = "Miner Id";
            ws.Cells[2, 6].Value = "Nonce";
            ws.Cells[2, 7].Value = "Id";
            ws.Cells[2, 8].Value = "Lock";

            String ck_box = null;
            for (int i = 0, cell = 0; i < listView3.Items.Count; i++)
            {
                if (listView3.Items[i].SubItems[0].Text == "checkbox_field")
                {
                    ck_box = listView3.Items[i].SubItems[1].Text;
                }
                else if (listView3.Items[i].SubItems[0].Text == "radio_btn")
                {

                }
                else if (listView3.Items[i].SubItems[0].Text == "check_box")
                {
                    ws.Cells[2, 9 + cell].Value = ck_box + ":-" + listView3.Items[i].SubItems[1].Text;
                    cell++;
                }
                else
                {
                    ws.Cells[2, 9 + cell].Value = listView3.Items[i].SubItems[1].Text;
                    cell++;
                }
            }

            //save the file to specific location
            wb.SaveAs(@"c:\Giled\organisations\" + label1.Text + "\\" + textBox2.Text + "\\" + textBox2.Text + ".xlsx");

            //close file
            wb.Close(0);
            excel.Quit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                panel4.Visible = true;
                progressBar1.Value = 0;
                progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));


                if (check_validity())
                {
                    progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));


                    int a = 0, b = 0;

                    if (ledger_structure_validity())
                    {
                        //send to firebase
                        progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
                        int ids, t_l;
                        string ida;
                        var result = client.Get("Ledgers/meta_data/total_count");
                        ids = result.ResultAs<int>();


                        //reading self id
                        StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                        ida = sr.ReadLine();
                        sr.Close();

                        progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));

                        //setting meta data of a ledgere
                        client.Set("Ledgers/" + ids.ToString() + "/ledger_id", ids.ToString());
                        client.Set("Ledgers/" + ids.ToString() + "/ledger_name", textBox2.Text);
                        client.Set("Ledgers/" + ids.ToString() + "/hashing_level", (comboBox1.SelectedIndex - 1).ToString());
                        client.Set("Ledgers/" + ids.ToString() + "/total_fields", listView3.Items.Count);
                        client.Set("Ledgers/" + ids.ToString() + "/organisation", label1.Text);
                        client.Set("Ledgers/" + ids.ToString() + "/organisation_id", label2.Text);
                        client.Set("Ledgers/" + ids.ToString() + "/miner_id", ida);

                        //create its  record in transaction
                        client.Set("Transactions/" + ids.ToString() + "/pending_blocks", "0");
                        client.Set("Transactions/" + ids.ToString() + "/lock", "0");
                        client.Set("Transactions/" + ids.ToString() + "/last_hash", "0");
                        client.Set("Transactions/" + ids.ToString() + "/initial_block", "0");
                        client.Set("Transactions/" + ids.ToString() + "/hashing_level", (comboBox1.SelectedIndex - 1).ToString());
                        client.Set("Transactions/" + ids.ToString() + "/total_elements", listView3.Items.Count);
                        client.Set("Transactions/" + ids.ToString() + "/unique_records", "0");

                        //create its unlocking
                        client.Set("Unlocking/" + ids.ToString() + "/initial_block", "0");
                        client.Set("Unlocking/" + ids.ToString() + "/pending_blocks", "0");
                        client.Set("Unlocking/" + ids.ToString() + "/lock", "0");


                        result = client.Get("Organisation/" + label2.Text + "/total_leger");
                        t_l = result.ResultAs<int>();
                        client.Set("Organisation/" + label2.Text + "/ledger_" + t_l + "_id", ids.ToString());
                        client.Set("Organisation/" + label2.Text + "/total_leger", ++t_l);


                        ListViewItem item;
                        item = new ListViewItem(textBox2.Text);
                        item.SubItems.Add(ids.ToString());
                        listView1.Items.Add(item);

                        progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));

                        for (i = 0; i < listView3.Items.Count; i++)
                        {
                            if (listView3.Items[i].SubItems[0].Text == "text")
                                client.Set("Ledgers/" + ids.ToString() + "/" + i, "00 " + listView3.Items[i].SubItems[1].Text);
                            else if (listView3.Items[i].SubItems[0].Text == "radio_field")
                                client.Set("Ledgers/" + ids.ToString() + "/" + i, "01 " + listView3.Items[i].SubItems[1].Text);
                            else if (listView3.Items[i].SubItems[0].Text == "radio_btn")
                                client.Set("Ledgers/" + ids.ToString() + "/" + i, "02 " + listView3.Items[i].SubItems[1].Text);
                            else if (listView3.Items[i].SubItems[0].Text == "checkbox_field")
                                client.Set("Ledgers/" + ids.ToString() + "/" + i, "03 " + listView3.Items[i].SubItems[1].Text);
                            else if (listView3.Items[i].SubItems[0].Text == "check_box")
                                client.Set("Ledgers/" + ids.ToString() + "/" + i, "04 " + listView3.Items[i].SubItems[1].Text);
                            else
                                MessageBox.Show("Error");
                            progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
                        }

                        ids = ids + 1;
                        var setter2 = client.Set("Ledgers/" + "meta_data/" + "total_count", ids.ToString());



                        if (setter2 != null)
                        {
                            //save to local storage
                            progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
                            DirectoryInfo dir = new DirectoryInfo(@"c:\Giled\organisations\" + label1.Text);
                            try
                            {
                                if (!(dir.Exists))
                                {
                                    dir.Create();
                                }
                                StreamWriter sw;
                                if (!(File.Exists(@"C:\Giled\organisations\" + label1.Text + "\\meta_data.txt")))
                                {
                                    sw = new StreamWriter(@"C:\Giled\organisations\" + label1.Text + "\\meta_data.txt");
                                    sw.WriteLine("1");
                                    sw.WriteLine(--ids);
                                    sw.WriteLine(textBox2.Text);
                                    sw.Close();
                                    progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
                                }
                                else
                                {
                                    //in
                                    string[] org_id = new string[50];
                                    string[] org_name = new string[50];
                                    sr = new StreamReader(@"C:\Giled\organisations\" + label1.Text + "\\meta_data.txt");
                                    ida = sr.ReadLine();

                                    //read line
                                    for (i = 0; i < int.Parse(ida); i++)
                                    {
                                        org_id[i] = sr.ReadLine();
                                        org_name[i] = sr.ReadLine();
                                    }
                                    sr.Close();
                                    sw = new StreamWriter(@"C:\Giled\organisations\" + label1.Text + "\\meta_data.txt");
                                    sw.WriteLine((int.Parse(ida) + 1).ToString());
                                    //write line
                                    for (i = 0; i < int.Parse(ida); i++)
                                    {
                                        sw.WriteLine(org_id[i]);
                                        sw.WriteLine(org_name[i]);
                                    }
                                    sw.WriteLine(--ids);
                                    sw.WriteLine(textBox2.Text);
                                    sw.Close();

                                    progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
                                }
                                dir = new DirectoryInfo(@"C:\Giled\organisations\" + label1.Text + "\\" + textBox2.Text);
                                dir.Create();

                                //cerate ledger structure
                                progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));
                                sw = new StreamWriter(@"C:\Giled\organisations\" + label1.Text + "\\" + textBox2.Text + "\\ledger_structure.txt");



                                sw.WriteLine(listView3.Items.Count);
                                for (i = 0; i < listView3.Items.Count; i++)
                                {
                                    if (listView3.Items[i].SubItems[0].Text == "text")
                                        sw.WriteLine("00 " + listView3.Items[i].SubItems[1].Text);
                                    else if (listView3.Items[i].SubItems[0].Text == "radio_field")
                                        sw.WriteLine("01 " + listView3.Items[i].SubItems[1].Text);
                                    else if (listView3.Items[i].SubItems[0].Text == "radio_btn")
                                    {
                                        a++;
                                        sw.WriteLine("02 " + listView3.Items[i].SubItems[1].Text);
                                    }
                                    else if (listView3.Items[i].SubItems[0].Text == "checkbox_field")
                                    {
                                        sw.WriteLine("03 " + listView3.Items[i].SubItems[1].Text);
                                        b++;
                                    }
                                    else if (listView3.Items[i].SubItems[0].Text == "check_box")
                                        sw.WriteLine("04 " + listView3.Items[i].SubItems[1].Text);
                                    else
                                        MessageBox.Show("Error");
                                }
                                progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));

                                sw.Close();






                            }
                            catch (Exception ex)
                            {
                                panel4.Visible = false;
                                MessageBox.Show(ex.Message);
                            }

                            progressBar1.Increment(100 / (9 + 3 * listView3.Items.Count));

                            //create actual ledger
                            create_actual_ledger(ids, a, b);



                            //finally set
                            panel4.Visible = false;
                            MessageBox.Show("Ledger Creation Success");
                            label4.Text = textBox2.Text;
                            label10.Text = ids.ToString();
                            panel4.Visible = false;
                            panel2.Visible = false;
                            panel3.Visible = true;
                            panel3.Dock = DockStyle.Fill;




                            //empty the flow layout panel
                            flowLayoutPanel1.Controls.Clear();

                            //create dynamic form
                            String input_str;

                            sr = new StreamReader(@"C:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\ledger_structure.txt");

                            if (sr != null)
                            {
                                total_element = int.Parse(sr.ReadLine());


                                //read line
                                int j, count = 0;
                                System.Windows.Forms.GroupBox gb = null;
                                total_radioButtons = 0;
                                total_checkBoxes = 0;
                                total_textBoxes = 0;
                                for (i = 0; i < total_element; i++)
                                {
                                    input_str = sr.ReadLine();
                                    element_code[i] = int.Parse(input_str.Substring(0, 2));
                                    element_name[i] = input_str.Substring(3);

                                    if (element_code[i] == 0)
                                    {
                                        textBoxes[total_textBoxes] = addTextBox(element_name[i]);
                                        total_textBoxes++;
                                    }
                                    else if (element_code[i] == 1 || element_code[i] == 3)
                                    {
                                        gb = new System.Windows.Forms.GroupBox();
                                        flowLayoutPanel1.Controls.Add(gb);

                                        gb.Text = element_name[i];
                                        gb.Name = gb.Text;
                                        gb.AutoSize = true;
                                        gb.Margin = new System.Windows.Forms.Padding(0, 30, 0, 0);
                                        gb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

                                        count = 0;
                                    }
                                    else if (element_code[i] == 2)
                                    {
                                        for (j = i - 1; element_code[j] == 2; j--)
                                        { }
                                        radioButtons[total_radioButtons] = addRadioButton(gb, element_name[j], element_name[i], count++);
                                        total_radioButtons++;

                                    }
                                    else if (element_code[i] == 4)
                                    {
                                        for (j = i - 1; element_code[j] == 4; j--)
                                        { }
                                        checkBoxes[total_checkBoxes] = addCheckBox(gb, element_name[j], element_name[i], count++);
                                        total_checkBoxes++;

                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid Ledger");
                                    }


                                }
                                sr.Close();
                            }
                            else
                                MessageBox.Show("Problem with the ledger file");


                        }
                        else
                        {
                            panel4.Visible = false;
                            MessageBox.Show("Error Occured, Ledger not created");
                        }
                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

            //empty the flow layout panel
            flowLayoutPanel1.Controls.Clear();

            //manage panel
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel1.Dock = DockStyle.Fill;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            label4.Text = listView1.SelectedItems[0].Text;
            label10.Text = listView1.SelectedItems[0].SubItems[1].Text;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            panel3.Dock = DockStyle.Fill;

            //empty the flow layout panel
            flowLayoutPanel1.Controls.Clear();

            //create dynamic form



            StreamReader sr;
            int i;
            String input_str;

            sr = new StreamReader(@"C:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\ledger_structure.txt");

            if (sr != null)
            {
                total_element = int.Parse(sr.ReadLine());


                //read line
                int j, count = 0;
                System.Windows.Forms.GroupBox gb = null;
                total_checkBoxes = 0;
                total_textBoxes = 0;
                total_radioButtons = 0;
                for (i = 0; i < total_element; i++)
                {
                    input_str = sr.ReadLine();
                    element_code[i] = int.Parse(input_str.Substring(0, 2));
                    element_name[i] = input_str.Substring(3);

                    if (element_code[i] == 0)
                    {
                        textBoxes[total_textBoxes++] = addTextBox(element_name[i]);

                    }
                    else if (element_code[i] == 1 || element_code[i] == 3)
                    {
                        gb = new System.Windows.Forms.GroupBox();
                        flowLayoutPanel1.Controls.Add(gb);

                        gb.Text = element_name[i];
                        gb.Name = gb.Text;
                        gb.AutoSize = true;
                        gb.Margin = new System.Windows.Forms.Padding(0, 30, 0, 0);
                        gb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

                        count = 0;
                    }
                    else if (element_code[i] == 2)
                    {
                        for (j = i - 1; element_code[j] == 2; j--)
                        { }
                        radioButtons[total_radioButtons++] = addRadioButton(gb, element_name[j], element_name[i], count++);


                    }
                    else if (element_code[i] == 4)
                    {
                        for (j = i - 1; element_code[j] == 4; j--)
                        { }
                        checkBoxes[total_checkBoxes++] = addCheckBox(gb, element_name[j], element_name[i], count++);


                    }
                    else
                    {
                        MessageBox.Show("Invalid Ledger");
                    }


                }
                sr.Close();
            }
            else
                MessageBox.Show("Problem with the ledger file");

        }

        static string ComputeSha256Hash(string rawData)
        {
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

        int Validate(String hash, int level)
        {
            int sum = 0;
            int i;

            for(i = 0; i < 64 ; i++)
            {
                sum += (int)hash[i];
            }

            if ((hash[0] == hash[1]) && (hash[1] == hash[2]) && ((int)hash[2] == level + 'a') && (sum % (1 + level) == 0))
            {
                //MessageBox.Show(hash);
                return 1;
            }
            else
            {
                return 0;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            panel4.Visible = true;
            label8.Text = "Verifying the block.....";
            progressBar1.Value = 0;
            progressBar1.Increment(1);


            //verify blocks

            //create hash
            label8.Text = "Generating hash.....";
            String hash;
            hash = ComputeSha256Hash("0");

            progressBar1.Increment(1);
            int k, tb = 0, cb = 0, rb = 0;
            for (k = 0; k < total_element; k++)
            {
                progressBar1.Increment(2);
                if (element_code[k] == 0)
                {
                    hash = ComputeSha256Hash(hash + textBoxes[tb++].Text);
                }
                else if (element_code[k] == 2)
                {
                    if (radioButtons[rb++].Checked == true)
                        hash = ComputeSha256Hash(hash + radioButtons[rb - 1].Text);
                }
                else if (element_code[k] == 4)
                {
                    if (checkBoxes[cb++].Checked == true)
                        hash = ComputeSha256Hash(hash + checkBoxes[cb - 1].Text);
                }
                else if (element_code[k] == 1 || element_code[k] == 3)
                {

                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }

            //send to firebase
            label8.Text = "Passing the block to network.....";
            progressBar1.Increment(1);
            int l_lock, total, total_miner, m_id, initial_block, unique_id;
            int level, nonce = -1;
            int u_id;
            String prev_hash;
            var result = client.Get("Transactions/" + label10.Text + "/lock");
            l_lock = result.ResultAs<int>();

            //get usser id
            progressBar1.Increment(1);
            StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
            u_id = int.Parse(sr.ReadLine());
            sr.Close();

            if (l_lock == 0)
            {
                //lock the transaction
                progressBar1.Increment(1);
                client.Set("Transactions/" + label10.Text + "/lock", "1");

                //get previous hash
                progressBar1.Increment(1);
                result = client.Get("Transactions/" + label10.Text + "/last_hash");
                prev_hash = result.ResultAs<string>();
                hash = ComputeSha256Hash(hash + prev_hash);

                //get level of the ledger
                result = client.Get("Transactions/" + label10.Text + "/hashing_level");
                level = result.ResultAs<int>();

                //add user id to it
                hash = ComputeSha256Hash(hash + u_id);

                label8.Text = "Calculating Nonce.....";
                do
                {
                    nonce++;

                } while (Validate(ComputeSha256Hash(hash + nonce), level) == 0);

                hash = ComputeSha256Hash(hash + nonce);


                label8.Text = "Passing the block to network.....";

                //get total transaction
                result = client.Get("Transactions/" + label10.Text + "/pending_blocks");
                total = result.ResultAs<int>();

                client.Set("Transactions/" + label10.Text + "/pending_blocks", total + 1);

                progressBar1.Increment(1);
                result = client.Get("Transactions/" + label10.Text + "/initial_block");
                initial_block = result.ResultAs<int>();

                total = total + initial_block;

                //now send it to database
                //set previous hash
                progressBar1.Increment(1);
                client.Set("Transactions/" + label10.Text + "//" + total + "//" + "prev_hash", prev_hash);

                //set unique id if needed
                if (textBox3.Text == "")
                {
                    result = client.Get("Transactions/" + label10.Text + "/unique_records");
                    unique_id = result.ResultAs<int>();

                    client.Set("Transactions/" + label10.Text + "//" + total + "//" + "id", unique_id);

                    client.Set("Transactions/" + label10.Text + "/unique_records", unique_id + 1);
                }
                else
                {
                    client.Set("Transactions/" + label10.Text + "//" + total + "//" + "id", textBox3.Text.ToString());
                }

                //send the data part

                tb = 0;
                rb = 0;
                cb = 0;
                progressBar1.Increment(1);
                for (i = 0; i < total_element; i++)
                {
                    progressBar1.Increment(2);
                    if (element_code[i] == 0)
                    {
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "00 " + textBoxes[tb++].Text);
                    }
                    else if (element_code[i] == 1)
                    {
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "01 " + element_name[i]);
                    }
                    else if (element_code[i] == 2)
                    {
                        if (radioButtons[rb++].Checked == true)
                            client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "02 y " + radioButtons[rb - 1].Text);
                        else
                            client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "02 n");
                    }
                    else if (element_code[i] == 3)
                    {
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "03 " + element_name[i]);
                    }
                    else if (element_code[i] == 4)
                    {
                        if (checkBoxes[cb++].Checked == true)
                            client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "04 y " + checkBoxes[cb - 1].Text);
                        else
                            client.Set("Transactions/" + label10.Text + "//" + total + "//" + i, "04 n");
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong");
                    }
                }

                client.Set("Transactions/" + label10.Text + "//" + total + "//" + "lock_status", "0");

                //now sent the miners to 0
                //first get total number of miners
                progressBar1.Increment(1);
                result = client.Get("Organisation/" + label2.Text + "/total_miner");
                total_miner = result.ResultAs<int>();

                //set the first miner
                progressBar1.Increment(1);
                result = client.Get("Organisation/" + label2.Text + "/miner_id");
                m_id = result.ResultAs<int>();
                client.Set("Transactions/" + label10.Text + "//" + total + "/miner_" + m_id, "0");

                //now set the rest of the miners
                for (i = 1; i < total_miner; i++)
                {
                    progressBar1.Increment(2);
                    result = client.Get("Organisation/" + label2.Text + "/miner" + i);
                    m_id = result.ResultAs<int>();

                    client.Set("Transactions/" + label10.Text + "//" + total + "/miner_" + m_id, "0");
                }

                progressBar1.Increment(1);
                client.Set("Transactions/" + label10.Text + "//" + total + "/miners_left", total_miner);
                client.Set("Transactions/" + label10.Text + "//" + total + "/miner_id", u_id);
                client.Set("Transactions/" + label10.Text + "//" + total + "/hash", hash);
                client.Set("Transactions/" + label10.Text + "//" + total + "/nonce", nonce);
                client.Set("Transactions/" + label10.Text + "/last_hash", hash);

                //release the transaction
                client.Set("Transactions/" + label10.Text + "/lock", "0");

                panel4.Visible = false;
                //done
                MessageBox.Show("Block Passed to network");
            }
            else if (l_lock == -1)
            {
                panel4.Visible = false;
                MessageBox.Show("Ledger is Locked");
            }
            else
            {
                panel4.Visible = false;
                MessageBox.Show("Congestion in network, please try after some seconds");
            }
            //complete
        }

        int get_pending_blocks()
        {
            var result = client.Get("Transactions/" + label10.Text + "/pending_blocks");
            return result.ResultAs<int>();
        }

        Boolean ledger_open_or_not()
        {
            FileInfo file_name = new FileInfo(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\" + label4.Text + ".xlsx");
            FileStream stream_input = null;

            try
            {
                stream_input = file_name.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (Exception)
            {
                return false;
            }

            if (stream_input != null)
            {
                stream_input.Close();
                return true;
            }
            else
            {
                return false;
            }

        }

        int get_ledger_lock_status()
        {
            var result = client.Get("Transactions/" + label10.Text + "/lock");
            return result.ResultAs<int>();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                //start the progress bar
                panel4.Visible = true;
                progressBar1.Value = 0;

                progressBar1.Increment(2);

                label8.Text = "Checking For Updates";

                //check local ledger open or not
                if (ledger_open_or_not())
                {
                    //get ledger lock status
                    int ledger_lock_status;
                    ledger_lock_status = get_ledger_lock_status();

                    progressBar1.Increment(2);

                    //check ledger is open or locked
                    if (ledger_lock_status != 1)
                    {
                        //ledger is open so lock the ledger
                        client.Set("Transactions/" + label10.Text + "/lock", "1");

                        progressBar1.Increment(2);

                        //now fetch the total pending blocks
                        int pending_blocks;
                        pending_blocks = get_pending_blocks();

                        progressBar1.Increment(2);

                        //perform action is we have pending blocks
                        if (pending_blocks > 0)
                        {
                            //from here atually work begins
                            int initial_block, total_field, prev_count, total_up, up_ids, miner_left, i_blocks, p_blocks;
                            int lock_status, block_id;
                            String curr_hash, field_data, field_id;

                            progressBar1.Increment(2);


                            //get user id
                            String u_id;
                            StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                            u_id = sr.ReadLine();
                            sr.Close();

                            //get initial block
                            var result = client.Get("Transactions/" + label10.Text + "/initial_block");
                            initial_block = result.ResultAs<int>();

                            //get total fields
                            result = client.Get("Transactions/" + label10.Text + "/total_elements");
                            total_field = result.ResultAs<int>();

                            //open local ledger
                            //file object
                            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                            //workbook object
                            Workbook wb;

                            //worksheet object
                            Worksheet ws;

                            wb = excel.Workbooks.Open(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\" + label4.Text + ".xlsx", true);
                            ws = wb.Worksheets[1];

                            //fetch the total count of blocks
                            prev_count = Convert.ToInt32(ws.Cells[1, 1].Value);

                            //get the last hash of the local ledger
                            curr_hash = Convert.ToString(ws.Cells[1, 2].Value);

                            int new_i = 0;

                            //check for each pending block
                            for (int i = 0; i < pending_blocks; i++)
                            {
                                label8.Text = "Fetching blocks from network";
                                progressBar1.Increment(1);

                                result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/miner_" + u_id);


                                //check we need this block or not
                                if (result.ResultAs<int>() == 0)
                                {


                                    //fetch it and store it to local ledger step by step

                                    //set the other parameters
                                    //first set entry number
                                    ws.Cells[3 + prev_count + new_i, 2].Value = prev_count + i;

                                    //now set the previous hash
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/prev_hash");
                                    ws.Cells[3 + prev_count + new_i, 3].Value = result.ResultAs<String>();

                                    //now set the current hash
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/hash");
                                    curr_hash = result.ResultAs<String>();
                                    ws.Cells[3 + prev_count + new_i, 4].Value = curr_hash;

                                    //now set the miner id
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/miner_id");
                                    ws.Cells[3 + prev_count + new_i, 5].Value = result.ResultAs<String>();

                                    //now set the nonce value
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/nonce");
                                    ws.Cells[3 + prev_count + new_i, 6].Value = result.ResultAs<String>();

                                    //now set the record/block unique id
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/id");
                                    block_id = result.ResultAs<int>();
                                    ws.Cells[3 + prev_count + new_i, 7].Value = result.ResultAs<int>();

                                    //now set the record lock status
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/lock_status");
                                    lock_status = result.ResultAs<int>();
                                    ws.Cells[3 + prev_count + new_i, 8].Value = lock_status;

                                    progressBar1.Increment(1);


                                    //lock or unlock request
                                    if (lock_status != 0)
                                    {
                                        //if it is lock request
                                        if (lock_status == 1)
                                        {
                                            //if ledger lock request
                                            if ((ws.Cells[1, 8].Value).ToString() == "0" && block_id == -1)
                                            {
                                                ws.Cells[1, 8].Value = "1";
                                                MessageBox.Show("Ledger has been Locked");
                                            }

                                        }

                                        //if it is unlock request
                                        if (lock_status == -1)
                                        {
                                            //if it is unlock request
                                            if ((ws.Cells[1, 8].Value).ToString() == "1" && block_id == -1)
                                            {
                                                ws.Cells[1, 8].Value = "0";
                                                MessageBox.Show("Ledger has been Unlocked");
                                            }

                                        }

                                        //total supported minors
                                        result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/total_up_votes");
                                        total_up = result.ResultAs<int>();

                                        //set into local ledger
                                        ws.Cells[3 + prev_count + new_i, 9] = total_up;

                                        //now set the id of minors who have supported this unlock
                                        for (int k = 0; k < total_up; k++)
                                        {
                                            result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/up_vote" + k);
                                            up_ids = result.ResultAs<int>();
                                            ws.Cells[3 + prev_count + i, 10 + k].Value = up_ids;
                                        }

                                    }

                                    //adding field
                                    if (lock_status == 0)
                                    {

                                        //now set the rest of parameters of the record
                                        for (int j = 0, cols = 0; j < total_field; j++)
                                        {

                                            progressBar1.Increment(1);

                                            //fetch the complete field
                                            result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/" + j);
                                            field_data = result.ResultAs<String>();

                                            //parse the field id
                                            field_id = field_data.Substring(0, 2);
                                            //parse the field data
                                            field_data = field_data.Substring(3);

                                            //store the data according to field id
                                            if (field_id == "00")
                                            {
                                                if (field_data != "")
                                                {
                                                    ws.Cells[3 + prev_count + new_i, 9 + cols].Value = field_data;
                                                }

                                                cols++;
                                            }
                                            else if (field_id == "01")
                                            {

                                            }
                                            else if (field_id == "02")
                                            {
                                                if (field_data.Substring(0, 1) == "y")
                                                {
                                                    ws.Cells[3 + prev_count + new_i, 9 + cols].Value = field_data.Substring(2);
                                                    cols++;
                                                }
                                            }
                                            else if (field_id == "03")
                                            {

                                            }
                                            else if (field_id == "04")
                                            {
                                                if (field_data.Substring(0, 1) == "y")
                                                {
                                                    ws.Cells[3 + prev_count + new_i, 9 + cols].Value = "Y";
                                                    cols++;
                                                }
                                                else
                                                {
                                                    ws.Cells[3 + prev_count + new_i, 9 + cols].Value = "N";
                                                    cols++;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Something Went Wrong");
                                            }
                                        }

                                    }

                                    //now this entry is completely recorded on local ledger


                                    //mark it as taken by updating its id by 1
                                    client.Set("Transactions/" + label10.Text + "//" + (initial_block + i) + "/miner_" + u_id, "1");

                                    //and decrement miners_left
                                    result = client.Get("Transactions/" + label10.Text + "//" + (initial_block + i) + "/miners_left");
                                    miner_left = result.ResultAs<int>();

                                    progressBar1.Increment(2);

                                    client.Set("Transactions/" + label10.Text + "//" + (initial_block + i) + "/miners_left", --miner_left);

                                    //remove it if all miner took it
                                    if (miner_left == 0)
                                    {
                                        client.Delete("Transactions/" + label10.Text + "//" + (initial_block + i));

                                        //initial block
                                        result = client.Get("Transactions/" + label10.Text + "/initial_block");
                                        i_blocks = result.ResultAs<int>();
                                        client.Set("Transactions/" + label10.Text + "/initial_block", ++i_blocks);

                                        //pending block
                                        result = client.Get("Transactions/" + label10.Text + "/pending_blocks");
                                        p_blocks = result.ResultAs<int>();
                                        client.Set("Transactions/" + label10.Text + "/pending_blocks", --p_blocks);
                                    }
                                    new_i++;
                                }

                            }

                            //update total block count and last hash
                            ws.Cells[1, 1].Value = new_i + prev_count;
                            ws.Cells[1, 2].Value = curr_hash;

                            //close ledger
                            wb.Save();
                            wb.Close(0);
                            excel.Quit();

                            progressBar1.Increment(2);



                            //task finally completed
                            MessageBox.Show("Updated Sucesfully");
                        }
                        else
                        {
                            //ledger is up to date
                            MessageBox.Show("Already Updated");
                        }

                        //release the lock of ledger
                        client.Set("Transactions/" + label10.Text + "/lock", ledger_lock_status);
                    }
                    else
                    {
                        //it was a congestion
                        MessageBox.Show("Congestion in Network");
                    }
                }
                else
                {
                    //ledger was open so give error message
                    MessageBox.Show("Please close the ledger to update it");
                }


                panel4.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\" + label4.Text + ".xlsx");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("Please enter record id");
                }
                else
                {
                    panel4.Visible = true;
                    if (textBox3.Text == "-1")
                        label8.Text = "Locking Ledger";
                    else
                        label8.Text = "Locking ID.....";

                    progressBar1.Value = 0;
                    progressBar1.Increment(1);


                    //verify blocks

                    //create hash
                    label8.Text = "Generating hash.....";
                    String hash;
                    hash = ComputeSha256Hash("0");

                    progressBar1.Increment(1);


                    //send to firebase
                    label8.Text = "Passing the block to network.....";
                    progressBar1.Increment(1);
                    int l_lock, total, total_miner, m_id, initial_block;
                    int level, nonce = -1;
                    int u_id;
                    String prev_hash;
                    var result = client.Get("Transactions/" + label10.Text + "/lock");
                    l_lock = result.ResultAs<int>();

                    //get usser id
                    progressBar1.Increment(1);
                    StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                    u_id = int.Parse(sr.ReadLine());
                    sr.Close();

                    if (l_lock == 0)
                    {
                        //lock the transaction
                        progressBar1.Increment(1);
                        client.Set("Transactions/" + label10.Text + "/lock", "1");

                        //get previous hash
                        progressBar1.Increment(1);
                        result = client.Get("Transactions/" + label10.Text + "/last_hash");
                        prev_hash = result.ResultAs<string>();
                        hash = ComputeSha256Hash(hash + prev_hash);

                        //get level of the ledger
                        result = client.Get("Transactions/" + label10.Text + "/hashing_level");
                        level = result.ResultAs<int>();

                        //add user id to it
                        hash = ComputeSha256Hash(hash + u_id);

                        label8.Text = "Calculating Nonce.....";
                        do
                        {
                            nonce++;

                        } while (Validate(ComputeSha256Hash(hash + nonce), level) == 0);

                        hash = ComputeSha256Hash(hash + nonce);


                        label8.Text = "Passing the block to network.....";

                        //get total transaction
                        result = client.Get("Transactions/" + label10.Text + "/pending_blocks");
                        total = result.ResultAs<int>();

                        client.Set("Transactions/" + label10.Text + "/pending_blocks", total + 1);

                        progressBar1.Increment(1);
                        result = client.Get("Transactions/" + label10.Text + "/initial_block");
                        initial_block = result.ResultAs<int>();

                        total = total + initial_block;

                        //now send it to database
                        //set previous hash
                        progressBar1.Increment(1);
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + "prev_hash", prev_hash);

                        //set total up votes to 0
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + "/total_up_votes", "-1");

                        //set record id
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + "id", textBox3.Text.ToString());

                        //set the lock status
                        client.Set("Transactions/" + label10.Text + "//" + total + "//" + "lock_status", "1");

                        //now sent the miners to 0
                        //first get total number of miners
                        progressBar1.Increment(1);
                        result = client.Get("Organisation/" + label2.Text + "/total_miner");
                        total_miner = result.ResultAs<int>();

                        //set the first miner
                        progressBar1.Increment(1);
                        result = client.Get("Organisation/" + label2.Text + "/miner_id");
                        m_id = result.ResultAs<int>();
                        client.Set("Transactions/" + label10.Text + "//" + total + "/miner_" + m_id, "0");

                        //now set the rest of the miners
                        for (i = 1; i < total_miner; i++)
                        {
                            progressBar1.Increment(2);
                            result = client.Get("Organisation/" + label2.Text + "/miner" + i);
                            m_id = result.ResultAs<int>();

                            client.Set("Transactions/" + label10.Text + "//" + total + "/miner_" + m_id, "0");
                        }

                        progressBar1.Increment(1);
                        client.Set("Transactions/" + label10.Text + "//" + total + "/miners_left", total_miner);
                        client.Set("Transactions/" + label10.Text + "//" + total + "/miner_id", u_id);
                        client.Set("Transactions/" + label10.Text + "//" + total + "/hash", hash);
                        client.Set("Transactions/" + label10.Text + "//" + total + "/nonce", nonce);
                        client.Set("Transactions/" + label10.Text + "/last_hash", hash);
                        panel4.Visible = false;

                        //finally sent

                        if (textBox3.Text == "-1")
                        {
                            client.Set("Transactions/" + label10.Text + "/lock", "-1");
                            MessageBox.Show("Ledger Locked");
                        }
                        else
                        {
                            client.Set("Transactions/" + label10.Text + "/lock", "0");
                            MessageBox.Show("Record ID Locked");
                        }

                    }
                    else if (l_lock == -1)
                    {
                        panel4.Visible = false;
                        MessageBox.Show("Ledger Already Locked");
                    }
                    else
                    {
                        panel4.Visible = false;
                        MessageBox.Show("Congestion in network, please try after some seconds");
                    }
                    //id locked
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("Please Mention Block ID");
                }
                else
                {
                    panel4.Visible = true;
                    label8.Text = "Sending Unlock Request........";
                    progressBar1.Value = 0;
                    progressBar1.Increment(1);

                    int locks, total_miners, initial_block, pending_block, miner_id;
                    progressBar1.Increment(1);

                    //fetch lock status
                    var result = client.Get("Unlocking/" + label10.Text + "/lock");
                    locks = result.ResultAs<int>();
                    progressBar1.Increment(1);

                    if (locks == 0)
                    {
                        //set lock to 1
                        client.Set("Unlocking/" + label10.Text + "/lock", "1");
                        progressBar1.Increment(1);

                        //get initial block
                        result = client.Get("Unlocking/" + label10.Text + "/initial_block");
                        initial_block = result.ResultAs<int>();
                        progressBar1.Increment(1);

                        //get pending block
                        result = client.Get("Unlocking/" + label10.Text + "/pending_blocks");
                        pending_block = result.ResultAs<int>();
                        progressBar1.Increment(1);


                        //set new incremented pending block
                        client.Set("Unlocking/" + label10.Text + "/pending_blocks", 1 + pending_block);
                        progressBar1.Increment(1);


                        //get total miners of the organization
                        result = client.Get("Organisation/" + label2.Text + "/total_miner");
                        total_miners = result.ResultAs<int>();
                        progressBar1.Increment(1);


                        initial_block = initial_block + pending_block;
                        //set pending miners
                        client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/pending_miners", total_miners);
                        progressBar1.Increment(1);

                        //now set the miners
                        //set first miner
                        result = client.Get("Organisation/" + label2.Text + "/miner_id");
                        miner_id = result.ResultAs<int>();
                        client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/miner_" + miner_id, "0");
                        //now rest of the miners
                        for (int i = 1; i < total_miners; i++)
                        {
                            progressBar1.Increment(1);
                            result = client.Get("Organisation/" + label2.Text + "/miner" + i);
                            miner_id = result.ResultAs<int>();

                            client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/miner_" + miner_id, "0");
                        }

                        //set up vote and down vote
                        client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/up_vote", "0");
                        client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/down_vote", "0");
                        progressBar1.Increment(1);

                        //set user id
                        //get user id
                        String u_id;
                        StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                        u_id = sr.ReadLine();
                        sr.Close();
                        client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/request_by", u_id);

                        //id to be unlocked
                        client.Set("Unlocking/" + label10.Text + "//" + initial_block + "/Unlock_id", textBox3.Text);
                        progressBar1.Increment(1);

                        //release the lock
                        client.Set("Unlocking/" + label10.Text + "/lock", "0");
                        progressBar1.Increment(1);

                        panel4.Visible = false;
                        MessageBox.Show("Request Sent To Network");
                    }
                    else
                    {
                        panel4.Visible = false;
                        MessageBox.Show("Congestion in Network, Please wait a while");
                    }


                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                int locks, pending_blocks, id;
                String update;

                //start progress bar
                panel4.Visible = true;
                label8.Text = "Fetching Pending Blocks";

                progressBar1.Value = 0;

                progressBar1.Increment(2);


                //get user id
                String u_id;
                StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                u_id = sr.ReadLine();
                sr.Close();

                progressBar1.Increment(2);

                //get lock status
                var result = client.Get("Unlocking/" + label10.Text + "/lock");
                locks = result.ResultAs<int>();

                progressBar1.Increment(2);

                //check lock status
                if (locks == 0)
                {
                    //set lock to 1
                    client.Set("Unlocking/" + label10.Text + "/lock", "1");

                    result = client.Get("Unlocking/" + label10.Text + "/pending_blocks");
                    pending_blocks = result.ResultAs<int>();

                    progressBar1.Increment(2);

                    if (pending_blocks == 0)
                    {
                        MessageBox.Show("No New Request Found");
                    }
                    else
                    {
                        //empty the list
                        listView2.Items.Clear();

                        ListViewItem items;

                        for (int i = 0; i < pending_blocks; i++)
                        {
                            result = client.Get("Unlocking/" + label10.Text + "//" + i + "/miner_" + u_id);

                            progressBar1.Increment(2);


                            if (result.ResultAs<string>() == null)
                            {
                                pending_blocks++;
                            }
                            else
                            {
                                update = result.ResultAs<string>();
                                if (update == "0")
                                {
                                    result = client.Get("Unlocking/" + label10.Text + "//" + i + "/Unlock_id");
                                    id = result.ResultAs<int>();

                                    items = new ListViewItem(id.ToString());
                                    items.SubItems.Add((i).ToString());
                                    listView2.Items.Add(items);
                                }
                            }

                            progressBar1.Increment(2);


                        }
                    }


                    //release the lock after fetching
                    client.Set("Unlocking/" + label10.Text + "/lock", "0");

                    progressBar1.Increment(2);
                }
                else
                {
                    MessageBox.Show("Congestion in Network");
                }

                //shut down progress
                panel4.Visible = false;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                //start the progress bar
                panel4.Visible = true;
                progressBar1.Value = 0;

                progressBar1.Increment(2);

                label8.Text = "Accepting";


                int i, pending_miners, temp, temp2, lock1, lock2;

                int nonce = -1, id, T_ini_block, T_pend_blocks, m_id, total_miner;
                String hash, prev_hash;

                //get user id
                int u_id;
                StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                u_id = int.Parse(sr.ReadLine());
                sr.Close();

                var result = client.Get("Unlocking/" + label10.Text + "/lock");
                lock1 = result.ResultAs<int>();

                result = client.Get("Transactions/" + label10.Text + "/lock");
                lock2 = result.ResultAs<int>();

                progressBar1.Increment(2);

                if (lock1 == 0 && lock2 != 1)
                {
                    //set both the locks
                    client.Set("Unlocking/" + label10.Text + "/lock", "1");
                    client.Set("Transactions/" + label10.Text + "/lock", "1");

                    //visit each and every entry
                    for (i = 0; i < listView2.Items.Count; i++)
                    {

                        progressBar1.Increment(2);

                        //check if ticked or not
                        if (listView2.Items[i].Checked == true)
                        {
                            //set its vote to 1
                            client.Set("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/miner_" + u_id, 1);

                            //increase one upvote
                            result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/up_vote");
                            temp = result.ResultAs<int>();
                            client.Set("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/up_vote", 1 + temp);

                            //now reduce one pending miners
                            //fetch total pending miners
                            result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/pending_miners");
                            pending_miners = result.ResultAs<int>();

                            //set new pending miners
                            client.Set("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/pending_miners", --pending_miners);

                            //check if all miners have voted or not
                            if (pending_miners == 0)
                            {

                                progressBar1.Increment(1);

                                //prepare this block to send in ledger(transaction)

                                //transaction initial block
                                result = client.Get("Transactions/" + label10.Text + "/initial_block");
                                T_ini_block = result.ResultAs<int>();

                                //transaction pending blocks
                                result = client.Get("Transactions/" + label10.Text + "/pending_blocks");
                                T_pend_blocks = result.ResultAs<int>();

                                //initial hash
                                hash = ComputeSha256Hash("0");

                                //id hash
                                hash = ComputeSha256Hash(hash + listView2.Items[i].SubItems[0].Text);
                                //set it to transactions

                                //prev hash
                                result = client.Get("Transactions/" + label10.Text + "/last_hash");
                                prev_hash = result.ResultAs<String>();
                                hash = ComputeSha256Hash(hash + prev_hash);

                                //request id hash
                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/request_by");
                                id = result.ResultAs<int>();
                                hash = ComputeSha256Hash(hash + id);
                                //also set request id
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miner_id", id);

                                //set for which ledger id it was sent
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/id", listView2.Items[i].SubItems[0].Text);

                                //set prev hash
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/prev_hash", prev_hash);

                                //increase pending block by 1
                                client.Set("Transactions/" + label10.Text + "/pending_blocks", T_pend_blocks + 1);

                                //get total upvotes
                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/up_vote");
                                temp = result.ResultAs<int>();
                                //send this upvote to transactions
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/total_up_votes", temp);

                                //get total downvotes
                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/down_vote");
                                temp2 = result.ResultAs<int>();

                                if ((100 * temp) / (temp + temp2) > 50)
                                {
                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/lock_status", "-1");

                                    //unlock the ledger if it is ledger id
                                    if (listView2.Items[i].SubItems[0].Text == "-1")
                                    {
                                        lock2 = 0;
                                        MessageBox.Show("Ledger is unlocked");
                                    }

                                }
                                else
                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/lock_status", "1");

                                //miner left
                                //now sent the miners to 0
                                //first get total number of miners

                                int up_v = 0;

                                result = client.Get("Organisation/" + label2.Text + "/total_miner");
                                total_miner = result.ResultAs<int>();

                                //set total pending miners
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miners_left", total_miner);

                                //set the first miner
                                result = client.Get("Organisation/" + label2.Text + "/miner_id");
                                m_id = result.ResultAs<int>();

                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/miner_" + m_id);
                                if (result.ResultAs<int>() == 1)
                                {
                                    hash = ComputeSha256Hash(hash + m_id);

                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/up_vote" + up_v, m_id);
                                    up_v++;
                                }
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miner_" + m_id, "0");

                                //now set the rest of the miners
                                for (int k = 1; k < total_miner; k++)
                                {
                                    result = client.Get("Organisation/" + label2.Text + "/miner" + k);
                                    m_id = result.ResultAs<int>();

                                    result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/miner_" + m_id);
                                    if (result.ResultAs<int>() == 1)
                                    {
                                        hash = ComputeSha256Hash(hash + m_id);

                                        client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/up_vote" + up_v, m_id);
                                        up_v++;
                                    }

                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miner_" + m_id, "0");

                                    progressBar1.Increment(1);
                                }

                                //calculate nonce
                                do
                                {
                                    nonce++;
                                } while (Validate(ComputeSha256Hash(hash + nonce), 1) == 0);

                                hash = ComputeSha256Hash(hash + nonce);
                                //set nonce
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/nonce", nonce);

                                //set hash
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/hash", hash);

                                //set total up votes
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/total_up_votes", up_v);

                                //set last hash
                                client.Set("Transactions/" + label10.Text + "/last_hash", hash);

                                //data sent to transactions


                                //now remove this request from the unlocking
                                //first increase initial block
                                result = client.Get("Unlocking/" + label10.Text + "/initial_block");
                                temp = result.ResultAs<int>();
                                client.Set("Unlocking/" + label10.Text + "/initial_block", ++temp);


                                //then decrease pending blocks
                                result = client.Get("Unlocking/" + label10.Text + "/pending_blocks");
                                temp = result.ResultAs<int>();
                                client.Set("Unlocking/" + label10.Text + "/pending_blocks", --temp);

                                //now remove this block from unlocking
                                client.Delete("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text);

                            }

                            //remove the item from list
                            listView2.Items.RemoveAt(i);

                            i--;
                        }
                    }

                    //refresh list
                    listView2.Refresh();

                    //release lock
                    client.Set("Unlocking/" + label10.Text + "/lock", "0");
                    client.Set("Transactions/" + label10.Text + "/lock", lock2);
                }
                else
                {
                    MessageBox.Show("Congestion In Network");
                }

                panel4.Visible = false;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (!check_conn())
            {
                MessageBox.Show("No Internet Connectivity");
            }
            else
            {
                //start the progress bar
                panel4.Visible = true;
                progressBar1.Value = 0;

                progressBar1.Increment(2);

                label8.Text = "Rejecting";

                int i, pending_miners, temp, temp2, lock1, lock2;

                int nonce = -1, id, T_ini_block, T_pend_blocks, m_id, total_miner;
                String hash, prev_hash;

                //get user id
                int u_id;
                StreamReader sr = new StreamReader(@"C:\Giled\userinfo\user.txt");
                u_id = int.Parse(sr.ReadLine());
                sr.Close();

                var result = client.Get("Unlocking/" + label10.Text + "/lock");
                lock1 = result.ResultAs<int>();

                result = client.Get("Transactions/" + label10.Text + "/lock");
                lock2 = result.ResultAs<int>();

                if (lock1 == 0 && lock2 != 1)
                {
                    //set both the locks
                    client.Set("Unlocking/" + label10.Text + "/lock", "1");
                    client.Set("Transactions/" + label10.Text + "/lock", "1");

                    //visit each and every entry
                    for (i = 0; i < listView2.Items.Count; i++)
                    {
                        progressBar1.Increment(1);

                        //check if ticked or not
                        if (listView2.Items[i].Checked == true)
                        {
                            progressBar1.Increment(1);

                            //set its vote to -1
                            client.Set("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/miner_" + u_id, -1);

                            //increase one downvote
                            result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/down_vote");
                            temp2 = result.ResultAs<int>();
                            client.Set("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/down_vote", 1 + temp2);

                            //now reduce one pending miners
                            //fetch total pending miners
                            result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/pending_miners");
                            pending_miners = result.ResultAs<int>();

                            //set new pending miners
                            client.Set("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/pending_miners", --pending_miners);

                            //check if all miners have voted or not
                            if (pending_miners == 0)
                            {
                                progressBar1.Increment(1);

                                //prepare this block to send in ledger(transaction)

                                //transaction initial block
                                result = client.Get("Transactions/" + label10.Text + "/initial_block");
                                T_ini_block = result.ResultAs<int>();

                                //transaction pending blocks
                                result = client.Get("Transactions/" + label10.Text + "/pending_blocks");
                                T_pend_blocks = result.ResultAs<int>();

                                //initial hash
                                hash = ComputeSha256Hash("0");

                                //id hash
                                hash = ComputeSha256Hash(hash + listView2.Items[i].SubItems[0].Text);
                                //set it to transactions

                                //prev hash
                                result = client.Get("Transactions/" + label10.Text + "/last_hash");
                                prev_hash = result.ResultAs<String>();
                                hash = ComputeSha256Hash(hash + prev_hash);

                                //request id hash
                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/request_by");
                                id = result.ResultAs<int>();
                                hash = ComputeSha256Hash(hash + id);
                                //also set request id
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miner_id", id);

                                //set for which ledger id it was sent
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/id", listView2.Items[i].SubItems[0].Text);

                                //set prev hash
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/prev_hash", prev_hash);

                                //increase pending block by 1
                                client.Set("Transactions/" + label10.Text + "/pending_blocks", T_pend_blocks + 1);

                                //get total upvotes
                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/up_vote");
                                temp = result.ResultAs<int>();
                                //send this upvote to transactions
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/total_up_votes", temp);

                                //get total downvotes
                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/down_vote");
                                temp2 = result.ResultAs<int>();

                                if ((100 * temp) / (temp + temp2) > 50)
                                {
                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/lock_status", "-1");

                                    //unlock the ledger if it is ledger id
                                    if (listView2.Items[i].SubItems[0].Text == "-1")
                                    {
                                        lock2 = 0;
                                        MessageBox.Show("Ledger is unlocked");
                                    }

                                }
                                else
                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/lock_status", "1");


                                //miner left
                                //now sent the miners to 0
                                //first get total number of miners
                                int up_v = 0;

                                result = client.Get("Organisation/" + label2.Text + "/total_miner");
                                total_miner = result.ResultAs<int>();

                                //set total pending miners/////////////////////////////////
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miners_left", total_miner);

                                //set the first miner
                                result = client.Get("Organisation/" + label2.Text + "/miner_id");
                                m_id = result.ResultAs<int>();

                                result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/miner_" + m_id);
                                if (result.ResultAs<int>() == 1)
                                {
                                    hash = ComputeSha256Hash(hash + m_id);

                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/up_vote" + up_v, m_id);
                                    up_v++;
                                }
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miner_" + m_id, "0");

                                //now set the rest of the miners
                                for (int k = 1; k < total_miner; k++)
                                {
                                    progressBar1.Increment(1);

                                    result = client.Get("Organisation/" + label2.Text + "/miner" + k);
                                    m_id = result.ResultAs<int>();

                                    result = client.Get("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text + "/miner_" + m_id);
                                    if (result.ResultAs<int>() == 1)
                                    {
                                        hash = ComputeSha256Hash(hash + m_id);

                                        client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/up_vote" + up_v, m_id);
                                        up_v++;
                                    }

                                    client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/miner_" + m_id, "0");
                                }

                                //calculate nonce
                                do
                                {
                                    nonce++;
                                } while (Validate(ComputeSha256Hash(hash + nonce), 1) == 0);

                                hash = ComputeSha256Hash(hash + nonce);
                                //set nonce
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/nonce", nonce);

                                //set hash
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/hash", hash);

                                //set total up votes
                                client.Set("Transactions/" + label10.Text + "//" + (T_ini_block + T_pend_blocks) + "/total_up_votes", up_v);

                                //set last hash
                                client.Set("Transactions/" + label10.Text + "/last_hash", hash);

                                //data sent to transactions


                                //now remove this request from the unlocking

                                //first increase initial block
                                result = client.Get("Unlocking/" + label10.Text + "/initial_block");
                                temp = result.ResultAs<int>();
                                client.Set("Unlocking/" + label10.Text + "/initial_block", ++temp);


                                //then decrease pending blocks
                                result = client.Get("Unlocking/" + label10.Text + "/pending_blocks");
                                temp = result.ResultAs<int>();
                                client.Set("Unlocking/" + label10.Text + "/pending_blocks", --temp);

                                //now remove this block from unlocking
                                client.Delete("Unlocking/" + label10.Text + "//" + listView2.Items[i].SubItems[1].Text);

                            }

                            //remove the item from list
                            listView2.Items.RemoveAt(i);
                            i--;
                        }
                    }

                    //refresh list
                    listView2.Refresh();

                    //release lock
                    client.Set("Unlocking/" + label10.Text + "/lock", "0");
                    client.Set("Transactions/" + label10.Text + "/lock", lock2);
                }
                else
                {
                    MessageBox.Show("Congestion In Network");
                }

                panel4.Visible = false;
            }
        }

        int verify_ledger()
        {
            int total_records, total_fields, minor_id, nonce, record_id, record_lock, total_up_votes;
            string prev_hash = "0", curr_hash, check_hash;
            int[] attributes;
            string[] field_data;
            string read_lines;
            //open ledger

            progressBar1.Increment(1);

            //file object
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //workbook object
            Workbook wb;

            //worksheet object
            Worksheet ws;

            wb = excel.Workbooks.Open(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\" + label4.Text + ".xlsx", true);
            ws = wb.Worksheets[1];

            //fetch the total count of records
            total_records = Convert.ToInt32(ws.Cells[1, 1].Value);

            progressBar1.Increment(1);

            if (total_records == 0)
            {
                //close ledger
                wb.Save();
                wb.Close(0);
                excel.Quit();
                return -2;
            }

            //fetch total fields
            total_fields = Convert.ToInt32(ws.Cells[1, 3].Value);

            attributes = new int[total_fields];
            field_data = new string[total_fields];
            //set this array
            //open ledger structure
            StreamReader sr = new StreamReader(@"C:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\ledger_structure.txt");
            read_lines = sr.ReadLine();
            for(int i = 0; i < total_fields; i++)
            {
                progressBar1.Increment(1);

                read_lines = sr.ReadLine();
                if (read_lines.Substring(0, 2) == "00" || read_lines.Substring(0, 2) == "01")
                    attributes[i] = 0;
                else if (read_lines.Substring(0, 2) == "02" || read_lines.Substring(0, 2) == "03")
                    i--;
                else if(read_lines.Substring(0,2) == "04")
                {
                    attributes[i] = 1;
                    field_data[i] = read_lines.Substring(3);
                }
                else
                { }
            }
            sr.Close();



            for (int i=0; i<total_records; i++)
            {
                progressBar1.Increment(1);

                check_hash = ComputeSha256Hash("0");

                //fetch current hash
                curr_hash = (ws.Cells[i + 3, 4].Value).ToString();

                //if not matching previous hashes
                if ((ws.Cells[i + 3, 3].Value).ToString() != prev_hash)
                {
                    //close ledger
                    wb.Save();
                    wb.Close(0);
                    excel.Quit();
                    return i;
                }

                //fetch minor id
                minor_id = int.Parse((ws.Cells[i + 3, 5].Value).ToString());

                //fetch nonce
                nonce = int.Parse((ws.Cells[i + 3, 6].Value).ToString());

                //fetch record id
                record_id = int.Parse((ws.Cells[i + 3, 7].Value).ToString());

                //fetch record lock
                record_lock = int.Parse((ws.Cells[i + 3, 8].Value).ToString());

                

                //if it is ledger id
                if (record_id == -1)
                {

                    //lock request
                    if (record_lock == 1)
                    {
                        //fetch total up votes cell
                        total_up_votes = int.Parse((ws.Cells[i + 3, 9].Value).ToString());

                        //due to direct locking
                        if (total_up_votes == -1)
                        {
                            
                            check_hash = ComputeSha256Hash(check_hash + prev_hash);
                            check_hash = ComputeSha256Hash(check_hash + minor_id);
                            check_hash = ComputeSha256Hash(check_hash + nonce);

                            if (check_hash != curr_hash)
                            {
                                //close ledger
                                wb.Save();
                                wb.Close(0);
                                excel.Quit();
                                return i;
                            }
                        }
                        //due to failed in unlocking
                        else if(total_up_votes >= 0)
                        {
                            check_hash = ComputeSha256Hash(check_hash + record_id);
                            check_hash = ComputeSha256Hash(check_hash + prev_hash);
                            check_hash = ComputeSha256Hash(check_hash + minor_id);

                            for(int k=0; k<total_up_votes; k++)
                                check_hash = ComputeSha256Hash(check_hash + (ws.Cells[i + 3, k + 10].Value).ToString());

                            check_hash = ComputeSha256Hash(check_hash + nonce);

                            if (check_hash != curr_hash)
                            {
                                //close ledger
                                wb.Save();
                                wb.Close(0);
                                excel.Quit();
                                return i;
                            }
                        }
                        //error
                        else
                        {
                            //close ledger
                            wb.Save();
                            wb.Close(0);
                            excel.Quit();
                            return i;
                        }

                    }

                    //unlock request
                    else if (record_lock == -1)
                    {
                        check_hash = ComputeSha256Hash(check_hash + record_id);
                        check_hash = ComputeSha256Hash(check_hash + prev_hash);
                        check_hash = ComputeSha256Hash(check_hash + minor_id);

                        //fetch total up votes cell
                        total_up_votes = int.Parse((ws.Cells[i + 3, 9].Value).ToString());

                        for (int k = 0; k < total_up_votes; k++)
                            check_hash = ComputeSha256Hash(check_hash + (ws.Cells[i + 3, k + 10].Value).ToString());

                        check_hash = ComputeSha256Hash(check_hash + nonce);

                        if (check_hash != curr_hash)
                        {
                            //close ledger
                            wb.Save();
                            wb.Close(0);
                            excel.Quit();
                            return i;
                        }
                    }

                    //error
                    else
                    {
                        //close ledger
                        wb.Save();
                        wb.Close(0);
                        excel.Quit();
                        return i;
                    }

                }
                //if it is record id
                else
                {
                    //adding/updating record data
                    if(record_lock == 0)
                    {
                        //record fields
                        for(int k=0; k < total_fields; k++)
                        {
                            if(attributes[k] == 1)
                            {
                                if((ws.Cells[i + 3, k + 9].Value).ToString() == "Y")
                                check_hash = ComputeSha256Hash(check_hash + field_data[k]);
                            }
                                
                            else
                            {
                                check_hash = ComputeSha256Hash(check_hash + ws.Cells[i + 3, k + 9].Value);
                                
                            }
                                
                        }

                        //previous hash
                        check_hash = ComputeSha256Hash(check_hash + prev_hash);
                        check_hash = ComputeSha256Hash(check_hash + minor_id);
                        check_hash = ComputeSha256Hash(check_hash + nonce);

                        if (check_hash != curr_hash)
                        {
                            //close ledger
                            wb.Save();
                            wb.Close(0);
                            excel.Quit();
                            return i;
                        }
                    }

                    //lock request
                    else if(record_lock == 1)
                    {
                           //fetch total up votes cell
                            total_up_votes = int.Parse((ws.Cells[i + 3, 9].Value).ToString());

                            //due to direct locking
                            if (total_up_votes == -1)
                            {

                                check_hash = ComputeSha256Hash(check_hash + prev_hash);
                                check_hash = ComputeSha256Hash(check_hash + minor_id);
                                check_hash = ComputeSha256Hash(check_hash + nonce);

                                if (check_hash != curr_hash)
                                {
                                    //close ledger
                                    wb.Save();
                                    wb.Close(0);
                                    excel.Quit();
                                    return i;
                                }
                            }
                            //due to failed in unlocking
                            else if (total_up_votes >= 0)
                            {
                                check_hash = ComputeSha256Hash(check_hash + record_id);
                                check_hash = ComputeSha256Hash(check_hash + prev_hash);
                                check_hash = ComputeSha256Hash(check_hash + minor_id);

                                for (int k = 0; k < total_up_votes; k++)
                                    check_hash = ComputeSha256Hash(check_hash + (ws.Cells[i + 3, k + 10].Value).ToString());

                                check_hash = ComputeSha256Hash(check_hash + nonce);

                                if (check_hash != curr_hash)
                                {
                                    //close ledger
                                    wb.Save();
                                    wb.Close(0);
                                    excel.Quit();
                                    return i;
                                }
                            }
                            //error
                            else
                            {
                                //close ledger
                                wb.Save();
                                wb.Close(0);
                                excel.Quit();
                                return i;
                            }

                        
                    }

                    //unlocking a lock
                    else if(record_lock == -1)
                    {
                        check_hash = ComputeSha256Hash(check_hash + record_id);
                        check_hash = ComputeSha256Hash(check_hash + prev_hash);
                        check_hash = ComputeSha256Hash(check_hash + minor_id);

                        //fetch total up votes cell
                        total_up_votes = int.Parse((ws.Cells[i + 3, 9].Value).ToString());

                        for (int k = 0; k < total_up_votes; k++)
                            check_hash = ComputeSha256Hash(check_hash + (ws.Cells[i + 3, k + 10].Value).ToString());

                        check_hash = ComputeSha256Hash(check_hash + nonce);

                        if (check_hash != curr_hash)
                        {
                            //close ledger
                            wb.Save();
                            wb.Close(0);
                            excel.Quit();
                            return i;
                        }
                    }

                    //error
                    else
                    {
                        //close ledger
                        wb.Save();
                        wb.Close(0);
                        excel.Quit();
                        return i;
                    }
                }

                //keep this hash as previous
                prev_hash = curr_hash;
            }


            //close ledger
            wb.Save();
            wb.Close(0);
            excel.Quit();

            progressBar1.Increment(1);


            //everything correct so return -1
            return -1;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //start the progress bar
            panel4.Visible = true;
            progressBar1.Value = 0;

            progressBar1.Increment(2);

            label8.Text = "Verifying Ledger";

            //check ledger closed or not
            if (ledger_open_or_not())
            {
                progressBar1.Increment(2);

                int ledger_validity;

                ledger_validity = verify_ledger();
                if (ledger_validity == -1)
                {
                    MessageBox.Show("Ledger Sucessfully Verified");
                }
                else if(ledger_validity == -2)
                {
                    MessageBox.Show("Empty Ledger");
                }
                else
                {
                    MessageBox.Show("Ledger corrupted at entry number: " + ledger_validity);
                }
            }
            //ledger is open
            else
            {
                MessageBox.Show("Ledger is open, please close it to verify");
            }
            
            panel4.Visible = false;
        }

        Boolean database_open_or_not()
        {
            if (File.Exists(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\Database_" + label4.Text + ".xlsx"))
            {
                FileInfo file_name = new FileInfo(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\Database_" + label4.Text + ".xlsx");
                FileStream stream_input = null;

                try
                {
                    stream_input = file_name.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception)
                {
                    return false;
                }

                if (stream_input != null)
                {
                    stream_input.Close();
                    File.Delete(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\Database_" + label4.Text + ".xlsx");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return true;
        }
            
                        

        private void button15_Click(object sender, EventArgs e)
        {
            //start the progress bar
            panel4.Visible = true;
            progressBar1.Value = 0;

            progressBar1.Increment(2);

            label8.Text = "Generating Database";

            //ledger close or not
            if (ledger_open_or_not())
            {
                if(database_open_or_not())
                {
                    //verify ledger
                    int ledger_validity = verify_ledger();
                    if (ledger_validity == -1)
                    {
                        //ledger is verified now proceed further

                        //****************open ledger*****************
                        //ledger file object
                        Microsoft.Office.Interop.Excel.Application excel_l = new Microsoft.Office.Interop.Excel.Application();
                        //ledger workbook object
                        Workbook wb_l;
                        //ledgerworksheet object
                        Worksheet ws_l;
                        //
                        wb_l = excel_l.Workbooks.Open(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\" + label4.Text + ".xlsx", true);
                        ws_l = wb_l.Worksheets[1];

                        //******************open database************************
                        //file object
                        Microsoft.Office.Interop.Excel.Application excel_d = new Microsoft.Office.Interop.Excel.Application();

                        //workbook object
                        Workbook wb_d;

                        //worksheet object
                        Worksheet ws_d;
                        wb_d = excel_d.Workbooks.Add();
                        ws_d = wb_d.Worksheets[1];

                        //******************************start writing the database**************************
                        int total_entries, total_attributes, record_lock, record_id;
                        //fetch the relavent data from ledger
                        total_entries = int.Parse(ws_l.Cells[1, 1].Value.ToString());
                        total_attributes = int.Parse(ws_l.Cells[1, 3].Value.ToString());

                        //set the first row
                        ws_d.Cells[2, 2].Value = "Id";
                        ws_d.Cells[2, 3].Value = "Lock";

                        for (int k = 0; k < total_attributes; k++)
                            ws_d.Cells[2, 4 + k].Value = ws_l.Cells[2, 9 + k].Value.ToString();

                        //now start copying the data from ledger to database
                        for (int i = 0; i < total_entries; i++)
                        {
                            record_id = int.Parse(ws_l.Cells[3 + i, 7].Value.ToString());
                            record_lock = int.Parse(ws_l.Cells[3 + i, 8].Value.ToString());

                            if (record_id >= 0)
                            {
                                ws_d.Cells[3 + record_id, 2].Value = record_id;
                                if (record_lock == -1)
                                {
                                    ws_d.Cells[3 + record_id, 3].Value = 0;
                                }
                                else if (record_lock == 1)
                                {
                                    ws_d.Cells[3 + record_id, 3].Value = 1;
                                }
                                else
                                {
                                    if (ws_d.Cells[3 + record_id, 3].Value != null && ws_d.Cells[3 + record_id, 3].Value.ToString() == "1")
                                    {
                                        MessageBox.Show("Record id " + record_id + " was being updated while it was locked");
                                    }
                                    else
                                    {
                                        ws_d.Cells[3 + record_id, 3].Value = 0;
                                        //write into database
                                        for (int k = 0; k < total_attributes; k++)
                                        {
                                            if (ws_l.Cells[3 + i, 9 + k].Value != null)
                                                ws_d.Cells[3 + record_id, 4 + k].Value = ws_l.Cells[3 + i, 9 + k].Value.ToString();
                                        }
                                    }
                                }
                            }
                        }

                        //task complete now close the files


                        //*********************************close database*****************************

                        wb_d.SaveAs(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\Database_" + label4.Text + ".xlsx");

                        //close file
                        wb_d.Close(0);
                        excel_d.Quit();

                        //*********************************close ledger*************************
                        wb_l.Save();
                        wb_l.Close(0);
                        excel_l.Quit();
                        MessageBox.Show("Database Created");
                        System.Diagnostics.Process.Start(@"c:\Giled\organisations\" + label1.Text + "\\" + label4.Text + "\\Database_" + label4.Text + ".xlsx");
                    }
                    else if (ledger_validity == -2)
                    {
                        MessageBox.Show("No entry in ledger");
                    }
                    else
                    {
                        MessageBox.Show("Ledger corrupted at entry number: " + ledger_validity + "\nCan't generate the database");
                    }
                }
                else
                {
                    MessageBox.Show("Database is open, please close it to verify");
                }
                
            }
            else
            {
                MessageBox.Show("Ledger is open, please close it to verify");
            }

            panel4.Visible = false;
            
        }

        private void label15_Click(object sender, EventArgs e)
        {
            HomePage lg = new HomePage();
            Hide();
            lg.ShowDialog();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "Select Level";
            label7.Text = label1.Text;
            button9.Visible = false;
            button11.Visible = false;

            panel2.Visible = true;
            panel1.Visible = false;
            panel3.Visible = false;
            panel2.Dock = DockStyle.Fill;
        }

        private void label21_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel1.Dock = DockStyle.Fill;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button7_MouseHover(object sender, EventArgs e)
        {
            button7.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button9_MouseHover(object sender, EventArgs e)
        {
            button9.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button8_MouseHover(object sender, EventArgs e)
        {
            button8.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button10_MouseHover(object sender, EventArgs e)
        {
            button10.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button11_MouseHover(object sender, EventArgs e)
        {
            button11.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button12_MouseHover(object sender, EventArgs e)
        {
            button12.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            button7.BackColor = Color.FromArgb(116, 52, 138);
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            button8.BackColor = Color.FromArgb(116, 52, 138);
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            button9.BackColor = Color.FromArgb(116, 52, 138);
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            button10.BackColor = Color.FromArgb(116, 52, 138);
        }

        private void button11_MouseLeave(object sender, EventArgs e)
        {
            button11.BackColor = Color.FromArgb(116, 52, 138);
        }

        private void button12_MouseLeave(object sender, EventArgs e)
        {
            button12.BackColor = Color.FromArgb(212, 23, 23);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button19_MouseHover(object sender, EventArgs e)
        {
            button19.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            label5.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button20_MouseHover(object sender, EventArgs e)
        {
            button20.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button14_MouseHover(object sender, EventArgs e)
        {
            button14.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button16_MouseHover(object sender, EventArgs e)
        {
            button16.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button15_MouseHover(object sender, EventArgs e)
        {
            button15.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button18_MouseHover(object sender, EventArgs e)
        {
            button18.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button17_MouseHover(object sender, EventArgs e)
        {
            button17.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void button14_MouseLeave(object sender, EventArgs e)
        {
            button14.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void button16_MouseLeave(object sender, EventArgs e)
        {
            button16.BackColor = Color.FromArgb(29, 71, 53);
        }

        private void button18_MouseLeave(object sender, EventArgs e)
        {
                button18.BackColor = Color.FromArgb(41, 71, 53);
        }

        private void button17_MouseLeave(object sender, EventArgs e)
        {
            button17.BackColor = Color.FromArgb(41, 71, 53);
        }

        private void button20_MouseLeave(object sender, EventArgs e)
        {
            button20.BackColor = Color.FromArgb(192, 0, 0);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(29, 35, 71);
        }

        private void button15_MouseLeave(object sender, EventArgs e)
        {
            button15.BackColor = Color.FromArgb(214, 160, 21);
        }

        private void button19_MouseLeave(object sender, EventArgs e)
        {
            button19.BackColor = Color.FromArgb(0, 231, 37);
        }

        private void label21_MouseHover(object sender, EventArgs e)
        {
            panel16.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label11_MouseHover(object sender, EventArgs e)
        {
            panel9.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label15_MouseHover(object sender, EventArgs e)
        {
            panel8.BackColor = Color.FromArgb(40, 190, 198);
        }

        private void label21_MouseLeave(object sender, EventArgs e)
        {
            panel16.BackColor = Color.FromArgb(3, 150, 159);
        }

        private void label11_MouseLeave(object sender, EventArgs e)
        {
            panel9.BackColor = Color.FromArgb(3, 150, 159);
        }

        private void label15_MouseLeave(object sender, EventArgs e)
        {
            panel8.BackColor = Color.FromArgb(19, 103, 130);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}