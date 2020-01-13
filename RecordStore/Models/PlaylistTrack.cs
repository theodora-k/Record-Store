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

        [Key]
        public int PlaylistId { get; set; }
        public int TrackId { get; set; }
    }
}