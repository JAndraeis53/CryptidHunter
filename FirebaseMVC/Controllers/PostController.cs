using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptidHunter.Repositories;
using CryptidHunter.Models;
using System.Security.Claims;

namespace CryptidHunter.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public PostController(IPostRepository postRepository, IUserProfileRepository userProfileRepository)
        {
            _postRepo = postRepository;
            _userProfileRepo = userProfileRepository;
        }

        // GET: PostController
        public ActionResult Index()
        {
            var post = _postRepo.GetAllPost();

            return View(post);
        }

        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            Post post = _postRepo.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            post.UserProfileId = GetCurrentUserProfileId();
            try
            {
                _postRepo.AddPost(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
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

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
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
