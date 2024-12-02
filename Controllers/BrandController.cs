using Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Model;

namespace Ecommerece_website.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbcontext.Brand.ToList();

            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                string uploadDir = Path.Combine(webRootPath, "images", "brand");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string extension = Path.GetExtension(files[0].FileName);

                // Validate file extension (e.g., allow only specific types)
                if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" }.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError("", "Invalid file type. Only .jpg, .jpeg, .png, .gif, .svg are allowed.");
                    return View(brand);
                }

                string filePath = Path.Combine(uploadDir, newFileName + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                // Assign relative path for the web URL
                brand.BrandLogo = $"/images/brand/{newFileName}{extension}";
            }

            if (ModelState.IsValid)
            {
                _dbcontext.Brand.Add(brand);
                _dbcontext.SaveChanges();

                TempData["success"] = "Record Created Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbcontext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbcontext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                string uploadDir = Path.Combine(webRootPath, "images", "brand");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string extension = Path.GetExtension(files[0].FileName);

                // delete old image to edit
                var objfromDb = _dbcontext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (objfromDb.BrandLogo != null)
                {

                    var oldImagePath = Path.Combine(webRootPath, objfromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Validate file extension (e.g., allow only specific types)
                if (!new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" }.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError("", "Invalid file type. Only .jpg, .jpeg, .png, .gif, .svg are allowed.");
                    return View(brand);
                }

                string filePath = Path.Combine(uploadDir, newFileName + extension);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                // Assign relative path for the web URL
                brand.BrandLogo = $"/images/brand/{newFileName}{extension}";
            }


            if (ModelState.IsValid)
            {
                var objFromDb = _dbcontext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                objFromDb.Name = brand.Name;
                objFromDb.EstablishedYear = brand.EstablishedYear;

                if (brand.BrandLogo != null)
                {
                    objFromDb.BrandLogo = brand.BrandLogo;
                }


                _dbcontext.Brand.Update(objFromDb);
                _dbcontext.SaveChanges();

                //for toastr
                TempData["warning"] = "Record Updated Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbcontext.Brand.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpPost]

        public IActionResult Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            if (string.IsNullOrEmpty(brand.BrandLogo))
            {
                var objfromDb = _dbcontext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (objfromDb.BrandLogo != null)
                {

                    var oldImagePath = Path.Combine(webRootPath, objfromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }

            _dbcontext.Brand.Remove(brand);
            _dbcontext.SaveChanges();

            //for toastr
                TempData["Del"] = "Record Deleted";

            return RedirectToAction("Index");
        }

    }
}
