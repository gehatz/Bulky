using Bulky.DataAccess;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Web.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        //IEnumerable<Product> product = _unitOfWork.Product.GetAll();
        return View();
    }

    [HttpGet]
    public IActionResult Upsert(int? id)
    {
        Company company = new();

        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(i => i.Id == id);
            return View(company);
        }

    }

    [HttpPost]
    public IActionResult Upsert(Company company)
    {
        if (ModelState.IsValid)
        {
            if (company.Id == 0)
            {
                _unitOfWork.Company.Add(company);
                TempData["success"] = "Company created successfully";
            }
            else
            {
                _unitOfWork.Company.Update(company);
                TempData["success"] = "Company updated successfully";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        else
        {
            return View(company);
        }

    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companies = _unitOfWork.Company.GetAll();
        return Json(new { data = companies });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        if (company == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        _unitOfWork.Company.Remove(company);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Company deleted successfully" });
    }
    #endregion
}

