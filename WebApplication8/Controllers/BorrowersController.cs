using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;

namespace WebApplication8.Controllers
{
    public class BorrowersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Borrowers
        public async Task<IActionResult> Index(string name)
        {
              
            var borrower = _unitOfWork.Borrower.GetAll();
            if (!string.IsNullOrEmpty(name))
            {
                borrower = _unitOfWork.Borrower.FindAll(b => b.BorrowerName.Contains(name) );
            }
            return borrower != null ?
                        View(borrower) :
                        Problem("Entity set 'MyDbContext.Items'  is null.");
        }

        // GET: Borrowers/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null || _unitOfWork.Borrower == null)
            {
                return NotFound();
            }

            var borrower = _unitOfWork.Borrower.GetById((Guid)id);
            if (borrower == null)
            {
                return NotFound();
            }

            return View(borrower);
        }

        // GET: Borrowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Borrowers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibraryCardNumber,BorrowerName,Address")] BorrowerModel borrower)
        {
            var borrowerMapper = _mapper.Map<Borrower>(borrower);
            if (ModelState.IsValid)
            {
                borrowerMapper.BorrowerId = Guid.NewGuid();
                _unitOfWork.Borrower.Add(borrowerMapper);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(borrower);
        }

        // GET: Borrowers/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null || _unitOfWork.Borrower == null)
            {
                return NotFound();
            }

            var borrower = _unitOfWork.Borrower.GetById((Guid)id);
            if (borrower == null)
            {
                return NotFound();
            }
            return View(borrower);
        }

        // POST: Borrowers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LibraryCardNumber,BorrowerName,Address")] BorrowerModel borrower)
        {
           

            if (ModelState.IsValid)
            {
                var borrowerMapper = _mapper.Map<Borrower>(borrower);
                borrowerMapper.BorrowerId = id;
                try
                {
                   
                    _unitOfWork.Borrower.Update(borrowerMapper);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                  throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(borrower);
        }

        // GET: Borrowers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _unitOfWork.Borrower == null)
            {
                return NotFound();
            }

            var borrower = _unitOfWork.Borrower.GetById((Guid)id);
            if (borrower == null)
            {
                return NotFound();
            }

            return View(borrower);
        }

        // POST: Borrowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_unitOfWork.Borrower == null)
            {
                return Problem("Entity set 'MyDbContext.Borrowers'  is null.");
            }
            var borrower = _unitOfWork.Borrower.GetById((Guid)id);
            if (borrower != null)
            {
                _unitOfWork.Borrower.Remove(borrower);
            }

            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
