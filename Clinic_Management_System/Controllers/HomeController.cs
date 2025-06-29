using Clinic_Management_System.Data;
using Clinic_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace Clinic_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClinicContext db;
        private readonly IHttpContextAccessor contx;
        public HomeController(ILogger<HomeController> logger, ClinicContext db, IHttpContextAccessor contx)
        {
            _logger = logger;
            this.db = db;
            this.contx = contx;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User model)
        {
            // Default role is User (assume RoleId = 2 for User)
            model.RoleId = 2; // user

            db.Users.Add(model);
            db.SaveChanges();

            return RedirectToAction("Login");

            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User data)
        {
            if (ModelState.IsValid == false)
            {
                var uservalidity = db.Users.Where(user => user.Email == data.Email && user.Password == data.Password).ToList();
                if (uservalidity[0].RoleId == 2) // USER
                {
                    HttpContext.Session.SetInt32("UserId", uservalidity[0].UserId);
                    HttpContext.Session.SetString("Userfirstname", uservalidity[0].FirstName);
                    HttpContext.Session.SetString("Userlastname", uservalidity[0].LastName);
                    HttpContext.Session.SetString("Userrole", Convert.ToString(uservalidity[0].RoleId));

                    return RedirectToAction("Index", "Home");
                }
                else if (uservalidity[0].RoleId == 3) // HR
                {
                    HttpContext.Session.SetInt32("UserId", uservalidity[0].UserId);
                    HttpContext.Session.SetString("Userfirstname", uservalidity[0].FirstName);
                    HttpContext.Session.SetString("Userlastname", uservalidity[0].LastName);
                    HttpContext.Session.SetString("Userrole", Convert.ToString(uservalidity[0].RoleId));

                    return RedirectToAction("Index", "Seller");
                }
                else if (uservalidity[0].RoleId == 1) // ADMIN
                {
                    HttpContext.Session.SetInt32("UserId", uservalidity[0].UserId);
                    HttpContext.Session.SetString("Userfirstname", uservalidity[0].FirstName);
                    HttpContext.Session.SetString("Userlastname", uservalidity[0].LastName);
                    HttpContext.Session.SetString("Userrole", Convert.ToString(uservalidity[0].RoleId));

                    return RedirectToAction("Index", "Admin");

                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }


        public IActionResult Index()
        {
            ViewBag.Carousels = db.Carousels.ToList();
            ViewBag.cat = db.Categorys.ToList();
            ViewBag.fac = db.Faculties.ToList();
            ViewBag.Feedbacks = db.Feedbacks.ToList();
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();


            return View();
        }

        public IActionResult ProductsByCategory(int id)
        {
            var category = db.Categorys.FirstOrDefault(c => c.CategorysId == id);
            if (category == null)
                return NotFound();

            switch (category.CategorysName)
            {
                case "Medicine":
                    ViewBag.products = db.MedicinesInfos.Where(m => m.MedCat == id).ToList();
                    break;
                case "Scientific":
                    ViewBag.products = db.ScientificInfos.Where(s => s.ScientificId == id).ToList();
                    break;
                case "Lecture":
                    ViewBag.products = db.Lectures
                                         .Include(l => l.LecFacultyNavigation)
                                         .Where(l => l.LecFaculty == id)
                                         .ToList();
                    break;
                case "Seminar":
                    ViewBag.products = db.Seminars
                                         .Include(s => s.SemFacultyNavigation)
                                         .Where(s => s.SemFaculty == id)
                                         .ToList();
                    break;
                case "Practical":
                    ViewBag.products = db.Practicals
                                         .Include(p => p.PracticalFacultyNavigation)
                                         .Where(p => p.PracticalFaculty == id)
                                         .ToList();
                    break;
                default:
                    ViewBag.products = new List<object>();
                    break;
            }

            ViewBag.categoryName = category.CategorysName;
            return View("SharedProductView");
        }





        public IActionResult Medicine(int? catId, string stockStatus, string medBrandName)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            // Distinct brand names from SellMedicinesInfos
            ViewBag.BrandNames = db.SellMedicinesInfos
                .Select(s => s.MedBrandName)
                .Distinct()
                .ToList();

            var medicines = db.MedicinesInfos.AsQueryable();
            var sellMedicines = db.SellMedicinesInfos.AsQueryable();

            if (catId.HasValue)
            {
                medicines = medicines.Where(m => m.MedCat == catId.Value);
                sellMedicines = sellMedicines.Where(s => s.MedCat == catId.Value);
            }

            if (!string.IsNullOrEmpty(stockStatus))
            {
                medicines = medicines.Where(m => m.StockStatus.ToLower() == stockStatus.ToLower());
                sellMedicines = sellMedicines.Where(s => s.StockStatus.ToLower() == stockStatus.ToLower());
            }

            if (!string.IsNullOrEmpty(medBrandName))
            {
                sellMedicines = sellMedicines.Where(s => s.MedBrandName.ToLower() == medBrandName.ToLower());
            }

            var medicinesList = medicines.ToList();
            var sellMedicinesList = sellMedicines.ToList();

            var combinedList = new List<dynamic>();

            foreach (var m in medicinesList)
            {
                combinedList.Add(new
                {
                    Id = m.MedId,
                    Name = m.MedName,
                    Image = m.MedImage,
                    Price = m.MedPrice,
                    StockStatus = m.StockStatus,
                    Type = "Normal"
                });
            }

            foreach (var s in sellMedicinesList)
            {
                combinedList.Add(new
                {
                    Id = s.MedId,
                    Name = s.MedName,
                    Brand = s.MedBrandName,
                    Image = s.MedImage,
                    Price = s.MedPrice,
                    StockStatus = s.StockStatus,
                    Type = "Sell"
                });
            }

            ViewBag.AllMedicines = combinedList.OrderBy(x => x.Name).ToList();

            return View();
        }







        public IActionResult Scientific(string catId, string stockStatus, string medBrandName)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            ViewBag.BrandNames = db.SellScientificInfos
                                   .Select(x => x.ScientificBrandName)
                                   .Distinct()
                                   .ToList();

            // Get queryables
            var scientificItems = db.ScientificInfos.AsQueryable();
            var sellScientificItems = db.SellScientificInfos.AsQueryable();

            // Apply category filter
            if (!string.IsNullOrEmpty(catId) && int.TryParse(catId, out int parsedCatId))
            {
                scientificItems = scientificItems.Where(a => a.ScientificCat == parsedCatId);
                sellScientificItems = sellScientificItems.Where(a => a.ScientificCat == parsedCatId);
            }

            // Apply stock status filter
            if (!string.IsNullOrEmpty(stockStatus))
            {
                scientificItems = scientificItems.Where(a => a.StockStatus.ToLower() == stockStatus.ToLower());
                sellScientificItems = sellScientificItems.Where(a => a.StockStatus.ToLower() == stockStatus.ToLower());
            }

            // ✅ Brand name filter — only apply to seller items AND exclude normal items
            bool isBrandFilterApplied = !string.IsNullOrEmpty(medBrandName);

            if (isBrandFilterApplied)
            {
                sellScientificItems = sellScientificItems.Where(a => a.ScientificBrandName == medBrandName);
                scientificItems = Enumerable.Empty<ScientificInfo>().AsQueryable(); // 🛑 Exclude normal items with no brand
            }

            // Combine results
            var combinedList = new List<dynamic>();

            foreach (var sci in scientificItems.ToList())
            {
                combinedList.Add(new
                {
                    Id = sci.ScientificId,
                    Name = sci.ScientificName,
                    Image = sci.ScientificImage,
                    Price = sci.ScientificPrice,
                    StockStatus = sci.StockStatus,
                    Type = "Normal"
                });
            }

            foreach (var sellSci in sellScientificItems.ToList())
            {
                combinedList.Add(new
                {
                    Id = sellSci.ScientificId,
                    Name = sellSci.ScientificName,
                    brand = sellSci.ScientificBrandName,
                    Image = sellSci.ScientificImage,
                    Price = sellSci.ScientificPrice,
                    StockStatus = sellSci.StockStatus,
                    Type = "Sell"
                });
            }

            ViewBag.AllScientific = combinedList.OrderBy(x => x.Name).ToList();

            return View();
        }









        public IActionResult Lecture(string LecCategoryId, string stockStatus)
        {
            ViewBag.cat = db.Faculties.ToList();
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            var lectures = db.Lectures.Include(p => p.LecFacultyNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(LecCategoryId) && int.TryParse(LecCategoryId, out int parsedId))
            {
                lectures = lectures.Where(p => p.LecFaculty == parsedId);
            }

            if (!string.IsNullOrEmpty(stockStatus))
            {
                lectures = lectures.Where(p => p.LecStockstatus.ToLower() == stockStatus.ToLower());
            }

            ViewBag.med = lectures.ToList();

            return View();
        }


        public IActionResult Practical(int? facultyId, int? CatId, string stockStatus)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.FacultyList = db.Faculties.ToList();
            ViewBag.CategoryList = db.PracticalCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            var practicals = db.Practicals
                .Include(p => p.PracticalFacultyNavigation)
                .Include(p => p.PracticalCategoryNavigation)
                .AsQueryable();

            if (facultyId.HasValue)
            {
                practicals = practicals.Where(p => p.PracticalFaculty == facultyId.Value);
            }

            if (CatId.HasValue)
            {
                practicals = practicals.Where(p => p.PracticalCategory == CatId.Value);
            }

            if (!string.IsNullOrEmpty(stockStatus))
            {
                practicals = practicals.Where(p => p.PracStockstatus.ToLower() == stockStatus.ToLower());
            }

            ViewBag.Practicals = practicals.ToList();

            return View();
        }






        public IActionResult Seminar(string SemId, string stockStatus)
        {
            ViewBag.cat = db.Faculties.ToList();
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            var seminars = db.Seminars.Include(p => p.SemFacultyNavigation).AsQueryable();

            if (!string.IsNullOrEmpty(SemId) && int.TryParse(SemId, out int parsedId))
            {
                seminars = seminars.Where(p => p.SemFaculty == parsedId);
            }

            if (!string.IsNullOrEmpty(stockStatus))
            {
                seminars = seminars.Where(p => p.SemStockstatus.ToLower() == stockStatus.ToLower());
            }

            ViewBag.med = seminars.ToList();

            return View();
        }



        public IActionResult SingleMedicine(int id, string type)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            if (type == "Normal")
            {
                var med = db.MedicinesInfos
                            .Include(m => m.MedCatNavigation)
                            .Where(m => m.MedId == id)
                            .ToList(); // to keep consistent with the existing code expecting a list

                ViewBag.Type = "Normal";
                ViewBag.pro = med;
            }
            else if (type == "Sell")
            {
                var sellMed = db.SellMedicinesInfos
                                .Include(s => s.MedCatNavigation)
                                .FirstOrDefault(s => s.MedId == id);

                ViewBag.Type = "Sell";
                ViewBag.sellPro = sellMed;
            }

            return View();
        }





        public IActionResult SingleScientific(int id, string type = "Normal")
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            if (type == "Normal")
            {
                var scientificItem = db.ScientificInfos
                    .Include(m => m.ScientificCatNavigation)
                    .Where(option => option.ScientificId == id)
                    .ToList();

                ViewBag.Type = "Normal";
                ViewBag.Sci = scientificItem;
            }
            else if (type == "Sell")
            {
                var sellItem = db.SellScientificInfos
                    .Include(m => m.ScientificCatNavigation)
                    .FirstOrDefault(option => option.ScientificId == id);

                ViewBag.Type = "Sell";
                ViewBag.SellSci = sellItem;
            }
            else
            {
                return Content("Invalid scientific item type.");
            }

            return View();
        }





        public IActionResult SingleLecture(int id)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            ViewBag.Sci = db.Lectures.Include(m => m.LecFacultyNavigation).Where(option => option.LecId == id).ToList();
            return View();
        }

        public IActionResult SingleSeminar(int id)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            ViewBag.Sem = db.Seminars.Include(m => m.SemFacultyNavigation).Where(option => option.SemId == id).ToList();
            return View();
        }

        public IActionResult SinglePractical(int id)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            var practical = db.Practicals
                .Include(p => p.PracticalFacultyNavigation)
                .Include(p => p.PracticalCategoryNavigation)
                .Where(p => p.PracId == id)
                .ToList();

            ViewBag.Prac = practical;
            return View();
        }


        [HttpGet]

        public IActionResult AddFeedback()
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            return View();
        }


        [HttpPost]
        public IActionResult AddFeedback(Feedback feedback, IFormFile img)
        {
            if (ModelState.IsValid)
            {

                if (img != null && img.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                    var fileExt = Path.GetExtension(img.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExt))
                    {
                        ModelState.AddModelError("img", "Only JPG, JPEG, PNG, and WEBP files are allowed.");
                        return View(feedback);
                    }

                    var fileName = Guid.NewGuid().ToString() + fileExt;
                    string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MyImages");

                    if (!Directory.Exists(imgFolder))
                    {
                        Directory.CreateDirectory(imgFolder);
                    }

                    string filePath = Path.Combine(imgFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }
                    feedback.ProductImg = Path.Combine("MyImages", fileName);
                }

                db.Feedbacks.Add(feedback);
                db.SaveChanges();

                return RedirectToAction("index");
            }

            return View(feedback);
        }











        [HttpGet]
        public IActionResult MedOrderForm(int id, string type)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();
            var banks = db.Banks.ToList();

            var viewModel = new OrderMedFormViewModel
            {
                BankDetails = banks,
                Type = type
            };

            if (type == "Normal")
            {
                var med = db.MedicinesInfos
                            .Include(m => m.MedCatNavigation)
                            .FirstOrDefault(m => m.MedId == id);

                if (med == null)
                    return NotFound();

                viewModel.Medicine = med;
                viewModel.MedId = med.MedId;
            }
            else if (type == "Sell")
            {
                var sellMed = db.SellMedicinesInfos
                                .Include(s => s.MedCatNavigation)
                                .FirstOrDefault(s => s.MedId == id);

                if (sellMed == null)
                    return NotFound();

                viewModel.SellMedicine = sellMed;
                viewModel.MedId = sellMed.MedId;
            }
            else
            {
                return BadRequest("Invalid medicine type.");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult MedOrderForm(OrderMedFormViewModel model, IFormFile TransactionScreenshot)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExt = Path.GetExtension(TransactionScreenshot?.FileName ?? "").ToLower();

            if (TransactionScreenshot == null || TransactionScreenshot.Length == 0)
            {
                ModelState.AddModelError("TransactionScreenshot", "Screenshot is required.");
                return View(model);
            }

            if (!allowedExtensions.Contains(fileExt))
            {
                ModelState.AddModelError("TransactionScreenshot", "Only JPG, JPEG, PNG, WEBP files are allowed.");
                return View(model);
            }

            var fileName = Guid.NewGuid().ToString() + fileExt;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/TransactionReceipts");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                TransactionScreenshot.CopyTo(stream);
            }

            // Fetch full medicine info from DB using MedId and Type (ignore Medicine and SellMedicine on model)
            string medName = "";
            string medCat = "";
            string medPrice = "";

            if (model.Type == "Normal")
            {
                var medicine = db.MedicinesInfos.Include(m => m.MedCatNavigation).FirstOrDefault(m => m.MedId == model.MedId);
                if (medicine == null)
                {
                    ModelState.AddModelError("", "Medicine not found.");
                    return View(model);
                }
                medName = medicine.MedName;
                medCat = medicine.MedCatNavigation?.MedCatName ?? "Unknown";
                medPrice = medicine.MedPrice;
            }
            else if (model.Type == "Sell")
            {
                var sellMed = db.SellMedicinesInfos.Include(s => s.MedCatNavigation).FirstOrDefault(s => s.MedId == model.MedId);
                if (sellMed == null)
                {
                    ModelState.AddModelError("", "Sell Medicine not found.");
                    return View(model);
                }
                medName = sellMed.MedName;
                medCat = sellMed.MedCatNavigation?.MedCatName ?? "Unknown";
                medPrice = sellMed.MedPrice;
            }
            else
            {
                ModelState.AddModelError("", "Invalid medicine type.");
                return View(model);
            }

            var order = new MedicineOrder
            {
                MedicineId = model.MedId,
                MedName = medName,
                MedCategory = medCat,
                MedPrice = medPrice,
                UserEmail = model.MedicineOrder.UserEmail,
                UserAddress = model.MedicineOrder.UserAddress,
                TransactionScreenshot = Path.Combine("TransactionReceipts", fileName),
                OrderDate = DateTime.Now
            };

            db.MedicineOrders.Add(order);
            db.SaveChanges();

            return RedirectToAction("Medicine");
        }





        [HttpGet]
        public IActionResult SciOrderForm(int id, string type)
        {
            ViewBag.MedCategories = db.MedicinesCategories.ToList();
            ViewBag.ScientificCategories = db.ScientificCategories.ToList();
            ViewBag.PracticalCategories = db.PracticalCategories.ToList();

            var banks = db.Banks.ToList();

            var viewModel = new OrderScientificFormViewModel
            {
                BankDetails = banks,
                Type = type
            };

            if (type == "Normal")
            {
                var sci = db.ScientificInfos
                            .Include(m => m.ScientificCatNavigation)
                            .FirstOrDefault(m => m.ScientificId == id);

                if (sci == null)
                    return NotFound();

                viewModel.Scientific = sci;
                viewModel.SciId = sci.ScientificId;
            }
            else if (type == "Sell")
            {
                var sellSci = db.SellScientificInfos
                                .Include(m => m.ScientificCatNavigation)
                                .FirstOrDefault(m => m.ScientificId == id);

                if (sellSci == null)
                    return NotFound();

                viewModel.SellScientific = sellSci;
                viewModel.SciId = sellSci.ScientificId;
            }
            else
            {
                return BadRequest("Invalid scientific machine type.");
            }

            return View(viewModel);
        }




        [HttpPost]
        public IActionResult SciOrderForm(OrderScientificFormViewModel model, IFormFile TransactionScreenshot)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExt = Path.GetExtension(TransactionScreenshot?.FileName ?? "").ToLower();

            if (TransactionScreenshot == null || TransactionScreenshot.Length == 0)
            {
                ModelState.AddModelError("TransactionScreenshot", "Screenshot is required.");
                return View(model);
            }

            if (!allowedExtensions.Contains(fileExt))
            {
                ModelState.AddModelError("TransactionScreenshot", "Only JPG, JPEG, PNG, WEBP files are allowed.");
                return View(model);
            }

            var fileName = Guid.NewGuid().ToString() + fileExt;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/TransactionReceipts");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                TransactionScreenshot.CopyTo(stream);
            }

            // Get data from correct table based on type
            string sciName = "", sciCat = "", sciPrice = "";

            if (model.Type == "Normal")
            {
                var sci = db.ScientificInfos.Include(m => m.ScientificCatNavigation)
                                            .FirstOrDefault(m => m.ScientificId == model.SciId);
                if (sci == null)
                {
                    ModelState.AddModelError("", "Scientific machine not found.");
                    return View(model);
                }

                sciName = sci.ScientificName;
                sciCat = sci.ScientificCatNavigation?.SciCatName ?? "Unknown";
                sciPrice = sci.ScientificPrice;
            }
            else if (model.Type == "Sell")
            {
                var sellSci = db.SellScientificInfos.Include(m => m.ScientificCatNavigation)
                                                    .FirstOrDefault(m => m.ScientificId == model.SciId);
                if (sellSci == null)
                {
                    ModelState.AddModelError("", "Sell Scientific machine not found.");
                    return View(model);
                }

                sciName = sellSci.ScientificName;
                sciCat = sellSci.ScientificCatNavigation?.SciCatName ?? "Unknown";
                sciPrice = sellSci.ScientificPrice.ToString("F2"); 

            }
            else
            {
                ModelState.AddModelError("", "Invalid scientific type.");
                return View(model);
            }

            var order = new ScientificOrder
            {
                ScientificId = model.SciId,
                SciName = sciName,
                SciCategory = sciCat,
                SciPrice = sciPrice,
                UserEmail = model.ScientificOrder.UserEmail,
                UserAddress = model.ScientificOrder.UserAddress,
                TransactionScreenshot = Path.Combine("TransactionReceipts", fileName),
                OrderDate = DateTime.Now
            };

            db.ScientificOrders.Add(order);
            db.SaveChanges();

            return RedirectToAction("Scientific");
        }














        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}