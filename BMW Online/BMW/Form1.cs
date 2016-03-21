using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BMW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        WebClient web = new WebClient();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            /*WebClient web = new WebClient();
            string str=web.UploadString("https://openmobile.qq.com/oauth2.0/m_sdkauthorize", "uin=2978003974&app_id=101089751&response_type=token&sdkv=2.8&status_version=9&client_id=101089751&app_name=%E7%A4%BE%E4%BA%A4%E4%BA%92%E8%81%94&status_machine=iPad2%2C5&skey=@PmcQ4oymZ&status_os=9.0.2&scope=get_user_info%2Cget_info%2Cget_simple_userinfo&openapi=&format=json&sdkp=i");
            Trace.WriteLine(str);*/
            
        }
        public static string Between(string str, string leftstr, string rightstr)
        {
            int i = str.IndexOf(leftstr) + leftstr.Length;
            string temp = str.Substring(i, str.IndexOf(rightstr, i) - i);
            return temp;
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(e.Url.ToString()== "http://www.qq.com/qq2012/loginSuccess.htm")
            {
                string[] CookieL=webBrowser1.Document.Cookie.Replace(" ","").Split(';');
                string skey="";
                string qq = "";
                foreach(string s in CookieL)
                {
                    if (s.Substring(0,5) =="skey=")
                    {
                        skey = s.Substring(5, s.Length - 5);
                    }else if (s.Substring(0, 4) == "uin=")
                    {

                        string ss = s.Replace("uin=", "");
                        bool bo = false;
                        for(int i=0; i<ss.Length;i++)
                        {
                            if (bo == false)
                            {
                                if (ss.Substring(0, 1) != "o" && ss.Substring(0, 1) != "0")
                                {
                                    bo = true;
                                    qq += ss.Substring(0, 1);
                                }
                            }
                            else
                            {
                                qq += ss;
                                break;
                            }

                            ss = ss.Substring(1, ss.Length - 1);

                        }
                    }
                    
                }
                if (skey=="" || qq=="")
                {
                    MessageBox.Show("登录失败!");
                    Close();
                }
                else
                {
                    try
                    {
                        web.Encoding = Encoding.UTF8;
                        web.Headers.Add("User-Agent", "TencentConnect SDK device = iPad Os = ios Version = 9.0.2");
                        string str=web.UploadString("https://openmobile.qq.com/oauth2.0/m_sdkauthorize", "uin="+qq+"&app_id=101089751&response_type=token&sdkv=2.8&status_version=9&client_id=101089751&app_name=社交互联&status_machine=iPad2,5&skey="+skey+"&status_os=9.0.2&scope=get_user_info,get_info,get_simple_userinfo&openapi=&format=json&sdkp=i");
                        string a_t = Between(str, "\"access_token\":\"", "\"");
                        string o= Between(str, "\"openid\":\"", "\"");
                        str=web.UploadString("https://opensdk.mobile.qq.com/v3/openqq/getfriendlist?sdkappid=101089751&accesstoken="+a_t+"&sdkapptoken=0138874c9385a18d173d0e82bc008a0cf7&openid="+o+"&openappid=101089751&apn=1", "{\r\n  \"GetFriendCount\" : \"100\",\r\n  \"StartFriendIndex\" : \"0\"\r\n}");
                        string echocode= Between(str, "\"ActionStatus\":\"", "\"");
                        if (echocode != "OK")
                        {
                            MessageBox.Show("出现错误!错误代码:" + echocode);
                        }
                        else
                        {
                            str = web.UploadString("https://opensdk.mobile.qq.com/v3/openqq/getmsg?sdkappid=101089751&accesstoken=" + a_t + "&sdkapptoken=01696ced2b5f1094fe2f0310b284a28901&openid=" + o + "&openappid=101089751&apn=1", "{\r\n  \"SyncFlag\" : 0,\r\n  \"Cookie\" : \"08acf7e6b50510acf7e6b50518b48ea0d00b20cea7b5d409288a9696800530cf9f03388ae2d3f30540a3f5034881faae9e0f58b6a5b3b405\"\r\n}");
                            echocode = Between(str, "\"ActionStatus\":\"", "\"");
                            if (echocode != "OK")
                            {
                                MessageBox.Show("出现错误!错误代码:" + echocode);
                            }
                            else
                            {
                                MessageBox.Show("完成!重启QQ可看到效果!\r\n贴吧:腾训公司\r\nQQ:949138278");
                            }
                        }
                        webBrowser1.Url = new Uri("http://ui.ptlogin2.qq.com/cgi-bin/login?hide_title_bar=0&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=636014201&target=self&s_url=http%3A//www.qq.com/qq2012/loginSuccess.htm");

                    }
                    catch
                    {
                        MessageBox.Show("出现错误!");
                        Close();
                    }
                }
            }
        }
    }
}
