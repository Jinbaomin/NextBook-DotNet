﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger ,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.productRepository.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details(int? productId)
        {
            Product product = _unitOfWork.productRepository.Get(o => o.ID == productId, includeProperties: "Category");
            ShoppingCart cart = new()
            {
                Product = product,
                Count = 1,
                ProductId = product.ID
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cart = _unitOfWork.shoppingCartRepository.Get(o => o.ApplicationUser.Id == userId && o.ProductId == shoppingCart.ProductId);

            if(cart != null)
            {
                // shopping cart exists
                cart.Count += shoppingCart.Count;
                _unitOfWork.shoppingCartRepository.Update(cart);
            }
            else
            {
                // add cart to database
                _unitOfWork.shoppingCartRepository.Add(shoppingCart);
            }
            
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
