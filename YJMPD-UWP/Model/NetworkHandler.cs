using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.Helpers.EventArgs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace YJMPD_UWP.Model
{
    public class NetworkHandler
    {

        public enum Commands
        {
            Hi,
            Name,
            Picture
        }

        public delegate void OnStatusUpdatedHandler(object sender, NetworkStatusUpdatedEventArgs e);
        public event OnStatusUpdatedHandler OnStatusUpdate;

        public enum NetworkStatus { DISCONNECTED, CONNECTING, CONNECTED }
        public NetworkStatus Status { get; private set; }

        private StreamSocket client;
        DataWriter dout;
        StreamReader din;

        private void UpdateNetworkStatus(NetworkStatus status)
        {
            Status = status;

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new NetworkStatusUpdatedEventArgs(status));
        }

        public NetworkHandler()
        {
            Status = NetworkStatus.DISCONNECTED;
            OpenConnection();
        }


        //API stuff
        public async Task<bool> SearchGame(GameHandler g, string playername)
        {
            JObject obj = JObject.FromObject(new
            {
                command = Commands.Name.ToString(),
                name = playername
            });
            Debug.WriteLine(obj.ToString(Formatting.None));
            Write(obj.ToString(Formatting.None));

            var response = await Read();
            Debug.WriteLine(response);
            JObject o = JObject.Parse(response);
            if(o["msg"].ToString() == "ok")
            {
                g.AddPlayer(playername);
                return true;
            }

            return false;
        }

        public async Task<bool> WaitingForPlayers(GameHandler g)
        {
            bool added = false;

            var response = await Read();
            Debug.WriteLine(response);
            JObject o = JObject.Parse(response);
            try {
                g.AddPlayer(o["player"].ToString());
                added = true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            return added;
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
            return din.ReadLine();
        }

        private async Task<bool> WriteData(string data)
        {
            dout.WriteString(data + Environment.NewLine);
            await dout.StoreAsync();
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

            UpdateNetworkStatus(NetworkStatus.CONNECTED);

            client = new StreamSocket();

            StreamSocketControl controller = client.Control;
            controller.KeepAlive = true;

            await client.ConnectAsync(new HostName("imegumii.space"), "3333");


            din = new StreamReader(client.InputStream.AsStreamForRead());
            dout = new DataWriter(client.OutputStream);

            //din.UnicodeEncoding = UnicodeEncoding.Utf8;
            //din.ByteOrder = ByteOrder.LittleEndian;

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
