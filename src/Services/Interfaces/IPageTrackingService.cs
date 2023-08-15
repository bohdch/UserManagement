using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookVerse.Models;
using BookVerse.Models.ViewModels;

namespace BookVerse.Services.Interfaces
{
    public interface IPageTrackingService
    {
        Task<bool> IsPageRequested(string category, int page);

        Task AddRequestedPage(string category, int page);
    }
}
