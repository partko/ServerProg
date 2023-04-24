using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Lab4.Services;
using static Lab4.Pages.GeneralModel;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lab4.Pages
{
    public class EmailMessage
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Select_service { get; set; }
        public string Select_price { get; set; }
        public string Comments { get; set; }

    }

    [IgnoreAntiforgeryToken]
    public class contactModel : GeneralModel
    {
        public contactModel(IDataReader reader) : base(reader, "contact")
        {
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public EmailMessage Message { get; set; }

        public IActionResult OnPost()
        {
            if (Message.First_Name == null || Message.First_Name == "")
            {
                return Content("<div class=\"error_message\">Attention! You must enter your name.</div>");
            }
            else if (Message.Last_Name == null || Message.Last_Name == "")
            {
                return Content("<div class=\"error_message\">Attention! You must enter your last name.</div>");
            }
            else if (Message.Email == null || Message.Email == "")
            {
                return Content("<div class=\"error_message\">Attention! Please enter a valid email address.</div>");
            }
            else if (!isEmail(Message.Email))
            {
                return Content("<div class=\"error_message\">Attention! You have enter an invalid e-mail address, try again.</div>");
            }
            else if (Message.Comments == null || Message.Comments == "")
            {
                return Content("<div class=\"error_message\">Attention! Please enter your message.</div>");
            }
            else
            {
                WriteFile("contact.csv", Message);
                return Content($@"<fieldset>
                        <div id='success_page'> 
                        <h1>Email Sent Successfully.</h1>
                        <p>Thank you <strong>{Message.First_Name} {Message.Last_Name}</strong>, your message has been submitted to us.<p>
                        </div>
                    <fieldset>");
            }
        }

        public bool isEmail(string email)
        {
            return Regex.IsMatch(email, "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        }
        private void WriteFile(string filepath, EmailMessage message)
        {
            var isPathExist = !System.IO.File.Exists(filepath);
            using (var writer = new StreamWriter(filepath, true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                if (isPathExist)
                {
                    csv.WriteHeader<EmailMessage>();
                    csv.NextRecord();
                }
                csv.WriteRecord(message);
                csv.NextRecord();
            }
        }
    }
}
