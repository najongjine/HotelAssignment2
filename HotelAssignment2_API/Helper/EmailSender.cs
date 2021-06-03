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
      MailjetClient client = new MailjetClient(_mailjetSettings.PublicKey, _mailjetSettings.PrivateKey);
      MailjetRequest request = new MailjetRequest
      {
        Resource = Send.Resource,
      }
         .Property(Send.FromEmail, _mailjetSettings.Email)

         //상대방이 받았을때 보낸사람의 이름
         .Property(Send.FromName, "From Hidden Villa")

         //메일 제목
         .Property(Send.Subject, subject)

         //뭔지 모름. 메일함에 표시도 안됨
         .Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")

         //본문내용.
         .Property(Send.HtmlPart, htmlMessage)

         //받는 사람들 목록 정해주기
         .Property(Send.Recipients, new JArray {
                new JObject {
                  //받는 사람의 이메일
                 {"Email", email}
                  ,{"Name","dummyRecipiName"} // 받은 사람의 닉네임 정해주기. 없어도 됨
                 }
                ,new JObject {
                 {"Email", "najongjin3@hotmail.com"}
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
