using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class PhotoHandler
    {
        public delegate void OnPhotoTakenHandler(object sender, PhotoTakenEventArgs e);
        public event OnPhotoTakenHandler OnPhotoTaken;

        public string Photo { get; private set; }

        public PhotoHandler()
        {

        }

        public void Reset()
        {
            Photo = null;
        }

        public void UpdatePhotoTaken(string photo)
        {
            Photo = photo;

            if (OnPhotoTaken == null) return;

            OnPhotoTaken(this, new PhotoTakenEventArgs(photo));
        }

        public async void Take()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;
            captureUI.PhotoSettings.AllowCropping = false;
            captureUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.Large3M;

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                return;
            }

            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                                                                        BitmapPixelFormat.Bgra8,
                                                                        BitmapAlphaMode.Premultiplied);

            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            string photoURL = await UploadImage(stream.AsStream());

            UpdatePhotoTaken(photoURL);
        }

        public async Task<string> UploadImage(Stream file)
        {
            string url = "http://jancokock.me/f/?plain";
            MultipartFormDataContent postdata = new MultipartFormDataContent();
            postdata.Add(new ByteArrayContent(ReadFully(file)), "file", "capture.png");
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var response = await hc.PostAsync(url, postdata);
                    response.EnsureSuccessStatusCode();
                    string imageurl = await response.Content.ReadAsStringAsync();
                    string imageid = new List<string>(imageurl.Split('/')).Last();

                    return imageurl + "/" + imageid;

                }
                catch
                {
                    throw new IOException("Network error");
                }
            }
        }

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
