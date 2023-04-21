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
    public class BorrowingHistoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public BorrowingHistoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: BorrowingHistories
        public async Task<IActionResult> Index()
        {
            var myDbContext = _unitOfWork.BorrowingHistory.GetAllBorrowingHistory();
            return myDbContext != null ?
                         View(myDbContext) :
                         Problem("Entity set 'MyDbContext.Items'  is null.");
        }

        //GET: BorrowingHistories/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null || _unitOfWork.BorrowingHistory == null)
            {
                return NotFound();
            }
            //_unitOfWork.BorrowingHistory.GetDefaultBorrowingHistory(id);
            var borrowingHistory = _unitOfWork.BorrowingHistory.GetDefaultBorrowingHistory(id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }
            return View(borrowingHistory);
        }

        //GET: BorrowingHistories/Create
        public IActionResult Create()
        {
            //ViewData["LibraryCardNumber"] = new SelectList(_unitOfWork.Borrower.GetAll(), "LibraryCardNumber", "BorrowerName");
            //ViewBag.Item = _unitOfWork.Item.GetAll();
            var viewModel = new BorrowingHistoryView
            {
                BorrowingHistories = _unitOfWork.BorrowingHistory.GetAll(),
                Borrowers = _unitOfWork.Borrower.GetAll(),
                Items = _unitOfWork.Item.GetAll()
            };

            return View(viewModel);
        }

        //POST: BorrowingHistories/Create
     
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowerId,ItemId")] BorrowingCreate borrowingHistory)
        {

            var borrowingHistoryConvert= _mapper.Map<BorrowingCreate,BorrowingHistory>(borrowingHistory);
            if (ModelState.IsValid)
            {
                borrowingHistoryConvert.BorrowingHistoryId = Guid.NewGuid();
                borrowingHistoryConvert.BorrowDate = DateTime.Now;
                var borrowingHistoryId = borrowingHistoryConvert.BorrowingHistoryId;
                var historyDetail = new HistoryDetail();
                _unitOfWork.BorrowingHistory.Add(borrowingHistoryConvert);
                await _unitOfWork.Save();
                for (int i = 0; i < borrowingHistory.ItemId.Count; i++)
                {
                   var item= _unitOfWork.Item.GetById(borrowingHistory.ItemId[i]);
                    historyDetail.HistoryDetailId = Guid.NewGuid();
                    historyDetail.ItemId = borrowingHistory.ItemId[i];
                    historyDetail.BorrowingHistoryId = borrowingHistoryId;
                    historyDetail.Quantity = 1;
                    historyDetail.Cost = item.Price;
                    _unitOfWork.HistoryDetail.Add(historyDetail);
                    await _unitOfWork.Save();
                }      
                return RedirectToAction(nameof(Index));
            }
            //ViewData["LibraryCardNumber"] = new SelectList(_context.Borrowers, "LibraryCardNumber", "BorrowerName", borrowingHistory.LibraryCardNumber);
            return View(borrowingHistory);
        }

        //    // GET: BorrowingHistories/Edit/5
        //    public async Task<IActionResult> Edit(Guid? id)
        //    {
        //        if (id == null || _context.BorrowingHistories == null)
        //        {
        //            return NotFound();
        //        }

        //        var borrowingHistory = await _context.BorrowingHistories.FindAsync(id);
        //        if (borrowingHistory == null)
        //        {
        //            return NotFound();
        //        }
        //        ViewData["LibraryCardNumber"] = new SelectList(_context.Borrowers, "LibraryCardNumber", "BorrowerName", borrowingHistory.LibraryCardNumber);
        //        return View(borrowingHistory);
        //    }

        //    // POST: BorrowingHistories/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(Guid id, [Bind("BorrowingHistoryId,BorrowDate,BorrowerLibraryCardNumber,LibraryCardNumber")] BorrowingHistory borrowingHistory)
        //    {
        //        if (id != borrowingHistory.BorrowingHistoryId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(borrowingHistory);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!BorrowingHistoryExists(borrowingHistory.BorrowingHistoryId))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["LibraryCardNumber"] = new SelectList(_context.Borrowers, "LibraryCardNumber", "BorrowerName", borrowingHistory.LibraryCardNumber);
        //        return View(borrowingHistory);
        //    }

        //    // GET: BorrowingHistories/Delete/5
        //    public async Task<IActionResult> Delete(Guid? id)
        //    {
        //        if (id == null || _context.BorrowingHistories == null)
        //        {
        //            return NotFound();
        //        }

        //        var borrowingHistory = await _context.BorrowingHistories
        //            .Include(b => b.Borrower)
        //            .FirstOrDefaultAsync(m => m.BorrowingHistoryId == id);
        //        if (borrowingHistory == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(borrowingHistory);
        //    }

        //    // POST: BorrowingHistories/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(Guid id)
        //    {
        //        if (_context.BorrowingHistories == null)
        //        {
        //            return Problem("Entity set 'MyDbContext.BorrowingHistories'  is null.");
        //        }
        //        var borrowingHistory = await _context.BorrowingHistories.FindAsync(id);
        //        if (borrowingHistory != null)
        //        {
        //            _context.BorrowingHistories.Remove(borrowingHistory);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool BorrowingHistoryExists(Guid id)
        //    {
        //      return (_context.BorrowingHistories?.Any(e => e.BorrowingHistoryId == id)).GetValueOrDefault();
        //    }
    }
}
