using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicsTMS365.Controllers
{
    public class vLookupListController : Controller
    {
        // GET: vLookupList
        public ActionResult Index()
        {
            return View();
        }

        // GET: vLookupList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: vLookupList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: vLookupList/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: vLookupList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: vLookupList/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: vLookupList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: vLookupList/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
