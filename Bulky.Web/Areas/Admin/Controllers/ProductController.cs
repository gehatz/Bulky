using Bulky.DataAccess;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Web.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
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
        ProductViewModel vm = new()
        {
            Product = new(),
            Categories = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypes = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            }),
        };

        if (id == null || id == 0)
        {
            // Create product
            //return View(vm);
        } else
        {
            // Update product
            vm.Product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id);
            //return View(vm);
        }

        return View(vm);
    }

    [HttpPost]
    public IActionResult Upsert(ProductViewModel productVM, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                if(productVM.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }

                using(var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                
                productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
            }

            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return View(productVM);
        }

    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productsList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new { data = productsList });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (product == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(product);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Producte delted successfully" });
    }
    #endregion
}

