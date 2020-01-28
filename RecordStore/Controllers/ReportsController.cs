﻿using System;
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

                //if no invoices found for the selected criteria then we show message to the rendered view
                if (invoice_ids == null)
                {
                    //no results found warning
                    ViewBag.Message = "104";
                    return View("Report");
                }
            
             
                //get artists ids & quantity
                Dictionary<int, int> ArtistsRecordsSold = new Dictionary<int, int>();
                //Dictionary containing the albuum id and sum of tracks sold corresponding to specific album
                foreach (Invoice invoice in invoice_ids)
                {
                    IQueryable<InvoiceLine> inv_lines = from line in rs.InvoiceLines
                                                        where line.InvoiceId == invoice.InvoiceId
                                                        select line;
                    //we loop for every line of each invoice
                    //to measure records sold per artist
                    foreach (InvoiceLine line in inv_lines)
                    {
                        Track trackData = rs.Tracks.SingleOrDefault(track => track.TrackId == line.TrackId);
                        Album albData = rs.Albums.SingleOrDefault(alb => alb.AlbumId == trackData.AlbumId);
                        Artist artData = rs.Artists.SingleOrDefault(art => art.ArtistId == albData.ArtistId);
                        if (!ArtistsRecordsSold.ContainsKey(trackData.AlbumId))
                        {//add album to dictionary
                            ArtistsRecordsSold.Add(artData.ArtistId, line.Quantity);
                        }
                        else
                        {//add quantity to sum of track pieces sold for album
                            ArtistsRecordsSold[artData.ArtistId] = ArtistsRecordsSold[artData.ArtistId] + line.Quantity;
                        }
                    }

                }
                //sort final countdown of sales
                var SortedList = ArtistsRecordsSold.ToList();

                SortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                //get artist name for sorted albums
                //until X is reached
                List<KeyValuePair<int, int>>.Enumerator em2 = SortedList.GetEnumerator();
                int i = 1; // count loop so that we only return the artists from X first record sales

                if (!x_entered)
                {
                    //in case x value is not given, we will iterate through all elements of the sorted list
                    X = SortedList.Count();
                }
                IList<String> artists = new List<String>();
                IList<String> records = new List<String>();


                while (em2.MoveNext() && i <= X)
                {
                    Artist artistData = rs.Artists.SingleOrDefault(artist => artist.ArtistId == em2.Current.Key);
                    artists.Add(artistData.Name);
                    records.Add((em2.Current.Value).ToString());
                    i++;
                }

                ViewData["columns"] = new List<String> { "Artist Name" ,"Records Sold" };
                ViewData["0"] = artists;
                ViewData["1"] = records;


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

            //if no invoices found for the selected criteria then we show message to the rendered view
            if (invoice_ids == null)
            {
                //no results found warning
                ViewBag.Message = "204";
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

            IList<String> titles = new List<String>();
            IList<String> albums = new List<String>();
            IList<String> artists = new List<String>();
            IList<String> quant = new List<String>();

            while (em.MoveNext() & i < 10)
            {
                int trackId = em.Current.Key;
                Track trackData = rs.Tracks.SingleOrDefault(track => track.TrackId == trackId);
                Album albumData = rs.Albums.SingleOrDefault(alb => alb.AlbumId == trackData.AlbumId);
                Artist artistData = rs.Artists.SingleOrDefault(art => art.ArtistId == albumData.ArtistId);
                //TODO Fix string of track info
                titles.Add(trackData.Name);
                albums.Add(albumData.Title);
                artists.Add(artistData.Name);
                quant.Add(em.Current.Value.ToString());//adding only the first 10 quantities
                i++;
            }

            ViewData["columns"] = new List<String>{ "Track Title", "Album", "Artist Name", " Quantity Sold "};

            ViewData["0"] = titles;
            ViewData["1"] = albums;
            ViewData["2"] = artists;
            ViewData["3"] = quant;

            return View("GetResults");

        }

        public ActionResult GetGenres()
        {
            RecordStoreContext rs = new RecordStoreContext();

            //we don't have to execute a linq query as we wish to find out 
            //the total ranking of music genres of all times
            List<InvoiceLine> inv_lines = rs.InvoiceLines.ToList();

            //if no invoices found for the selected criteria then we show message to the rendered view
            if (inv_lines == null)
            {
                //no results found warning
                ViewBag.Message = "304";
                return View("Report");
            }

            //get track id & quantity
            Dictionary<String, int> FamousGenre = new Dictionary<String, int>();
            //Dictionary containing the albuum id and sum of tracks sold corresponding to specific album
            foreach (InvoiceLine piece in inv_lines)
            {
                Track trackData = rs.Tracks.SingleOrDefault(track => track.TrackId == piece.TrackId);
                Genre genreData = rs.Genres.SingleOrDefault(genre => genre.GenreId == trackData.GenreId);
                //we get only the first element because track id is unique
                //trackData.AlbumId is the id of the current album
                if (!FamousGenre.ContainsKey(genreData.Name))
                {//add track to dictionary
                    FamousGenre.Add(genreData.Name, piece.Quantity);
                }
                else
                {//add quantity to sum of track pieces sold 
                    FamousGenre[genreData.Name] = FamousGenre[genreData.Name] + piece.Quantity;
                }

            }
            //sort final countdown of sales
            var SortedList = FamousGenre.ToList();

            SortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            ViewData["columns"] = new List<String> { "Genre", "Pieces Sold"};

            //converting back to dictionary so that we can store key and value columns separately
            var dictionary = SortedList.ToDictionary(x => x.Key, x => x.Value);

            ViewData["0"] = dictionary.Keys.ToList();
            ViewData["1"] = dictionary.Values.ToList();

            return View("GetResults");

        }
    }
}