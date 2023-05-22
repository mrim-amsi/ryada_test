using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ryada.Models;

namespace ryada.Services
{
    public class BookService 
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDBContext _context;

        public BookService(UserManager<IdentityUser> userManager, AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<List<Book>> GetBooksAsync(string? search)
        {
            var query =  _context.Books.Include(n => n.Category).AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Category.Name.Contains(search) || p.Author.Contains(search));
            }

            return await query .ToListAsync();
        }



        public async Task Remove(int id)
        {

            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        public BookDTO Add()
        {
            List<Category> categories = _context.Categories.ToList();
            BookDTO Book = new BookDTO();
            Book.categories = new SelectList(categories, "Id", "Name");
            return Book;
        }
        public async Task<Book> AddAsync(BookDTO post)
        {

            
            var Added = new Book
            {
                Title = post.Title,
                CategoryId = post.CategoryId,
                Price = post.Price,
                Quantity = post.Quantity,
                Author = post.Author,

            };
            _context.Books.Add(Added);

            _context.SaveChanges();
            
            return Added;
        }
    }
}
