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

        public ActionResult Index()
        {  
            string sql = @" SELECT TABLE_NAME
                            FROM INFORMATION_SCHEMA.TABLES
                            ;";

            List<TableModel> data = DataAccess.SqlDataAccess.LoadData<Models.TableModel>(sql);

            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
            rs.SaveChanges();

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsArtist");

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

    }
}