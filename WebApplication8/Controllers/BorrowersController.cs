using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;
using WebApplication8.Services;
using X.PagedList;

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
        public async Task<IActionResult> Index(int? page, int pageSize = 10, string name = "")
        {

            var options = new List<SelectListItem>
            {
                new () { Value = "10", Text = "10" },
                new () { Value = "20", Text = "20" },
                new () { Value = "30", Text = "30" },
            };
            ViewBag.PageSizeOptions = options;
            ViewBag.nameSearch = name;
            var borrower = await _unitOfWork.Borrower.FindAll(b => b.BorrowerName.Contains(name));
            var sortedBorrowersQuery = borrower.OrderByDescending(i => i.BorrowerName);

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfBorrowers = (sortedBorrowersQuery).ToPagedList(pageNumber, pageSize);
            ViewBag.borrowerCount = onePageOfBorrowers.TotalItemCount;

            ViewBag.onePageOfBorrowers = onePageOfBorrowers;
            return View();
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

            return PartialView("_DetailBorrowerPartial", _mapper.Map<BorrowerVm>(borrower));
        }

        // GET: Borrowers/Create
        public IActionResult Create()
        {
            return PartialView("_CreateBorrowerPartial");
        }

        // POST: Borrowers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowerVm borrower)
        {
            var borrowerMapper = _mapper.Map<Borrower>(borrower);
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            var uploadResult = await _cloudinaryService.UploadImage(borrower.FileUpload);
            var imageUrl = await _cloudinaryService.GetImageUrl(uploadResult.PublicId, width: 500, height: 500);
            borrowerMapper.ImageUrl = imageUrl;
            await _unitOfWork.Borrower.Add(borrowerMapper);
            await _unitOfWork.Save();
            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Create borrowers success ";
            return RedirectToAction(nameof(Index));
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
            return PartialView("_EditBorrowerPartial", _mapper.Map<BorrowerVm>(borrower));

        }
        // POST: Borrowers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BorrowerUpdate borrower)
        {
            if (!ModelState.IsValid)
            {
                TempData["MessageStatus"] = "false";
                TempData["MessageData"] = "Cannot update borrowers ";
                return RedirectToAction(nameof(Index));
            }
            var borrowerMapper = _mapper.Map<Borrower>(borrower);
            await _unitOfWork.Borrower.Update(borrowerMapper);
            await _unitOfWork.Save();
            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Update borrowers success ";
            return RedirectToAction(nameof(Index));
        }

        // GET: Borrowers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return PartialView("_DeleteBorrowerPartial", _mapper.Map<BorrowerVm>(borrower));

        }

        // POST: Borrowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(BorrowerVm borrower)
        {
            if (_unitOfWork.Borrower == null)
            {
                return Problem("Entity set 'MyDbContext.Borrowers'  is null.");
            }

            var checkBorrow = await _unitOfWork.BorrowingHistory.FindAll(x => x.BorrowerId == borrower.BorrowerId && x.Status == true);
            if (checkBorrow != null && checkBorrow.Any())
            {
                TempData["MessageStatus"] = "false";
                TempData["MessageData"] = "Cannot delete Borrowers. Please check history!";
                return RedirectToAction(nameof(Index));

            }
            var delete = borrower != null && await _unitOfWork.Borrower.Remove(borrower.BorrowerId);
            if (!delete) Problem("Cannot delete");
            await _unitOfWork.Save();
            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "delete borrowers success ";
            return RedirectToAction(nameof(Index));
        }
    }
}
