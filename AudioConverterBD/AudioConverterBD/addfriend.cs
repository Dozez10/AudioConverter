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

namespace AudioConverterBD
{
    public partial class addfriend : Form
    {


        string user;
        public addfriend(string username)
        {
            InitializeComponent();
            user = username;

        }
        public MySqlConnection rcon(string server, string user, string database, string password)
        {
            return new MySqlConnection("server=" + server + ";UserId=" + user + ";database=" + database + ";password=" + password + ";");
        }

        public bool checklogin(string n)
        {
            MySqlConnection mysql = rcon("localhost", "Frost", "kvsl", "Frost1234!");
            MySqlCommand mycm = new MySqlCommand();
            mycm.Connection = mysql;
            mycm.CommandText = "select * from kvsl.userinfo where login='" + n + "';";
            mysql.Open();
            MySqlDataReader mydr = mycm.ExecuteReader();
            if (mydr.HasRows)
            {
                mysql.Close();
                mysql.Dispose();
                mydr.Close();
                mycm.Dispose();
                mydr.Dispose();
                return true;
            }
            mysql.Close();
            mysql.Dispose();
            mydr.Close();
            mycm.Dispose();
            mydr.Dispose();
            return false;


        }
        public int getidbynickname(string nickname)
        {
            int c = 0;
            MySqlConnection mysql = rcon("localhost", "Frost", "kvsl", "Frost1234!");
            MySqlCommand mycm = new MySqlCommand();
            mycm.Connection = mysql;
            mycm.CommandText = "SELECT id FROM kvsl.userinfo where login='" + nickname + "';";
            mysql.Open();
            MySqlDataReader myrd = mycm.ExecuteReader();
            if (myrd.HasRows)
            {
                if (myrd.Read()) { c= Convert.ToInt32(myrd.GetValue(0)); }

                else
                
                  c= 0;
                
            }
            myrd.Dispose();
            mycm.Dispose();
            myrd.Close();
            mysql.Dispose();
            mysql.Close();
            return c;

        }
        public bool checkfi(int ido, int idp)
        {
            MySqlConnection mysql = rcon("localhost", "Frost", "kvsl", "Frost1234!");
            MySqlCommand mycm = new MySqlCommand();
            mycm.Connection = mysql;
            mysql.Open();
            mycm.CommandText = "select * from kvsl.friendinvite where ido=" + ido + " and idp=" + idp + " and status=0;";
            MySqlDataReader mydr = mycm.ExecuteReader();
            if (mydr.HasRows)

            {
                return true;
            }
            else return false;

        }
        public int deletefriend(string kogo)
        {
            int usero = getidbynickname(user);
            int kogo1 = getidbynickname(kogo);
            int rowsa = 0;
            if (usero < kogo1) { rowsa = usero;usero = kogo1;kogo1 = rowsa;rowsa = 0; }
            MySqlConnection mysql = rcon("localhost", "Frost", "kvsl", "Frost1234!");
            MySqlCommand mycm = new MySqlCommand();
            mycm.Connection = mysql;
            mysql.Open();
            mycm.CommandText = "update kvsl.friendsrelations set kvsl.friendsrelations.relation=@dval where kvsl.friendsrelations.idf=@usr and kvsl.friendsrelations.idftwo=@fri;";
            mycm.Parameters.AddWithValue("@dval", 0);
            mycm.Parameters.AddWithValue("@usr",usero );
            mycm.Parameters.AddWithValue("@fri",kogo1);

            rowsa = mycm.ExecuteNonQuery();
   
            mysql.Dispose();
            mysql.Close();
            mycm.Dispose();
            return rowsa;

        }
        
            
            public int addfriendinvi(string komu)
        {   
            int usero = getidbynickname(user);
            int komy = getidbynickname(komu);
            int rowsa = 0;
            if (!checkfi(usero, komy))
            {
                MySqlConnection mysql = rcon("localhost", "Frost", "kvsl", "Frost1234!");
                MySqlCommand mycm = new MySqlCommand();
                mycm.Connection = mysql;
                mysql.Open();
                mycm.CommandText = "insert into kvsl.friendinvite(ido,idp,status)values(@idotp,@idpol,@stat);";
                mycm.Parameters.Add("@idotp", MySqlDbType.Int32).Value = usero;
                mycm.Parameters.Add("@idpol", MySqlDbType.Int32).Value = komy;
                mycm.Parameters.Add(new MySqlParameter("@stat", MySqlDbType.Bit)).Value = 0 ;
                rowsa = mycm.ExecuteNonQuery();
                mysql.Dispose();
                mysql.Close();
                mycm.Dispose();

            }
          
            return rowsa;

        }
      
      
       

        private void addfriend_Load(object sender, EventArgs e)
        {
            label1.Text = "Nickname";
            button1.Text = "Send";
            button2.Text = "Delete";
            label2.Visible = true;
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
          

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checklogin(textBox1.Text))

            {
                if (textBox1.Text == user)
                {
                    label2.Text = "You cant add/delete yourself";

                }
                else label2.Text = "√";

            }
            else if (textBox1.Text=="") label2.Text = "Invalid name";
            else label2.Text = "No users";



              }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label2.Text == "√")

            {
                if (addfriendinvi(textBox1.Text) > 0)
                {
                    label3.Text = "Invitation send";
                }
                else label3.Text = "Invitation already send";
            }
            else label3.Text = "Invalid Nickname";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label2.Text == "√")
            {
                if (deletefriend(textBox1.Text) > 0)
                {
                    label4.Text = "Delete succesefull";
                }
                else label4.Text = "You need to add first to delete";

            }
            else label4.Text = "Invalid nickname";
        }
    }
}
