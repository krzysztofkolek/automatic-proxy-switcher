using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ProxyToggle
{
    public partial class Form1 : Form
    {
        public DataModel Data { get; set; }

        public Form1()
        {
            InitializeComponent();

            using (StreamReader sr = new StreamReader("Data.json"))
            {
                Data = JsonConvert.DeserializeObject<DataModel>(sr.ReadToEnd());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "inetcpl.cpl";
            process.StartInfo.Arguments = "-n";            
            process.Start();

            // Get internet options window
            Window internetOptions = Desktop.Instance.Windows().Find(window => window.Title.Equals("Internet Properties"));

            // Move to "Connections" tab
            internetOptions.Get<TestStack.White.UIItems.TabItems.Tab>().SelectTabPage("Connections");
            var tab = internetOptions.Get<TestStack.White.UIItems.TabItems.Tab>().SelectedTab;

            // Select LAN settings
            internetOptions.Get<TestStack.White.UIItems.Button>("LAN settings").Click();
                        
            var modal = internetOptions.ModalWindows().Find(window => window.Title.Contains("Local Area Network (LAN) Settings"));
            modal.Get<TestStack.White.UIItems.CheckBox>(SearchCriteria.ByText("Use a proxy server for your LAN (These settings will not apply to dial-up or VPN connections).")).Click();
            modal.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Advanced")).Click();

            var proxy = modal.ModalWindows().First();
            var textBoxesInProxyWindow = proxy.GetMultiple(SearchCriteria.All);
            textBoxesInProxyWindow[6].Enter("Test");
            textBoxesInProxyWindow[8].Enter("1234");
            proxy.Get<TestStack.White.UIItems.Button>("Use the same proxy server for all protocols").Click();
            proxy.Get<TestStack.White.UIItems.Button>("OK").Click();

            modal.Get<TestStack.White.UIItems.CheckBox>(SearchCriteria.ByText("Bypass proxy server for local addresses")).Click();
            modal.Get<TestStack.White.UIItems.Button>("OK").Click();

            internetOptions.Get<TestStack.White.UIItems.Button>("OK").Click();
        }

        private void ToggleProxyBtn_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "inetcpl.cpl";
            process.StartInfo.Arguments = "-n";
            process.Start();

            // Get internet options window
            Window internetOptions = Desktop.Instance.Windows().Find(window => window.Title.Equals("Internet Properties"));

            // Move to "Connections" tab
            internetOptions.Get<TestStack.White.UIItems.TabItems.Tab>().SelectTabPage("Connections");
            var tab = internetOptions.Get<TestStack.White.UIItems.TabItems.Tab>().SelectedTab;

            // Select LAN settings
            internetOptions.Get<TestStack.White.UIItems.Button>("LAN settings").Click();

            var modal = internetOptions.ModalWindows().Find(window => window.Title.Contains("Local Area Network (LAN) Settings"));
            modal.Get<TestStack.White.UIItems.CheckBox>(SearchCriteria.ByText("Use a proxy server for your LAN (These settings will not apply to dial-up or VPN connections).")).Click();
            modal.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Advanced")).Click();

            internetOptions.Get<TestStack.White.UIItems.Button>("OK").Click();
        }        
    }
}
