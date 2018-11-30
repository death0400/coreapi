using System;
using System.Collections.Generic;

namespace Db.CommomLotteryData.Models
{
    public partial class CqsscGameWinNumber
    {
        public long Id { get; set; }
        public string IssuseNumber { get; set; }
        public string GameCode { get; set; }
        public string WinNumber { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
