using Labb2_eveyrloop.Data;
using Labb2_eveyrloop.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for TrackToAlbumWindow.xaml
    /// </summary>
    public partial class TrackToAlbumWindow : Window
    {
        private EveryloopDBContext _dbContext;
        private Album _selectedAlbum;
        public event Action albumUpdated;

        public TrackToAlbumWindow(Album selectedAlbum)
        {
            InitializeComponent();
            _dbContext = new EveryloopDBContext();
            _selectedAlbum = selectedAlbum;
            LoadTracks();
        }

        private void LoadTracks()
        {
            var allTracks = _dbContext.Tracks.ToList();

            AllTracksListView.ItemsSource = allTracks;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AllTracksListView.SelectedItem != null)
            {
                var selectedTrack = AllTracksListView.SelectedItem as Track;
                selectedTrack.AlbumId = _selectedAlbum.AlbumId;

                _dbContext.SaveChanges();
                albumUpdated?.Invoke();
                LoadTracks();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = SearchTextBox.Text.ToLower();
            var filteredList = _dbContext.Tracks.Where(track => track.Name.ToLower().Contains(filter)).ToList();

            AllTracksListView.ItemsSource = filteredList;
        }
    }
}
