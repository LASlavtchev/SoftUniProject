﻿namespace PlanIt.Web.Areas.Administration.Controllers
{
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.EntityFrameworkCore;
    using PlanIt.Common;
    using PlanIt.Data.Models;
    using PlanIt.Services.Data;
    using PlanIt.Services.Messaging;
    using PlanIt.Web.Extensions;
    using PlanIt.Web.ViewModels.Administration.Invites;

    public class InvitesController : AdministrationController
    {
        private readonly IInvitesService invitesService;
        private readonly UserManager<PlanItUser> userManager;
        private readonly IEmailSender emailSender;

        public InvitesController(
            IInvitesService invitesService,
            UserManager<PlanItUser> userManager,
            IEmailSender emailSender)
        {
            this.invitesService = invitesService;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        // GET: Invites
        public async Task<IActionResult> Index()
        {
            var invites = await this.invitesService.GetAllAsync<InviteViewModel>();
            var model = new InvitesListViewModel
            {
                Invites = invites,
            };

            return this.View(model);
        }

        // GET: Invites/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Invites/Create
        [HttpPost]
        public async Task<IActionResult> Create(InviteCreateInputModel inputModel)
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
                    var invite = await this.invitesService.CreateAsync<InviteCreateInputModel>(inputModel);

                    var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(invite.SecurityValue));
                    var callbackUrl = this.Url.Page(
                        "/Account/Register",
                        pageHandler: null,
                        values: new { area = "Identity", inviteId = invite.Id, code },
                        protocol: this.Request.Scheme);

                    var messageToSend = $"You are invited to our platform. " +
                        $"Please register till {invite.ExpiredOn}(UTC) by: " +
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

                    await this.SendEmailAsync(messageToSend, invite.Email);
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
        public async Task<IActionResult> Approve(int id)
        {
            var inputInvite = await this.invitesService
                .GetByIdAsync<InviteApproveInputModel>(id);

            if (inputInvite == null)
            {
                return this.NotFound();
            }

            var invite = await this.invitesService.ApproveAsync(id);

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(invite.SecurityValue));
            var callbackUrl = this.Url.Page(
                "/Account/Register",
                pageHandler: null,
                values: new { area = "Identity", inviteId = invite.Id, code },
                protocol: this.Request.Scheme);

            var messageToSend = $"Your request invitation was approved. Please register till {invite.ExpiredOn}(UTC) by: " +
                            $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

            await this.SendEmailAsync(messageToSend, invite.Email);

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Invites/Extend/id?
        public async Task<IActionResult> Extend(int? id)
        {
            var invites = await this.invitesService
                .GetAllAsync<InviteExtendInputModel>();

            this.ViewData["Invites"] = invites;

            if (id == null)
            {
                return this.View();
            }

            var invite = await this.invitesService
                .GetByIdAsync<InviteExtendInputModel>(id);

            if (invite == null)
            {
                return this.NotFound();
            }

            return this.View(invite);
        }

        // POST: Invites/Extend/5
        [HttpPost]
        public async Task<IActionResult> Extend(int id, InviteExtendInputModel inputModel)
        {
            if (id != inputModel.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    var invite = await this.invitesService.ExtendAsync<InviteExtendInputModel>(inputModel);

                    var messageToSend = $"Your request invitation was extended to {invite.RequestExpiredOn}(UTC). " +
                        $"If you had been approved please respond to our previous email";

                    await this.SendEmailAsync(messageToSend, invite.Email);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existInvite = await this.invitesService.GetByIdAsync<InviteExtendInputModel>(inputModel.Id);
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

            var invites = await this.invitesService
                .GetAllAsync<InviteExtendInputModel>();

            this.ViewData["Invites"] = invites;

            return this.View(inputModel);
        }

        // GET: Invites/Delete/
        public async Task<IActionResult> Delete()
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
        public async Task<IActionResult> Delete(int? id)
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

            var messageToSend = $"Your request invitation was denied.";

            await this.SendEmailAsync(messageToSend, invite.Email);

            return this.RedirectToAction(nameof(this.Index));
        }

        [NonAction]
        private async Task SendEmailAsync(string messageToSend, string email)
        {
            await this.emailSender.SendEmailAsync(
                $"{this.User.Identity.Name}",
                $"{this.User.GetFullName()}",
                email,
                $"Invitation to {GlobalConstants.SystemName}",
                $"{messageToSend}");
        }
    }
}
