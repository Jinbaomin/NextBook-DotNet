using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;
using WebApplication1.Utility;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM cart = new()
            {
                shoppingCartList = _unitOfWork.shoppingCartRepository.GetAll(o => o.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach(var item in cart.shoppingCartList)
            {
                item.Total = getPriceBasedOnCount(item);
                cart.OrderHeader.OrderTotal += item.Count * item.Total;
            }

            return View(cart);
        }

        public IActionResult Plus(int cartId)
        {
            ShoppingCart cart = _unitOfWork.shoppingCartRepository.Get(o => o.Id == cartId);

            cart.Count += 1;
            _unitOfWork.shoppingCartRepository.Update(cart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            ShoppingCart cart = _unitOfWork.shoppingCartRepository.Get(o => o.Id == cartId);

            if(cart.Count > 1)
            {
                cart.Count -= 1;
                _unitOfWork.shoppingCartRepository.Update(cart);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.shoppingCartRepository.Remove(cart);
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.shoppingCartRepository.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count() - 1);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            ShoppingCart cart = _unitOfWork.shoppingCartRepository.Get(o => o.Id == cartId);
            HttpContext.Session.SetInt32(SD.SessionCart, 
                _unitOfWork.shoppingCartRepository.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count() - 1);
            _unitOfWork.shoppingCartRepository.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCart = new()
            {
                shoppingCartList = _unitOfWork.shoppingCartRepository.GetAll(o => o.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            shoppingCart.OrderHeader.ApplicationUser = _unitOfWork.applicationUserRepository.Get(o => o.Id == userId);


            shoppingCart.OrderHeader.StreetAddress = shoppingCart.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCart.OrderHeader.PhoneNunber = shoppingCart.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCart.OrderHeader.City = shoppingCart.OrderHeader.ApplicationUser.City;
            shoppingCart.OrderHeader.State = shoppingCart.OrderHeader.ApplicationUser.State;
            shoppingCart.OrderHeader.PostalCode = shoppingCart.OrderHeader.ApplicationUser.PostalCode;
            shoppingCart.OrderHeader.Name = shoppingCart.OrderHeader.ApplicationUser.Name;

            foreach (var item in shoppingCart.shoppingCartList)
            {
                item.Total = getPriceBasedOnCount(item);
                shoppingCart.OrderHeader.OrderTotal += item.Count * item.Total;
            }

            return View(shoppingCart);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCart = new()
            {
                shoppingCartList = _unitOfWork.shoppingCartRepository.GetAll(o => o.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            shoppingCart.OrderHeader.ApplicationUser = _unitOfWork.applicationUserRepository.Get(o => o.Id == userId);
            shoppingCart.OrderHeader.ApplicationUserId = userId;
            shoppingCart.OrderHeader.OrderDate = DateTime.Now;
            shoppingCart.OrderHeader.OrderStatus = SD.StatusPending;

            shoppingCart.OrderHeader.StreetAddress = shoppingCart.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCart.OrderHeader.PhoneNunber = shoppingCart.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCart.OrderHeader.City = shoppingCart.OrderHeader.ApplicationUser.City;
            shoppingCart.OrderHeader.State = shoppingCart.OrderHeader.ApplicationUser.State;
            shoppingCart.OrderHeader.PostalCode = shoppingCart.OrderHeader.ApplicationUser.PostalCode;
            shoppingCart.OrderHeader.Name = shoppingCart.OrderHeader.ApplicationUser.Name;

            foreach (var item in shoppingCart.shoppingCartList)
            {
                item.Total = getPriceBasedOnCount(item);
                shoppingCart.OrderHeader.OrderTotal += item.Count * item.Total;
            }

            _unitOfWork.orderHeaderRepository.Add(shoppingCart.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in shoppingCart.shoppingCartList)
            {
                OrderDetail orderDetails = new()
                {
                    productId = item.ProductId,
                    OrderHeaderId = shoppingCart.OrderHeader.Id,
                    Price = item.Total,
                    Count = item.Count
                };

                _unitOfWork.orderDetailRepository.Add(orderDetails);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(OrderConfirmation), new {userId = shoppingCart.OrderHeader.ApplicationUserId, orderId = shoppingCart.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(string userId, int orderId)
        {
            List<ShoppingCart> carts = _unitOfWork.shoppingCartRepository.GetAll(o => o.ApplicationUserId == userId).ToList();
            _unitOfWork.shoppingCartRepository.DeleteRange(carts);
            _unitOfWork.Save();
            return View(orderId);
        }

        public double getPriceBasedOnCount(ShoppingCart shoppingCart)
        {
            int count = shoppingCart.Count;
            return (count < 50 ? shoppingCart.Product.Price : (count < 100 ? shoppingCart.Product.Price50 : shoppingCart.Product.Price100));
        }
    }
}
