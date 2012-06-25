using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using CqrsSample.Domain.Comands;
using CqrsSample.EventHandlers;
using CqrsSample.Models;

namespace CqrsSample.Controllers
{
    public class UsersController : BaseController
    {
        public List<User> Users { get { return UserEntityEventHandler.Users; } } 

        public UsersController(ICommandService commandService) : base(commandService)
        {
        }

        public ActionResult Index()
        {
            return View(Users);
        }

        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var cmd = new User_ChangePasswordCommand
                          {
                              NewPassword = model.NewPassword,
                              OldPassword = model.OldPassword,
                              UserId = model.UserId
                          };
            try
            {
                Send(cmd);
            }
            catch (AuthenticationException)
            {
                return Content("Invalid password.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create(CreateUserModel model)
        {
            var cmd = new User_CreateCommand
            {
                Name = model.Name,
                Password = model.Password,
                UserId = Guid.NewGuid().ToString()
            };       
            Send(cmd);
            return RedirectToAction("Index");
        }
    }
}
