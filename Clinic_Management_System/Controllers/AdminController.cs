using System.Linq;
using Clinic_Management_System.Data;
using Clinic_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly ClinicContext db;
        public AdminController(ClinicContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User model)
        {
            // Default role is User (assume RoleId = 1 for Admin)
            model.RoleId = 1;

            db.Users.Add(model);
            db.SaveChanges();

            return RedirectToAction("Login", "Home");

            return View(model);
        }

        [HttpGet]
        public IActionResult RecruiterRequestStatus()
        {
            var requests = db.RecruiterRequests.ToList();
            return View(requests);
        }

        [HttpPost]
        public IActionResult RecruiterRequestStatus(int requestId, string newStatus)
        {
            var request = db.RecruiterRequests.FirstOrDefault(r => r.RequestId == requestId);

            if (request != null)
            {
                request.Status = newStatus;

                var user = db.Users.FirstOrDefault(u => u.UserId == request.UserId);
                if (user != null)
                {
                    if (newStatus == "Approved")
                    {
                        user.RoleId = 3; // Seller
                    }
                    else
                    {
                        user.RoleId = 2; // Regular user

                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("RecruiterRequestStatus");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddBank()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBank(Bank addBank)
        {
            if (ModelState.IsValid)
            {
                db.Banks.Add(addBank);
                db.SaveChanges();
                return RedirectToAction("ShowBank");
            }

            return View(addBank); 
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
        public IActionResult AddParCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddParCategory(PracticalCategory addparcat)
        {
            if (ModelState.IsValid)
            {
                db.PracticalCategories.Add(addparcat);
                db.SaveChanges();
                return RedirectToAction("ShowPracCategory");
            }
            return View();
        }



        [HttpGet]
        public IActionResult AddFaculty()
        {
            AddFacultyViewModel fac = new AddFacultyViewModel()
            {
                FacultyList = db.Faculties.ToList(),  
                faculty = new Faculty()
            };
            return View(fac);
        }

        [HttpPost]
        public IActionResult AddFaculty(AddFacultyViewModel newFaculty, IFormFile img)
        {
            if (img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, JFIF, PNG, and WEBP files are allowed.");
                    return View(newFaculty);
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

                newFaculty.faculty.FacultyImage = Path.Combine("MyImages", fileName);
            }
            else
            {
                ModelState.AddModelError("img", "Please upload an image.");
                return View(newFaculty);
            }

            db.Faculties.Add(newFaculty.faculty);
            db.SaveChanges();

            return RedirectToAction("ShowFaculty");
        }


        [HttpGet]
        public IActionResult AddCategory()
        {
            AddCategoryViewModel Category = new AddCategoryViewModel()
            {
                categoryList = db.Categorys.ToList(),
                category = new Category()

            };
            return View(Category);
        }

        [HttpPost]
        public IActionResult AddCategory(AddCategoryViewModel newCategory, IFormFile img)
        {
            if (img != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, JFIF, PNG, and WEBP files are allowed.");
                    return View(newCategory);
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

                newCategory.category.CategorysImg = Path.Combine("MyImages", fileName);
            }
            else
            {
                ModelState.AddModelError("img", "Please upload an image.");
                return View(newCategory);
            }

            db.Categorys.Add(newCategory.category);
            db.SaveChanges();

            return RedirectToAction("ShowCategory");
        }


        [HttpGet]
        public IActionResult AddCarousel()
        {
            var viewModel = new AddCarouselViewModel
            {
                Carousel = new Carousel()
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddCarousel(AddCarouselViewModel newCarousel, IFormFile img)
        {
            if (newCarousel.Carousel == null)
            {
                newCarousel.Carousel = new Carousel();
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExt = Path.GetExtension(img.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExt))
            {
                ModelState.AddModelError("img", "Only JPG, JPEG, PNG, and WEBP files are allowed.");
                return View(newCarousel);
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

            newCarousel.Carousel.CarouselImg = Path.Combine("MyImages", fileName);
            db.Carousels.Add(newCarousel.Carousel);
            db.SaveChanges();

            return RedirectToAction("ShowCarousel");
        }







        [HttpGet]
        public IActionResult AddMed()
        {
            AddMedViewModel data = new AddMedViewModel()
            {
                MedCategoryList = db.MedicinesCategories.ToList(),
                medicineinfo = new MedicinesInfo()
            };
            return View(data);
        }

        [HttpPost]

        public IActionResult AddMed(AddMedViewModel newProduct, IFormFile img)
        {
            var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
            var fileExt = Path.GetExtension(img.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExt))
            {
                ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
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

            newProduct.medicineinfo.MedImage = Path.Combine("MyImages", fileName);
            db.MedicinesInfos.Add(newProduct.medicineinfo);
            db.SaveChanges();

            return RedirectToAction("showMed");
        }



        [HttpGet]
        public IActionResult AddSci()
        {
            AddSciViewModel sci = new AddSciViewModel()
            {
                SciCategoryList = db.ScientificCategories.ToList(),
                scientificinfo = new ScientificInfo()
            };
            return View(sci);
        }

        [HttpPost]

        public IActionResult AddSci(AddSciViewModel newSci, IFormFile img)
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

            newSci.scientificinfo.ScientificImage = Path.Combine("MyImages", filename);
            db.ScientificInfos.Add(newSci.scientificinfo);
            db.SaveChanges();

            return RedirectToAction("ShowSci");
        }

        [HttpGet]
        public IActionResult AddLec()
        {
            AddLecViewModel lec = new AddLecViewModel()
            {
                lecList = db.Lectures.ToList(),
                facultyList = db.Faculties.ToList(),
                lecture = new Lecture()
            };
            ViewBag.LecFaculty = new SelectList(lec.facultyList, "FacultyId", "FacultyName");
            return View(lec);
        }

        [HttpPost]

        public IActionResult AddLec(AddLecViewModel newLec, IFormFile img)
        {
            var allowedextensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
            var fileext = Path.GetExtension(img.FileName).ToLower();

            if (!allowedextensions.Contains(fileext))
            {
                ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");

                newLec.facultyList = db.Faculties.ToList();
                return View(newLec);
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

            newLec.lecture.LecImg = Path.Combine("MyImages", filename);
            db.Lectures.Add(newLec.lecture);
            db.SaveChanges();

            return RedirectToAction("ShowLec");
        }





        [HttpGet]
        public IActionResult AddSeminar()
        {
            AddSeminarViewModel sem = new AddSeminarViewModel()
            {
                semList = db.Seminars.ToList(),
                FacultyList = db.Faculties.ToList(),
                seminar = new Seminar()
            };
            ViewBag.SemFaculty = new SelectList(sem.FacultyList, "FacultyId", "FacultyName");
            return View(sem);
        }


        [HttpPost]
        public IActionResult AddSeminar(AddSeminarViewModel newSem, IFormFile img)
        {
            var allowedextensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
            var fileext = Path.GetExtension(img.FileName).ToLower();

            if (!allowedextensions.Contains(fileext))
            {
                ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");

                newSem.FacultyList = db.Faculties.ToList();
                return View(newSem);
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

            newSem.seminar.SemImg = Path.Combine("MyImages", filename);
            db.Seminars.Add(newSem.seminar);
            db.SaveChanges();

            return RedirectToAction("ShowSem");
        }




        [HttpGet]
        public IActionResult AddPractical()
        {
            AddParcViewModel model = new AddParcViewModel()
            {
                pracList = db.PracticalCategories.ToList(),
                FacultyList = db.Faculties.ToList(),
                practical = new Practical()
            };

            ViewBag.FacultyList = new SelectList(model.FacultyList, "FacultyId", "FacultyName");
            ViewBag.CategoryList = new SelectList(model.pracList, "PracCatId", "PracCatName");

            return View(model);
        }

        [HttpPost]
        public IActionResult AddPractical(AddParcViewModel model, IFormFile img)
        {
            var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
            var fileExt = Path.GetExtension(img.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExt))
            {
                ModelState.AddModelError("img", "Only JPG, PNG, and WEBP files are allowed.");

                model.FacultyList = db.Faculties.ToList();
                model.pracList = db.PracticalCategories.ToList();
                ViewBag.FacultyList = new SelectList(model.FacultyList, "FacultyId", "FacultyName");
                ViewBag.CategoryList = new SelectList(model.pracList, "PracCatId", "PracCatName");
                return View(model);
            }

            var filename = Guid.NewGuid().ToString() + fileExt;
            string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MyImages");

            if (!Directory.Exists(imgFolder))
            {
                Directory.CreateDirectory(imgFolder);
            }

            string filePath = Path.Combine(imgFolder, filename);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            model.practical.PracImg = Path.Combine("MyImages", filename);
            db.Practicals.Add(model.practical);
            db.SaveChanges();

            return RedirectToAction("ShowPrac");
        }




        [HttpGet]
        public IActionResult UpdateCarousel(int id)
        {
            var existingCarousel = db.Carousels.Find(id);
            if (existingCarousel == null) return NotFound();

            var viewModel = new AddCarouselViewModel
            {
                Carousel = existingCarousel
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateCarousel(AddCarouselViewModel updatedCarousel, IFormFile? img)
        {
            var existing = db.Carousels.Find(updatedCarousel.Carousel.CarouselId);
            if (existing == null) return NotFound();

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, JPEG, PNG, and WEBP files are allowed.");
                    return View(updatedCarousel);
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

                existing.CarouselImg = Path.Combine("MyImages", fileName);
            }

            db.SaveChanges();

            return RedirectToAction("ShowCarousel");
        }



        [HttpGet]
        public IActionResult UpdateFeedback(int id)
        {
            var feedback = db.Feedbacks.Find(id);
            if (feedback == null) return NotFound();
            return View(feedback);
        }

        [HttpPost]
        public IActionResult UpdateFeedback(Feedback updated, IFormFile? img)
        {
            var existing = db.Feedbacks.Find(updated.FeedbackId);
            if (existing == null) return NotFound();

            existing.ClientName = updated.ClientName;
            existing.ProductName = updated.ProductName;
            existing.ClientFeedback = updated.ClientFeedback;

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, JPEG, PNG, and WEBP files are allowed.");
                    return View(updated);
                }

                var fileName = Guid.NewGuid().ToString() + fileExt;
                string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FeedbackImages");

                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }

                string filePath = Path.Combine(imgFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    img.CopyTo(stream);
                }

                existing.ProductImg = Path.Combine("FeedbackImages", fileName);
            }

            db.SaveChanges();
            return RedirectToAction("ShowFeedback");
        }





        [HttpGet]
        public IActionResult UpdateMed(int id)
        {
            var medicne = db.MedicinesInfos.Find(id);
            if (medicne == null) NotFound();
            var viewmodel = new AddMedViewModel()
            {
                medicineinfo = medicne,
                MedCategoryList = db.MedicinesCategories.ToList()
            };
            return View(viewmodel);
        }


        [HttpPost]
        public IActionResult UpdateMed(AddMedViewModel updatemed, IFormFile? img)
        {
            var exisitingmed = db.MedicinesInfos.Find(updatemed.medicineinfo.MedId);
            if (exisitingmed == null) return NotFound();
            exisitingmed.MedName = updatemed.medicineinfo.MedName;
            exisitingmed.MedDescription = updatemed.medicineinfo.MedDescription;
            exisitingmed.MedPrice = updatemed.medicineinfo.MedPrice;
            exisitingmed.MedCat = updatemed.medicineinfo.MedCat;
            exisitingmed.StockStatus = updatemed.medicineinfo.StockStatus;

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
                    return View(updatemed);
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
                exisitingmed.MedImage = Path.Combine("MyImages", fileName);

            }
            db.SaveChanges();

            return RedirectToAction("ShowMed");
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
        public IActionResult UpdateSci(int id)
        {
            var scientific = db.ScientificInfos.Find(id);
            if (scientific == null) NotFound();
            var viewmodel = new AddSciViewModel()
            {
                scientificinfo = scientific,
                SciCategoryList = db.ScientificCategories.ToList()
            };
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult UpdateSci(AddSciViewModel updateSci, IFormFile? img)
        {
            var exisitingmed = db.ScientificInfos.Find(updateSci.scientificinfo.ScientificId);
            if (exisitingmed == null) return NotFound();
            exisitingmed.ScientificName = updateSci.scientificinfo.ScientificName;
            exisitingmed.ScientificDescription = updateSci.scientificinfo.ScientificDescription;
            exisitingmed.ScientificPrice = updateSci.scientificinfo.ScientificPrice;
            exisitingmed.ScientificCat = updateSci.scientificinfo.ScientificCat;
            exisitingmed.StockStatus = updateSci.scientificinfo.StockStatus;

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
                    return View(updateSci);
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
                exisitingmed.ScientificImage = Path.Combine("MyImages", fileName);

            }
            db.SaveChanges();

            return RedirectToAction("ShowSci");
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

            return RedirectToAction("ShowSciCategory"); 
        }





        [HttpGet]
        public IActionResult UpdateFaculty(int id)
        {
            var UpFacc = db.Faculties.Find(id);
            if (UpFacc == null) NotFound();
            var viewmodel = new AddFacultyViewModel()
            {
                faculty = UpFacc
            };
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult UpdateFaculty(AddFacultyViewModel updatefac, IFormFile? img)
        {
            var existingFaculty = db.Faculties.Find(updatefac.faculty.FacultyId);
            if (existingFaculty == null) return NotFound();


            existingFaculty.FacultyName = updatefac.faculty.FacultyName;
            existingFaculty.FacultyGender = updatefac.faculty.FacultyGender;
            existingFaculty.FacultyAge = updatefac.faculty.FacultyAge;
            existingFaculty.FacultyEmail = updatefac.faculty.FacultyEmail;
            existingFaculty.FacultyQualification = updatefac.faculty.FacultyQualification;
            existingFaculty.FacultySpecialization = updatefac.faculty.FacultySpecialization;
            existingFaculty.FacultyPhone = updatefac.faculty.FacultyPhone;


            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
                    return View(updatefac);
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


                existingFaculty.FacultyImage = Path.Combine("MyImages", fileName);
            }
            else
            {

                existingFaculty.FacultyImage = existingFaculty.FacultyImage;
            }

            db.SaveChanges();

            return RedirectToAction("ShowFaculty");
        }



        [HttpGet]
        public IActionResult UpdateLec(int id)
        {
            var UpLecc = db.Lectures.Find(id);
            if (UpLecc == null) NotFound();
            var viewmodel = new AddLecViewModel()
            {
                lecture = UpLecc,
                facultyList = db.Faculties.ToList()
            };
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult UpdateLec(AddLecViewModel updatelec, IFormFile? img)
        {
            var existingFaculty = db.Lectures.Find(updatelec.lecture.LecId);
            if (existingFaculty == null) return NotFound();

            existingFaculty.LecName = updatelec.lecture.LecName;
            existingFaculty.LecShortDescription = updatelec.lecture.LecShortDescription;
            existingFaculty.LecDescription = updatelec.lecture.LecDescription;
            existingFaculty.LecDatetime = updatelec.lecture.LecDatetime;
            existingFaculty.LecDuration = updatelec.lecture.LecDuration;
            existingFaculty.LecStockstatus = updatelec.lecture.LecStockstatus;
            existingFaculty.LecPrice = updatelec.lecture.LecPrice;
            existingFaculty.LecFaculty = updatelec.lecture.LecFaculty; 

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
                    return View(updatelec);
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

                existingFaculty.LecImg = Path.Combine("MyImages", fileName);
            }

            db.SaveChanges();

            return RedirectToAction("ShowLec");
        }




        [HttpGet]
        public IActionResult UpdateSem(int id)
        {
            var UpSem = db.Seminars.Find(id);
            if (UpSem == null) NotFound();
            var viewmodel = new AddSeminarViewModel()
            {
                seminar = UpSem,
                FacultyList = db.Faculties.ToList()
            };
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult UpdateSem(AddSeminarViewModel updateSem, IFormFile? img)
        {
            var existingSem = db.Seminars.Find(updateSem.seminar.SemId);
            if (existingSem == null) return NotFound();

            existingSem.SemName = updateSem.seminar.SemName;
            existingSem.SemShortDescription = updateSem.seminar.SemShortDescription;
            existingSem.SemDescription = updateSem.seminar.SemDescription;
            existingSem.SemDatetime = updateSem.seminar.SemDatetime;
            existingSem.SemDuration = updateSem.seminar.SemDuration;
            existingSem.SemStockstatus = updateSem.seminar.SemStockstatus;
            existingSem.SemPrice = updateSem.seminar.SemPrice;
            existingSem.SemFaculty = updateSem.seminar.SemFaculty;

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG and PNG files are allowed.");
                    return View(updateSem);
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

                existingSem.SemImg = Path.Combine("MyImages", fileName);
            }

            db.SaveChanges();

            return RedirectToAction("ShowSem");
        }


        [HttpGet]
        public IActionResult UpdatePractical(int id)
        {
            var upPractical = db.Practicals.Find(id);
            if (upPractical == null) return NotFound();

            var viewModel = new AddParcViewModel()
            {
                practical = upPractical,
                FacultyList = db.Faculties.ToList(),
                pracList = db.PracticalCategories.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdatePractical(AddParcViewModel updatePractical, IFormFile? img)
        {
            var existingPractical = db.Practicals.Find(updatePractical.practical.PracId);
            if (existingPractical == null) return NotFound();

            existingPractical.PracName = updatePractical.practical.PracName;
            existingPractical.PracShortDescription = updatePractical.practical.PracShortDescription;
            existingPractical.PracDescription = updatePractical.practical.PracDescription;
            existingPractical.PracDatetime = updatePractical.practical.PracDatetime;
            existingPractical.ParcDuration = updatePractical.practical.ParcDuration;
            existingPractical.PracPrice = updatePractical.practical.PracPrice;
            existingPractical.PracStockstatus = updatePractical.practical.PracStockstatus;
            existingPractical.PracticalFaculty = updatePractical.practical.PracticalFaculty;
            existingPractical.PracticalCategory = updatePractical.practical.PracticalCategory;

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, PNG, and WebP files are allowed.");
                    return View(updatePractical);
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

                existingPractical.PracImg = Path.Combine("MyImages", fileName);
            }

            db.SaveChanges();

            return RedirectToAction("ShowPrac");
        }



        [HttpGet]
        public IActionResult UpdatePracCategory(int id)
        {
            var pracCategory = db.PracticalCategories.Find(id);
            if (pracCategory == null) return NotFound();

            var viewmodel = new AddParcViewModel
            {
                pracList = db.PracticalCategories.ToList(),
                SelectedCategory = pracCategory
            };

            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult UpdatePracCategory(AddParcViewModel model)
        {
            var updatedCategory = model.SelectedCategory;
            var existingCategory = db.PracticalCategories.Find(updatedCategory.PracCatId);
            if (existingCategory == null) return NotFound();

            existingCategory.PracCatName = updatedCategory.PracCatName;
            db.SaveChanges();

            return RedirectToAction("ShowPracCategory");
        }



        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var existingCategory = db.Categorys.Find(id);
            if (existingCategory == null) return NotFound();

            var viewModel = new AddCategoryViewModel()
            {
                category = existingCategory,
                categoryList = db.Categorys.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateCategory(AddCategoryViewModel updatedCategory, IFormFile? img)
        {
            var existingCategory = db.Categorys.Find(updatedCategory.category.CategorysId);
            if (existingCategory == null) return NotFound();

            existingCategory.CategorysName = updatedCategory.category.CategorysName;

            if (img != null && img.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jfif", ".png", ".webp" };
                var fileExt = Path.GetExtension(img.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ModelState.AddModelError("img", "Only JPG, JFIF, PNG, and WEBP files are allowed.");
                    return View(updatedCategory);
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

                existingCategory.CategorysImg = Path.Combine("MyImages", fileName);
            }

            db.SaveChanges();
            return RedirectToAction("ShowCategory");
        }

        [HttpGet]
        public IActionResult UpdateBank(int id)
        {
            var bank = db.Banks.Find(id);
            if (bank == null) return NotFound();

            return View(bank);
        }

        [HttpPost]
        public IActionResult UpdateBank(Bank updatedBank)
        {
            var existingBank = db.Banks.Find(updatedBank.AccId);
            if (existingBank == null) return NotFound();

            existingBank.AccName = updatedBank.AccName;
            existingBank.AccNumber = updatedBank.AccNumber;

            db.SaveChanges();
            return RedirectToAction("ShowBank");
        }



        public IActionResult DelCarousel(int id)
        {
            var dataid = db.Carousels.Find(id);
            db.Carousels.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowCarousel");
        }

        public IActionResult DeleteFeedback(int id)
        {
            var feedback = db.Feedbacks.Find(id);
            if (feedback == null) return NotFound();

            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
            return RedirectToAction("ShowFeedback");
        }


        public IActionResult DelMed(int id)
        {
            var dataid = db.MedicinesInfos.Find(id);
            db.MedicinesInfos.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowMed");
        }


        public IActionResult DelMedCategory(int id)
        {
            var category = db.MedicinesCategories.Find(id);
            if (category == null) return NotFound();

            db.MedicinesCategories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("ShowMedCategory");
        }



        public IActionResult DelSci(int id)
        {
            var dataid = db.ScientificInfos.Find(id);
            db.ScientificInfos.Remove(dataid);
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


        public IActionResult DelFaculty(int id)
        {
            var faculty = db.Faculties
                            .Include(f => f.Lectures)
                            .Include(f => f.Seminars)
                            .FirstOrDefault(f => f.FacultyId == id);

            if (faculty != null)
            {
                db.Lectures.RemoveRange(faculty.Lectures);
                db.Seminars.RemoveRange(faculty.Seminars);

                db.Faculties.Remove(faculty);
                db.SaveChanges();
            }

            return RedirectToAction("ShowFaculty");
        }


        public IActionResult DelLec(int id)
        {
            var dataid = db.Lectures.Find(id);
            db.Lectures.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowLec");
        }

        public IActionResult DelSem(int id)
        {
            var dataid = db.Seminars.Find(id);
            db.Seminars.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowSem");
        }

        public IActionResult DelPractical(int id)
        {
            var practical = db.Practicals
                              .Include(p => p.PracticalFacultyNavigation)  
                              .Include(p => p.PracticalCategoryNavigation) 
                              .FirstOrDefault(p => p.PracId == id);

            if (practical != null)
            {

                db.Practicals.Remove(practical);  
                db.SaveChanges();  
            }

            return RedirectToAction("ShowPrac");  
        }

        public IActionResult DelPracCategory(int id)
        {
            var category = db.PracticalCategories
                .Include(c => c.Practicals) 
                .FirstOrDefault(c => c.PracCatId == id);

            if (category == null) return NotFound();

            if (category.Practicals.Any())
            {
                TempData["Error"] = "Cannot delete category with related practicals.";
                return RedirectToAction("ShowPracCategory");
            }

            db.PracticalCategories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("ShowPracCategory");
        }



        public IActionResult DelCategory(int id)
        {
            var dataid = db.Categorys.Find(id);
            db.Categorys.Remove(dataid);
            db.SaveChanges();
            return RedirectToAction("ShowCategory");
        }

        public IActionResult DelBank(int id)
        {
            var bank = db.Banks.Find(id);
            if (bank == null) return NotFound();

            db.Banks.Remove(bank);
            db.SaveChanges();

            return RedirectToAction("ShowBank");
        }

        public IActionResult DelMedicineOrder(int id)
        {
            var order = db.MedicineOrders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.MedicineOrders.Remove(order);
            db.SaveChanges();

            return RedirectToAction("ShowMedOrders");
        }

        public IActionResult DelScientificOrder(int id)
        {
            var order = db.ScientificOrders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.ScientificOrders.Remove(order);
            db.SaveChanges();

            return RedirectToAction("ShowScientificOrders"); 
        }

        public IActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return NotFound();

            // Delete dependent RecruiterRequests
            var recruiterRequests = db.RecruiterRequests.Where(r => r.UserId == id).ToList();
            db.RecruiterRequests.RemoveRange(recruiterRequests);

            db.Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("ShowUsers");
        }





        [HttpGet]
        public IActionResult ShowUsers()
        {
            // Include Role to get role details with each user
            var users = db.Users.Include(u => u.Role).ToList();
            return View(users);
        }


        [HttpGet]
        public IActionResult ShowMed()
        {
            var showdata = db.MedicinesInfos.Include(p => p.MedCatNavigation);
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowMedCategory()
        {
            var showdata = db.MedicinesCategories.Include(p => p.MedicinesInfos);
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowSci()
        {
            var showdata = db.ScientificInfos.Include(p => p.ScientificCatNavigation);
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
        public IActionResult ShowFaculty()
        {
            var data = db.Faculties
                         .Include(f => f.Lectures)
                         .ToList();
            return View(data);
        }



        [HttpGet]
        public IActionResult ShowLec()
        {
            var showdata = db.Lectures.Include(p => p.LecFacultyNavigation);
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowSem()
        {
            var showdata = db.Seminars.Include(p => p.SemFacultyNavigation);
            var data = showdata.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult ShowPrac()
        {
            var showdata = db.Practicals.Include(p => p.PracticalFacultyNavigation);
            var data = showdata.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult ShowPracCategory()
        {
            var showdata = db.PracticalCategories.Include(p => p.Practicals);
            var data = showdata.ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult ShowCategory()
        {
            var showdata = db.Categorys;
            var data = showdata.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult ShowCarousel()
        {
            var carouselImages = db.Carousels.ToList();
            return View(carouselImages);
        }

        public IActionResult ShowFeedback()
        {
            var feedbacks = db.Feedbacks.ToList();
            return View(feedbacks);
        }

        [HttpGet]
        public IActionResult ShowMedOrders()
        {
            var orders = db.MedicineOrders.ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult ShowScientificOrders()
        {
            var orders = db.ScientificOrders.ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult ShowBank()
        {
            var data = db.Banks.ToList();
            return View(data);
        }



    }
}
