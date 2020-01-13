using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RecordStore.Models;
//TODO Fix Routes for controllers
namespace RecordStore.Controllers
{
    public class ReportsController : Controller
    {

        public ActionResult Report()
        {

            return View();
        }

        // GET: Reports
        public ActionResult MissingParamWarning()
        {
            ViewBag.Message = "101";
            return View("Report");
        }

        public ActionResult NoResultsWarning()
        {
            ViewBag.Message = "404";
            return View("Report");
        }

        [HttpPost]
        public ActionResult GetArtists(string xnumber, string datepicker1, string datepicker2)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fromdate = DateTime.ParseExact(datepicker1, "d", provider);
            DateTime todate = DateTime.ParseExact(datepicker2, "d", provider);
            int X = Int32.Parse(xnumber);

            if (X == 0 && (fromdate.Equals(null) || todate.Equals(null)))
            {
                //missing parameters warning
                ViewBag.Message = "101";
                return View("Report");
            }
            else
            {
                RecordStoreContext rs = new RecordStoreContext();

                //we gather the invoice ids that occur during the desired period
                var invoice_ids = from invoice in rs.Invoices
                                  where invoice.InvoiceDate >= fromdate && invoice.InvoiceDate <= todate
                                  select invoice;
                var em = invoice_ids.GetEnumerator();

                while (em.MoveNext())
                {

                }


                //count invoice lines (tracks sold) - customer can buy once from various artists
                List<InvoiceLine> totalSoldPieces = new List<InvoiceLine>();//= rs.InvoiceLines.SqlQuery("SELECT * FROM InvoiceLine");


                List<Album> allAlbums = rs.Albums.ToList();
                //get track ids & quantity
                Dictionary<int, int> AlbumPiecesSold = new Dictionary<int, int>();
                //Dictionary containing the albuum id and sum of tracks sold corresponding to specific album
                foreach (InvoiceLine piece in totalSoldPieces)
                {
                    //Track trackData = rs.Tracks.SqlQuery("SELECT * FROM Tracks WHERE TrackId = @p0", piece.TrackId).First();
                    Track trackData = rs.Tracks.SingleOrDefault(track => track.TrackId == piece.TrackId);
                    //we get only the first element because track id is unique
                    //trackData.AlbumId is the id of the current album
                    if (!AlbumPiecesSold.ContainsKey(trackData.AlbumId))
                    {//add album to dictionary
                        AlbumPiecesSold.Add(trackData.AlbumId, piece.Quantity);
                    }
                    else
                    {//add quantity to sum of track pieces sold for album
                        AlbumPiecesSold[trackData.AlbumId] = AlbumPiecesSold[trackData.AlbumId] + piece.Quantity;
                    }
                }
                //sort final countdown of sales
                var SortedList = AlbumPiecesSold.ToList();

                SortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                //get artist name for sorted albums
                //until X is reached
                List<KeyValuePair<int, int>>.Enumerator em2 = SortedList.GetEnumerator();
                int i = 1; // count loop so that we only return the artists from X first record sales

                while (em.MoveNext() && i <= X)
                {
                    int albumid = em2.Current.Key;
                    Album albumData = rs.Albums.SingleOrDefault(album => album.AlbumId == albumid);
                    //Artist aristData = rs.Artists.SqlQuery("SELECT * FROM Artist WHERE ArtistId = @p0", albumData.ArtistId).First();
                    Artist artistData = rs.Artists.SingleOrDefault(artist => artist.ArtistId == albumData.ArtistId);
                    ViewData.Add(artistData.Name, i);
                    i++;
                }

                ViewBag.Title = "Artists with over than " + X + " record sales from " + datepicker1 + " to " + datepicker2;
                return View("GetResults");
            }
        }

        public ActionResult GetSongs()
        {
            RecordStoreContext rs = new RecordStoreContext();

            /*var movies = from artist in rs.Movies
            where m.ReleaseDate > new DateTime(1984, 6, 1)
            select m;*/

            return View();

        }

        public ActionResult GetGenres()
        {
            RecordStoreContext rs = new RecordStoreContext();

            /*var movies = from artist in rs.Movies
            where m.ReleaseDate > new DateTime(1984, 6, 1)
            select m;*/

            return View();

        }
    }
}