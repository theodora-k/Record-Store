using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Globalization;
using System.Linq;
using RecordStore.Models;
using System.Web.UI;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RecordStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly RecordStoreContext rs;

        public HomeController()
        {
            rs = new RecordStoreContext();
        }

        public ActionResult Manage()
        {  
            string sql = @" SELECT TABLE_NAME
                            FROM INFORMATION_SCHEMA.TABLES
                            ;";

            List<TableModel> data = DataAccess.SqlDataAccess.LoadData<Models.TableModel>(sql);

            return View(data);
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Choice()
        {
            ViewBag.Message = "Please choose one of the following ";

            return View();
        }

        //------------------Albums

        public ActionResult DetailsAlbum(string tableName)
        {
            return View(rs.Albums);
        }
        public ActionResult CreateAlbum(//string name
            )
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAlb(string title, string artistId)
        {

            int artid = Int32.Parse(artistId);

            Album newalb = new Album();
            newalb.Title = title;
            newalb.ArtistId = artid;

            rs.Albums.Add(newalb);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsAlbum");
        }



        public ActionResult DeleteAlbum(string albumid)
        {
            int id = Int32.Parse(albumid);
            rs.Tracks.RemoveRange(rs.Tracks.Where(c => c.AlbumId == id));
            rs.Albums.RemoveRange(rs.Albums.Where(c => c.AlbumId == id));
            
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsAlbum");

        }

        public ActionResult EditAlbum(int albumid, string title, int artistId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditAlb(int albumid, string title, string artistId)
        {
            int artid = Int32.Parse(artistId);

            Album newalb = new Album();
            newalb.AlbumId = albumid;
            newalb.Title = title;
            newalb.ArtistId = artid;

            Album albumData = rs.Albums.SingleOrDefault(album => album.AlbumId == albumid);

            rs.Entry(albumData).CurrentValues.SetValues(newalb);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsAlbum");
        }

        //-------- Artist
        public ActionResult DetailsArtist(string tableName)
        {
            return View(rs.Artists);
        }
        public ActionResult CreateArtist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateArt(string name)
        {

            Artist newEntry = new Artist();
            newEntry.Name = name;

            rs.Artists.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsArtist");
        }

        public ActionResult DeleteArtist(string artistid)
        {
            int id = Int32.Parse(artistid);

            rs.Artists.RemoveRange(rs.Artists.Where(c => c.ArtistId == id));
            rs.Albums.RemoveRange(rs.Albums.Where(c => c.ArtistId == id));
            
            rs.SaveChanges();

            ViewBag.Message = "Record was succesfully deleted.";

            Response.Write("<script>alert('Data removed')</script>");

            return View("DetailsArtist");

        }

        public ActionResult EditArtist(int artistId, string name)
        {

            return View();
        }

        [HttpPost]
        public ActionResult EditArt(int artistid, string name)
        {
            Artist newEntry = new Artist();
            newEntry.ArtistId = artistid;
            newEntry.Name = name;

            Artist entryData = rs.Artists.SingleOrDefault(artist => artist.ArtistId == artistid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsArtist");
        }

        //-------- Employee
        public ActionResult DetailsEmployee(string tableName)
        {
            return View(rs.Employees);
        }
        public ActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmpl(string lastname, string firstname, string title, int? reportsto, DateTime birthdate, DateTime hiredate, string address, string city, string state, string country, string postalcode, string phone, string fax, string email)
        {

            Employee newEntry = new Employee();
            newEntry.LastName = lastname;
            newEntry.FirstName = firstname;
            newEntry.Title = title;
            newEntry.ReportsTo = reportsto;
            newEntry.BirthDate = birthdate;
            newEntry.HireDate = hiredate;
            newEntry.Address = address;
            newEntry.City = city;
            newEntry.State = state;
            newEntry.Country = country;
            newEntry.PostalCode = postalcode;
            newEntry.Phone = phone;
            newEntry.Fax = fax;
            newEntry.Email = email;

            rs.Employees.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsEmployee");
        }

        public ActionResult DeleteEmployee(string employeeid)
        {
            int id = Int32.Parse(employeeid);

            rs.Employees.RemoveRange(rs.Employees.Where(c => c.EmployeeId == id));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsEmployee");

        }

        public ActionResult EditEmployee(int employeeid, string lastname, string firstname, string title, int? reportsto, DateTime birthdate, DateTime hiredate, string address, string city, string state, string country, string postalcode, string phone, string fax, string email)
        {

            return View();
        }

        [HttpPost]
        public ActionResult EditEmpl(int employeeid, string lastname, string firstname, string title, int? reportsto, DateTime birthdate, DateTime hiredate, string address, string city, string state, string country, string postalcode, string phone, string fax, string email)
        {
            Employee newEntry = new Employee();
            newEntry.EmployeeId = employeeid;
            newEntry.LastName = lastname;
            newEntry.FirstName = firstname;
            newEntry.Title = title;
            newEntry.ReportsTo = reportsto;
            newEntry.BirthDate = birthdate;
            newEntry.HireDate = hiredate;
            newEntry.Address = address;
            newEntry.City = city;
            newEntry.State = state;
            newEntry.Country = country;
            newEntry.PostalCode = postalcode;
            newEntry.Phone = phone;
            newEntry.Fax = fax;
            newEntry.Email = email;

            Employee entryData = rs.Employees.SingleOrDefault(employee => employee.EmployeeId == employeeid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsEmployee");
        }


        //------------ Customer
        public ActionResult DetailsCustomer(string tableName)
        {
            return View(rs.Customers);
        }
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCust(string firstName, string lastName, string company, string address, string city,
            string state, string country, string postalCode, string phone, string fax, string email, int supportRepId)
        {

            Customer newEntry = new Customer();
            //newEntry.CustomerId = customerid;
            newEntry.FirstName = firstName;
            newEntry.LastName = lastName;
            newEntry.Company = company;
            newEntry.Address = address;
            newEntry.City = city;
            newEntry.State = state;
            newEntry.Country = country;
            newEntry.PostalCode = postalCode;
            newEntry.Phone = phone;
            newEntry.Fax = fax;
            newEntry.Email = email;
            newEntry.SupportRepId = supportRepId; 

            rs.Customers.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsCustomer");
        }

        public ActionResult DeleteCustomer(string customerid)
        {
            int id = Int32.Parse(customerid);

            rs.Customers.RemoveRange(rs.Customers.Where(c => c.CustomerId == id));
            rs.Invoices.RemoveRange(rs.Invoices.Where(c => c.CustomerId == id));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsCustomer");

        }

        public ActionResult EditCustomer(int customerId, string firstName, string lastName, string company, string address, string city,
            string state, string country, string postalCode, string phone, string fax, string email, int supportRepId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditCust(int customerid, string firstName, string lastName, string company, string address, string city,
            string state, string country, string postalCode, string phone, string fax, string email, int supportRepId)
        {
            Customer newEntry = new Customer();
            newEntry.CustomerId = customerid;
            newEntry.FirstName = firstName;
            newEntry.LastName = lastName;
            newEntry.Company = company;
            newEntry.Address = address;
            newEntry.City = city;
            newEntry.State = state;
            newEntry.Country = country;
            newEntry.PostalCode = postalCode;
            newEntry.Phone = phone;
            newEntry.Fax = fax;
            newEntry.Email = email;
            newEntry.SupportRepId = supportRepId;

            Customer entryData = rs.Customers.SingleOrDefault(customer => customer.CustomerId == customerid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsCustomer");
        }

        //------------ Genre
        public ActionResult DetailsGenre(string tableName)
        {
            return View(rs.Genres);
        }
        public ActionResult CreateGenre()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGen(string name)
        {

            Genre newEntry = new Genre();
            //newEntry.CustomerId = customerid;
            newEntry.Name = name;

            rs.Genres.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsGenre");
        }

        public ActionResult DeleteGenre(string genreid)
        {
            int id = Int32.Parse(genreid);

            rs.Genres.RemoveRange(rs.Genres.Where(c => c.GenreId == id));
            rs.Tracks.RemoveRange(rs.Tracks.Where(c => c.GenreId == id));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsGenre");

        }

        public ActionResult EditGenre(int genreid, string name)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditGen(int genreid, string name)
        {
            Genre newEntry = new Genre();
            newEntry.GenreId = genreid;
            newEntry.Name = name;

            Genre entryData = rs.Genres.SingleOrDefault(genre => genre.GenreId == genreid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsGenre");
        }

        //------------ Invoice
        public ActionResult DetailsInvoice(string tableName)
        {
            return View(rs.Invoices);
        }
        public ActionResult CreateInvoice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateInvo(int customerid, DateTime invoicedate,
            string billingaddress, string billingcity, string billingstate, 
            string billingcountry, string billingpostalcode, decimal? total )
        {

            Invoice newEntry = new Invoice();
            //newEntry.InvoiceId = invoiceid;
            newEntry.CustomerId = customerid;
            newEntry.InvoiceDate = invoicedate;
            newEntry.BillingAddress = billingaddress;
            newEntry.BillingCity = billingcity;
            newEntry.BillingState = billingstate;
            newEntry.BillingCountry = billingcountry;
            newEntry.BillingPostalCode = billingpostalcode;
            newEntry.Total = total;

            rs.Invoices.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsInvoice");
        }

        public ActionResult DeleteInvoice(string invoiceid)
        {
            int id = Int32.Parse(invoiceid);

            rs.Invoices.RemoveRange(rs.Invoices.Where(c => c.InvoiceId == id));
            rs.InvoiceLines.RemoveRange(rs.InvoiceLines.Where(c => c.InvoiceId == id));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsInvoice");

        }

        public ActionResult EditInvoice(int invoiceid, int customerid, DateTime invoicedate,
            string billingaddress, string billingcity, string billingstate,
            string billingcountry, string billingpostalcode, decimal? total)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditInvo(int invoiceid, int customerid, DateTime invoicedate,
            string billingaddress, string billingcity, string billingstate,
            string billingcountry, string billingpostalcode, decimal? total)
        {
            Invoice newEntry = new Invoice();
            newEntry.InvoiceId = invoiceid;
            newEntry.CustomerId = customerid;
            newEntry.InvoiceDate = invoicedate;
            newEntry.BillingAddress = billingaddress;
            newEntry.BillingCity = billingcity;
            newEntry.BillingState = billingstate;
            newEntry.BillingCountry = billingcountry;
            newEntry.BillingPostalCode = billingpostalcode;
            newEntry.Total = total;

            Invoice entryData = rs.Invoices.SingleOrDefault(invoice => invoice.InvoiceId == invoiceid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsInvoice");
        }

        //------------ Track

        public ActionResult DetailsTrack(string tableName)
        {
            return View(rs.Tracks);
        }
        public ActionResult CreateTrack()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTra(string name, int albumid, int mediatypeid, int genreid, string composer, int milliseconds, int bytes, decimal unitprice)
        {

            Track newEntry = new Track();
            //newEntry.CustomerId = customerid;
            newEntry.Name = name;
            newEntry.AlbumId = albumid;
            newEntry.MediaTypeId = mediatypeid;
            newEntry.GenreId = genreid;
            newEntry.Composer = composer;
            newEntry.Milliseconds = milliseconds;
            newEntry.Bytes = bytes;
            newEntry.UnitPrice = unitprice;

            rs.Tracks.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsTrack");
        }

        public ActionResult DeleteTrack(int trackid)
        {
            //int id = Int32.Parse(trackid);

            rs.Tracks.RemoveRange(rs.Tracks.Where(c => c.TrackId == trackid));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsTrack");

        }

        public ActionResult EditTrack(int trackid, string name, int albumid, int mediatypeid, int genreid, 
        string composer, int milliseconds, int bytes, decimal unitprice)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditTra(int trackid, string name, int albumid, int mediatypeid, int genreid, 
        string composer, int milliseconds, int bytes, decimal unitprice)
        {
            Track newEntry = new Track();
            newEntry.TrackId = trackid;
            newEntry.Name = name;
            newEntry.AlbumId = albumid;
            newEntry.MediaTypeId = mediatypeid;
            newEntry.GenreId = genreid;
            newEntry.Composer = composer;
            newEntry.Milliseconds = milliseconds;
            newEntry.Bytes = bytes;
            newEntry.UnitPrice = unitprice;

            Track entryData = rs.Tracks.SingleOrDefault(track => track.TrackId == trackid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsTrack");
        }

        //------------ PlaylistTrack

        public ActionResult DetailsPlaylistTrack(string tableName)
        {
            return View(rs.PlaylistTracks);
        }
        public ActionResult CreatePlaylistTrack()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePlayTrack(int trackid)
        {

            PlaylistTrack newEntry = new PlaylistTrack();
            //newEntry.CustomerId = customerid;
            newEntry.TrackId = trackid;

            rs.PlaylistTracks.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsPlaylistTrack");
        }

        public ActionResult DeletePlaylistTrack(int playlistid, int trackid)
        {
            //int id = Int32.Parse(playlistid);

            rs.PlaylistTracks.RemoveRange(rs.PlaylistTracks.Where(c => c.PlaylistId == playlistid && c.TrackId == trackid));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsPlaylistTrack");

        }

        public ActionResult EditPlaylistTrack(int playlistid, int trackid)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPlayTrack(int playlistid, int trackid)
        {
            PlaylistTrack newEntry = new PlaylistTrack();
            newEntry.PlaylistId = playlistid;
            newEntry.TrackId = trackid;

            PlaylistTrack entryData = rs.PlaylistTracks.SingleOrDefault(playlistTrack => playlistTrack.PlaylistId == playlistid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsPlaylistTrack");
        }

        //------------ Playlist

        public ActionResult DetailsPlaylist(string tableName)
        {
            return View(rs.Playlists);
        }
        public ActionResult CreatePlaylist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePlay(string name)
        {

            Playlist newEntry = new Playlist();
            //newEntry.CustomerId = customerid;
            newEntry.Name = name;

            rs.Playlists.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsPlaylist");
        }

        public ActionResult DeletePlaylist(int playlistid)
        {
            //int id = Int32.Parse(playlistid);

            rs.Playlists.RemoveRange(rs.Playlists.Where(c => c.PlaylistId == playlistid));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsPlaylist");

        }

        public ActionResult EditPlaylist(int playlistid, string name)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPlay(int playlistid, string name)
        {
            Playlist newEntry = new Playlist();
            newEntry.PlaylistId = playlistid;
            newEntry.Name = name;

            Playlist entryData = rs.Playlists.SingleOrDefault(playlist => playlist.PlaylistId == playlistid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsPlaylist");
        }

        //------------ MediaType

        public ActionResult DetailsMediaType(string tableName)
        {
            return View(rs.MediaTypes);
        }
        public ActionResult CreateMediaType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMedia(string name)
        {

            MediaType newEntry = new MediaType();
            //newEntry.CustomerId = customerid;
            newEntry.Name = name;

            rs.MediaTypes.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsMediaType");
        }

        public ActionResult DeleteMediaType(int mediatypeid)
        {
            //int id = Int32.Parse(mediatypeid);

            rs.MediaTypes.RemoveRange(rs.MediaTypes.Where(c => c.MediaTypeId == mediatypeid));
            rs.Tracks.RemoveRange(rs.Tracks.Where(c => c.MediaTypeId == mediatypeid));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsMediaType");

        }

        public ActionResult EditMediaType(int mediatypeid, string name)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditMedia(int mediatypeid, string name)
        {
            MediaType newEntry = new MediaType();
            newEntry.MediaTypeId = mediatypeid;
            newEntry.Name = name;

            MediaType entryData = rs.MediaTypes.SingleOrDefault(mediatype => mediatype.MediaTypeId == mediatypeid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsMediaType");
        }

        //------------ InvoiceLine

        public ActionResult DetailsInvoiceLine(string tableName)
        {
            return View(rs.InvoiceLines);
        }
        public ActionResult CreateInvoiceLine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateInvoiceL(int invoiceid, int trackid, decimal? unitprice, int quantity)
        {

            InvoiceLine newEntry = new InvoiceLine();
            //newEntry.CustomerId = customerid;
            newEntry.InvoiceId = invoiceid;
            newEntry.TrackId = trackid;
            newEntry.UnitPrice = unitprice;
            newEntry.Quantity = quantity;

            rs.InvoiceLines.Add(newEntry);
            rs.SaveChanges();

            Response.Write(@"<script language='javascript'>alert('Data inserted successfully');</script>"); //---------------------------Not working

            return RedirectToAction("DetailsInvoiceLine");
        }

        public ActionResult DeleteInvoiceLine(int invoicelineid)
        {
            //int id = Int32.Parse(invoicelineid);

            rs.InvoiceLines.RemoveRange(rs.InvoiceLines.Where(c => c.InvoiceLineId == invoicelineid));
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsInvoiceLine");

        }

        public ActionResult EditInvoiceLine(int invoicelineid, int invoiceid, int trackid, decimal? unitprice, int quantity)
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditInvoiceL(int invoicelineid, int invoiceid, int trackid, decimal? unitprice, int quantity)
        {
            InvoiceLine newEntry = new InvoiceLine();
            newEntry.InvoiceLineId = invoicelineid;
            newEntry.InvoiceId = invoiceid;
            newEntry.TrackId = trackid;
            newEntry.UnitPrice = unitprice;
            newEntry.Quantity = quantity;

            InvoiceLine entryData = rs.InvoiceLines.SingleOrDefault(invoiceline => invoiceline.InvoiceLineId == invoicelineid);

            rs.Entry(entryData).CurrentValues.SetValues(newEntry);
            rs.SaveChanges();

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsInvoiceLine");
        }
    }
}