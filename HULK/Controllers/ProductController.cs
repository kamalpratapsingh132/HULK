using HULK.Data;
using HULK.Models;
using HULK.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HULK.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {


        private readonly ApplicationDBContext dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            dbcontext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objlist = dbcontext.Product;

            foreach(var obj in objlist)
            {
                obj.Category = dbcontext.Category.FirstOrDefault(e => e.Id == obj.CategoryId);
                obj.ApplicationType = dbcontext.ApplicationType.FirstOrDefault(e => e.Id == obj.ApplicationTypeId);
            };
            return View(objlist);
        }




        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = dbcontext.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = dbcontext.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeSelectList = dbcontext.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = dbcontext.Product.Find(id);
                if(productVM == null)
                {
                    return NotFound();
                }
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName); 

                    using(var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    dbcontext.Product.Add(productVM.Product);
                }
                else
                {
                    //Updating
                    var objFromDb = dbcontext.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    dbcontext.Update(productVM.Product);
                }
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = dbcontext.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = dbcontext.Product.Find(id);
            if(obj ==null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                dbcontext.Product.Update(obj);
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = dbcontext.Product.Include(u=>u.Category).Include(u=>u.ApplicationType).FirstOrDefault(u=>u.Id==id);
            product.Category = dbcontext.Category.Find(product.CategoryId);
            var obj = dbcontext.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = dbcontext.Product.Find(id);
            if (obj == null)
            {

                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);


            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            dbcontext.Product.Remove(obj);
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            
          
        }
    }
}
