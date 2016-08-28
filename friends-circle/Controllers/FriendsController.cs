using friends_circle.Models;
using Newtonsoft.Json;
using System;
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
                             select friendList).FirstOrDefault();

                if (check != null)
                {
                    return RedirectToAction("Add", new { message = 0 });
                }

                db.friends.Add(viewModel.friend);
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