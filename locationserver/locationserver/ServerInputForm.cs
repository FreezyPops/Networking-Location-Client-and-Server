using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace locationserver
{
    public partial class ServerInputForm : Form
    {
        public ServerInputForm()
        {
            InitializeComponent();
        }

        private void launchServerButton_Click(object sender, EventArgs e)
        {
            int timeout = int.Parse(timeoutTextBox.Text.ToString());
            int[] commandLineArray = new int[1];
            commandLineArray[0] = timeout;
            Whois.setArrayFromGUI(commandLineArray);
            this.Close();
        }
    }
}
