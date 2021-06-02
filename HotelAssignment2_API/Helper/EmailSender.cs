using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAssignment2_API.Helper
{
  public class EmailSender :IEmailSender
  {
    private readonly MailJetSettings _mailjetSettings;

    //services.Configure에 등록한 json 객체를 가져오는 코드
    public EmailSender(IOptions<MailJetSettings> mailjetSettings)
    {
      _mailjetSettings = mailjetSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable(_mailjetSettings.PublicKey), Environment.GetEnvironmentVariable(_mailjetSettings.PrivateKey));
      MailjetRequest request = new MailjetRequest
      {
        Resource = Send.Resource,
      }
         .Property(Send.FromEmail, _mailjetSettings.Email)
         .Property(Send.FromName, "Mailjet Pilot")
         .Property(Send.Subject, "Your email flight plan!")
         .Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
         .Property(Send.HtmlPart, "<h3>Dear passenger, welcome to <a href=\"https://www.mailjet.com/\">Mailjet</a>!<br />May the delivery force be with you!")
         .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                  ,{"Name","dummyRecipiName"}
                 }
             });
      MailjetResponse response = await client.PostAsync(request);
      if (response.IsSuccessStatusCode)
      {
        Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
        Console.WriteLine(response.GetData());
      }
      else
      {
        Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
        Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
        Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
      }
    }
  }
}
