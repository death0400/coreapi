using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.GemTech
{
    public class GemTechConfig
    {
        public string GemTechKey { get; set; }
        public TelegramBot TelegramBot { get; set; }

        public string TestRedisApiUrl { get; set; }
        public string ToucaiRedisApiUrl { get; set; }
        public string TaocaiRedisApiUrl { get; set; }
        public string HuancaiRedisApiUrl { get; set; }
    }

}
