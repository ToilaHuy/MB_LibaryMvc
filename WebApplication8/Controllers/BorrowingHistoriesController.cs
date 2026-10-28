
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebApplication8.Data;

using WebApplication8.Models;
using WebApplication8.Repositories;
using X.PagedList;

namespace WebApplication8.Controllers
{
    public class BorrowingHistoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public const string ITEMKEY = "listItem";

        public BorrowingHistoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int? page, bool? status, DateTime? date,int pageSize=10, string search = "")
        {

            var options = new List<SelectListItem>()
            {
                new() { Text = "Open", Value = "true" },
                new() { Text = "Closed", Value = "false" }
            };
            ViewData["Options"] = options;

            var optionsPageSize = new List<SelectListItem>
            {
                new () { Value = "10", Text = "10" },
                new () { Value = "20", Text = "20" },
                new () { Value = "30", Text = "30" },

            };

            ViewBag.PageSizeOptions = optionsPageSize;
          
            ViewBag.status = status;
            ViewBag.date = date;
            ViewBag.search = search;
            var myDbContext = await _unitOfWork.BorrowingHistory.GetByBorrowingHistory(status, date, search);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfHistory = (myDbContext).ToPagedList(pageNumber, pageSize);
            ViewBag.onePageOfHistory = onePageOfHistory;
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitOfWork.BorrowingHistory == null)
            {
                return NotFound();
            }
            var borrowingHistory = await _unitOfWork.BorrowingHistory.GetById((int)id);
            if (borrowingHistory == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<BorrowingHistoryVm>(borrowingHistory));
        }
        public async Task<IActionResult> Create(int? page, string name = "")
        {
            var listItems = await _unitOfWork.Item.FindAll(i =>
                (i.Title.Contains(name) || i.Author.Contains(name)) && i.AvailableQuantity > 0);
            var sortedItemsQuery = listItems.OrderByDescending(i => i.PublicationDate);

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfItems = sortedItemsQuery.ToPagedList(pageNumber, 7);
            ViewBag.nameSearch = name;
            ViewBag.onePageOfItems = onePageOfItems;
            ViewBag.itemCount = onePageOfItems.TotalItemCount;
            var viewModel = new BorrowingHistoryVmCreate
            {
                Borrowers = _mapper.Map<IEnumerable<BorrowerVm>>(await _unitOfWork.Borrower.GetAll()),
                Items = _mapper.Map<IEnumerable<ItemViewModel>>(onePageOfItems)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowingHistoryCreate borrowingHistory)
        {
            var borrowingHistoryConvert = _mapper.Map<BorrowingHistory>(borrowingHistory);
            if (!ModelState.IsValid) return View();
            var listItem = GetListItem();

            if (listItem != null)
                for (var i = 0; i < listItem.ToList().Count; i++)
                {
                    var item = await _unitOfWork.Item.GetById(listItem[i].ItemId);
                    item.AvailableQuantity -= listItem[i].Quantity;
                    await _unitOfWork.Item.Update(item);
                    listItem[i].Item = null;
                    borrowingHistoryConvert.Quantity += listItem[i].Quantity;
                    borrowingHistoryConvert.Cost += listItem[i].Cost;
                }
            borrowingHistoryConvert.HistoryDetails = _mapper.Map<ICollection<HistoryDetail>>(listItem);
            await _unitOfWork.BorrowingHistory.Add(borrowingHistoryConvert);
            await _unitOfWork.Save();
            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Borrow items success";
            ClearListItem();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitOfWork.BorrowingHistory == null)
            {
                return NotFound();
            }
            var borrowingHistory = await _unitOfWork.BorrowingHistory.GetById((int)id);
            return PartialView("_DeleteHistoryPartial", _mapper.Map<BorrowingHistoryVm>(borrowingHistory));

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowingHistory = await _unitOfWork.BorrowingHistory.GetById(id);
            if (borrowingHistory.Status)
            {
                for (var i = 0; i < borrowingHistory.HistoryDetails.Count; i++)
                {
                    var historyDetail = borrowingHistory.HistoryDetails.ToList()[i];
                    if (!historyDetail.Status) continue;
                    var item = await _unitOfWork.Item.GetById(historyDetail.ItemId);
                    item.AvailableQuantity += historyDetail.Quantity;
                    await _unitOfWork.Item.Update(item);
                }
            }

            var delete = await _unitOfWork.BorrowingHistory.Remove(id);
            if (!delete) Problem("Cannot delete");
            await _unitOfWork.Save();
            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Delete history success";
            return RedirectToAction(nameof(Index));
        }

        #region Borrow

        public IActionResult GetItemChoose()
        {
            return Ok(GetListItem());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemToList( int itemId)
        {
            var item = await _unitOfWork.Item.GetById(itemId);

            if (item == null)
                return NotFound("Not found item");
            var listItemBorrow = GetListItem();
            var itemBorrow = listItemBorrow.Find(p => p.ItemId == itemId);
            if (itemBorrow != null)
            {
                if (item.AvailableQuantity > itemBorrow.Quantity)
                {
                    itemBorrow.Quantity++;
                    itemBorrow.Cost = itemBorrow.Quantity * item.Price;
                }
            }
            else
            {
                listItemBorrow.Add(new HistoryDetailViewModel() { Quantity = 1, Item = _mapper.Map<ItemViewModel>(item), ItemId = itemId, Cost = item.Price, Status = true });
            }
            SaveListItemSession(listItemBorrow);
            return Ok(GetListItem());
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]

        public IActionResult RemoveItemBorrow( int itemId)
        {
            var listItemBorrow = GetListItem();
            var itemBorrow = listItemBorrow.Find(p => p.ItemId == itemId);
            if (itemBorrow != null)
            {
                listItemBorrow.Remove(itemBorrow);
            }
            SaveListItemSession(listItemBorrow);
            return Ok(GetListItem());
        }
        [HttpPut]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateItemChoose( int itemId, int quantity)
        {
            var listItem = GetListItem();
            var itemBorrow = listItem.Find(p => p.ItemId == itemId);
            if (itemBorrow != null)
            {
                itemBorrow.Quantity = quantity;
                itemBorrow.Cost = itemBorrow.Quantity * itemBorrow.Item.Price;
            }
            SaveListItemSession(listItem);

            return Ok(GetListItem());
        }
      
        public List<HistoryDetailViewModel> GetListItem()
        {

            var session = HttpContext.Session;
            var jsonItem = session.GetString(ITEMKEY);
            if (jsonItem == null) return new List<HistoryDetailViewModel>();
            var listItem = JsonConvert.DeserializeObject<List<HistoryDetailViewModel>>(jsonItem);
            return listItem;
        }
        private void ClearListItem()
        {
            var session = HttpContext.Session;
            session.Remove(ITEMKEY);
        }
        private void SaveListItemSession(List<HistoryDetailViewModel> ls)
        {
            var session = HttpContext.Session;
            var jsonItem = JsonConvert.SerializeObject(ls);
            session.SetString(ITEMKEY, jsonItem);
        }
        #endregion

        #region ReturnItem

        public IActionResult ReturnItemPartial(int idBorrowingHistory, int? idHistoryDetails)
        {
            ViewBag.idBorrowingHistory = idBorrowingHistory;
            ViewBag.idHistoryDetails = idHistoryDetails;
            return PartialView("_ReturnHistoryPartial");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnItem(int idBorrowingHistory, int? idHistoryDetails)
        {
            var borrowingHistory = await _unitOfWork.BorrowingHistory.GetById(idBorrowingHistory);
            if (idHistoryDetails != null)
            {
                var historyDetail = borrowingHistory.HistoryDetails.ToList()
                    .Find(x => x.HistoryDetailId == idHistoryDetails);
                historyDetail.Status = false;
                var item = await _unitOfWork.Item.GetById(historyDetail.ItemId);
                item.AvailableQuantity += historyDetail.Quantity;
                await _unitOfWork.Item.Update(item);
                var findHistoryStatus = borrowingHistory.HistoryDetails.ToList().Exists(x => x.Status);
                if (!findHistoryStatus) borrowingHistory.Status = false;
            }
            else
            {
                for (var i = 0; i < borrowingHistory.HistoryDetails.Count; i++)
                {

                    var historyDetail = borrowingHistory.HistoryDetails.ToList()[i];
                    if (!historyDetail.Status) continue;
                    var item = await _unitOfWork.Item.GetById(historyDetail.ItemId);
                    item.AvailableQuantity += historyDetail.Quantity;
                    await _unitOfWork.Item.Update(item);
                    historyDetail.Status = false;

                }
                borrowingHistory.Status = false;
            }

            await _unitOfWork.BorrowingHistory.Update(borrowingHistory);
            await _unitOfWork.Save();
            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Return items success";
            return RedirectToAction("Details", new { id = idBorrowingHistory });

        }

        #endregion
    }
}
