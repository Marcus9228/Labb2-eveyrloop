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
    /// Interaction logic for EditArtistAndAlbumsWindow.xaml
    /// </summary>
    public partial class EditArtistAndAlbumsWindow : Window
    {
        private EveryloopDBContext _dbContext;

        public EditArtistAndAlbumsWindow()
        {
            InitializeComponent();
            _dbContext = new EveryloopDBContext();
            LoadArtists();
        }
        private void LoadArtists()
        {
            var artists = _dbContext.Artists.ToList();
            ArtistsListBox.ItemsSource = artists;
        }
        private void LoadAlbums()
        {
            if (ArtistsListBox.SelectedItem != null)
            {
                var selectedArtist = ArtistsListBox.SelectedItem as Artist;
                var albumsForSelectedArtist = _dbContext.Albums.Where(album => album.ArtistId == selectedArtist.ArtistId).ToList();
                AlbumsListBox.ItemsSource = albumsForSelectedArtist;
            }
        }

        private void ArtistsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AlbumTracks.ItemsSource = null;
            LoadAlbums();
        }

        private void LoadTracks()
        {
            if (AlbumsListBox.SelectedItem != null)
            {
                var selectedAlbum = AlbumsListBox.SelectedItem as Album;

                var tracksForSelectedAlbum = _dbContext.Tracks.Where(track => track.AlbumId == selectedAlbum.AlbumId).ToList();
                AlbumTracks.ItemsSource = tracksForSelectedAlbum;
            }
        }

        private void AlbumsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTracks();
        }

        private void EditTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumTracks.SelectedItem != null)
            {
                var selectedSong = AlbumTracks.SelectedItem as Track;
                var songToUpdate = _dbContext.Tracks.FirstOrDefault(t => t.TrackId == selectedSong.TrackId);

                var editTrackWindow = new EditTrackWindow(songToUpdate);
                editTrackWindow.SongUpdated += LoadTracks;
                editTrackWindow.ShowDialog();
            }
        }

        //private void RefreshSongList()
        //{
        //    LoadTracks();
        //}

        private void DeleteTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumTracks.SelectedItem != null)
            {
                var selectedTrack = AlbumTracks.SelectedItem as Track;

                selectedTrack.AlbumId = null;

                _dbContext.SaveChanges();

                LoadTracks();
            }
        }

        private void DeleteAlbumBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumsListBox.SelectedItem != null)
            {
                AlbumTracks.ItemsSource = null;

                var selectedAlbum = AlbumsListBox.SelectedItem as Album;

                _dbContext.Albums.Remove(selectedAlbum);

                _dbContext.SaveChanges();

                LoadAlbums();
            }
        }

        private void AddTrackBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedAlbum = AlbumsListBox.SelectedItem as Album;
            var trackToAlbumWindow = new TrackToAlbumWindow(selectedAlbum);
            trackToAlbumWindow.albumUpdated += LoadTracks;
            trackToAlbumWindow.ShowDialog();
        }

        private void AddAlbumBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ArtistsListBox.SelectedItem != null)
            {
                var selectedArtist = ArtistsListBox.SelectedItem as Artist;
                var newAlbum = new Album();
                newAlbum.ArtistId = selectedArtist.ArtistId;
                newAlbum.Title = "New Album";
                newAlbum.AlbumId = _dbContext.Albums.Max(x => x.AlbumId) + 1;

                _dbContext.Albums.Add(newAlbum);
                _dbContext.SaveChanges();
                LoadAlbums();
            }
        }

        private void EditAlbumBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AlbumsListBox.SelectedItem != null)
            {
                var selectedAlbum = AlbumsListBox.SelectedItem as Album;
                if (RenameAlbumTextBox.Text.Length > 0)
                {
                    selectedAlbum.Title = RenameAlbumTextBox.Text;
                    _dbContext.SaveChanges();
                    LoadAlbums();
                }
            }
        }

        private void EditArtistBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ArtistsListBox.SelectedItem != null)
            {
                var selectedArtist = ArtistsListBox.SelectedItem as Artist;
                if (RenameArtistTextBox.Text.Length > 0)
                {
                    selectedArtist.Name = RenameArtistTextBox.Text;
                    _dbContext.SaveChanges();
                    LoadArtists();
                }
            }
        }

        private void AddArtistBtn_Click(object sender, RoutedEventArgs e)
        {
            var newArtist = new Artist();
            newArtist.Name = "New Artist";
            newArtist.ArtistId = _dbContext.Artists.Max(x => x.ArtistId) + 1;

            _dbContext.Artists.Add(newArtist);
            _dbContext.SaveChanges();
            LoadArtists();
            var addedArtist = _dbContext.Artists.FirstOrDefault(a => a.ArtistId == newArtist.ArtistId);
            if (addedArtist != null)
            {
                ArtistsListBox.SelectedItem = addedArtist;
                ArtistsListBox.ScrollIntoView(addedArtist);
            }
        }

        private void DeleteArtistBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ArtistsListBox.SelectedItem != null)
            {
                var selectedArtist = ArtistsListBox.SelectedItem as Artist;
                foreach (Album album in selectedArtist.Albums)
                {
                    _dbContext.Albums.Remove(album);
                }
                _dbContext.Artists.Remove(selectedArtist);
                _dbContext.SaveChanges();
                LoadAlbums();
                LoadArtists();
            }
        }
    }
}
