using System;
using System.Windows.Forms;
using CovertTweeter.Core;

namespace CovertTweeter
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var repo = new TweetRepository();
            //var result = repo.GetTweets();
            
            //result.ForEach(t => { 
            //    var text = string.Format("User: {0}, Tweet: {1}\n", 
            //        t.User.ScreenName,
            //        t.Text);
            //    txtMain.AppendText(text);
            //});
                
        }
    }
}
