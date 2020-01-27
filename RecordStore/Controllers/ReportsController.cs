using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RecordStore.Models;
namespace RecordStore.Controllers
{
    public class ReportsController : Controller
    {

        public ActionResult Report()
        {

            return View();
        }


        [HttpPost]
        public ActionResult GetArtists(string xnumber, string datepicker1, string datepicker2)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fromdate = DateTime.ParseExact(datepicker1, "d", provider);
            DateTime todate = DateTime.ParseExact(datepicker2, "d", provider);
            int X = Int32.Parse(xnumber);

            //1st case: no valid parameters entered 
            //for X, we check for null because the script automatically 
            //erases any text entered that is not a number
            if ((X.Equals(null) || X <= 0)  && (fromdate.Equals(null) || todate.Equals(null)))
            {
                //missing parameters warning
                ViewBag.Message = "101";
                return View("Report");
                
            } 
            else
            {
                RecordStoreContext rs = new RecordStoreContext();
                IQueryable<Invoice> invoice_ids = null;
                bool x_entered = true;
                //2nd case: Only X entered with from date
                if (X > 0 && !(fromdate.Equals(null)) && todate.Equals(null))
                {
                    invoice_ids = from invoice in rs.Invoices
                                  where invoice.InvoiceDate >= fromdate 
                                  select invoice;
                    ViewBag.Title = "First " + X + " top-selling artists from " + datepicker1 ;
                }
                //3rd case: Only X entered with to date
                else if (X > 0 && fromdate.Equals(null) && !(todate.Equals(null)))
                {
                    invoice_ids = from invoice in rs.Invoices
                                  where invoice.InvoiceDate < todate
                                  select invoice;
                    ViewBag.Title = "First " + X + " top-selling artists up to " + datepicker2;

                }
                //4th case: both dates entered
                else if (!(fromdate.Equals(null)) && !(todate.Equals(null)))
                {
                    //we will need to iterate through the same amount of invoices whether X is entered or not
                    invoice_ids = from invoice in rs.Invoices
                                  where invoice.InvoiceDate >= fromdate  && invoice.InvoiceDate < todate
                                  select invoice;
                    if (X.Equals(null) || X <= 0)
                    {
                        x_entered = false;
                        ViewBag.Title = "All top-selling artists from " + datepicker1 + " to " + datepicker2;

                    }
                    else
                    {
                        ViewBag.Title = "First " + X + " top-selling artists from " + datepicker1 + " to " + datepicker2;

                    }
                }
                    //we gather the invoice ids that occur during the desired period

                var em = invoice_ids.GetEnumerator();

                List<InvoiceLine> totalSoldPieces = new List<InvoiceLine>();
                //count invoice lines (tracks sold) - customer can buy once from various artists

                while (em.MoveNext())
                {
                    // InvoiceLine inv_linee = rs.InvoiceLines.SingleOrDefault(inv  => inv.InvoiceId == em.Current.InvoiceId );
                    //if we find an invoice line that is part of an invoice in the range we are interested at, we add it to a list
                    IQueryable<InvoiceLine> inv_lines = from line in rs.InvoiceLines
                                                        where line.InvoiceId == em.Current.InvoiceId
                                                        select line;
                    totalSoldPieces.AddRange(inv_lines);
                }
             
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

                if (x_entered)
                {
                    while (em2.MoveNext() && i <= X)
                    {
                        int albumid = em2.Current.Key;
                        Album albumData = rs.Albums.SingleOrDefault(album => album.AlbumId == albumid);
                        //Artist aristData = rs.Artists.SqlQuery("SELECT * FROM Artist WHERE ArtistId = @p0", albumData.ArtistId).First();
                        Artist artistData = rs.Artists.SingleOrDefault(artist => artist.ArtistId == albumData.ArtistId);
                        ViewData.Add(artistData.Name, i);
                        i++;
                    }
                } 
                else //we add all data because user didn't select X
                {
                    while (em2.MoveNext())
                    {
                        int albumid = em2.Current.Key;
                        Album albumData = rs.Albums.SingleOrDefault(album => album.AlbumId == albumid);
                        Artist artistData = rs.Artists.SingleOrDefault(artist => artist.ArtistId == albumData.ArtistId);
                        ViewData.Add(artistData.Name, i);
                        i++;
                    }
                }

                if (ViewData == null)
                {
                    //no results found warning
                    ViewBag.Message = "104";
                    return View("Report");
                }

                return View("GetResults");
            }
        }

        [HttpPost]
        public ActionResult GetSongs(string datepicker3, string datepicker4)
        {
            RecordStoreContext rs = new RecordStoreContext();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fromdate = DateTime.ParseExact(datepicker3, "d", provider);
            DateTime todate = DateTime.ParseExact(datepicker4, "d", provider);
            //1st case
            //we get the dates given from the user input
            //we get all invoices for specific dates
            IQueryable<Invoice> invoice_ids = null;
            bool x_entered = true;
            if (!(fromdate.Equals(null)) && !(todate.Equals(null)))
            {
                invoice_ids = from invoice in rs.Invoices
                              where invoice.InvoiceDate >= fromdate && invoice.InvoiceDate < todate
                              select invoice;
                ViewBag.Title = "First 10 top songs from " + datepicker3 + "to " + datepicker4;
            }
            else if (fromdate.Equals(null) && !(todate.Equals(null)))
            {
                invoice_ids = from invoice in rs.Invoices
                              where invoice.InvoiceDate < todate
                              select invoice;
                ViewBag.Title = "First 10 top songs up to" + datepicker4;
            }
            else if (!(fromdate.Equals(null)) && todate.Equals(null))
            {
                invoice_ids = from invoice in rs.Invoices
                              where invoice.InvoiceDate < todate
                              select invoice;
                ViewBag.Title = "First 10 top songs from" + datepicker3;
            }
            else
            {
                //missing parameters warning
                ViewBag.Message = "201";
                return View("Report");
            }

            //we sort the first 10 most bought songs for the range we are checking

            List<InvoiceLine> totalSoldPieces = new List<InvoiceLine>();

            //get track id & quantity
            Dictionary<int, int> TracksSold = new Dictionary<int, int>();
            //Dictionary containing the albuum id and sum of tracks sold corresponding to specific album
            foreach (InvoiceLine piece in totalSoldPieces)
            {
                Track trackData = rs.Tracks.SingleOrDefault(track => track.TrackId == piece.TrackId);
                //we get only the first element because track id is unique
                //trackData.AlbumId is the id of the current album
                if (!TracksSold.ContainsKey(trackData.TrackId))
                {//add track to dictionary
                    TracksSold.Add(trackData.TrackId, piece.Quantity);
                }
                else
                {//add quantity to sum of track pieces sold 
                    TracksSold[trackData.TrackId] = TracksSold[trackData.TrackId] + piece.Quantity;
                }

            }
            //sort final countdown of sales
            var SortedList = TracksSold.ToList();

            SortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            //get artist name for sorted albums
            //until X is reached
            List<KeyValuePair<int, int>>.Enumerator em = SortedList.GetEnumerator();
            int i = 0; // count loop so that we only return the artists from X first record sales

            while (em.MoveNext() & i < 10)
            {
                int trackId = em.Current.Key;
                Track trackData = rs.Tracks.SingleOrDefault(track => track.TrackId == trackId);
                //TODO Fix string of track info
                ViewData.Add(trackData.Name, em.Current.Value);
                i++;
            }

            ViewBag.Message = "Ranking | Track Title | Artist Name | Album |  Quantity Sold ";
            return View("GetResults");

        }

        public ActionResult GetGenres()
        {
            RecordStoreContext rs = new RecordStoreContext();

            //TODO Remove X number choice from frontend
            //TODO Show all genres sorted by counting songs in database 
            /*var movies = from artist in rs.Movies
            where m.ReleaseDate > new DateTime(1984, 6, 1)
            select m;*/

            return View();

        }
    }
}