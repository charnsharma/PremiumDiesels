using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using PremiumDiesel.Model.Constants;
using PremiumDiesel.Model.Context;
using PremiumDiesel.Model.DTOs;
using PremiumDiesel.Model.Models;
using PremiumDiesel.Repository.UnitOfWork;

namespace PremiumDiesel.Web.Controllers
{
    [Authorize(Roles = UserRoles.SuperAdmin)]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClientDTO
        public ActionResult Index()
        {
            using (var unitOfWork = new UnitOfWork(db))
            {
                var clients = unitOfWork.Clients.GetAll();
                var clientDTOs = AutoMapper.Mapper.Map<IEnumerable<ClientDTO>>(clients);

                return View(clientDTOs);
            }
        }

        // GET: ClientDTO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            using (var unitOfWork = new UnitOfWork(db))
            {
                var client = unitOfWork.Clients.Get((int)id);
                if (client == null)
                    return HttpNotFound();

                var clientDTO = AutoMapper.Mapper.Map<ClientDTO>(client);
                return View(clientDTO);
            }
        }

        // GET: ClientDTO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientDTO/Create To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientDTO clientDTO)
        {
            if (ModelState.IsValid)
            {
                using (var unitOfWork = new UnitOfWork(db))
                {
                    var client = AutoMapper.Mapper.Map<Client>(clientDTO);

                    client.CreatedDate = DateTime.UtcNow;
                    client.CreatedByUserId = MyHelper.CurrentUserId;

                    unitOfWork.Clients.Add(client);
                    unitOfWork.SaveChanges();
                }
                //db.ClientDTOes.Add(clientDTO);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clientDTO);
        }

        // GET: ClientDTO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var unitOfWork = new UnitOfWork(db))
            {
                var client = unitOfWork.Clients.Get((int)id);
                var clientDTO = AutoMapper.Mapper.Map<ClientDTO>(client);

                if (clientDTO == null)
                    return HttpNotFound();

                //ClientDTO clientDTO = db.ClientDTOes.Find(id);
                //if (clientDTO == null)
                //{
                //    return HttpNotFound();
                //}
                return View(clientDTO);
            }
        }

        // POST: ClientDTO/Edit/5 To protect from overposting attacks, please enable the specific
        // properties you want to bind to, for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientDTO clientDTO)
        {
            if (ModelState.IsValid)
            {
                using (var unitOfWork = new UnitOfWork(db))
                {
                    var client = AutoMapper.Mapper.Map<Client>(clientDTO);

                    unitOfWork.SaveChanges();
                }
                //db.Entry(clientDTO).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientDTO);
        }

        // GET: ClientDTO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            using (var unitOfWork = new UnitOfWork(db))
            {
                var client = unitOfWork.Clients.Get((int)id);
                var clientDTO = AutoMapper.Mapper.Map<ClientDTO>(client);
                //ClientDTO clientDTO = db.ClientDTOes.Find(id);
                //ClientDTO clientDTO = db.ClientDTOes.Find(id);
                if (clientDTO == null)
                    return HttpNotFound();

                return View(clientDTO);
            }
        }

        // POST: ClientDTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var unitOfWork = new UnitOfWork(db))
            {
                //ClientDTO clientDTO = db.ClientDTOes.Find(id);
                //db.ClientDTOes.Remove(clientDTO);
                //db.SaveChanges();
                var client = unitOfWork.Clients.Get(id);
                unitOfWork.Clients.Delete(client);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}