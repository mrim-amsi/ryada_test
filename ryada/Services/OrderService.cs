using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ryada.Models;

namespace ryada.Services
{
    public class OrderService 
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDBContext _context;

        public OrderService(UserManager<IdentityUser> userManager, AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<List<order>> GetOrderAsync(string? search)
        {
            var query =  _context.orders.AsQueryable();
            if(!string.IsNullOrEmpty(search))
            {
                query = query.Include(x => x.Post).Where(p => p.Post.Author.Contains(search));
            }

            return await query.Include(x=>x.Post).ToListAsync();
        }
        public async Task<List<order>> GetAllOrders()
        {
            List<order> posts = _context.orders.Include(x => x.Post)
               .ToList();
            return posts;
        }

        public async Task Remove(int id)
        {

            var order = await _context.orders.FindAsync(id);
            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        public OrderDTO Add()
        {
            List<Book> books = _context.Books.ToList();

            OrderDTO addOrder = new OrderDTO();
            addOrder.Books = new SelectList(books, "Id", "Title");
            return addOrder;
        }
        public async Task<order> AddAsync(order post)
        {

            var order = await _context.orders.FindAsync(post.BookId);

            _context.orders.Add(post);
            _context.SaveChanges();

            
            return order;
        }
    }
}
