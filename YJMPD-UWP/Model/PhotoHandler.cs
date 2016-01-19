using System;
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

        public SoftwareBitmapSource Photo { get; private set; }

        public PhotoHandler()
        {

        }

        public void Reset()
        {
            Photo = null;
        }

        private void UpdatePhotoTaken(SoftwareBitmapSource photo)
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

            UpdatePhotoTaken(bitmapSource);
        }
    }
}
