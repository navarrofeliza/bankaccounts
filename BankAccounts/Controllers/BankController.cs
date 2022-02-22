using System.Linq;
using BankAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    [Route("bank")]
    public class BankController : Controller
    {
        private bool inSession
        {
            get { return HttpContext.Session.GetInt32("userId") != null; }
        }
        private User loggedInUser
        {
            get
            {
                return _context.Users.Include(u => u.Transactions)
                    .FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
            }
        }
        private MyContext _context;
        public BankController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            if (!inSession)
                return RedirectToAction("Login", "Home");

            var user = loggedInUser;
            ViewBag.User = user;
            ViewBag.Transactions = _context.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .Where(t => t.UserId == loggedInUser.UserId);
            return View();
        }
        [HttpPost("money")]
        public IActionResult Money(Transaction trans)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(trans);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            var user = loggedInUser;
            ViewBag.User = user;
            ViewBag.Transactions = _context.Transactions
                .OrderByDescending(t => t.CreatedAt)
                .Where(t => t.UserId == loggedInUser.UserId);
            return View("Index");
        }
    }
}