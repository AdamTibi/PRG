# AdamTibi.Web.Prg NuGet package and sample code
This is an ASP.NET MVC Core NuGet Package to support the PRG pattern. You can see a description about the PRG pattern here: https://andrewlock.net/post-redirect-get-using-tempdata-in-asp-net-core/ and some of the code used in this package is adopted from this post by Andrew Lock.

The package includes 2 attributes, `PreserveModelStateAttribute` and `RestoreModelStateAttribute` that you should use on your action methods when you intend to redirect the PRG style. These two classes will provide a clean way of saving and restoring your `ModelState`. 

# Using the NuGet package
- Your project should support TempData, to enable TempData: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.2#tempdata
- Add support to the NuGet package AdamTibi.Web.Prg fro NuGet.org
- Below is an example of how to use the package:

```C#
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
```

