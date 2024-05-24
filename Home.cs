using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Caro
{
    public partial class Home : Form
    {
        public  string Username;
        
        public Home(string username)
        {
            InitializeComponent();
            this.Username = username;
          
            
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "ZSYPCgwNgtDZLgNkwTsJyN6Z6tc6IKfG8gJNJL6S",
            BasePath = "https://game-caro-f1c0c-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        private void Home_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }

            catch
            {
                MessageBox.Show("No Internet or Connection Problem");
            }
            ShowInfo();
        }
        private async void ShowInfo()
        {
            FirebaseResponse res;
            try
            {
                res = await client.GetAsync(@"Player " + Username);
                Dictionary<string, string> data = JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(res.Body.ToString());
                txb_Password.Text = data.ElementAt(3).Value;
                txb_Age.Text = data.ElementAt(0).Value;
                txb_Fullname.Text = data.ElementAt(1).Value;
                txb_Username.Text = data.ElementAt(4).Value;
                txb_Win.Text = data.ElementAt(5).Value;
                txb_Lose.Text = data.ElementAt(2).Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private void btn_PlayGame_Click(object sender, EventArgs e)
        {

            this.Hide();
            Game_Caro g=new Game_Caro(txb_Username.Text, this);
            g.ShowDialog();
            
           
        }

        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Do you want to log out?", "Question?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
            {
                this.Hide();
                LogIn g = new LogIn();
                g.ShowDialog();
                this.Close();
            }
        }

        private async void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                FirebaseResponse res = await client.GetAsync(@"Player " + Username);
                tbPlayer pl = new tbPlayer();
                Dictionary<string, string> data = JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(res.Body.ToString());
                pl.Password = txb_Password.Text;
                pl.Age = int.Parse(txb_Age.Text);
                pl.Fullname = txb_Fullname.Text;
                pl.Username = data.ElementAt(4).Value;
                pl.Win = int.Parse(data.ElementAt(5).Value);
                pl.Lose = int.Parse(data.ElementAt(2).Value);
                var update = await client.UpdateAsync(@"Player " + pl.Username, pl);
                MessageBox.Show("Update successfully! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                FirebaseResponse res = await client.GetAsync(@"Player " + Username);
                tbPlayer pl = new tbPlayer();
                Dictionary<string, string> data = JsonConvert.DeserializeObject
                    <Dictionary<string, string>>(res.Body.ToString());
                pl.Password = txb_Password.Text;
                pl.Age = int.Parse(txb_Age.Text);
                pl.Fullname = txb_Fullname.Text;
                pl.Username = data.ElementAt(4).Value;
                pl.Win = int.Parse(data.ElementAt(5).Value);
                pl.Lose = int.Parse(data.ElementAt(2).Value);

                DialogResult dialog;
                dialog = MessageBox.Show("Are you sure you want to delete this account ?", 
                    "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    var delete = await client.DeleteAsync(@"Player " + pl.Username);
                    MessageBox.Show("Delete successfully! ");
                    this.Hide();
                    LogIn l=new LogIn();
                    l.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
