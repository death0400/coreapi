using Core.Data.GemTech.Model;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core.Data.GemTech.DependencyInjection
{
    public static class StartUpExtension
    {
        public static IMvcBuilder AddGemTechWebApi(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services.AddScoped<Model.Pipe>();

            return mvcBuilder.ConfigureApplicationPartManager(apm =>
            {
                apm.ApplicationParts.Add(new AssemblyPart(typeof(GemTechController).GetTypeInfo().Assembly));
            }).AddControllersAsServices();
        }
    }
}
