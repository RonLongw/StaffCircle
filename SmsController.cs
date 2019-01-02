using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using api.Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Cors;
using RepositoryCore.Model;
using RepositoryCore.Repositories;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.DataAccessLayer;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SmsController : ControllerBase
    {
        private string conn = "Data Source=DESKTOP-IPPVKT1\\SQLEXPRESS;Initial Catalog=SMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        [HttpGet]
        public List<SmsMessage> Get()
        {
            List<SmsMessage> result = null;

            var optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();
            optionsBuilder.UseSqlServer(conn);

            using (var context = new SmsDbContext(optionsBuilder.Options))
            {
                var sms = new EFSmsMessageRepository(context);

                result = sms.GetAll();
            }

            Response.Headers.Append("Access-Control-Allow-Origin", "*");

            return result;
        }

        [HttpGet]
        [Route("GetAll")]
        public List<SmsMessage> GetAll()
        {
            List<SmsMessage> result = null;

            var optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();
            optionsBuilder.UseSqlServer(conn);

            using (var context = new SmsDbContext(optionsBuilder.Options))
            {
                var sms = new EFSmsMessageRepository(context);

                result = sms.GetAll();
            }

            Response.Headers.Append("Access-Control-Allow-Origin", "*");

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public SmsMessage Get(long id)
        {
            SmsMessage result = null;

            var optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();
            optionsBuilder.UseSqlServer(conn);

            using (var context = new SmsDbContext(optionsBuilder.Options))
            {
                var sms = new EFSmsMessageRepository(context);

                result = sms.Find(s => s.Id == id);
            }

            Response.Headers.Append("Access-Control-Allow-Origin", "*");

            return result;
        }

        [HttpGet]
        [Route("TwilioAccount/{id}")]
        public TwilioAccount GetTwilioAccount(long id)
        {
            TwilioAccount result = null;

            var optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();
            optionsBuilder.UseSqlServer(conn);

            using (var context = new SmsDbContext(optionsBuilder.Options))
            {
                var tw = new EFTwilioAccountsRepository(context);

                result = tw.Find(t => t.Id == id);
            }

            Response.Headers.Append("Access-Control-Allow-Origin", "*");

            return result;
        }

        //public async Task<IActionResult> Post(HttpRequestMessage request)
        [HttpPost]
        public IActionResult Post(Sms sms)
        {
            const string accountSid = "ACd558b971d3acfea6621b55da91447dfc";
            const string authToken = "f1cc381607948552ed28cf898aa9101c";

            var number = sms.Number;
            var message = sms.Message;

            try
            {
                TwilioClient.Init(accountSid, authToken);

                var smsMessage = MessageResource.Create(
                    body: sms.Message,
                    from: new Twilio.Types.PhoneNumber("+441980322062"),
                    to: new Twilio.Types.PhoneNumber("+44" + sms.Number.TrimStart('0'))
                );

                var optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();
                optionsBuilder.UseSqlServer(conn);

                using (var context = new SmsDbContext(optionsBuilder.Options))
                {
                    var repository = new EFSmsMessageRepository(context);

                    var result = repository.Add(new SmsMessage() { Message = sms.Message, Number = sms.Number, Created = DateTime.Now });

                    if (result == 0)
                    {
                        return this.BadRequest("Failed to save SMS to the database.");
                    }
                }
            }
            catch (Exception exc)
            {
                var msg = exc.Message;
                return this.BadRequest("Failed to send SMS");
            }

            Response.Headers.Append("Access-Control-Allow-Origin", "*");
            return this.Ok("Everything OK");
        }

        [HttpPost]
        [Route("AddTwilioAccount")]
        public IActionResult AddTwilioAccount(TwilioAccount tw)
        {  
            var optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();
            optionsBuilder.UseSqlServer(conn);

            using (var context = new SmsDbContext(optionsBuilder.Options))
            {
                var repository = new EFTwilioAccountsRepository(context);

                var result = repository.Add(new TwilioAccount() { AccountSid = tw.AccountSid, AuthToken = tw.AuthToken, Created = DateTime.Now });

                if (result == 0)
                {
                    return this.BadRequest("Failed to save twilio account to the database.");
                }
            }
            
            Response.Headers.Append("Access-Control-Allow-Origin", "*");
            return this.Ok("Everything OK");
        }
    }
}
