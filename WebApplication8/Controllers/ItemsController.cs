
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;
using WebApplication8.Services;


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
        public async  Task<IActionResult> Index()
        {
            var item = await _unitOfWork.Item.GetAll();

            //var item =  await _unitOfWork.Item.GetAll();
            ////!string.IsNullOrEmpty(name) ? await _unitOfWork.Item.FindAll(item => item.Title.Contains(name) || item.Author.Contains(name)) :
            //return item != null ?
            //            View(_mapper.Map<IEnumerable<ItemViewModel>>(item)) :
            //            Problem("Entity set 'MyDbContext.Items'  is null.");
            return View(_mapper.Map<IEnumerable<ItemViewModel>>(item));         
        }
      
        public async Task<ActionResult> Delete(int id)
        {
            Console.WriteLine(id);
            var myData = await _unitOfWork.Item.GetById(id);
            if (myData == null) return BadRequest();


            return PartialView("_RemoveItemPartial", _mapper.Map<ItemViewModel>(myData));
        }

        //GET: Items/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _unitOfWork.Item.GetById((int)id);
            
            if (item == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ItemViewModel>(item));
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
        public async Task<IActionResult> Create( ItemVMCreate item)
        {
            //var uploadResult =await _cloudinaryService.UploadImage(item.FileUpload);

            //var imageUrl =await  _cloudinaryService.GetImageUrl(uploadResult.PublicId, width: 500, height: 500);
            //Console.WriteLine(imageUrl);
            var itemMapper = _mapper.Map<Item>(item);
            if (ModelState.IsValid)
            {
                    // itemMapper.ImageUrl = imageUrl;
                 await _unitOfWork.Item.Add(itemMapper);
                 await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
            return View(_mapper.Map<ItemViewModel>(item));
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

            return View(_mapper.Map<ItemViewModel>(item));
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Title,PublicationDate,Author,Price,NumberOfPages,RunTime,Category")] ItemVMCreate item)
        {
            if (ModelState.IsValid)
            {
                var itemMapper = _mapper.Map<Item>(item);
                itemMapper.ItemId = id;
                try
                {
                  await _unitOfWork.Item.Update(itemMapper);
                  await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound();                     
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //// GET: Items/Delete/5
 
       
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(ItemViewModel id)
        {
         
            if (_unitOfWork.Item == null)
            {
                return Problem("Entity set 'MyDbContext.Items'  is null.");
            }
           
            var delete = await _unitOfWork.Item.Remove(id.ItemId);
            if (!delete) Problem("Cannot delete");
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
          

        }    
    }
}
