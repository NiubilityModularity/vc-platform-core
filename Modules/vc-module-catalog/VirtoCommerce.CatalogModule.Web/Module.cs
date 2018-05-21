using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Data.Validation;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.CatalogModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            serviceCollection.AddTransient<ICatalogRepository, CatalogRepository>();
            serviceCollection.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("VirtoCommerce")));
            serviceCollection.AddSingleton<Func<ICatalogRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<ICatalogRepository>());
            serviceCollection.AddSingleton<ICatalogService, CatalogService>();
            serviceCollection.AddSingleton<ICategoryService, CategoryService>();
            serviceCollection.AddSingleton<IOutlineService, OutlineService>();
            serviceCollection.AddSingleton<IItemService, ItemService>();
            serviceCollection.AddSingleton<IPropertyService, PropertyService>();
            serviceCollection.AddSingleton<IPropertySearchService, PropertySearchService>();
            serviceCollection.AddSingleton<ISkuGenerator, DefaultSkuGenerator>();
            serviceCollection.AddSingleton<IListEntrySearchService, ListEntrySearchService>();

            Func<PropertyValidationRule, PropertyValueValidator> propertyValueValidatorFactory =
              rule => new PropertyValueValidator(rule);

            serviceCollection.AddSingleton(propertyValueValidatorFactory);
            serviceCollection.AddSingleton<AbstractValidator<IHasProperties>, HasPropertiesValidator>();
        }

        public void PostInitialize(IServiceProvider serviceProvider)
        {
            //Force migrations
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var catalogDbContext = serviceScope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                catalogDbContext.Database.EnsureCreated();
                catalogDbContext.Database.Migrate();
            }
        }

        public void Uninstall()
        {
        }
    }
}
