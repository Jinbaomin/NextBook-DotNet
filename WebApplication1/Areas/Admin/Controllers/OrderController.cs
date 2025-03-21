using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;
using WebApplication1.Utility;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }   
        public IActionResult Index(String? status)
        {
            //List<OrderHeader> orderHeaders = _unitOfWork.orderHeaderRepository.GetAll(includeProperties: "ApplicationUser").ToList();
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.orderHeaderRepository.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderHeaders = _unitOfWork.orderHeaderRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser").ToList();
            }

            if (!String.IsNullOrEmpty(status))
            {
                switch (status)
                {
                    case "pending":
                        orderHeaders = orderHeaders.Where(o => o.OrderStatus == SD.StatusPending);
                        break;
                    case "inprocess":
                        orderHeaders = orderHeaders.Where(o => o.OrderStatus == SD.StatusInProcess);
                        break;
                    case "approved":
                        orderHeaders = orderHeaders.Where(o => o.OrderStatus == SD.StatusApproved);
                        break;
                    case "completed":
                        orderHeaders = orderHeaders.Where(o => o.OrderStatus == SD.StatusShipped);
                        break;
                    default:
                        break;
                }
            }

            return View(orderHeaders);
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.orderHeaderRepository.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.orderDetailRepository.GetAll(o => o.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.orderHeaderRepository.Get(u => u.Id == OrderVM.OrderHeader.Id);

            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.PhoneNunber = OrderVM.OrderHeader.PhoneNunber;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;

            _unitOfWork.orderHeaderRepository.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["success"] = "Update order header succesfully";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeaderRepository.Get(u => u.Id == orderId);
            orderHeader.OrderStatus = SD.StatusInProcess;
            _unitOfWork.Save();

            TempData["success"] = "Update order status successfully";
            return RedirectToAction(nameof(Details), new { orderId = orderId });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeaderRepository.Get(u => u.Id == orderId);
            orderHeader.OrderStatus = SD.StatusShipped;
            _unitOfWork.Save();

            TempData["success"] = "Update order status successfully";
            return RedirectToAction(nameof(Details), new { orderId = orderId });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder(int orderId)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeaderRepository.Get(u => u.Id == orderId);
            orderHeader.OrderStatus = SD.StatusCancelled;
            _unitOfWork.Save();

            TempData["success"] = "Update order status successfully";
            return RedirectToAction(nameof(Details), new { orderId = orderId });
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> orderHeaders = _unitOfWork.orderHeaderRepository.GetAll(includeProperties: "ApplicationUser").ToList();
            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
