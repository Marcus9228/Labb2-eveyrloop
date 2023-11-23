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
    /// Interaction logic for SelectedPlaylistWindow.xaml
    /// </summary>
    public partial class SelectedPlaylistWindow : Window
    {
        private EveryloopDBContext _dbContext;
        private Playlist _selectedPlaylist;
        public SelectedPlaylistWindow(Playlist selectedPlaylist)
        {
            InitializeComponent();
            _dbContext = new EveryloopDBContext();
            _selectedPlaylist = selectedPlaylist;
            LoadMusic();
            PlaylistNameTextBlock.Text = _selectedPlaylist.Name;
        }


        private void LoadMusic()
        {
            var playlistTrackIds = _dbContext.PlaylistTracks.Where(pt => pt.PlaylistId == _selectedPlaylist.PlaylistId).Select(pt => pt.TrackId).ToList();

            var musicTracks = _dbContext.Tracks.Where(mt => playlistTrackIds.Contains(mt.TrackId)).ToList();

            PlaylistTracksListBox.ItemsSource = musicTracks;


            var allTracks = _dbContext.Tracks.ToList();

            TracksListBox.ItemsSource = allTracks;
        }

        private void RefreshSongList()
        {
            LoadMusic();
        }

        private void EditSongBtn_Click(object sender, RoutedEventArgs e)
        {
            if(PlaylistTracksListBox.SelectedItem != null)
            {
                var selectedSong = PlaylistTracksListBox.SelectedItem as Track;
                var songToUpdate = _dbContext.Tracks.FirstOrDefault(t => t.TrackId == selectedSong.TrackId);

                var editTrackWindow = new EditTrackWindow(songToUpdate);
                editTrackWindow.SongUpdated += LoadMusic;
                editTrackWindow.ShowDialog();
            }
        }

        private void DeleteSongBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PlaylistTracksListBox.SelectedItem != null)
            {
                var selectedSong = PlaylistTracksListBox.SelectedItem as Track;

                var playlistTrackRecord = _dbContext.PlaylistTracks.FirstOrDefault(pt => pt.PlaylistId == _selectedPlaylist.PlaylistId && pt.TrackId == selectedSong.TrackId);

                if (playlistTrackRecord != null)
                {
                    _dbContext.PlaylistTracks.Remove(playlistTrackRecord);
                    _dbContext.SaveChanges();

                    LoadMusic();
                }
            }
        }

        private void AddSongBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = TracksListBox.SelectedItem as Track;

            var newPlaylistTrackRecord = new PlaylistTrack();
            newPlaylistTrackRecord.PlaylistId = _selectedPlaylist.PlaylistId;
            newPlaylistTrackRecord.TrackId = selectedSong.TrackId;

            _dbContext.PlaylistTracks.Add(newPlaylistTrackRecord);
            _dbContext.SaveChanges();

            LoadMusic();

        }

        private void PlaylistSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = PlaylistSearchTextBox.Text.ToLower();
            var playlistTrackIds = _dbContext.PlaylistTracks.Where(pt => pt.PlaylistId == _selectedPlaylist.PlaylistId).Select(pt => pt.TrackId).ToList();

            var filteredList = _dbContext.Tracks.Where(fl => playlistTrackIds.Contains(fl.TrackId) && fl.Name.ToLower().Contains(filter)).ToList();

            PlaylistTracksListBox.ItemsSource = filteredList;
        }



        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = SearchTextBox.Text.ToLower();
            var filteredList = _dbContext.Tracks.Where(track => track.Name.ToLower().Contains(filter)).ToList();

            TracksListBox.ItemsSource = filteredList;
        }

    }
}

