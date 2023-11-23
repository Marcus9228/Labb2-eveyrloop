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
    /// Interaction logic for EditPlaylistWindow.xaml
    /// </summary>
    public partial class EditPlaylistWindow : Window
    {
        private EveryloopDBContext _dbContext;
        public EditPlaylistWindow()
        {
            InitializeComponent();
            _dbContext = new EveryloopDBContext();
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            var musicGenreIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 24, 25 };

            var playlists = _dbContext.Playlists.Where(playlist => !_dbContext.PlaylistTracks.Any(pt => pt.PlaylistId == playlist.PlaylistId) || _dbContext.PlaylistTracks.Any(pt => pt.PlaylistId == playlist.PlaylistId && _dbContext.Tracks.Any(track => track.TrackId == pt.TrackId && musicGenreIds.Contains(track.GenreId.Value)))).ToList();

            PlaylistListBox.ItemsSource = playlists;
        }

        private void NewPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            if (newPlaylistName.Text.Length > 0)
            {
                var playlistName = newPlaylistName.Text;
                var newPlaylist = new Playlist();
                newPlaylist.Name = playlistName;

                var maxPlaylistId = _dbContext.Playlists.Max(p => (int?)p.PlaylistId) ?? 0;
                newPlaylist.PlaylistId = maxPlaylistId + 1;

                _dbContext.Add(newPlaylist);
                _dbContext.SaveChanges();

                newPlaylistName.Clear();
                LoadPlaylists();
            }
        }

        private void EditPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlaylist = PlaylistListBox.SelectedItem as Playlist;

            if (selectedPlaylist != null)
            {
                var selectedPlaylistWindow = new SelectedPlaylistWindow(selectedPlaylist);
                selectedPlaylistWindow.ShowDialog();
            }
        }

        private void DeletePlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlaylist = PlaylistListBox.SelectedItem as Playlist;

            if (selectedPlaylist != null)
            {

                var playlistTracks = _dbContext.PlaylistTracks.Where(pt => pt.PlaylistId == selectedPlaylist.PlaylistId).ToList();

                foreach (var playlistTrack in playlistTracks)
                {
                    _dbContext.PlaylistTracks.Remove(playlistTrack);
                }

                _dbContext.Playlists.Remove(selectedPlaylist);

                _dbContext.SaveChanges();

                LoadPlaylists();
            }
        }

        private void RenamePlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPlaylist = PlaylistListBox.SelectedItem as Playlist;
            if (selectedPlaylist != null)
            {
                if (RenamePlaylistTextBox.Text.Length > 0)
                {
                    selectedPlaylist.Name = RenamePlaylistTextBox.Text;
                    _dbContext.SaveChanges();
                    LoadPlaylists();
                }
            }
        }
    }
}
