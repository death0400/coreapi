using Core.Data.GemTech.Model.Poco;
using Db.CommomLotteryData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Core.Data.GemTech.Model
{
    public class Pipe
    {
        private readonly GemTechConfig config;
        private readonly TelegramBot bot;
        private readonly Common_LotteryDataContext context;
        public Pipe(GemTechConfig config, Common_LotteryDataContext context)
        {
            this.config = config;
            bot = this.config.TelegramBot;
            this.context = context;
        }

        public Action<GemTechData[]> SaveSscDataToSqlServer => (datas) =>
        {
            context.CqsscGameWinNumber.AddRange(
            datas.Select(x => GemTechData.ConvertToCqsscGameWinNumber(x)));
            context.SaveChanges();
            context.Dispose();
        };
        public Func<HttpRequest, Action<GemTechData[]>> MonitorByTelegramApiAsync => (request) =>
        {
            var strBuilder = new StringBuilder();
            var keys = request.Form.Keys;
            var body = request.Body;
            var hasData = keys.Any();
            return (datas) =>
            {
                if (hasData)
                {
                    foreach (var data in datas)
                    {
                        strBuilder.AppendLine(data.ToString());
                    }
                }
                else
                {
                    strBuilder.AppendLine("No Data");
                }
                if (bot.IsEnable && hasData)
                {
                    strBuilder.AppendLine("--Url--------------------");
                    strBuilder.AppendLine(request.Path);
                    strBuilder.AppendLine("--HttpRequestHeader--------------------");
                    strBuilder.AppendLine(string.Join("\r\n", request.Headers.Select(x => x.Key + ":" + x.Value).ToArray()));
                    strBuilder.AppendLine("--Form--------------------");
                    strBuilder.AppendLine(string.Join("r\n", request.Form.Select(x => x.Key + ":" + x.Value).ToArray()));
                    strBuilder.AppendLine("--------------this is area-line-----------By Paul.Lin-----");
                    bot.SendMessageAsync(strBuilder.ToString()).Wait();
                }
                if (!hasData)
                {
                    using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
                    {
                        strBuilder.AppendLine("body==========" + reader.ReadToEnd());
                    }
                    throw new Exception(strBuilder.ToString());
                }
            };
        };
        public Func<string, Action<GemTechData[]>> PostToRedis => (url) =>
        {
            return (datas) =>
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    using (var client = new HttpClient())
                    {
                        var json = JsonConvert.SerializeObject(new
                        {
                            GameIssuse = datas.Select(x => GemTechData.ConvertToCqsscGameWinNumber(x))
                        });
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var result = client.PostAsync(url, content).Result;
                        var times = 0;
                        while (result.StatusCode != System.Net.HttpStatusCode.OK && times < 5)
                        {
                            times++;
                            Thread.Sleep(1000);
                            result = client.PostAsync(url, content).Result;
                        }
                        if (times > 5)
                            throw new Exception("push redis fault");
                    }
                }
            };
        };
    }
}
