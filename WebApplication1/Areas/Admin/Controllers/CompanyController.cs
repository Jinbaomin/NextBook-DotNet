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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> listCompany = _unitOfWork.companyRepository.GetAll().ToList();
            return View(listCompany);
        }

        // HTTP - GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.companyRepository.Add(company);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        // HTTP - GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id < 0)
            {
                return NotFound();
            }

            Company companyObj = _unitOfWork.companyRepository.Get(o => o.Id == id);

            if (companyObj == null)
            {
                return NotFound();
            }

            return View(companyObj);
        }

        // HTTP - POST
        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.companyRepository.Update(obj);
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

            Company objCompany = _unitOfWork.companyRepository.Get(o => o.Id == id);

            if (objCompany == null)
            {
                return NotFound();
            }

            _unitOfWork.companyRepository.Remove(objCompany);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
