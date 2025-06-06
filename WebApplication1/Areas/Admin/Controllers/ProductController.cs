﻿using Microsoft.AspNetCore.Mvc;
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
        [BindProperty]
        public Product product { get; set; }

        //[BindProperty(SupportsGet = true)]
        //public string searchTerm { get; set; }
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public string handleUploadFile(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath + @"/images/product");

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return @"/images/product/" + fileName;
        }

        public IActionResult Index(String? searchTerm)
        {
            List<Product> listProduct;

            if (String.IsNullOrEmpty(searchTerm))
            {
                listProduct = _unitOfWork.productRepository.GetAll().ToList();
            }
            else
            {
                listProduct = _unitOfWork.productRepository.GetAll(
                    product => product.Title.ToLower().Contains(searchTerm.ToLower()) || 
                    product.Author.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.categoryRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.ID.ToString()
            });

            ProductSearch productSearch = new()
            {
                listProduct = listProduct,
                searchTerm = ""
            };

            return View(productSearch);
        }

        [HttpPost]
        public IActionResult Index(ProductSearch productSearch)
        {
            return RedirectToAction(nameof(Index), new { searchTerm = productSearch.searchTerm });
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
                if(file != null)
                {
                    //string wwwRootPath = _webHostEnvironment.WebRootPath;
                    //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //string productPath = Path.Combine(wwwRootPath + @"/images/product");

                    //using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    //{
                    //    file.CopyTo(fileStream);
                    //}

                    productVM.Product.ImageUrl = handleUploadFile(file);
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

            product = _unitOfWork.productRepository.Get(o => o.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            ProductVM productVM = new()
            {
                Product = product,
                CategoryList = _unitOfWork.categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                })
            };

            
            return View(productVM);
        }

        // HTTP - POST
        [HttpPost]
        public IActionResult Edit(IFormFile? file)
        {
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // Delete the existing image
                    //string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                    //if (System.IO.File.Exists(oldImagePath))
                    //{
                    //    System.IO.File.Delete(oldImagePath);
                    //}

                    // Assign a new image path
                    product.ImageUrl = handleUploadFile(file);
                }

                _unitOfWork.productRepository.Update(product);
                _unitOfWork.Save();

                TempData["success"] = "Update product successfully";
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
