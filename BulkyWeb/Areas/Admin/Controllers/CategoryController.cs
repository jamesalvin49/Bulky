using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitofWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                // Inorder to add a custom validation
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            }
            if (category.Name.ToLower() == "test")
            {
                // Inorder to add a custom validation WITH NO KEY
                ModelState.AddModelError("", "Test is an invalid value for Name.");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Add(category);
                _unitofWork.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            //Category? category = context.Categories.Find(categoryId);
            //Category? category = context.Categories.Where(c => c.Id == categoryId).FirstOrDefault(); 
            Category? category = _unitofWork.Category.Get(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Update(category);
                _unitofWork.Save();
                TempData["success"] = "Category updated successfully.";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            //Category? category = context.Categories.Find(categoryId);
            //Category? category = context.Categories.Where(c => c.Id == categoryId).FirstOrDefault(); 
            Category? category = _unitofWork.Category.Get(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? categoryId)
        {
            Category? category = _unitofWork.Category.Get(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }
            _unitofWork.Category.Remove(category);
            _unitofWork.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index", "Category");
        }
    }
}
