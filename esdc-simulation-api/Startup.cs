using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestSharp;

using esdc_simulation_api.Controllers;

using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Storage;
using esdc_simulation_base.Src.Rules;

using maternity_benefits;
using maternity_benefits.Storage.EF;
using maternity_benefits.Storage.EF.Store;
using maternity_benefits.Storage.Cache;
using maternity_benefits.Storage.Mock;

namespace esdc_simulation_api
{
    public class Startup
    {
        // Toggle for using DB. If 'false', then cache storage implementation is used
        private static readonly bool USING_DB = false;

        // Toggle true for automatically adding mock persons to the storage system
        private static readonly bool USING_MOCK = true;

        // If using mocks, then specify the number of mocks to add to the system
        private static readonly int MOCK_SEED_AMOUNT = 1000;

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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Maternity Benefit Simulation API",
                    Description = "...",
                    TermsOfService = new Uri("https://example.com/terms")
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMemoryCache();

            InjectMaternityBenefits(services);
            
            services.AddScoped<IJoinResults, Joiner>();
            
            services.AddScoped<IRestClient, RestSharp.RestClient>();

            // Rules options
            var rulesUrl = Configuration["RulesOptions:Url"] ?? 
                Environment.GetEnvironmentVariable("RULES_URL");

            var rulesOptions = new RulesOptions() {
                Url = rulesUrl
            };
            services.AddSingleton<IOptions<RulesOptions>>(x => Options.Create(rulesOptions));

            // Password Filter options
            var passwordFilter = Configuration["PasswordOptions:Password"] ?? 
                Environment.GetEnvironmentVariable("PASSWORD_FILTER");

            var passwordOptions = new PasswordFilterOptions() {
                Password = passwordFilter
            };
            services.AddSingleton<IOptions<PasswordFilterOptions>>(x => Options.Create(passwordOptions));
            services.AddScoped<PasswordFilterAttribute>();

            // DB injection: Only needed if using a DB implementation for storage
            if (USING_DB) {
                string connectionString = Configuration.GetConnectionString("DefaultDB") ??
                    Environment.GetEnvironmentVariable("DEFAULT_DB");
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("esdc-simulation-api")));
            }
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Apply migrations
            if (USING_DB) {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        context.Database.Migrate();
                    }
                }
            }
            

            // Seed data store with mock persons. Comment this out if using real data
            if (USING_MOCK) {
                SeedDataStoreWithPersons(app.ApplicationServices, MOCK_SEED_AMOUNT);
            }
            

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/spec.json", "Maternity Benefit Simulation API Spec");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InjectMaternityBenefits(IServiceCollection services) {
            services.AddScoped<IHandleSimulationRequests, SimulationRequestHandler>();

            services.AddScoped<IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson>,
                SimulationRunner<MaternityBenefitsCase, MaternityBenefitsPerson>>();

            services.AddScoped<IRunCases<MaternityBenefitsCase, MaternityBenefitsPerson>, 
                BulkCaseRunner<MaternityBenefitsCase, MaternityBenefitsPerson>>();
            //services.AddScoped<IRunCases<MaternityBenefitsCase, MaternityBenefitsPerson>, 
                //CaseRunner<MaternityBenefitsCase, MaternityBenefitsPerson>>();

            services.AddScoped<IExecuteRules<MaternityBenefitsCase, MaternityBenefitsPerson>,
                MaternityBenefitsExecutor>();
            services.AddScoped<IExecuteBulkRules<MaternityBenefitsCase, MaternityBenefitsPerson>,
                MaternityBenefitsBulkExecutor>();

            services.AddScoped<IRulesEngine, RulesApi>();

            // Cache Storage
            if (USING_DB) {
                services.AddScoped<IStorePersons<MaternityBenefitsPerson>, MaternityBenefitsPersonEFStore>();
                services.AddScoped<IStoreSimulations<MaternityBenefitsCase>, MaternityBenefitsSimulationEFStore>();
                services.AddScoped<IStoreSimulationResults<MaternityBenefitsCase>, MaternityBenefitsSimulationResultsEFStore>();
            } else {     
                services.AddScoped<IStorePersons<MaternityBenefitsPerson>, MaternityBenefitsPersonCacheStore>();
                services.AddScoped<IStoreSimulations<MaternityBenefitsCase>, MaternityBenefitsSimulationCacheStore>();
                services.AddScoped<IStoreSimulationResults<MaternityBenefitsCase>, MaternityBenefitsSimulationResultsCacheStore>();
            }
        }
    
        private void SeedDataStoreWithPersons(IServiceProvider services, int numPersons) {
            var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var personStore = scope.ServiceProvider.GetRequiredService<IStorePersons<MaternityBenefitsPerson>>();
        
            var persons = personStore.GetAllPersons();
            if (persons.Count() == 0) {
                var mockPersons = MockCreator.GetMockPersons(numPersons);
                personStore.AddPersons(mockPersons);
            }
        }
    }
}
