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
using System.Net.Http;
using System.Text;
using Windows.Storage;

namespace YJMPD_UWP.Model
{
    public class NetworkHandler
    {
        public delegate void OnStatusUpdateHandler(object sender, NetworkStatusUpdatedEventArgs e);
        public event OnStatusUpdateHandler OnStatusUpdate;

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
            if (Status != NetworkStatus.CONNECTED)
                return "error";

            string data = await din.ReadLineAsync();

            Debug.WriteLine("Receiving -> " + data);

            return data;
        }

        public async Task<bool> Write(string data)
        {
            if (Status != NetworkStatus.CONNECTED)
                return false;

            Debug.WriteLine("Sending -> " + data);

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

            dout.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            dout.ByteOrder = ByteOrder.LittleEndian;

            UpdateNetworkStatus(NetworkStatus.CONNECTED);

            BackgroundReader = Windows.System.Threading.ThreadPool.RunAsync(async (workItem) =>
            {
                bool running = true;

                while (running)
                {

                    string data = "";

                    try
                    {
                        data = await Read();
                    }
                    catch (Exception)
                    {
                        data = null;
                    }

                    if (data == null)
                    {
                        Disconnect();
                        running = false;
                    }
                    else {
                        HandleMessage(data);
                    }
                }
            });

            return true;
        }

        public async Task<bool> Disconnect()
        {
            Debug.WriteLine("Disconnecting...");

            if (App.Game.Status != GameHandler.GameStatus.STOPPED)
                await App.Game.StopGame();

            UpdateNetworkStatus(NetworkStatus.DISCONNECTED);

            if (BackgroundReader != null)
            {
                BackgroundReader.Cancel();
                BackgroundReader = null;
            }

            din.Dispose();
            dout.Dispose();

            client.Dispose();

            Debug.WriteLine("Disconnected...");
            return true;
        }

        public void HandleMessage(string data)
        {
            JObject o = JObject.Parse(data);
            App.Api.HandleMessage(o);
            
        }

    }
}
