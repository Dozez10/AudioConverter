using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Utils;
using NAudio;
using MySql;
using NAudio.Wave;
using NAudio.Codecs;
using MySql.Data.MySqlClient;

namespace AudioConverterBD
{

    public partial class Form1 : Form
    {
        List<string> tpat = new List<string>();
        string u, p;
        userinfo us;
        bool dtem = false;
        bool yt = false;
        bool wt = false;
        private AudioFileReader reader;
        private WaveOut waveOut;
       
        Color n;
        public void setmpos(float seconds)
        {
            reader.CurrentTime = TimeSpan.FromSeconds(seconds);

        }
        public string getpos()
        {
            string temp = "";
            if (reader != null)
            {
                temp = Convert.ToString(reader.TotalTime.Minutes) + ":" + Convert.ToString(reader.TotalTime.Seconds);
               

            }
            return temp;

        }

        public string curpos()
        {
            string temp="";
            if (reader != null)
            {
                temp = Convert.ToString(reader.CurrentTime.Minutes) + ":" + Convert.ToString(reader.CurrentTime.Seconds);
            }
            
            return temp;


        }
        public void cvol(float volume)
        {
            if (reader != null & waveOut != null)
            {
                reader.Volume = volume;


            }

            else MessageBox.Show("No music selected");

        }

        public void dispose()
        {
            if (waveOut != null & reader != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing) waveOut.Stop();
                if (waveOut.PlaybackState == PlaybackState.Paused) waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
                reader.Dispose();
                reader = null;
                timer1.Enabled = false;

            }



        }

        public void rewindnazad(ListBox k)
        {


            if (k.SelectedIndex - 1 >= 0)
            {
                k.SelectedIndex =k.SelectedIndex - 1;
                dispose();
                play_sound(tpat.ElementAt(k.SelectedIndex));
            }
            else if (k.SelectedIndex == 0)
            {

                k.SelectedIndex = k.Items.Count - 1;
                dispose();
                play_sound(tpat.ElementAt(k.SelectedIndex));
            }
            else MessageBox.Show("No music selected", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }



        public void rewindvpered(ListBox k)
        {
            if (k.SelectedIndex + 1 <= k.Items.Count - 1)
            {
                k.SelectedIndex = k.SelectedIndex + 1;
                dispose();
                play_sound(tpat.ElementAt(k.SelectedIndex));
            }
            else if (k.SelectedIndex == k.Items.Count - 1)
            {

                k.SelectedIndex = 0;
                dispose();
                play_sound(tpat.ElementAt(k.SelectedIndex));
            }
            else MessageBox.Show("No music selected", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);



        }

        public void play_sound(string check)
        {

            if (check != null)
            {
                dispose();
                this.reader = new AudioFileReader(check);
                waveOut = new WaveOut();
                // or WaveOutEvent()
                this.waveOut.Init(reader);
                timer1.Enabled = true;
                this.waveOut.Play();
                
                textBox1.Text = getpos();

                textBox2.Text = curpos();
                trackBar1.Minimum = 0;
              
                trackBar1.Maximum = (int)reader.TotalTime.TotalSeconds;

            }

            else MessageBox.Show("No music selected", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);


        }
        public void pause_sound()
        {
            if (waveOut.PlaybackState == PlaybackState.Playing) { waveOut.Pause(); }
            else { waveOut.Resume(); }



       

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
        public MySqlConnection rcon(string server, string user, string database, string password)
        {
            return new MySqlConnection("server=" + server + ";UserId=" + user + ";database=" + database + ";password=" + password + ";");
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
                if (myrd.Read()) { c = Convert.ToInt32(myrd.GetValue(0)); }

                else

                    c = 0;

            }
            myrd.Dispose();
            mycm.Dispose();
            myrd.Close();
            mysql.Dispose();
            mysql.Close();
            return c;

        }
        public void updateonlinestatus(string username,int c)
        {
            if (checklogin(username))
            {

                int  k = getidbynickname(username);
                MySqlConnection mysql = rcon("localhost", "Frost", "kvsl", "Frost1234!");
                MySqlCommand mycm = new MySqlCommand();
                mycm.Connection = mysql;
                mysql.Open();
                mycm.CommandText = "update kvsl.userinfo set kvsl.userinfo.onoff=@dval where kvsl.userinfo.id=@usr;";
                mycm.Parameters.AddWithValue("@dval", c);
                mycm.Parameters.AddWithValue("@usr", k);
                mycm.ExecuteNonQuery();
                mysql.Dispose();
                mysql.Close();
                mycm.Dispose();

            }
        }
        public int ist(string c, char d)
        {
            int s = 0;
            
            for (int i = 0; i < c.Length; i++)
            {
                if (c.ElementAt(i) == d) { s = i; }

            }
            return s;

        }
        
        bool volu = true;
        string typeofaccountf;
        public Form1(string username,string password,string typeofaccount)
        { 
            u = username;
            p = password;
            us = new userinfo(username);
            typeofaccountf = typeofaccount;

            InitializeComponent();
        }
        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = false;
            updateonlinestatus(u,1);
            groupBox1.AllowDrop = true;
            pictureBox1.MouseEnter += new EventHandler(pictureBox_MouseEnter);
            pictureBox2.MouseEnter += new EventHandler(pictureBox_MouseEnter);
            pictureBox3.MouseEnter += new EventHandler(pictureBox_MouseEnter);
            pictureBox4.MouseEnter += new EventHandler(pictureBox_MouseEnter);
            pictureBox1.MouseLeave += new EventHandler(pictureBox_MouseLeave);
            pictureBox2.MouseLeave += new EventHandler(pictureBox_MouseLeave);
            pictureBox3.MouseLeave += new EventHandler(pictureBox_MouseLeave);
            pictureBox4.MouseLeave += new EventHandler(pictureBox_MouseLeave);
           pictureBox6.Image = (Image)AudioConverterBD.Properties.Resources.Forward;
            pictureBox10.Image = (Image)AudioConverterBD.Properties.Resources.Back;
            pictureBox7.Image= (Image)AudioConverterBD.Properties.Resources.Rewind;
            pictureBox9.Image = (Image)AudioConverterBD.Properties.Resources.unwindt2;
            pictureBox8.Image= (Image)AudioConverterBD.Properties.Resources.Play;
            pictureBox11.Image = (Image)AudioConverterBD.Properties.Resources.volu;
            pictureBox13.Image = (Image)AudioConverterBD.Properties.Resources.Pause;
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox10.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            
            ToolStripMenuItem Osnovnie = new ToolStripMenuItem();
            Osnovnie.Text = "Options";
            ToolStripMenuItem Profil = new ToolStripMenuItem();
            Profil.Text = "Profile";
            Profil.Click += new EventHandler(profil);
            ToolStripMenuItem Nastroyki = new ToolStripMenuItem();
            Nastroyki.Text = "GUI";
            ToolStripMenuItem Themes = new ToolStripMenuItem();
            Themes.Text = "Themes";
            ToolStripMenuItem Logout = new ToolStripMenuItem();
            Logout.Text = "Logout";
            Logout.Click += new EventHandler(logout);
            Themes.DropDownItems.Add(new ToolStripMenuItem("Dark") { CheckOnClick = true,Checked=false});
            Themes.DropDownItems.Add(new ToolStripMenuItem("Yellow") { CheckOnClick = true, Checked = false});
            Themes.DropDownItems.Add(new ToolStripMenuItem("WhiteBlue") { CheckOnClick = true, Checked = false});
            Themes.DropDownItems[0].Click += new EventHandler(darktheeme);
            Themes.DropDownItems[1].Click += new EventHandler(yellowtheme);
            Themes.DropDownItems[2].Click += new EventHandler(whitebluetheme);
            Nastroyki.DropDownItems.Add(Themes);
            Osnovnie.DropDownItems.Add(Nastroyki);
            Osnovnie.DropDownItems.Add(Profil);
            Osnovnie.DropDownItems.Add(Logout);
            menuStrip1.Items.Add(Osnovnie);
            this.Icon = Icon.FromHandle(AudioConverterBD.Properties.Resources.utesve.GetHicon());
            n = this.BackColor;

        }
        private void logout(object sender, EventArgs e)
        {
            this.Close();
            bdwindowcs t = new bdwindowcs();
            t.Show();
                
        }
        private void darktheeme(object sender, EventArgs e)
        {
            if (!dtem )
            {
                this.BackColor = Color.DarkBlue;
                dtem = true;
            }
            else
            {
                this.BackColor = n;
                dtem = false;
            }
        }
        private void yellowtheme(object sender, EventArgs e)
        {
            if (!yt)
            {
                this.BackColor = Color.LightYellow;
                yt = true;
            }
            else
            {
                this.BackColor = n;
                yt = false;
            }
        }
        private void whitebluetheme(object sender, EventArgs e)
        {
            if (!wt)
            {
                this.BackColor = Color.GhostWhite;
                wt = true;
            }
            else
            {
                this.BackColor = n;
                 wt = false;
            }
        }

        private void profil(object sender, EventArgs e)
        {

            if (us.IsDisposed || us == null)
            {
                us = new userinfo(u); us.Show();
            }
            else {us.Show(); }
        }




        private void groupBox1_DragDrop(object sender, DragEventArgs e)
        {
            string [] l1 = (string [])e.Data.GetData(DataFormats.FileDrop,false);
            string k = string.Join("", l1);
            tpat.Add(k);
            
            int sa1 = ist(k, '\\');
            k = k.Substring(sa1+1, k.Length - sa1 - 1);
            MusicChart.Items.Add(k);
            
         
        
           
        }

        private void groupBox1_DragEnter(object sender, DragEventArgs e)
        {
            
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {


                e.Effect = DragDropEffects.None;

            }
            else
            {

                e.Effect = DragDropEffects.All;
            }
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            Size k = ((PictureBox)sender).Size;
            k.Height = k.Height + 30;
            k.Width = k.Width + 40;
           ((PictureBox)sender).Size = k;

            

        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            Size k = ((PictureBox)sender).Size;
            k.Height = k.Height - 30;
            k.Width = k.Width -40;
            ((PictureBox)sender).Size = k;
        }

      

        private void pictureBox8_Click(object sender, EventArgs e)
        {
           

            play_sound(tpat.ElementAt(MusicChart.SelectedIndex));

            

           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            updateonlinestatus(u,0);
            dispose();
            
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            pause_sound();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            dispose();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            rewindvpered(MusicChart);
        }

      

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            cvol(trackBar2.Value);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (reader != null)
            {   
                setmpos(trackBar1.Value);
                
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            rewindnazad(MusicChart);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                setmpos((float)(reader.CurrentTime.TotalSeconds - 10));
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                setmpos((float)(reader.CurrentTime.TotalSeconds + 10));
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (typeofaccountf == "standart")
            {
                MediaFoundationReader aac = new MediaFoundationReader(tpat.ElementAt(MusicChart.SelectedIndex));
                WaveFormat news = new WaveFormat(aac.WaveFormat.SampleRate, aac.WaveFormat.BitsPerSample, aac.WaveFormat.Channels);
                saveFileDialog1.Filter = "AAC|*.aac";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    MediaFoundationEncoder.EncodeToAac(aac, saveFileDialog1.FileName);


                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (typeofaccountf == "standart")
            {
                MediaFoundationReader mp3 = new MediaFoundationReader(tpat.ElementAt(MusicChart.SelectedIndex));
                saveFileDialog1.Filter = "MP3|*.mp3";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    MediaFoundationEncoder.EncodeToMp3(mp3, saveFileDialog1.FileName);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MediaFoundationReader audio = new MediaFoundationReader(tpat.ElementAt(MusicChart.SelectedIndex));
            WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(audio);
            saveFileDialog1.Filter = "WAV|.*wav";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                WaveFileWriter.CreateWaveFile(saveFileDialog1.FileName, pcm);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MediaFoundationReader wma = new MediaFoundationReader(tpat.ElementAt(MusicChart.SelectedIndex));
            saveFileDialog1.Filter = "WMA|*.wma";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                MediaFoundationEncoder.EncodeToWma(wma, saveFileDialog1.FileName);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            textBox2.Text = curpos();
            last5timeloude();
            
            
        }
        private void last5timeloude()
        {
            if (reader != null )
            {
                if ((int)Math.Abs(reader.CurrentTime.TotalSeconds - reader.TotalTime.TotalSeconds) == 200)
                {
                    

                    reader.Volume = 0;
                }

            }

        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            if (volu)
            {
                pictureBox11.Image = (Image)AudioConverterBD.Properties.Resources.novolum;
                volu = false;
                reader.Volume = 0.0f;

            }

            else
            {
                pictureBox11.Image = (Image)AudioConverterBD.Properties.Resources.volu;
                volu = true;
                reader.Volume = 1.0f;
            }
        }
    }
}
