using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace Change_IP_Address_V01._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isInitialized = false; //Boolean to keep radio buttons from executing during initialization
        private const string ADAPTER_NAME = "Local Area Connection";//Constant that gives the name of the adapter to change
    
        public MainWindow()
        {
            InitializeComponent();
            SetCurrentIPSetting(ADAPTER_NAME);//Gets the current IP setup and applies it to the Radio Buttons
            isInitialized = true;//Change boolean to indicate initialization is complete
        }

        /// <summary>
        /// Sets the IP address to DHCP when the autoIP radio button is checked
        /// Uses isInitialized boolean to prevent unneccesary execution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoIP_Checked(object sender, RoutedEventArgs e)
        {
            if (isInitialized)
            {
                setAutoIP();
            }
        }

        /// <summary>
        /// Sets the IP address to Static when the staticIP radio button is checked
        /// Uses isInitialized boolean to prevent unneccesary execution
        /// Static IP: 192.168.29.50
        /// Subnet Mask: 255.255.255.0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void staticIP_Checked(object sender, RoutedEventArgs e)
        {
            if (isInitialized)
            {
                setStaticIP("192.168.29.50", "255.255.255.0");
            }
        }
        /*
        private void confirmIPUpdate(string adapterName, bool isDHCP)
        {
            bool ipUpdated = false;
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            while (!ipUpdated)
            {
                foreach (NetworkInterface adapter in interfaces)
                {
                    if (adapter.Name == adapterName)
                    {
                        if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                        {
                            if (adapter.GetIPProperties().GetIPv4Properties() != null)
                            {
                                if (adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled == isDHCP)
                                {
                                    MessageBox.Show("The IP settings have successfully updated.");
                                    ipUpdated = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        */

        /// <summary>
        /// Closes the application when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void done_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Set's a new IP Address and it's Submask of the local machine
        /// </summary>
        /// <param name="ip_address">The IP Address</param>
        /// <param name="subnet_mask">The Submask IP Address</param>
        public void setStaticIP(string ip_address, string subnet_mask)
        {
            string arguments = "interface ip set address \"" + ADAPTER_NAME + "\" static " + ip_address + " " + subnet_mask;
            setIP(arguments);
        }

        /// <summary>
        /// Sets the network Adapter to DHCP
        /// </summary>
        public void setAutoIP()
        {
            string arguments = "interface ip set address \"" + ADAPTER_NAME + "\" dhcp";
            setIP(arguments);
        }

        /// <summary>
        /// Runs a netsh command with the given arguments
        /// </summary>
        /// <param name="arguments">The arguments for the netsh command</param>
        private static void setIP(string arguments)
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("netsh", arguments);
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            p.StartInfo = psi;
            p.Start();
        }

        /// <summary>
        /// Initializes the Radio Buttons so they reflect the current IP setup
        /// </summary>
        /// <param name="adapterName">The name of the adapter to use</param>
        public void SetCurrentIPSetting(string adapterName)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in interfaces)
            {
                if (adapter.Name == adapterName)
                {
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        if (adapter.GetIPProperties().GetIPv4Properties() != null)
                        {
                            if (adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
                            {
                                autoIP.IsChecked = true;
                            }
                            else
                            {
                                staticIP.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
