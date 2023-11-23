using Labb2_eveyrloop.Data;
using Labb2_eveyrloop.Models;
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
using System.Windows.Shapes;

namespace Labb2_eveyrloop
{
    /// <summary>
    /// Interaction logic for CreateNewTrackWindow.xaml
    /// </summary>
    public partial class CreateNewTrackWindow : Window
    {
        private EveryloopDBContext _dbContext;

        public CreateNewTrackWindow()
        {
            InitializeComponent();
            _dbContext = new EveryloopDBContext();
            LoadGenres();
            LoadMediaTypes();
            LoadTracks();
        }
        private void LoadGenres() 
        {
            var genres = _dbContext.Genres.ToList();
            GenresListBox.ItemsSource = genres;
        }

        private void LoadMediaTypes()
        {
            var mediaTypes = _dbContext.MediaTypes.ToList();
            MediaTypesListBox.ItemsSource = mediaTypes;
        }

        private void LoadTracks()
        {
            var allTracks = _dbContext.Tracks.ToList();
            TracksListView.ItemsSource = allTracks;
        }

        private void CreateTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            var newTrack = new Models.Track();
            if (TrackNameTextBox.Text.Length > 0)
            {
                newTrack.Name = TrackNameTextBox.Text;
            }
            newTrack.TrackId = _dbContext.Tracks.Max(x => x.TrackId) + 1;
            var duration = TrackDurationTextBox.Text;
            var newDuration = TrackDurationTextBox.Text;
            try
            {
                newTrack.Milliseconds = (Convert.ToInt32(newDuration) * 1000);
            }
            catch
            {
                MessageBox.Show("Invalid duration input, please insert time in seconds");
            }
            if (ArtistNameTextBox.Text.Length > 0)
            {
                newTrack.Composer = ArtistNameTextBox.Text;
            }
            if (GenresListBox.SelectedItem != null)
            {
                var selectedGenre = GenresListBox.SelectedItem as Models.Genre;
                newTrack.GenreId = selectedGenre.GenreId;
            }
            if (MediaTypesListBox.SelectedItem != null)
            {
                var selectedMediaType = MediaTypesListBox.SelectedItem as Models.MediaType;
                newTrack.MediaTypeId = selectedMediaType.MediaTypeId;
            }

            if (newTrack.Name != null && newTrack.Milliseconds != 0 && newTrack.Composer != null && newTrack.GenreId != null && newTrack.MediaTypeId != 0)
            {
                _dbContext.Tracks.Add(newTrack);
                _dbContext.SaveChanges();
                LoadTracks();
                MessageBox.Show(newTrack.Name + " was created");
                TrackNameTextBox.Clear();
                ArtistNameTextBox.Clear();
                TrackDurationTextBox.Clear();
                GenresListBox.SelectedItem = null;
                MediaTypesListBox.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Please enter all the information");
            }

        }

        private void SaveGenreBtn_Click(object sender, RoutedEventArgs e)
        {
            var newGenre = new Genre();
            if (NewGenreTextBox.Text.Length > 0)
            {
                newGenre.Name = NewGenreTextBox.Text;
                newGenre.GenreId = _dbContext.Genres.Max(x => x.GenreId) + 1;
                _dbContext.Genres.Add(newGenre);
                _dbContext.SaveChanges();
                LoadGenres();
                MessageBox.Show(newGenre.Name + " was added to the list");
                NewGenreTextBox.Clear();
            }
        }

        private void DeleteGenreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GenresListBox.SelectedItem != null)
            {
                var selectedGenre = GenresListBox.SelectedItem as Genre;
                _dbContext.Genres.Remove(selectedGenre);
                _dbContext.SaveChanges();
                LoadGenres();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = SearchTextBox.Text.ToLower();
            var filteredList = _dbContext.Tracks.Where(track => track.Name.ToLower().Contains(filter)).ToList();

            TracksListView.ItemsSource = filteredList;
        }

        private void DeleteTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TracksListView.SelectedItem != null)
            {
                var selectedTrack = TracksListView.SelectedItem as Track;

                _dbContext.Tracks.Remove(selectedTrack);
                _dbContext.SaveChanges();
                LoadTracks();
                SearchTextBox.Clear();
                MessageBox.Show($"{selectedTrack.Name} was removed");
            }
        }

        private void EditTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = TracksListView.SelectedItem as Track;
            var editTrackWindow = new EditTrackWindow(selectedSong);
            editTrackWindow.SongUpdated += LoadTracks;

            editTrackWindow.ShowDialog();
        }
    }
}
