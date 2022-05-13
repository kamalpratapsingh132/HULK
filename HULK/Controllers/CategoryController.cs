using HULK.Data;
using HULK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HULK.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
       
        private readonly ApplicationDBContext dbcontext;

        public CategoryController (ApplicationDBContext context)
        {
            dbcontext = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objlist = dbcontext.Category;
            return View(objlist);
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(ModelState.IsValid)
            {
                dbcontext.Category.Add(obj);
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
           
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = dbcontext.Category.Find(id);
            if(obj ==null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                dbcontext.Category.Update(obj);
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
            var obj = dbcontext.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = dbcontext.Category.Find(id);
            if (obj == null)
            {

                return NotFound();
            }
                dbcontext.Category.Remove(obj);
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            
          
        }
    }
}
