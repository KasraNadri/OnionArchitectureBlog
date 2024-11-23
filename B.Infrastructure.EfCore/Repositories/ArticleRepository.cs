﻿using Blog.Management.Domain.ArticleAgg;
using Blog.Management.Infrastructure.EfCore.Repositories.Shared;

namespace Blog.Management.Infrastructure.EfCore.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly BlogContext _dbContext;
        public ArticleRepository(BlogContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Publish(long articleId)
        {
            try
            {
                Article val = await _dbContext.FindAsync<Article>(new object[1] { articleId });
                
                if (val != null)
                {
                    _dbContext.Entry(val).State = EntityState.Detached;
                }
                val.Publish();
                _dbContext.Entry(val).State = EntityState.Modified;
                await SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task Archive(long articleId)
        {
            try
            {
                Article val = await _dbContext.FindAsync<Article>(new object[1] { articleId });

                if (val != null)
                {
                    _dbContext.Entry(val).State = EntityState.Detached;
                }
                val.Archive();
                _dbContext.Entry(val).State = EntityState.Modified;
                await SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task GetAll(long authorId)
        {
            List<Article> articles = await _dbContext.Articles.Where(x => x.AuthorId == authorId).ToListAsync();
        }
    }
}
