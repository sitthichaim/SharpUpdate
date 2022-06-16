using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using SharpUpdate;

namespace UpdateApp
{
    public partial class Form1 : Form, SharpUpdateInterface
    {

        public Form1()
        {
            InitializeComponent();

            this.label1.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public string ApplicationName => "UpdateApp";

        public string ApplicationId => "UpdateApp";

        public Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

        public Icon ApplicationIcon => this.Icon;

        public Uri UpdateXmlLocation => new Uri("http://127.0.0.1:8080/update.xml");

        public Form Context => this;

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}