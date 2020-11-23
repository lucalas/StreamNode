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
        private OBSScene _scene;
        private SceneControllerData AppData;

        public SceneController() : this(null)
        {

        }

        public SceneController(OBSScene scene)
        {
            _scene = scene;
            InitializeComponent();
            AppData = new SceneControllerData() { title = scene.Name };
            Title.DataContext = AppData;
        }
    }

    public class SceneControllerData
    {
        public string title { get; set; }
    }
}
