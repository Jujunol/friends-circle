using friends_circle.Models;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace friends_circle.Controllers
{
    public class FriendsController : Controller
    {
        FriendContext db = new FriendContext();
        string[] messages = { "That Friend has already been added!", "That address doesn't exist!", "Unknown Error" };

        // GET: Friends
        public ActionResult Index(string DistanceSearch, string FromWhere, string FromWhereCity)
        {
            GoogleMapsAPI maps = GoogleMapsAPI.getInstance();

            // retrieve the current Client's IP
            string clientIp = Request.UserHostAddress;
            
            // debugging for localhost
            if(clientIp == "::1")
            {
                clientIp = "65.92.33.108";
            }

            // use IpApi to retrieve the client's location
            // or use search city
            string lat = null, lng = null;
            if(!String.IsNullOrEmpty(FromWhere) && FromWhere == "other" && !String.IsNullOrEmpty(FromWhereCity))
            {
                string[] location = maps.getAddressInfoByStreet(FromWhereCity);
                if(location != null)
                {
                    lat = location[0];
                    lng = location[1];
                }
            }
            if(String.IsNullOrEmpty(lat) || String.IsNullOrEmpty(lng))
            {
                string location = new WebClient().DownloadString(String.Format("https://ipapi.co/{0}/latlong/", clientIp));
                lat = location.Substring(0, location.IndexOf(",")).Trim();
                lng = location.Substring(lat.Length + 1).Trim();
            }


            // check distance from search
            double dist = 10000;
            if (!String.IsNullOrEmpty(DistanceSearch))
                Double.TryParse(DistanceSearch, out dist);

            //set up for query call
            var pLat = new SqlParameter("p_lat", lat);
            var pLng = new SqlParameter("p_lng", lng);
            var pDist = new SqlParameter("p_dist", dist);

            // call the procedure
            var friendList = db.Database.SqlQuery<FriendWithDistanceViewModel>("exec geodist @p_lat, @p_lng, @p_dist", pLat, pLng, pDist);

            string address = maps.getAddressInfoByLocation(lat, lng);

            FriendListViewModel viewModel = new FriendListViewModel()
            {
                friendList = friendList,
                lat = lat,
                lng = lng,
                location = address
            };
            return View(viewModel);
        }

        // GET: Friends/Add
        public ActionResult Add(int? message)
        {
            FriendAddViewModel viewModel = new FriendAddViewModel();

            if (message != null)
            {
                // safeguard range
                int index = message >= messages.Length || message < 0 ? messages.Length - 1 : (int)message;
                viewModel.message = messages[index];
            }

            return View(viewModel);
        }

        // POST: Friends/Add
        [HttpPost]
        public ActionResult Add(FriendAddViewModel viewModel)
        {
            GoogleMapsAPI maps = GoogleMapsAPI.getInstance();
            string[] location = maps.getAddressInfoByStreet(viewModel.friend.street);

            if (maps.statusCode == GoogleMapsAPI.RESPONSE_OK)
            {
                // grab the latitude and longitude and store them
                viewModel.friend.latitude = location[0];
                viewModel.friend.longitude = location[1];
                viewModel.friend.full_address = location[2];

                // check if the friend already exists
                var check = (from friendList in db.friends
                             where friendList.full_address == viewModel.friend.full_address
                             && friendList.name == viewModel.friend.name 
                             select friendList).FirstOrDefault();

                if (check != null)
                {
                    return RedirectToAction("Add", new { message = 0 });
                }

                db.friends.Add((Friend)viewModel.friend);
                db.SaveChanges();
            }
            else if (maps.statusCode == GoogleMapsAPI.RESPONSE_NOT_FOUND)
            {
                return RedirectToAction("Add", new { message = 1 });
            }
            else
            {
                return RedirectToAction("Add", new { message = 2 });
            }

            return RedirectToAction("Index");
        }

    }
}