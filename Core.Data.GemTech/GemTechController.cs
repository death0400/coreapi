using Core.Data.GemTech.Model;
using Core.Data.GemTech.Model.Poco;
using Db.CommomLotteryData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Core.Data.GemTech
{
    [Route("api/[controller]")]
    public class GemTechController : ControllerBase
    {
        private readonly GemTechConfig config;
        private readonly TelegramBot bot;
        private readonly ILogger<GemTechController> logger;
        private readonly Pipe pipe;
        public GemTechController(ILogger<GemTechController> logger,GemTechConfig config,Pipe pipe)
        {
            this.config = config;
            this.bot = config.TelegramBot;
            this.logger = logger;
            this.pipe = pipe;
        }
        [HttpPost("set")]
        public async Task<string> Recived()
        {
            try
            {
                var datas = new GemTechDataHandler(Request.Form.Keys,config)
                    .AddActionPipe(pipe.MonitorByTelegramApiAsync(Request))
                    .AddActionPipe(pipe.SaveSscDataToSqlServer)
                    .AddActionPipe(pipe.PostToRedis(config.TaocaiRedisApiUrl))
                    .Run();
                return "OK";
            }
            catch (Exception e)
            {
                logger.LogInformation(e.ToString());
                if (bot.IsEnable)
                    await bot.SendMessageAsync(e.ToString());
            }
            return "False";
        }
    }
}
