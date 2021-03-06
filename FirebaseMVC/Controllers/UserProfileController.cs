using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptidHunter.Models;
using CryptidHunter.Repositories;

namespace CryptidHunter.Controllers
{
    
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userRepo;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userRepo = userProfileRepository;
        }

        // GET: UserProfile
        public ActionResult Index()
        {
            List<UserProfile> userProfiles = _userRepo.GetAllUsers();

            return View(userProfiles);
        }

        // GET: UserProfile/Details/5
        public ActionResult Details(int id)
        {
            UserProfile userProfile = _userRepo.GetUserById(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        // GET: UserProfile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProfile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProfile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProfile/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
