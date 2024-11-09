﻿using Blog.Management.Application;
using Blog.Management.Application.Contracts.ArticleCategory;
using Blog.Management.Domain.ArticleCategoryAgg;
using Blog.Management.Infrastructure.EfCore;
using Blog.Management.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Management.Infrastructure.Core
{
    public static class Bootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IArticleCategoryApplication, ArticleCategoryApplication>();
            services.AddTransient<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddDbContext<BlogContext>(options => options.UseSqlServer("BlogDb"));
        }
    }
}
