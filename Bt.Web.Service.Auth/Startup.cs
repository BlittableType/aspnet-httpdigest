using System;
using Bt.Web.Authentication.HttpDigest;
using Bt.Web.Authentication.HttpDigest.AspNetCore;
using Bt.Web.Service.Auth.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Bt.Web.Service.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bt.Web.Service.Auth", Version = "v1" });
            });
            
            services.AddScoped<IAuthController, AuthController>();
            services.AddScoped<IUserPasswordProvider, UserPasswordProvider>();
            services.AddSingleton<INonceStorage, NonceStorage>();
            
            services.AddAuthentication(HttpDigestDefaults.AuthenticationScheme)
                .AddHttpDigest(HttpDigestDefaults.AuthenticationScheme, options =>
                {
                    options.HashingAlgorithm = GetHashingAlgorithm(Configuration["HttpDigestSettings:HashingAlgorithm"]);
                    options.Realm = Configuration["HttpDigestSettings:Realm"];
                    options.NonceTtl = GetNonceTtl(Configuration["HttpDigestSettings:NonceTtlMin"]);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bt.Web.Service.Auth v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static HashingAlgorithm GetHashingAlgorithm(string algString)
        {
            return Enum.Parse<HashingAlgorithm>(algString);
        }
        
        private static TimeSpan GetNonceTtl(string nonceTtlMin)
        {
            return TimeSpan.FromMinutes(Double.Parse(nonceTtlMin));
        }
    }
}