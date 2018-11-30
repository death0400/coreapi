using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Core.Data.GemTech
{
    public class TelegramBot
    {
        public bool IsEnable { get; set; } = false;
        public string BotId { get; set; }
        public string ChatId { get; set; }

        private TelegramBotClient bot;

        public Task SendMessageAsync(string message)
        {
            if (IsEnable || !string.IsNullOrWhiteSpace(message))
            {
                if (bot == null)
                    bot = new TelegramBotClient(BotId);
                 bot.SendTextMessageAsync(this.ChatId, message);
            }
            return Task.CompletedTask;
        }
    }
}
