using Labb2_eveyrloop.Data;
using Labb2_eveyrloop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Labb2_eveyrloop
{
    /// <summary>
    /// Interaction logic for EditTrackWindow.xaml
    /// </summary>
    public partial class EditTrackWindow : Window
    {
        private EveryloopDBContext _dbContext;
        private Models.Track _songToUpdate;

        public event Action SongUpdated;

        public EditTrackWindow(Models.Track songToUpdate)
        {
            InitializeComponent();
            _songToUpdate = songToUpdate;
            _dbContext = new EveryloopDBContext();
            LoadTrack();
        }

        private void LoadTrack()
        {

            TrackNameTextBlock.Text = _songToUpdate.Name;
            TrackDurationTextBlock.Text = _songToUpdate.FormattedTime;
        }

        private void TrackNameBtn_Click(object sender, RoutedEventArgs e)
        {
            var newName = TrackNameTextBox.Text;
            _songToUpdate.Name = newName;
            _dbContext.Tracks.Update(_songToUpdate);
            _dbContext.SaveChanges();
            LoadTrack();

            SongUpdated?.Invoke();
        }

        private void DurationBtn_Click(object sender, RoutedEventArgs e)
        {
            var newDuration = TrackDurationTextBox.Text;
            try
            {
                _songToUpdate.Milliseconds = (Convert.ToInt32(newDuration) * 1000);
                _dbContext.Tracks.Update(_songToUpdate);
                _dbContext.SaveChanges();
                LoadTrack();

                SongUpdated?.Invoke();
            }
            catch
            {
                MessageBox.Show("Invalid input, please insert time in seconds");
            }
        }
    }
}
