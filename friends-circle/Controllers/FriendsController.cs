using friends_circle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace friends_circle.Controllers
{
    public class FriendsController : Controller
    {
        FriendContext db = new FriendContext();

        // GET: Friends
        public ActionResult Index()
        {
            
            return View();
        }

        // GET: Friends/Add
        public ActionResult Add()
        {
            return View();
        }
    }
}