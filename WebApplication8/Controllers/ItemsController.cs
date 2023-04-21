using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication8.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ItemsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            _mapper = mapper;
        }

        // GET: Items
        public IActionResult Index(string name)
        {
            var item =  _unitOfWork.Item.GetAll();
            if (!string.IsNullOrEmpty(name))
            {
                item = _unitOfWork.Item.FindAll( item => item.Title.Contains(name) || item.Author.Contains(name));
            }
            return item != null ?
                        View(item) :
                        Problem("Entity set 'MyDbContext.Items'  is null.");
            
        }

        //GET: Items/Details/5
        
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _unitOfWork.Item.GetById((Guid)id);
            ViewBag.Category = item.Category;

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
            return View();
        }

        //POST: Items/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,Title,PublicationDate,Author,Price,NumberOfPages,RunTime,Category")] ItemModel item)
        {
            var itemMapper = _mapper.Map<Item>(item);
            if (ModelState.IsValid)
            {
                itemMapper.ItemId = new Guid();
                 _unitOfWork.Item.Add(itemMapper);
                 await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category"] = new SelectList(Enum.GetValues(typeof(Item.CategoryName)));
            return View(item);
        }

        //// GET: Items/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            Console.WriteLine(_unitOfWork.Item);
            if (id == null || _unitOfWork.Item == null)
            {
                return NotFound();
            }

            var item =  _unitOfWork.Item.GetById((Guid)id);
            if (item == null)
            {
                return NotFound();
            }
            ViewBag.Category = item.Category;
            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ItemId,Title,PublicationDate,Author,Price,NumberOfPages,RunTime,Category")] ItemModel item)
        {
            if (ModelState.IsValid)
            {
                var itemMapper = _mapper.Map<Item>(item);
                itemMapper.ItemId = id;
                try
                {
                     _unitOfWork.Item.Update(itemMapper);
                  await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound();                     
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        //// GET: Items/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _unitOfWork.Item == null)
            {
                return NotFound();
            }

            var item = _unitOfWork.Item.GetById((Guid)id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_unitOfWork.Item == null)
            {
                return Problem("Entity set 'MyDbContext.Items'  is null.");
            }
            var item =  _unitOfWork.Item.GetById(id);
            if (item != null)
            {
                _unitOfWork.Item.Remove(item);
            }

            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        //private bool ItemExists(Guid id)
        //{
        //    return (_dbContext.Items?.Any(e => e.ItemId == id)).GetValueOrDefault();
        //}
    }
}
