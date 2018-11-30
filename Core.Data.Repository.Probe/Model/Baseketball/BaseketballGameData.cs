using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Repository.Probe.Model.Baseketball
{
    public class BaseketballGameData
    {
        public BasketballTeam HomeTeam { get; set; }
        public BasketballTeam AwayTeam { get; set; }
        public int TatalScore { get { return this.HomeTeam.Score + this.AwayTeam.Score; } }
        public GameResult Result { get; set; }
        public GameStatus Status { get; set; }
        public double SF_SP { get; set; }
        public GameResult SF_Result { get; set; }

        public double RFSF_SP { get; set; }
        public GameResult RFSF_Result { get; set; }
        public double SFC_SP { get; set; }

        public int SFC_Result { get; set; }
        public double DXF_SP { get; set; }
        public GameResult DXF_Result { get; set; }

        public string RFSF_Trent { get; set; }
        public string DXF_Trent { get; set; }
        public DateTime CreateTime { get; set; }


    }

    public enum GameResult
    {
        HomeTeamWin = 3,
        AwayTeamWin = 0
    }
    public enum GameStatus
    {
        比賽中 = 2,
        比賽結束 = 1,
        未開賽=0,
        取消比賽 = 9
    }
}
