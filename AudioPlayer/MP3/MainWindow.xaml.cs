using Microsoft.Win32;
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
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.IO;



namespace Audioplayer
{

    public partial class MainWindow : Window
    {

        readonly MediaPlayer mediaPlayer = new MediaPlayer();
        readonly DispatcherTimer timer = new DispatcherTimer();
        int positionSliderIsMoving = 0;
        int isPlaying = 0;
        int playing = -1;
        int repeatType = 0;

        void timer_Tick(object sender, EventArgs e)
        {
            PositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            PositionSlider.IsEnabled = true;
            PositionSlider.Value = mediaPlayer.Position.TotalSeconds;
            if (PositionSlider.Value == mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
            {
                timer.Stop();
                PositionSlider.Value = 0;
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

        private void VolumeSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mediaPlayer.Volume = volumeSlider.Value;
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)volumeSlider.Value;
        }
        private void AddSongsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (!SongsListBox.Items.Contains(file))
                    {
                        SongsListBox.Items.Add(file);
                    }
                }
            }
        }

        private void SongsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            playing = Convert.ToInt32(SongsListBox.SelectedIndex);
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Start();
            PositionSlider.IsEnabled = true;

        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count != 0)
            {
                if (isPlaying == 0)
                {
                    if (SongsListBox.SelectedIndex != -1)
                    {
                        try
                        {
                            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
                            mediaPlayer.Open(new Uri(file));
                            mediaPlayer.Play();
                            isPlaying = 2;
                            playing = Convert.ToInt32(SongsListBox.SelectedIndex);
                            timer.Interval = TimeSpan.FromMilliseconds(200);
                            timer.Tick += timer_Tick;
                            timer.Start();
                            PositionSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                            PositionSlider.IsEnabled = true;

                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            SongsListBox.SelectedIndex = 0;
                            string file = SongsListBox.Items[0].ToString();
                            mediaPlayer.Open(new Uri(file));
                            mediaPlayer.Play();
                            isPlaying = 2;
                            playing = 0;
                            timer.Interval = TimeSpan.FromSeconds(1);
                            timer.Tick += timer_Tick;
                            timer.Start();
                            PositionSlider.IsEnabled = true;

                        }
                        catch
                        {

                        }
                    }
                }
                else if (isPlaying == 1)
                {
                    mediaPlayer.Play();
                    isPlaying = 2;
                }
                else
                {
                    mediaPlayer.Pause();
                    isPlaying = 1;
                }
            }
        }




        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

            playing++;
            SongsListBox.SelectedIndex = playing;
            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            PositionSlider.IsEnabled = true;

        }

        private void elozo(object sender, RoutedEventArgs e)
        {

            playing = playing - 1;
            SongsListBox.SelectedIndex = playing;
            string file = SongsListBox.Items[SongsListBox.SelectedIndex].ToString();
            mediaPlayer.Open(new Uri(file));
            mediaPlayer.Play();
            isPlaying = 2;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            PositionSlider.IsEnabled = true;

        }

        private void RemoveSongButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsListBox.Items.Count == 0)
            {
            }
            else
            {
                if (SongsListBox.SelectedIndex == -1)
                {

                }
                else
                {
                    if (SongsListBox.SelectedIndex == playing)
                    {
                        SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                        SongsListBox.SelectedIndex = -1;
                        mediaPlayer.Close();
                        timer.Stop();
                        isPlaying = 0;
                        playing = -1;
                        PositionSlider.Value = 0;
                        PositionSlider.Maximum = 1;
                        PositionLabel1.Content = "00:00/00:00";
                        PositionSlider.IsEnabled = false;
                    }
                    else
                    {
                        if (SongsListBox.SelectedIndex > playing)
                        {
                            SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                            SongsListBox.SelectedIndex = playing;
                        }
                        else
                        {
                            playing--;
                            SongsListBox.Items.Remove(SongsListBox.SelectedItem);
                            SongsListBox.SelectedIndex = playing;
                        }
                    }
                }
            }
        }

        
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (repeatType == 0)
            {
                repeatType = 1;
            }
            else if (repeatType == 1)
            {
                repeatType = 2;
                REPLAY.Content = "Ismétlés";
            }
            else
            {
                repeatType = 0;
                REPLAY.Content = "Nincs ismétlés";

            }
        }

        private void PositionSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            positionSliderIsMoving = 1;
        }

        private void PositionSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            positionSliderIsMoving = 0;
            mediaPlayer.Position = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        private void PositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PositionLabel1.Content = String.Format("{0}/{1}", TimeSpan.FromSeconds(PositionSlider.Value).ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void SongsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}




