using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Banha_UniverCity.Models;
using Stripe.Climate;
using System.Net.Mail;

namespace Banha_UniverCity
{
    public static class StaticData
    {

        public static int StaticDataPageSize = 6;

        public static string StaticDataSuccessPayment = "Success-Confirmed";
        public static string StaticDataRefundedPayment = "Payment-Refunded";
        public static string StaticDataInProcessPayment = "In-Process";
        public static string StaticDataPending = "Pending";



        //Static Data For Roles 
        public static string role_Admin = "Admin";
        public static string role_Student = "Student";
        public static string role_Instructor = "Instructor";
        public static string role_Customer = "Customer";

        //Static Data For Course Status
        public static string In_Process_Status="In-Process";
        public static string Comming_Status = "In-Process";
        public static string Available_Status = "Available";
        public static string Full_Status = "Full";



        public static JsonResult CheckValidation(ModelStateDictionary modelState, HttpRequest request, bool state)
        {

            if (request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                if (state != true)
                {

                    var nameErrors = modelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToList());

                    return new JsonResult(new { isvalid = false, nameErrors });
                }
                else
                {
                    return new JsonResult(new { isvalid = true });

                }
            }
            return null!;




        }
        public static IEnumerable< ApplicationUser> GetUsers(UserManager<IdentityUser> _userManager)
        {
            var users = _userManager.Users.ToList();
            var listOfUsers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                listOfUsers.Add(user as ApplicationUser);
            }
            return listOfUsers;
        }


        public static bool SendConfirmationEmail(string userEmail, Order order)
        {

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(userEmail);
                mail.From = new MailAddress("oa38150@gmail.com");
                mail.Subject = "Booking Confirmation";
                //mail.Body = $"Dear {order.APplicaUser.UserName},THank you";
                mail.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com ",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("oa38150@gmail.com", "deeo bpmm pmty pzcf"),
                    EnableSsl = true,

                };


                smtp.Send(mail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


    }
}

