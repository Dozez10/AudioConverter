using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql;

namespace AudioConverterBD
{
    public partial class bdwindowcs : Form
    {
        Form1 form1;
        registercs rg;
        public MySqlConnection rcon(string server, string user, string database, string password)
        {
            return new MySqlConnection("server=" + server + ";UserId=" + user + ";database=" + database + ";password=" + password + ";");
        }
        public string getpass(string namet)
        {

            MySqlConnection myc = rcon("localhost","Frost","kvsl","Frost1234!");
            MySqlCommand mycs = new MySqlCommand("select password from kvsl.userinfo where login='"+namet+"';", myc);
            myc.Open();
            string pass="n";
            MySqlDataReader mydr = mycs.ExecuteReader();
            if (mydr.HasRows)
            {
                if (mydr.Read()){
                    
                    pass = mydr.GetString(0);
                }
                
               
            }
          else
            {
                mydr.Close();
                mydr.Dispose();
                mycs.CommandText = "select password from kvsl.userinfo where email='" + namet + "';";
                mydr = mycs.ExecuteReader();
                if (mydr.HasRows)
                {

                    if (mydr.Read())
                    {

                        pass = mydr.GetString(0);
                    }

                }
                else pass = "Invalid user or email";



            }
          
            myc.Close();
            myc.Dispose();
            return pass;



        }

        public bdwindowcs()
        {
            InitializeComponent();

            rg = new registercs();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

            this.Visible = false;
            if (rg.IsDisposed || rg == null)
                {
                
                    rg = new registercs();
             
                    rg.Show();
                

                }
                else
                {
                
                rg.Show();
                

               }
            
            
            
        
           
        }

        private void bdwindowcs_Load(object sender, EventArgs e)
        {
            label2.Text = "username or email";
            label3.Text = "password";
            textBox1.TabIndex = 1;
            textBox2.TabIndex = 2;
            button2.TabIndex = 3;
            textBox1.MaxLength = 30;
            textBox2.MaxLength = 20;
            label1.Text = "Dont have an account then register";
            button2.Text = "Login";
            label4.Visible = false;
            label4.Text = "Invalid password or email";
            this.Name = "login window";
            this.Text = "login window";
            IntPtr ip = AudioConverterBD.Properties.Resources.google_png_icons.GetHicon();
            this.Icon = Icon.FromHandle(ip);

            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (getpass(textBox1.Text) == textBox2.Text)
            { try
                {
                    if (form1.IsDisposed||form1==null)
                    {
                        this.Visible = false;
                        form1 = new Form1(textBox1.Text, textBox2.Text,gettype(textBox1.Text));
                        form1.Show();
                        label4.Visible = false;
                    }
                    else
                    {
                        this.Visible = false;
                        form1.Show();
                        label4.Visible = false;
                    }
                }catch(NullReferenceException es)
                {
                    this.Visible = false;
                    form1 = new Form1(textBox1.Text, textBox2.Text,gettype(textBox1.Text));
                    form1.Show();
                    label4.Visible = false;
                }
               
            }
            else label4.Visible = true;
            
           
           
        }
        public string gettype(string username1)
        {
            string pesf = "";
            MySqlConnection myc = rcon("localhost", "Frost", "kvsl", "Frost1234!");
            MySqlCommand mycs = new MySqlCommand("select typeof from kvsl.userinfo where kvsl.userinfo.login='" + username1 + "';", myc);
            myc.Open();
            MySqlDataReader myrd = mycs.ExecuteReader();
            if (myrd.HasRows)
            {
                if (myrd.Read()) pesf = myrd.GetString(0);
            }
            myc.Close();
            myc.Dispose();
            mycs.Dispose();
            myrd.Close();
            myrd.Dispose();
            return pesf;


        }

        private void bdwindowcs_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
