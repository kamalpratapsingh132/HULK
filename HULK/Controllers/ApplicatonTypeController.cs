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
    public class ApplicatonTypeController : Controller
    {
       
        private readonly ApplicationDBContext dbcontext;

        public ApplicatonTypeController(ApplicationDBContext context)
        {
            dbcontext = context;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objlist = dbcontext.ApplicationType;
            return View(objlist);
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            dbcontext.ApplicationType.Add(obj);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = dbcontext.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                dbcontext.ApplicationType.Update(obj);
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
            var obj = dbcontext.ApplicationType.Find(id);
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
            var obj = dbcontext.ApplicationType.Find(id);
            if (obj == null)
            {

                return NotFound();
            }
            dbcontext.ApplicationType.Remove(obj);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}
