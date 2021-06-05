using System;
using System.Windows;
using Serilog;
using MaterialDesignThemes.Wpf;
using QRCoder;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using StreamNode.Services.Settings;

namespace StreamNode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerContext serverContext = new ServerContext();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = serverContext;
            SettingsTab.DataContext = App.settingsService.settings;
        }
        private void OpenApp(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = serverContext.UrlQRCode,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        private void QRCodeShow(object sender, RoutedEventArgs e)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(serverContext.UrlQRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeAsBitmap = qrCode.GetGraphic(20);

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            qrCodeAsBitmap.GetHbitmap(),
            IntPtr.Zero,
            System.Windows.Int32Rect.Empty,
            BitmapSizeOptions.FromWidthAndHeight(qrCodeAsBitmap.Width, qrCodeAsBitmap.Height));
            ImageBrush ib = new ImageBrush(bs);
            QRCodePanel.Background = ib;

            QRCodeDialog.IsOpen = true;
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            ISettings settings = App.settingsService.settings;

            if (!App.engine.isConnected)
            {
                App.engine.ConfigOBSWebSocket(settings);
                App.settingsService.SaveSettings();
                Log.Information("Settings saved successfully [{@settings}]", settings);
                Alert(SaveResult, "Saved successfully");
            }
            else
            {
                Alert(SaveResult, "Server is running, do you want to save and restart server?",
                    "RESTART",
                    action =>
                    {
                        Log.Information("Saving settings require server restart");
                        App.engine.Disconnect();
                        Log.Debug("StreamNode engine disconnected");
                        App.engine.ConfigOBSWebSocket(settings);
                        App.engine.Connect();
                        App.settingsService.SaveSettings();
                        Log.Information("Settings saved successfully [{@settings}]", settings);
                        Log.Debug("StreamNode engine connected");
                        Alert(SaveResult, "Configurations saved successfully, server restarted");
                    });
            }
        }
        private void StartServer(object sender, RoutedEventArgs e)
        {
            try
            {
                App.engine.Connect();
                Log.Information("StreamNode engine started");
                App.httpServer.Start();
                Log.Information("Http server started");
            }
            catch (Exception ex)
            {
                Alert(ErrorResult, String.Format("An error occured during server start [{0}]", ex.Message));
                Log.Error("An error occured during server start [{@ex}]", ex);
            }
        }

        private void StopServer(object sender, RoutedEventArgs e)
        {
            try
            {
                App.httpServer.Stop();
                Log.Information("Http server stopped");
                App.engine.Disconnect();
                Log.Information("StreamNode engine stopped");
            }
            catch (Exception ex)
            {
                Alert(ErrorResult, String.Format("An error occured during server shutdown [{0}]", ex.Message));
                Log.Error("An error occured during server shutdown [{@ex}]", ex);
            }
        }

        private void Alert(Snackbar alertBar, String message)
        {
            Alert(alertBar, message, null, null);
        }

        private void Alert(Snackbar alertBar, String message, object? actionContent, Action<object?>? actionHandler)
        {
            alertBar.MessageQueue?.Enqueue(
                message,
                actionContent,
                actionHandler,
                null,
                false,
                true,
                TimeSpan.FromSeconds(3));
        }
    }
}
