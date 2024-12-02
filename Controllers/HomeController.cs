using Microsoft.AspNetCore.Mvc;
using Email.Services;

public class HomeController : Controller
{
    private readonly EmailService _emailService;

    public HomeController(EmailService emailService)
    {
        _emailService = emailService;
    }

    // Home Page
    public IActionResult Index()
    {
        return View();
    }

    // Handle Compliments
    [HttpPost]
    public IActionResult SubmitCompliment(string name, string message)
    {
        try
        {
            string subject = "New Compliment from Your Website";
            string body = $@"
                <h3>New Compliment Received</h3>
                <p><strong>Name:</strong> {name}</p>
                <p><strong>Message:</strong></p>
                <p>{message}</p>
            ";

            // Send the email to the provided Gmail address
            bool isSent = _emailService.SendEmail("anbumanickam28@gmail.com", subject, body);

            if (isSent)
            {
                TempData["Message"] = "Thank you for your compliment! We have received your message.";
            }
            else
            {
                TempData["Message"] = "Sorry, something went wrong. Please try again.";
            }
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    // Resume Download
    public IActionResult DownloadResume()
    {
        var resumePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "ANBUMANICKAM-Resume.pdf");
        return PhysicalFile(resumePath, "application/pdf", "Resume.pdf");
    }
}
