using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        //public CategoryController(ApplicationDbContext db)
        //{
        //    _db = db;
        //}

        //private readonly ICategoryRepository _categoryRepo;
        //public CategoryController(ICategoryRepository db)
        //{
        //    _categoryRepo = db;
        //}

        private readonly IUnitOfWork _unitWork;
        public CategoryController(IUnitOfWork unitWork)
        {
            _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            //List<Category> objCategoryList = _db.Categories.ToList();

            //List<Category> objCategoryList = _categoryRepo.GetAll().ToList();

            List<Category> objCategoryList = _unitWork.categoryRepository.GetAll().ToList();
            return View(objCategoryList);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST-CREATE
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj);
                //_db.SaveChanges();

                //_categoryRepo.Add(obj);
                //_categoryRepo.Save();

                _unitWork.categoryRepository.Add(obj);
                _unitWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }
            //Category objCategory1 = _db.Categories.Find(id);
            //Category objCategory2 = _db.Categories.FirstOrDefault(u => u.ID ==id);
            //Category objCategory3 = _db.Categories.Where(u => u.ID ==id).FirstOrDefault();

            //Category objCategory1 = _categoryRepo.Get(o => o.ID == id);
            Category objCategory1 = _unitWork.categoryRepository.Get(o => o.ID == id);
            if (objCategory1 == null)
            {
                return NotFound();
            }
            return View(objCategory1);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(obj);
                //_db.SaveChanges();

                //_categoryRepo.Update(obj);
                //_categoryRepo.Save();

                _unitWork.categoryRepository.Update(obj);
                _unitWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }
            //Category objCategory1 = _db.Categories.Find(id);

            //Category objCategory1 = _categoryRepo.Get(o => o.ID == id);

            Category objCategory1 = _unitWork.categoryRepository.Get(o => o.ID == id);

            if (objCategory1 == null)
            {
                return NotFound();
            }

            //_categoryRepo.Remove(objCategory1);
            //_categoryRepo.Save();

            _unitWork.categoryRepository.Remove(objCategory1);
            _unitWork.Save();

            return RedirectToAction("Index");
        }
    }
}
