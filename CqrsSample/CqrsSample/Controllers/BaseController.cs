using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paralect.Domain;

namespace CqrsSample.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ICommandService CommandService { get; set; }

        protected string UserId
        {
            get
            {
                return "Admin"; // replace with real userId }
            }
        }

        protected BaseController(ICommandService commandService)
        {
            CommandService = commandService;
        }

        public virtual void Send(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                command.Metadata.UserId = UserId;
            }

            CommandService.Send(commands);
        }
    }
}
