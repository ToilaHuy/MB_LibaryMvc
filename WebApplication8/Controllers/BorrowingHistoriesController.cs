
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            var myDbContext = await _unitOfWork.BorrowingHistory.GetAll();
            return myDbContext != null ?
                         View(myDbContext) :
                         Problem("Entity set 'MyDbContext.Items'  is null.");
        }

        //GET: BorrowingHistories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _unitOfWork.BorrowingHistory == null)
            {
                return NotFound();
            }
            var borrowingHistory =await _unitOfWork.BorrowingHistory.GetById((Guid)id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }
            return View(borrowingHistory);
        }

        //GET: BorrowingHistories/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new BorrowingHistoryView
            {
                Borrowers = await _unitOfWork.Borrower.GetAll(),
                Items =await _unitOfWork.Item.GetAll()
            };
            return View(viewModel);
        }

        //POST: BorrowingHistories/Create
     
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BorrowerId,ItemId,Quantity")] BorrowingCreate borrowingHistory)
        {
           
            if(borrowingHistory.ItemId == null) return RedirectToAction(nameof(Create));
            borrowingHistory.Quantity.RemoveAll(item => item == 0);
            var borrowingHistoryConvert= _mapper.Map<BorrowingHistory>(borrowingHistory);
            if (ModelState.IsValid)
            {
                borrowingHistoryConvert.BorrowDate = DateTime.Now;            
                borrowingHistoryConvert.HistoryDetails = new List<HistoryDetail>();
                for (int i = 0; i < borrowingHistory.ItemId.Count; i++)
                {
                    var historyDetail = new HistoryDetail(); 
                    var item =await _unitOfWork.Item.GetById(borrowingHistory.ItemId[i]);
                    historyDetail.ItemId = borrowingHistory.ItemId[i];
                    historyDetail.Quantity = borrowingHistory.Quantity[i];
                    historyDetail.Cost = item.Price * borrowingHistory.Quantity[i];
                    borrowingHistoryConvert.HistoryDetails.Add(historyDetail);
                }
                await _unitOfWork.BorrowingHistory.Add(borrowingHistoryConvert);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(borrowingHistory);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _unitOfWork.BorrowingHistory == null)
            {
                return NotFound();
            }
            var borroweingHistory =await _unitOfWork.BorrowingHistory.GetById((Guid)id);
            if (borroweingHistory == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_unitOfWork.BorrowingHistory == null)
            {
                return Problem("Entity set 'MyDbContext.Borrowers'  is null.");
            }
            var delete = await _unitOfWork.BorrowingHistory.Remove(id);
            if(!delete) Problem("Cannot delete");
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
     
     
    }
}
