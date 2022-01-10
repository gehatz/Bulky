using Bulky.DataAccess;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Web.Controllers;

[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<CoverType> coverType = _unitOfWork.CoverType.GetAll();
        return View(coverType);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType coverType)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(coverType);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return View(coverType);
        }

    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (coverType == null)
        {
            return NotFound();
        }

        return View(coverType);
    }

    [HttpPost]
    public IActionResult Edit(CoverType coverType)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(coverType);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type updated successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return View(coverType);
        }

    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (coverType == null)
        {
            return NotFound();
        }

        return View(coverType);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteCoverType(int id)
    {
        var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        if (coverType == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(coverType);
        _unitOfWork.Save();
        TempData["success"] = "Cover Type deleted successfully";
        return RedirectToAction("Index");
    }
}

