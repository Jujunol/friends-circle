using friends_circle.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace friends_circle.Controllers
{
    public class FriendsController : Controller
    {
        FriendContext db = new FriendContext();
        string[] messages = { "That Friend has already been added!", "That address doesn't exist!", "Unknown Error" };

        // GET: Friends
        public ActionResult Index()
        {
            /* @TODO Disance logic */
            return View(db.friends.ToList());
        }

        // GET: Friends/Add
        public ActionResult Add(int? message)
        {
            AddFriendViewModel viewModel = new AddFriendViewModel();

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
        public ActionResult Add(AddFriendViewModel viewModel)
        {
            // check if the friend already exists
            var check = (from friendList in db.friends
                         where friendList.name == viewModel.friend.name
                         && friendList.street == viewModel.friend.street
                         select friendList).FirstOrDefault();

            if (check != null)
            {
                return RedirectToAction("Index", new { message = 0 });
            }

            // grab longitude and latitude from Google API
            string url = String.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}",
                Url.Encode(viewModel.friend.street),
                "AIzaSyBSq0tOBLsN4x6qxSRZ5unjeVdAiBeoFXM");
            string jsonResponse = new WebClient().DownloadString(url);

            // attempt to deserialize the object
            using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
            {
                dynamic response = JsonConvert.DeserializeObject(jsonResponse);
                string status = response.status;

                /*
                    Status codes are standard GoogleAPI responses
                    https://developers.google.com/maps/documentation/geocoding/intro#StatusCodes
                */
                if (status == "OK")
                {
                    // grab the latitude and longitude and store them
                    dynamic location = response.results.First.geometry.location;
                    string lat = location.lat;
                    string lng = location.lng;

                    viewModel.friend.latitude = lat;
                    viewModel.friend.longitude = lng;

                    db.friends.Add(viewModel.friend);
                    db.SaveChanges();
                }
                else if (status == "ZERO_RESULTS")
                {
                    return RedirectToAction("Add", new { message = 1 });
                }
                else
                {
                    return RedirectToAction("Add", new { message = 2 });
                }
            }

            return RedirectToAction("Index");
        }

    }
}