namespace PlanIt.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Web.ViewModels.Invites;

    public class InvitesController : AdministrationController
    {
        private readonly IInvitesService invitesService;
        private readonly UserManager<PlanItUser> userManager;

        public InvitesController(
            IInvitesService invitesService,
            UserManager<PlanItUser> userManager)
        {
            this.invitesService = invitesService;
            this.userManager = userManager;
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

        // GET: Invites/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Invites/Create
        [HttpPost]
        public async Task<ActionResult> Create(InviteCreateInputModel inputModel)
        {
            var existingUser = await this.userManager
                .Users
                .SingleOrDefaultAsync(u => u.Email == inputModel.Email);

            var existingInvite = await this.invitesService
                .GetByEmailAsync<InviteCreateInputModel>(inputModel.Email);

            if (existingUser != null)
            {
                this.ModelState.AddModelError(string.Empty, $"User with email {existingUser.Email} already exists!!!");
            }

            if (existingInvite != null)
            {
                this.ModelState.AddModelError(string.Empty, $"Invite with email {existingInvite.Email} already exists!!!");
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.invitesService.CreateAsync<InviteCreateInputModel>(inputModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(inputModel);
        }

        // POST: Invites/Approve/id
        [HttpPost]
        public async Task<ActionResult> Approve(int id)
        {
            var invite = await this.invitesService
                .GetByIdAsync<InviteApproveInputModel>(id);

            if (invite == null)
            {
                return this.NotFound();
            }

            await this.invitesService.ApproveAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Invites/Find/id
        public async Task<ActionResult> Find(int id)
        {
            var invite = await this.invitesService
                 .GetByIdAsync<InviteEditInputModel>(id);

            if (invite == null)
            {
                return this.NotFound();
            }

            return new JsonResult(invite);
        }

        // GET: Invites/Edit/id?
        public async Task<ActionResult> Edit(int? id)
        {
            var invites = await this.invitesService
                .GetAllAsync<InviteEditInputModel>();

            this.ViewData["Invites"] = invites;

            if (id == null)
            {
                return this.View();
            }

            var invite = await this.invitesService
                .GetByIdAsync<InviteEditInputModel>(id);

            if (invite == null)
            {
                return this.NotFound();
            }

            return this.View(invite);
        }

        // POST: Invites/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, InviteEditInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.invitesService.EditAsync<InviteEditInputModel>(inputModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existInvite = await this.invitesService.GetByIdAsync<InviteEditInputModel>(inputModel.Id);
                    if (existInvite == null)
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(inputModel);
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
