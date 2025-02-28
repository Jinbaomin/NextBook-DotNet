using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository.IRepository;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Utility;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> listProduct = _unitOfWork.productRepository.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.categoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.ID.ToString()
            });
            return View(listProduct);
        }

        // HTTP - GET
        public IActionResult Create()
        {
            List<Product> listProduct = _unitOfWork.productRepository.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.categoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.ID.ToString()
            });

            //ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = CategoryList
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath + @"/images/product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"/images/product/" + fileName;
                }
                _unitOfWork.productRepository.Add(productVM.Product);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        // HTTP - GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }

            Product ProductObj = _unitOfWork.productRepository.Get(o => o.ID == id);

            if (ProductObj == null)
            {
                return NotFound();
            }

            return View(ProductObj);
        }

        // HTTP - POST
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.productRepository.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View();
        }

        // HTTP - GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }
            Product objProduct = _unitOfWork.productRepository.Get(o => o.ID == id);
            if (objProduct == null)
            {
                return NotFound();
            }

            _unitOfWork.productRepository.Remove(objProduct);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
