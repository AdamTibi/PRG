using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdamTibi.Web.Website.ViewModels;
using AdamTibi.Web.Prg;
using System;

namespace AdamTibi.Web.Website.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [RestoreModelState(nameof(ContactUsSubmit))]  // Add here
        public IActionResult Index()
        {
            ContactUsViewModel contactUs = new ContactUsViewModel
            {
                CurrentTime = DateTime.Now,
                ResponseTime = 2 // Assume this is a dynamic value that we are getting from somewhere
            };
            return View(contactUs);
        }

        [HttpPost]
        [PreserveModelState(nameof(ContactUsSubmit))] // Add here
        public IActionResult ContactUsSubmit(ContactUsViewModel contactUs)
        {
            if (!ModelState.IsValid)
            {
                // Show the form with error messages
                // This is the right way, PRG, redirecting, rather than returning the View
                return Redirect("/");
            }

            // process email and message from the contactUs object
            // ...

            // Show the user a thank you page.
            // This is the right way, PRG, redirecting, rather than returning the View
            return Redirect("thank-you");
        }

        [HttpGet]
        [ActionName("thank-you")]
        public IActionResult ContactUsThankYou()
        {
            return View("ContactUsThankYou");
        }

    }
}