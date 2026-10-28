
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;
using WebApplication8.Services;
using X.PagedList;


namespace WebApplication8.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

       
        public ItemsController(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Items
        public async Task<IActionResult> Index(Item.CategoryName? category,  int? page, int pageSize = 10, string name = "")
        {
            ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
            var options = new List<SelectListItem>
            {
                new () { Value = "10", Text = "10" },
                new () { Value = "20", Text = "20" },
                new () { Value = "30", Text = "30" },
            
            };
       
            ViewBag.PageSizeOptions = options;
            ViewBag.nameSearch = name;
            ViewBag.categorySearch = category;

            var item =
                await _unitOfWork.Item.FindAll(i =>
                    (i.Author.Contains(name) || i.Title.Contains(name)) &&
                    (!category.HasValue || i.Category == category));

            var sortedItemsQuery = item.OrderByDescending(i => i.PublicationDate).Select(x => new
            {
                x.ItemId, x.Title, x.Author, x.Category, x.ImageUrl, x.Price
            });
            var pageNumber = page ?? 1;
            
            var onePageOfProducts = (sortedItemsQuery).ToPagedList(pageNumber, pageSize);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            return View();
        }


        //GET: Items/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var myData = await _unitOfWork.Item.GetById(id);
            if (myData == null) return BadRequest();

            return PartialView("_DetailItemPartial", _mapper.Map<ItemViewModel>(myData));

        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
            return PartialView("_CreateItemPartial");
        }

        //POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemVmCreate item)
        {
            if (!ModelState.IsValid) return BadRequest();
            var uploadResult = await _cloudinaryService.UploadImage(item.FileUpload);
            var imageUrl = await _cloudinaryService.GetImageUrl(uploadResult.PublicId, width: 500, height: 500);
            var itemMapper = _mapper.Map<Item>(item);
            itemMapper.AvailableQuantity = item.Quantity;
            itemMapper.ImageUrl = imageUrl;
            await _unitOfWork.Item.Add(itemMapper);
            await _unitOfWork.Save();

            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Create items success ";

            return RedirectToAction(nameof(Index));
            // ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
        }

        //// GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _unitOfWork.Item == null)
            {
                return NotFound();
            }

            var item = await _unitOfWork.Item.GetById((int)id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
            
            return PartialView("_EditItemPartial", _mapper.Map<ItemViewModel>(item));
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemViewUpdateModel item)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            var itemMapper = _mapper.Map<Item>(item);
            try
            {
                await _unitOfWork.Item.Update(itemMapper);
                await _unitOfWork.Save();
                TempData["MessageStatus"] = "true";
                TempData["MessageData"] = "Update items success";
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        //// GET: Items/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var myData = await _unitOfWork.Item.GetById(id);
            if (myData == null) return BadRequest();

            return PartialView("_RemoveItemPartial", _mapper.Map<ItemViewModel>(myData));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(ItemViewModel item)
        {
            if (_unitOfWork.Item == null)
            {
                return Problem("Entity set 'MyDbContext.Items'  is null.");
            }
            var myData = await _unitOfWork.Item.GetById(item.ItemId);
            if (myData.AvailableQuantity != myData.Quantity)
            {

                TempData["MessageStatus"] = "false";
                TempData["MessageData"] = "Cannot delete items. Please check history! ";
                return RedirectToAction(nameof(Index));

            }
            var delete = await _unitOfWork.Item.Remove(item.ItemId);
            if (!delete) Problem("Cannot delete");
            await _unitOfWork.Save();

            TempData["MessageStatus"] = "true";
            TempData["MessageData"] = "Delete items success ";

            return RedirectToAction(nameof(Index));
        }
    }
}
