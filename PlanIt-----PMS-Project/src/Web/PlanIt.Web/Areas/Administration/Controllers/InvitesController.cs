namespace PlanIt.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PlanIt.Data.Common.Repositories;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Invites;

    public class InvitesController : AdministrationController
    {
        private readonly IInvitesService invitesService;
        private readonly IRepository<Invite> repository;

        public InvitesController(IInvitesService invitesService, IRepository<Invite> repository)
        {
            this.invitesService = invitesService;
            this.repository = repository;
        }

        // GET: Invites
        public async Task<ActionResult> Index()
        {
            var invites = await this.invitesService.GetAllAsync<InviteViewModel>();
            var model = new InvitesListViewModel
            {
                Invites = invites,
            };

            return this.View(model);
        }

        // GET: Invites/Details/5
        public ActionResult Details(int id)
        {
            return this.View();
        }

        // GET: Invites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Invites/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Invites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Invites/Delete/
        public async Task<ActionResult> Delete()
        {
            var invites = await this.invitesService
                .GetAllAsync<InviteDeleteViewModel>();

            var model = new InviteSelectViewModel
            {
                Invites = new SelectList(invites, "Id", "Email"),
            };

            return this.View(model);
        }

        // POST: Invites/Delete/id
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var invite = await this.invitesService.GetByIdAsync<InviteDeleteViewModel>(id);

            if (invite == null)
            {
                return this.NotFound();
            }

            await this.invitesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}