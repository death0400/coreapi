using Db.CommomLotteryData.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Data.GemTech.Model.Poco
{

    public class GemTechData
    {
        public int Event { get; set; }
        public int Priority { get; set; }
        public string Serial { get; set; }
        public string Handler { get; set; }
        public string Version { get; set; }
        public DateTime Time { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public InnerData Data { get; set; }
        public class InnerData
        {
            public string Lottery { get; set; }
            public string Issue { get; set; }
            public string Code { get; set; }

        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine( "Lottery:"+this.Data.Lottery.ToString());
            sb.AppendLine("Issue:" + this.Data.Issue.ToString());
            sb.AppendLine("Code:" + string.Join(',', this.Data.Code.Select(x => x)));
            sb.AppendLine("Time:" + this.Time.ToString());
            return sb.ToString();
        }
        public static CqsscGameWinNumber ConvertToCqsscGameWinNumber(GemTechData gemTechData)
        {
            return new CqsscGameWinNumber
            {
                IssuseNumber ="20"+gemTechData.Data.Issue.Insert(6,"-"),
                GameCode = gemTechData.Data.Lottery,
                WinNumber = string.Join(',', gemTechData.Data.Code.Select(y => y)),
                CreateTime = gemTechData.Time
            };
        }
    }
    

}