using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rac.VOne.Common.Security;

namespace Rac.VOne.Client.Screen
{
    public partial class dlgPassWordAnswer : Form
    {
        CompanyPasswordCreator Creator;
        private int clickCount;

        public dlgPassWordAnswer()
        {
            InitializeComponent();
            Text = "パスワード確認";
            Creator = new CompanyPasswordCreator();
            this.ActiveControl = this.txtGetPassWord;
            string n = GetKeyWord(8);
            txtKeyWord.Text = n;
        }

        private string GetKeyWord(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random s = new Random();
            string KeyWordChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
            for (int i = 0; i<length; i++)
            {  
                int pos = s.Next(KeyWordChars.Length);
                char c = KeyWordChars[pos];
                sb.Append(c);
            }
            return　sb.ToString();
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            string result = Creator.ConvertKeyWordToPassword(txtKeyWord.Text);
         


            if (result == txtGetPassWord.Text.ToLower())
            {
                DialogResult = DialogResult.OK;
            }

            else
            {
                if (txtGetPassWord.Text == "")
                {
                  MessageBox.Show("パスワードが未入力です");
                  this.ActiveControl = this.txtGetPassWord;
                }
                else
                {
                    clickCount++;
                    int p = clickCount;

                    if (p <= 2)
                    {
                        MessageBox.Show("パスワードが正しくありません");
                        this.ActiveControl = this.txtGetPassWord;
                        
                    }
                    else
                    {
                        MessageBox.Show("もう一度やり直してください。");
                        this.Close();
                        p = 0;
                    }
                }
            }
        }                       

    private void CloseButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
          
            this.Close();  
           
        }
    }
}

           



 