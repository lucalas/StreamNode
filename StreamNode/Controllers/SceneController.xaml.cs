using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
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

namespace RemoteControl.Controllers
{
    /// <summary>
    /// Logica di interazione per SceneController.xaml
    /// </summary>
    public partial class SceneController : UserControl
    {
        public class OnButtonClickEventArgs : EventArgs
        {
            public string sceneName;
        }

        public delegate void OnButtonClickEventHandler(object sender, OnButtonClickEventArgs e);
        private OBSScene _scene;
        private SceneControllerData AppData;
        public event OnButtonClickEventHandler onButtonClick;

        public SceneController() : this(null)
        {

        }

        public SceneController(OBSScene scene)
        {
            _scene = scene;
            InitializeComponent();
            AppData = new SceneControllerData() { title = scene.Name };
            SceneButton.DataContext = AppData;
        }

        private void SceneButton_Click(object sender, RoutedEventArgs e)
        {
            Button sceneButton = (Button)sender;
            OnButtonClickEventArgs args = new OnButtonClickEventArgs();
            args.sceneName = sceneButton.Content.ToString();
            onButtonClick(sender, args);
        }
    }

    public class SceneControllerData
    {
        public string title { get; set; }
    }
}
