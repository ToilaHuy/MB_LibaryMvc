
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;
using WebApplication8.Services;

namespace WebApplication8.Controllers
{
    public class BorrowersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;


        public BorrowersController(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Borrowers
        public async Task<IActionResult> Index(string name)
        {
           var borrower = !string.IsNullOrEmpty(name) ? await _unitOfWork.Borrower.FindAll(b => b.BorrowerName.Contains(name)) : await _unitOfWork.Borrower.GetAll();
            return borrower != null ?
                        View(_mapper.Map<IEnumerable<BorrowerVM>>(borrower)) :
                        Problem("Entity set 'MyDbContext.Items'  is null.");
        }

        // GET: Borrowers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitOfWork.Borrower == null)
            {
                return NotFound();
            }

            var borrower = await _unitOfWork.Borrower.GetById((int)id);
            if (borrower == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<BorrowerVM>(borrower));
        }

        // GET: Borrowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Borrowers/Create
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibraryCardNumber,BorrowerName,Address")] BorrowerVM borrower)
        {
            var borrowerMapper = _mapper.Map<Borrower>(borrower);
            if (ModelState.IsValid)
            {
          
                await _unitOfWork.Borrower.Add(borrowerMapper);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(borrower);
        }

        // GET: Borrowers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitOfWork.Borrower == null)
            {
                return NotFound();
            }

            var borrower = await _unitOfWork.Borrower.GetById((int)id);
            if (borrower == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<BorrowerVM>(borrower));
        }

        // POST: Borrowers/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibraryCardNumber,BorrowerName,Address")] BorrowerVM borrower)
        {
            if (!ModelState.IsValid) return View(borrower);
            var borrowerMapper = _mapper.Map<Borrower>(borrower);
            borrowerMapper.BorrowerId = id;
           
                   
                await _unitOfWork.Borrower.Update(borrowerMapper);
                await _unitOfWork.Save();
           
            return RedirectToAction(nameof(Index));
        }

        // GET: Borrowers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.Borrower == null)
            {
                return NotFound();
            }

            var borrower =await _unitOfWork.Borrower.GetById((int)id);
            if (borrower == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<BorrowerVM>(borrower));
        }

        // POST: Borrowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_unitOfWork.Borrower == null)
            {
                return Problem("Entity set 'MyDbContext.Borrowers'  is null.");
            }
            var delete =  id != null && await _unitOfWork.Borrower.Remove((int)id);
            if (!delete) Problem("Cannot delete");
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
