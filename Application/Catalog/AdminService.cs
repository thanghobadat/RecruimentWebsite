using Application.Common;
using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Catalog
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RecruimentWebsiteDbContext _context;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        public AdminService(RecruimentWebsiteDbContext context, IStorageService storageService,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _storageService = storageService;
            _mapper = mapper;
        }
    }
}
