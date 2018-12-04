using Core.Data.GemTech.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.GemTech
{
    public class GemTechConfig
    {
        public string GemTechKey { get; set; }
        public TelegramBot TelegramBot { get; set; }

        public RedisUrlCollection RedisUrlCollection { get; set; }

  
    }

}
