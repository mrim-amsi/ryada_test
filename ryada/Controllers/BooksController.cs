using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ryada.Models;
using ryada.Services;

namespace ryada.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BookService _bookService;
        private readonly UserManager<IdentityUser> _userManager;


        public BooksController(UserManager<IdentityUser> userManager,AppDBContext postDbContext,
            IWebHostEnvironment webHostEnvironment,BookService bookService)
        {
            _context = postDbContext;
            _hostEnvironment = webHostEnvironment;
            _bookService = bookService;
            _userManager = userManager;

        }
        public async Task<IActionResult> Index(string? search)
        {
         
            return View(await _bookService.GetBooksAsync(search ?? ""));
        }

        [HttpGet]
        public IActionResult Add()
        {
          return View( _bookService.Add());
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(BookDTO post)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;
            Book book = await _bookService.AddAsync(post);
            book.InsertById = userId;
            TempData["success"] = "Successfully Added";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            await _bookService.Remove(id);

            return RedirectToAction(nameof(Index));

        }
    }
}
