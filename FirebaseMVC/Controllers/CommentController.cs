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
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IPostRepository _postRepo;
        private readonly IUserProfileRepository _userProfileRepo;

        public CommentController (ICommentRepository commentRepository, IUserProfileRepository userProfileRepository, IPostRepository postRepository)
        {
            _postRepo = postRepository;
            _userProfileRepo = userProfileRepository;
            _commentRepo = commentRepository;
        }

        // GET: CommentController
        public ActionResult Index(int id)
        {
            var comment = _commentRepo.GetCommentByPostId(id);
            
            return View(comment);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // GET: CommentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Comment comment)
        {
            comment.UserProfileId = GetCurrentUserProfileId();
            comment.PostId = id;
            try
            {
                _commentRepo.AddComment(comment);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

       
        public ActionResult Edit(int id)
        {

            Comment comment = _commentRepo.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepo.UpdateComment(comment);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);

            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)

        {
            var getcomment = _commentRepo.GetCommentById(id);

            try
            {
                _commentRepo.DeleteComment(id);
                return RedirectToAction("Index", new { id = getcomment.PostId });


            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }
    }
}
