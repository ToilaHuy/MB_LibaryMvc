using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _dbContext;

        public HomeController(MyDbContext dbContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(UserViewModel user)
        {
            var session = HttpContext.Session;

            var checkName = _dbContext.Users.SingleOrDefault(u => u.UserName.Equals(user.Username));
            if (checkName == null)
            {
                ViewBag.loginError = "cannot find account or password is incorrect";
                return View();
            }
            var validPassword = BCrypt.Net.BCrypt.Verify(user.Password, checkName.Password);
            if (validPassword) session.SetString("userName", checkName.UserName);
            else
            {
                ViewBag.loginError = "cannot find account or password is incorrect";
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout(User user)
        {
            var session = HttpContext.Session;
            session.Clear();
            return RedirectToAction(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}