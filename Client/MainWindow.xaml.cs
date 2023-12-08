using Server;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace Client
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListenClientAsync();
        }

        private async void ListenClientAsync()
        {
            UdpClient udpClient=new UdpClient(27001);
            while(true)
            {
                var result = await udpClient.ReceiveAsync();
                var bytes = result.Buffer;
                dataGrid.ItemsSource = ByteArrayToObject(bytes) as List<Car>;
            }
        }


        static object ByteArrayToObject(byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return formatter.Deserialize(memoryStream);
            }
        }
        static byte[] ObjectToByteArray(object command)
        {
            if (command == null)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, command);
                return memoryStream.ToArray();
            }
        }

        private void getClick(object sender, RoutedEventArgs e)
        {
            var command = new Command()
            {
                Method = HttpMethods.GET
            };
            var client = new UdpClient();
            var connectionEp = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 12345);
            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectionEp);
        }

        private void postClick(object sender, RoutedEventArgs e)
        {
            var command = new Command()
            {
                Method = HttpMethods.POST,
                Car=dataGrid.SelectedItem as Car
            };
            var client = new UdpClient();
            var connectionEp = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 12345);
            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectionEp);
        }

        private void putClick(object sender, RoutedEventArgs e)
        {
            var command = new Command()
            {
                Method = HttpMethods.PUT,
                Car = dataGrid.SelectedItem as Car

            };
            var client = new UdpClient();
            var connectionEp = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 12345);
            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectionEp);
        }

        private void deleteClick(object sender, RoutedEventArgs e)
        {
            var command = new Command()
            {
                Method = HttpMethods.DELETE,
                Car = dataGrid.SelectedItem as Car
            };
            var client = new UdpClient();
            var connectionEp = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 12345);
            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectionEp);
        }
    }
}