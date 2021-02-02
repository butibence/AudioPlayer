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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace audioplayer
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        MediaPlayer mediaPlayer = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();
        string fajlnev;
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;


        private void betoltes_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = ".mp3"

            };
            bool? dialogOk = filedialog.ShowDialog();
            if (dialogOk == true)
            {

                fajlnev = filedialog.FileName;
                Box.Text = filedialog.SafeFileName;
                mediaPlayer.Open(new Uri(fajlnev));
            }
        }

        private void lejatszas_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            if (sliProgress.Value == mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
            {
                timer.Stop();
                sliProgress.Value = 0;
            }
        }

        private void szunet_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
        }




        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)volumeSlider.Value;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }
        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }



    }
}
