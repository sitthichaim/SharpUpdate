using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using SharpUpdate;
using System.Diagnostics;

namespace UpdateApp
{
    public partial class Form1 : Form, SharpUpdateInterface
    {

        private SharpUpdater updater;

        public Form1()
        {
            InitializeComponent();

            this.label1.Text = this.ApplicationAssembly.GetName().Version.ToString();
            updater = new SharpUpdater(this);
        }

        public string ApplicationName => "UpdateApp";

        public string ApplicationId => "UpdateApp";

        public Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

        public Icon ApplicationIcon => this.Icon;

        public Uri UpdateXmlLocation => new Uri("http://127.0.0.1:8080/update.xml");

        public Form Context => this;

        private void button1_Click(object sender, EventArgs e)
        {
            updater.DoUpdate();
        }
    }
}