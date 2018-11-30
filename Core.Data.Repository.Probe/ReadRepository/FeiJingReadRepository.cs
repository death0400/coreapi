using Core.Data.Repository.Probe.Model;
using Core.Data.Repository.Probe.Model.Baseketball;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;

namespace Core.Data.Repository.Probe.ReadRepository
{
   public class FeiJingReadRepository : IReadRepository<BaseketballGameData>
    {
        HttpClient httpClient;
        public FeiJingReadRepository()
        {
            this.httpClient = new HttpClient();
        }

        public IEnumerable<BaseketballGameData> GetList(Expression<Func<BaseketballGameData, bool>> predicate = null)
        {
            return new List<BaseketballGameData> { new BaseketballGameData {Result=GameResult.AwayTeamWin} };
        }

        public BaseketballGameData Get(params object[] keyValues)
        {
            return new BaseketballGameData { Result = GameResult.AwayTeamWin };
        }

        public IEnumerable<BaseketballGameData> GetList(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
