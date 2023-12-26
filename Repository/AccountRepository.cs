using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Data;
using PaintShopMVC.Interfaces;
using PaintShopMVC.Models.Accounts;
using System.Security.Claims;

namespace PaintShopMVC.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppdbContext _context;
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(AppdbContext appdbContext, UserManager<AppUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = appdbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public bool Add(Address address)
        {
            _context.Add(address);
            return Save();
        }

        public bool Delete(Address address)
        {
            _context.Remove(address);
            return Save();
        }

        public async Task<Address> GetAddressAsync(int addressId)
        {
            var address = await _context.Addresses.Include(a => a.Orders).FirstOrDefaultAsync(a => a.Id == addressId);

            if (address != null)
            {
                return address;
            }
            else
            {
                
                return null;
            }
        }

        public async Task<IEnumerable<Address>> GetAllAddressAsync()
        {
            if (_context.Addresses.Any())
            {
                return  await _context.Addresses.ToListAsync();
            }
            else
            {
                return Enumerable.Empty<Address>();
            }
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool Update(Address address)
        {
            _context.Update(address);
            return Save();
        }
        public async Task<IEnumerable<AppUsers>> GetAllUserAsync()
        {
            var users = await _context.AppUsers.ToListAsync();
            return users;

        }
    }
}
