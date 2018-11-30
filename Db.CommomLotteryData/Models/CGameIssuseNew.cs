using System;
using System.Collections.Generic;

namespace Db.CommomLotteryData.Models
{
    public partial class CGameIssuseNew
    {
        public int Id { get; set; }
        public string Issue { get; set; }
        public string Lottery { get; set; }
        public string Code { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime? ServerTime { get; set; }
    }
}
