using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Interfaces;

namespace PaintShopMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accRepo;
        private readonly UserManager<AppUsers> _userManager;


        public AccountController(IAccountRepository accRepo, UserManager<AppUsers> userManager)
        {
            _accRepo = accRepo;
            _userManager = userManager;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var adresses = await _accRepo.GetAllAddressAsync();
            if (User.IsInRole("Admin")) return View(adresses);
            else return View(adresses.Where(a => a.UserId == user.Id));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersList()
        {
            var users = await _accRepo.GetAllUserAsync();
            return View(users);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var address = await _accRepo.GetAddressAsync(id);

            if (!User.IsInRole("Admin"))
                if (address == null || address.UserId != _userManager.GetUserId(User))
                {
                    TempData["Error"] = "Próbowano uzyskać adres innego użytkownika";
                    return RedirectToAction("Index");
                }

            return View(address);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,City,PostalCode,Country,HouseNumber,ApartmentNumber, FirstName, LastName")] Address address)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                address.UserId = user.Id;
                if (address.Email == string.Empty) address.Email = user.Email;
                if (address.PhoneNumber == string.Empty) address.PhoneNumber = user.PhoneNumber;

                if (_accRepo.Add(address))
                    return RedirectToAction(nameof(Index));
                else return View(address);
            }
            return View(address);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var address = await _accRepo.GetAddressAsync(id);
            if (!User.IsInRole("Admin"))
                if (address == null || address.UserId != _userManager.GetUserId(User))
                {
                    TempData["Error"] = "Próbowano zmienić adres innego użytkownika";
                    return RedirectToAction("Index");
                }
            return View(address);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street,City,PostalCode,Country,HouseNumber,ApartmentNumber,PhoneNumber,Email")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }
            if (address.UserId == _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _accRepo.Update(address);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AddressExists(address.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else return View(address);
            }
            else
            {
                TempData["Error"] = "Adres nie należy do użytkownika";
                return View(address);
            }

        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _accRepo.GetAddressAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _accRepo.GetAddressAsync(id);
            if (address != null || address.UserId != _userManager.GetUserId(User))
            {
                if (address.Orders.Any())
                {
                    TempData["Error"] = "Nie można usuną adresu z powiązanymi zamówieniami";
                    return RedirectToAction(nameof(Index));
                }
                _accRepo.Delete(address);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return (_accRepo.GetAddressAsync(id) != null);
        }
        private async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(User);

            return user.Id.ToString();

        }
    }
}
