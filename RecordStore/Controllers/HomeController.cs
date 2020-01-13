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
    {//The Controller that includes the code for the employees view functions

        static string ancientName = "";
        static int ancientId;

        private readonly RecordStoreContext _context;

        public HomeController(RecordStoreContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            /*ViewBag.Message = "Tables List";

            RecordStoreContext rs = new RecordStoreContext();
            //TODO Continue
            //rs.Set

            var mapping = rs  FindEntityType(typeof(YourEntity)).Relational();
            var schema = mapping.Schema;
            var tableName = mapping.TableName;

            List<string> = rs.Database.SqlQuery("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ");

            string sql = @" SELECT TABLE_NAME
                            FROM INFORMATION_SCHEMA.TABLES
                            ;";

            return DataAccess.SqlDataAccess.LoadData<Models.TableModel>(sql); */

            //List<> data = Processors.TableProcessor.LoadTables();
            

            RecordStoreContext rs = new RecordStoreContext();
            
            //var mapping = rs.Model.FindEntityType(typeof(YourEntity)).Relational();
            


            return View(rs);
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

        /*
        public ActionResult DetailsArtist(string tableName)
        {
            RecordStoreContext rs = new RecordStoreContext();
            
            
            return View();
        }

        [HttpPost]
        public ActionResult CreateA(string name)
        {
            Processors.ArtistProcessor.createArtist(name);

            return RedirectToAction("DetailsArtist");
        }

        public ActionResult DeleteArtist(string name)
        {
            Processors.ArtistProcessor.deleteArtist(name);

            return RedirectToAction("DetailsArtist");

        }
        public ActionResult EditArtist(string name)
        {
            ancientName = name;

            return View();
        }

        [HttpPost]
        public ActionResult EditArtistA(string name)
        {
            string name1 = name;
            string sql = @"select * from Artist where (Name) ='" + ancientName + "'";
            var ida = SqlDataAccess.LoadData<Artist>(sql);

            Processors.ArtistProcessor.editArtist(name, ida[0].ArtistId);

            return RedirectToAction("DetailsArtist");
        }
        */


        //------------------Albums

        public ActionResult DetailsAlbum(string tableName)
        {
            return View();
        }
        public ActionResult CreateAlbum(string name)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAlb(string albumid, string title, string artistId)
        {
            int id = Int32.Parse(albumid);

            int artid = Int32.Parse(artistId);
            RecordStoreContext rs = new RecordStoreContext();

            Album newalb = new Album();
            newalb.AlbumId = id;
            newalb.Title = title;
            newalb.ArtistId = artid;

            rs.Albums.Add(newalb);
       

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsAlbum");
        }

        public ActionResult DeleteAlbum(string albumid)
        {
            int id = Int32.Parse(albumid);

            RecordStoreContext rs = new RecordStoreContext();

            rs.Albums.RemoveRange(rs.Albums.Where(c => c.AlbumId == id));

            Response.Write("<script>alert('Data removed')</script>");

            return RedirectToAction("DetailsAlbum");

        }

        public ActionResult EditAlbum(string title, int artistId)
        {
            /*
             * ancientName = title;
            ancientId = artistId;
            */

            return View();
        }

        [HttpPost]
        public ActionResult EditAlb(string albumid, string title, string artistId)
        {
            string name1 = title;
            int id = Int32.Parse(albumid);

            int artid = Int32.Parse(artistId);
            RecordStoreContext rs = new RecordStoreContext();

            Album newalb = new Album();
            newalb.AlbumId = id;
            newalb.Title = title;
            newalb.ArtistId = artid;

            rs.Albums.Add(newalb);

            Album albumData = rs.Albums.SingleOrDefault(album => album.AlbumId == id);

            rs.Entry(albumData).CurrentValues.SetValues(newalb);

            Response.Write("<script>alert('Data inserted successfully')</script>");

            return RedirectToAction("DetailsAlbum");
        }
    }
}