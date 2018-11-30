using Core.Data.GemTech.Model;
using Core.Data.GemTech.Model.Poco;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.GemTech
{
    public class GemTechDataHandler
    {
        private readonly GemTechConfig config;
        private readonly ICollection<string> keys;
        bool hasData;
        public Action<GemTechData[]> action;
        public GemTechDataHandler(ICollection<string> keys, GemTechConfig config)
        {
            this.keys = keys;
            this.config = config;
        }
        public GemTechData[] Run()
        {
            hasData = keys.Any();
            GemTechData[] datas = keys.Select(data => data.DecrypGemTechDataString(config.GemTechKey)).ToArray();
            //action pipe here
            action?.Invoke(datas);
            return datas;
        }
        public GemTechDataHandler AddActionPipe(Action<GemTechData[]> _action)
        {
            this.action += _action;
            return this;
        }
    }
}
