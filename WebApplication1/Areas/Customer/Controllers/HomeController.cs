using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository.IRepository;

namespace WebApplication1.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.productRepository.GetAll();
            return View(productList);
        }
    }
}
