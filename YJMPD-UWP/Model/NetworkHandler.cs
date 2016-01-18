using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class NetworkHandler
    {
        public delegate void OnStatusUpdatedHandler(object sender, NetworkStatusUpdatedEventArgs e);
        public OnStatusUpdatedHandler OnStatusUpdated;

        public enum NetworkStatus { DISCONNECTED, CONNECTING, CONNECTED }
        public NetworkStatus Status { get; private set; }

        private StreamSocket client;
        DataWriter dout;
        DataReader din;

        private void UpdateNetworkStatus(NetworkStatus status)
        {
            Status = status;

            if (OnStatusUpdated == null) return;

            OnStatusUpdated(this, new NetworkStatusUpdatedEventArgs(status));
        }

        public NetworkHandler()
        {
            Status = NetworkStatus.DISCONNECTED;
            OpenConnection();
        }


        //API stuff
        public async Task<bool> SearchGame()
        {
            Random r = new Random();
            int l = r.Next(0, 2);
            return l < 1;
        }


        //Writing and Reading
        public async Task<string> Read()
        {
            return await ReadData();
        }

        public async Task<bool> Write(string data)
        {
            return await WriteData(data);
        }

        private async Task<string> ReadData()
        {
            uint length = din.ReadUInt32();
            string data = din.ReadString(length);

            return data;
        }

        private async Task<bool> WriteData(string data)
        {
            dout.WriteUInt32((uint)data.Length);
            dout.WriteString(data);

            await dout.FlushAsync();
            
            return true;
        }

        //Connecting and Disconnecting

        public async Task<bool> Connect()
        {
            return await OpenConnection();
        }

        public async Task<bool> Disconnect()
        {
            return await CloseConnection();
        }

        private async Task<bool> OpenConnection()
        {
            UpdateNetworkStatus(NetworkStatus.CONNECTING);

            client = new StreamSocket();

            StreamSocketControl controller = client.Control;
            controller.KeepAlive = true;

            await client.ConnectAsync(new HostName(Settings.Values["hostname"] as string), "YJMPD-UWP-Server");


            din = new DataReader(client.InputStream);
            dout = new DataWriter(client.OutputStream);

            din.UnicodeEncoding = UnicodeEncoding.Utf8;
            din.ByteOrder = ByteOrder.LittleEndian;

            dout.UnicodeEncoding = UnicodeEncoding.Utf8;
            dout.ByteOrder = ByteOrder.LittleEndian;


            UpdateNetworkStatus(NetworkStatus.CONNECTED);

            return true;
        }

        private async Task<bool> CloseConnection()
        {
            UpdateNetworkStatus(NetworkStatus.DISCONNECTED);

            din.Dispose();
            dout.Dispose();

            client.Dispose();

            return true;
        }
    }
}
