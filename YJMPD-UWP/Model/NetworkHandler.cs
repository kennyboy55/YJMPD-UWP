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
using Windows.Foundation;

namespace YJMPD_UWP.Model
{
    public class NetworkHandler
    {
        public delegate void OnStatusUpdatedHandler(object sender, NetworkStatusUpdatedEventArgs e);
        public event OnStatusUpdatedHandler OnStatusUpdate;

        public enum NetworkStatus { DISCONNECTED, CONNECTING, CONNECTED }
        public NetworkStatus Status { get; private set; }

        private IAsyncAction BackgroundReader;

        private StreamSocket client;
        private DataWriter dout;
        private StreamReader din;

        private void UpdateNetworkStatus(NetworkStatus status)
        {
            Status = status;

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new NetworkStatusUpdatedEventArgs(status));
        }

        public NetworkHandler()
        {
            Status = NetworkStatus.DISCONNECTED;
            Connect();
        }

        private async Task<string> Read()
        {
            return await din.ReadLineAsync();
        }

        public async Task<bool> Write(string data)
        {
            dout.WriteString(data + Environment.NewLine);
            await dout.StoreAsync();
            await dout.FlushAsync();
            
            return true;
        }

        //Connecting and Disconnecting

        private async Task<bool> Connect()
        {
            UpdateNetworkStatus(NetworkStatus.CONNECTING);

            client = new StreamSocket();

            StreamSocketControl controller = client.Control;
            controller.KeepAlive = true;

            await client.ConnectAsync(new HostName(Settings.Values["hostname"] as string), Settings.Values["port"] as string);


            din = new StreamReader(client.InputStream.AsStreamForRead());
            dout = new DataWriter(client.OutputStream);

            //din.UnicodeEncoding = UnicodeEncoding.Utf8;
            //din.ByteOrder = ByteOrder.LittleEndian;

            dout.UnicodeEncoding = UnicodeEncoding.Utf8;
            dout.ByteOrder = ByteOrder.LittleEndian;

            UpdateNetworkStatus(NetworkStatus.CONNECTED);

            BackgroundReader = Windows.System.Threading.ThreadPool.RunAsync(async (workItem) =>
            {
                while (workItem.Status != AsyncStatus.Canceled)
                {
                    Debug.WriteLine("Awaiting incoming data...");
                    string data = await App.Network.Read();
                    App.Network.HandleMessage(data);
                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                }
            });

            return true;
        }

        public async Task<bool> Disconnect()
        {
            if(App.Game.Status != GameHandler.GameStatus.STOPPED)
                App.Api.LeaveGame();

            UpdateNetworkStatus(NetworkStatus.DISCONNECTED);

            if (BackgroundReader != null)
                BackgroundReader.Cancel();

            din.Dispose();
            dout.Dispose();

            client.Dispose();

            return true;
        }

        public void HandleMessage(string data)
        {
            JObject o = JObject.Parse(data);
            App.Api.HandleMessage(o);
            
        }
    }
}
