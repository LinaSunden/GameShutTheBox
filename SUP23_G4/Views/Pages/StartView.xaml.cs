using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SUP23_G4.Views.Pages
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl
    {
        #region Konstruktor
        public StartView()
        {
            InitializeComponent();
            LoadDice();
        }
        #endregion



        #region Instansvariabler
        private Model3D _die3D;
        private ModelVisual3D _die1ModelVisual;
        private ModelVisual3D _die2ModelVisual;
        private ModelImporter _importer = new();
        private DispatcherTimer _rotationTimer;
        private double _rotationAngleDie1 = 0.5;
        private double _rotationAngleDie2 = 0.5;
        #endregion



        #region Metoder
        /// <summary>
        /// Öppnar 3D tärning .obj fil och lägger in varsin tärning i en Viewport vardera
        /// </summary>
        private void LoadDice()
        {
            _die3D = _importer.Load("Resources/Dice3DO.obj");

            _die1ModelVisual = new()
            {
                Content = _die3D,

            };
            helixViewportDie1.Children.Add(_die1ModelVisual);


            _die2ModelVisual = new()
            {
                Content = _die3D,

            };
            helixViewportDie2.Children.Add(_die2ModelVisual);

            
            RotationTimerDice();
        }   
        /// <summary>
        /// Startar timer och ställer in dess intervall
        /// </summary>
        private void RotationTimerDice()
        {
            _rotationTimer = new DispatcherTimer();
            _rotationTimer.Interval = TimeSpan.FromMilliseconds(10);
            _rotationTimer.Tick += RotateDie1;
            _rotationTimer.Tick += RotateDie2;
            _rotationTimer.Start();
        }
        /// <summary>
        /// Sätter rotation på y-axel till 1 för Die1
        /// </summary>
        private void RotateDie1(object sender, EventArgs e)
        {
            RotateTransform3D rotation = new (new AxisAngleRotation3D(new Vector3D(0, 1, 0), _rotationAngleDie1));

            _die1ModelVisual.Transform = rotation;

            _rotationAngleDie1 += 0.5;
        }
        /// <summary>
        /// Sätter rotation på z-axel (djupet) till 1 för Die2
        /// </summary>
        private void RotateDie2(object sender, EventArgs e)
        {
            RotateTransform3D rotation = new(new AxisAngleRotation3D(new Vector3D(0, 0, 1), _rotationAngleDie2));

            _die2ModelVisual.Transform = rotation;

            _rotationAngleDie2 += 0.5;
        }

        #endregion
    }
}
