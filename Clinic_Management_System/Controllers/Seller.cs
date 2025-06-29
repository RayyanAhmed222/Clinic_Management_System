using Clinic_Management_System.Data;
using Clinic_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Controllers
{
    public class Seller : Controller
    {
        private readonly ClinicContext db;
        private readonly IHttpContextAccessor contx;

        public Seller(ClinicContext db, IHttpContextAccessor contx)
        {
            this.db = db;
            this.contx = contx;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BecomeRecruiter()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == userId);

                if (user != null)
                {
                    var model = new RecruiterRequest
                    {
                        Name = user.FirstName + " " + user.LastName,
                        Email = user.Email
                    };

                    return View(model);
                }
            }

            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult SubmitRecruiterRequest(RecruiterRequest model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                model.UserId = userId.Value;
                model.RequestDate = DateTime.Now;
                model.Status = "Pending";

                db.RecruiterRequests.Add(model);
                db.SaveChanges();

                TempData["Message"] = "Request submitted successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Session expired. Please log in again.";
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult HRStatus()
        {

            var userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }



            var myRequests = db.RecruiterRequests
                                .Where(r => r.UserId == userId)
                                .ToList();

            return View(myRequests); 
        }

        [HttpGet]
        public IActionResult AddMedCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMedCategory(MedicinesCategory addmedcat)
        {
            if (ModelState.IsValid)
            {
                db.MedicinesCategories.Add(addmedcat);
                db.SaveChanges();
                return RedirectToAction("ShowMedCategory");
            }
            return View();
        }


        [HttpGet]
        public IActionResult AddSciCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSciCategory(ScientificCategory addscicat)
        {
            if (ModelState.IsValid)
            {
                db.ScientificCategories.Add(addscicat);
                db.SaveChanges();
                return RedirectToAction("ShowSciCategory");
            }
            return View();
        }


        [HttpGet]
        public IActionResult AddMed()
        {
            AddSellMedViewModel data = new AddSellMedViewModel()
            {
                medcategory = db.MedicinesCategories.ToList(),
                sellmedicineinfo = new SellMedicinesInfo()
            };
            return View(data);
        }

        [HttpPost]
        public IActionResult AddMed(AddSellMedViewModel newProduct, IFormFile img)
        {
            if (img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, PNG, JFIF, and WEBP files are allowed.");
                    newProduct.medcategory = db.MedicinesCategories.ToList();
                    return View(newProduct);
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

                newProduct.sellmedicineinfo.MedImage = Path.Combine("MyImages", fileName);
            }

            db.SellMedicinesInfos.Add(newProduct.sellmedicineinfo);
            db.SaveChanges();

            return RedirectToAction("ShowMed");

        }

        [HttpGet]
        public IActionResult AddSci()
        {
            AddSellSciViewModel sci = new AddSellSciViewModel()
            {
                SciCategoryList = db.ScientificCategories.ToList(),
                sellscientificinfo = new SellScientificInfo()
            };
            return View(sci);
        }

        [HttpPost]

        public IActionResult AddSci(AddSellSciViewModel newSci, IFormFile img)
        {
            var allowedextensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
            var fileext = Path.GetExtension(img.FileName).ToLower();

            if (!allowedextensions.Contains(fileext))
            {
                ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
                return View(newSci);
            }

            var filename = Guid.NewGuid().ToString() + fileext;
            string imgfolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MyImages");

            if (!Directory.Exists(imgfolder))
            {
                Directory.CreateDirectory(imgfolder);
            }

            string filePath = Path.Combine(imgfolder, filename);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            newSci.sellscientificinfo.ScientificImage = Path.Combine("MyImages", filename);
            db.SellScientificInfos.Add(newSci.sellscientificinfo);
            db.SaveChanges();

            return RedirectToAction("ShowSci");
        }



        [HttpGet]
        public IActionResult UpdateMed(int id)
        {
            var existingMed = db.SellMedicinesInfos.FirstOrDefault(m => m.MedId == id);
            if (existingMed == null)
            {
                return NotFound();
            }

            var viewModel = new AddSellMedViewModel
            {
                medcategory = db.MedicinesCategories.ToList(),
                sellmedicineinfo = existingMed
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult UpdateMed(AddSellMedViewModel updatedProduct, IFormFile img)
        {
            var existingMed = db.SellMedicinesInfos.FirstOrDefault(m => m.MedId == updatedProduct.sellmedicineinfo.MedId);
            if (existingMed == null)
            {
                return NotFound();
            }

            if (img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, PNG, JFIF, and WEBP files are allowed.");
                    updatedProduct.medcategory = db.MedicinesCategories.ToList();
                    return View(updatedProduct);
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

                existingMed.MedImage = Path.Combine("MyImages", fileName);
            }

            // Update all necessary properties
            existingMed.MedName = updatedProduct.sellmedicineinfo.MedName;
            existingMed.MedBrandName = updatedProduct.sellmedicineinfo.MedBrandName;
            existingMed.MedDescription = updatedProduct.sellmedicineinfo.MedDescription;
            existingMed.MedPrice = updatedProduct.sellmedicineinfo.MedPrice;
            existingMed.StockStatus = updatedProduct.sellmedicineinfo.StockStatus;
            existingMed.MedCat = updatedProduct.sellmedicineinfo.MedCat;

            db.SaveChanges();

            return RedirectToAction("ShowMed");
        }



        [HttpGet]
        public IActionResult UpdateSci(int id)
        {
            var existingSci = db.SellScientificInfos.FirstOrDefault(s => s.ScientificId == id);
            if (existingSci == null)
            {
                return NotFound();
            }

            var viewModel = new AddSellSciViewModel
            {
                SciCategoryList = db.ScientificCategories.ToList(),
                sellscientificinfo = existingSci
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateSci(AddSellSciViewModel updatedSci, IFormFile img)
        {
            var existingSci = db.SellScientificInfos.FirstOrDefault(s => s.ScientificId == updatedSci.sellscientificinfo.ScientificId);
            if (existingSci == null)
            {
                return NotFound();
            }

            if (img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, JFIF, PNG, and WEBP files are allowed.");
                    updatedSci.SciCategoryList = db.ScientificCategories.ToList();
                    return View(updatedSci);
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

                existingSci.ScientificImage = Path.Combine("MyImages", fileName);
            }

            // Update all fields
            existingSci.ScientificName = updatedSci.sellscientificinfo.ScientificName;
            existingSci.ScientificBrandName = updatedSci.sellscientificinfo.ScientificBrandName;
            existingSci.ScientificDescription = updatedSci.sellscientificinfo.ScientificDescription;
            existingSci.ScientificPrice = updatedSci.sellscientificinfo.ScientificPrice;
            existingSci.StockStatus = updatedSci.sellscientificinfo.StockStatus;
            existingSci.ScientificCat = updatedSci.sellscientificinfo.ScientificCat;

            db.SaveChanges();

            return RedirectToAction("ShowSci");
        }



        [HttpGet]
        public IActionResult UpdateMedCategory(int id)
        {
            var medCategory = db.MedicinesCategories.Find(id);
            if (medCategory == null) return NotFound();

            var viewmodel = new AddMedViewModel
            {
                MedCategoryList = db.MedicinesCategories.ToList(),
                SelectedCategory = medCategory
            };

            return View(viewmodel);
        }


        [HttpPost]
        public IActionResult UpdateMedCategory(AddMedViewModel model)
        {
            var updatedCategory = model.SelectedCategory;
            var existingCategory = db.MedicinesCategories.Find(updatedCategory.MedCatId);
            if (existingCategory == null) return NotFound();

            existingCategory.MedCatName = updatedCategory.MedCatName;
            db.SaveChanges();

            return RedirectToAction("ShowMedCategory");
        }


        [HttpGet]
        public IActionResult UpdateScientificCategory(int id)
        {
            var sciCategory = db.ScientificCategories.Find(id);
            if (sciCategory == null) return NotFound();

            var viewmodel = new AddSciViewModel
            {
                SciCategoryList = db.ScientificCategories.ToList(),
                SelectedCategory = sciCategory
            };

            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult UpdateScientificCategory(AddSciViewModel model)
        {
            var updatedCategory = model.SelectedCategory;
            var existingCategory = db.ScientificCategories.Find(updatedCategory.SciCatId);
            if (existingCategory == null) return NotFound();

            existingCategory.SciCatName = updatedCategory.SciCatName;
            db.SaveChanges();

            return RedirectToAction("ShowSciCategory"); // Adjust name if needed
        }





        [HttpGet]
        public IActionResult ShowMedCategory()
        {
            var showdata = db.MedicinesCategories.Include(p => p.MedicinesInfos);
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowSciCategory()
        {
            var showdata = db.ScientificCategories;
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowMed()
        {
            var showdata = db.SellMedicinesInfos.Include(p => p.MedCatNavigation);
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowSci()
        {
            var showdata = db.SellScientificInfos.Include(p => p.ScientificCatNavigation);
            var data = showdata.ToList();
            return View(data);
        }





        public IActionResult DelMedCategory(int id)
        {
            var category = db.MedicinesCategories.Find(id);
            if (category == null) return NotFound();

            db.MedicinesCategories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("ShowMedCategory");
        }

        public IActionResult DelMed(int id)
        {
            var dataid = db.SellMedicinesInfos.Find(id);
            db.SellMedicinesInfos.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowMed");
        }


        public IActionResult DelSci(int id)
        {
            var dataid = db.SellScientificInfos.Find(id);
            db.SellScientificInfos.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowSci");
        }


        public IActionResult DelScientificCategory(int id)
        {
            var category = db.ScientificCategories.Find(id);
            if (category == null) return NotFound();

            db.ScientificCategories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("ShowSciCategory");
        }
    }
}
