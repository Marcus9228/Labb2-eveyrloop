using System;
using System.Collections.Generic;

namespace Labb2_eveyrloop.Models;

public partial class PlaylistTrack
{
    public int PlaylistTrackId { get; set; }

    public int PlaylistId { get; set; }

    public int TrackId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
