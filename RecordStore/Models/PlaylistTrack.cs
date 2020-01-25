using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecordStore.Models
{
    public class PlaylistTrack
    {

        [Key, Column(Order = 1)]
        public int PlaylistId { get; set; }
        [Key, Column(Order = 2)]
        public int TrackId { get; set; }
    }
}