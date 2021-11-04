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
        private readonly IFavoriteRepository _favoriteRepo;

        public PostController(IPostRepository postRepository, IUserProfileRepository userProfileRepository, IFavoriteRepository favoriteRepository)
        {
            _postRepo = postRepository;
            _userProfileRepo = userProfileRepository;
            _favoriteRepo = favoriteRepository;
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
            Post post = _postRepo.GetPostById(id, GetCurrentUserProfileId());
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
            Post post = _postRepo.GetPostById(id, GetCurrentUserProfileId());
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepo.UpdatePost(post);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            Post post = _postRepo.GetPostById(id, GetCurrentUserProfileId());

            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepo.DeletePost(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        public ActionResult Favorite(int id)
        {
            var favorite = new Favorite
            {
                UserProfileId = GetCurrentUserProfileId(),
                PostId = id
            };

            _favoriteRepo.AddFavorite(favorite);

            return RedirectToAction("Details", new { id });

        }

        
        public ActionResult UnFavorite(int id, int postId)
        {

            try
            {
                _favoriteRepo.DeleteFavorite(id);
                return RedirectToAction("Details", new { id = postId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = postId });

            }
        }

        //currenlty using asp route id for the favorite not the post. Need to use the post Id instead

        //public ActionResult Delete(int id)
        //{
        //    Favorite favorite = _postRepo.GetPostById(id, GetCurrentUserProfileId());

        //    return View(post);
        //}
    }
}
