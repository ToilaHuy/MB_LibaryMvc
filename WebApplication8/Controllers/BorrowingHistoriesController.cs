
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
        public IActionResult Index()
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
            if (borrowingHistory.ItemId == null)
            {
                return RedirectToAction(nameof(Create));
            }
            var borrowingHistoryConvert= _mapper.Map<BorrowingCreate,BorrowingHistory>(borrowingHistory);
            if (ModelState.IsValid)
            {
                borrowingHistoryConvert.BorrowingHistoryId = Guid.NewGuid();
                borrowingHistoryConvert.BorrowDate = DateTime.Now;
                var borrowingHistoryId = borrowingHistoryConvert.BorrowingHistoryId;
                _unitOfWork.BorrowingHistory.Add(borrowingHistoryConvert);
                var historyDetail = new HistoryDetail();
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
            return View(borrowingHistory);
        }

        public IActionResult Delete()
        {
            return View();
        }
        public  async Task<IActionResult> DeleteAccept()
        {
            Console.WriteLine("dusoethign");
                _unitOfWork.BorrowingHistory.DeleteAllRecordHistory();
               await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
      

     
    }
}
