using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookVerse.Services.Interfaces;
using BookVerse.Models;
using BookVerse.Models.ViewModels;
using BookVerse.Data;

namespace BookVerse.Services
{
    public class PageTrackingService : IPageTrackingService
    {
        private readonly BookVerseDbContext _dbContext;

        public PageTrackingService(BookVerseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsPageRequested(string category, int page)
        {
            return await _dbContext.RequestedPages.AnyAsync(rp => rp.Category == category && rp.PageNumber == page);
        }

        public async Task AddRequestedPage(string category, int page)
        {
            bool pageExists = await _dbContext.RequestedPages.AnyAsync(rp => rp.Category == category && rp.PageNumber == page);

            if (!pageExists)
            {
                var requestedPage = new RequestedPage { Category = category, PageNumber = page };
                _dbContext.RequestedPages.Add(requestedPage);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
