using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TESTSERVE
{
    public partial class Form1 : Form
    {
        public linkTest link;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            link = new linkTest();
            link.linkEntrance();
            link.ReceiveMessage += (message) => AddLine("haha: " + message);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMessage.Text))
                return;

            link.PushMessage(textBoxMessage.Text);
            textBoxMessage.Text = "";
        }

        private void AddLine(string html)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();//测试时间的类
            stopwatch.Start(); //  开始监视代码运行时间
            //listView1.Items.Insert(0, "<div>" + html + "</div>");
            //richtextboxmessages.invoke(new action(delegate
            //{
            //    richtextboxmessages.text += environment.newline + "<div>" + html + "</div>";//  需要测试的代码 ....
            //}));
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            Trace.WriteLine(timespan.TotalMilliseconds);
        }

    }
}
