using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }   
        public IActionResult Index()
        {
            return View("Index");
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
