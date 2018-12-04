using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Data.GemTech.DependencyInjection;
using Core.Data.Repository.CRUDApi.DependencyInjection;
using Core.Data.Repository.Probe.Model.Baseketball;
using Core.Data.Repository.Probe.ReadRepository;
using Core.Data.GemTech;
using Db.CommomLotteryData.Models;
using Microsoft.Extensions.Options;
using Core.Data.GemTech.Model;

namespace Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .Configure<GemTechConfig>(Configuration)
            .AddSingleton(sp => sp.GetRequiredService<IOptions<GemTechConfig>>().Value)
            .AddScoped(p =>
            {
                return new RedisUrlCollection { Urls = Configuration.GetSection("RedisURL").AsEnumerable().Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(url => new Uri(url.Value.ToString())) };
            });
            services.AddDbContext<Common_LotteryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CommomLotteryData")));
            services.AddMvc()
               .AddDbRepositoryWebApi<Common_LotteryDataContext>(register =>
               {
                   register.RegisterAllEntityTypes();
               })
               .AddGemTechWebApi();
            //.AddReadOnlyRepositoryWebApi<BaseketballGameData,string>(option=> {
            //    option.RegisterIReadRepository<FeiJingReadRepository>();
            //});
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
