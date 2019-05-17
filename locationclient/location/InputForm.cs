using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace location
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
            portNumberTextBox.Text = "43";
            serverAddressTextBox.Text = "whois.net.dcs.hull.ac.uk";
            timeoutTextBox.Text = "1000";

            protocolComboBox.Items.Add("Whois");
            protocolComboBox.Items.Add("HTTP/0.9");
            protocolComboBox.Items.Add("HTTP/1.0");
            protocolComboBox.Items.Add("HTTP/1.1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string portnumber = portNumberTextBox.Text.ToString();
            string address = serverAddressTextBox.Text.ToString();
            string protocolChoice = protocolComboBox.SelectedItem.ToString();
            string user = userTextBox.Text.ToString();
            string location = locationTextBox.Text.ToString();
            string timeout = timeoutTextBox.Text.ToString();

            string finalCommandLineString;

            switch (protocolChoice)
            {
                case "HTTP/0.9":
                    protocolChoice = "-h9";
                    finalCommandLineString = "-t" + " " + timeout + " " + "-p" + " " + portnumber + " " + "-h" + " "+ address + " " + protocolChoice + " " + user + " " + location;
                    break;

                case "HTTP/1.0":
                    protocolChoice = "-h0";
                    finalCommandLineString = "-t" + " " + timeout + " " + "-p" + " " + portnumber + " " + "-h" + " " + address + " " + protocolChoice + " " + user + " " + location;
                    break;

                case "HTTP/1.1":
                    protocolChoice = "-h1";
                    finalCommandLineString = "-t" + " " + timeout + " " + "-p" + " " + portnumber + " " + "-h" + " " + address + " " + protocolChoice + " " + user + " " + location;
                    break;

                default:
                    finalCommandLineString = "-t" + " " + timeout + " " + "-p" + " " + portnumber + " " + "-h" + " " + address + " " + user + " " + location;
                    break;
            }

            string[] commandLineArray = finalCommandLineString.Trim().Split(new char[] { ' ' });
            
            Whois.setStringFromGUI(commandLineArray);
            this.Close();
        }

        
    }
}
