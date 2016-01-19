using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class PhotoView : Page
    {
        PhotoVM photovm;

        public PhotoView()
        {
            photovm = new PhotoVM();
            this.DataContext = photovm;
            this.InitializeComponent();
        }
    }
}
