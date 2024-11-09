﻿namespace Blog.Management.Domain.ArticleCategoryAgg
{
    public class ArticleCategory
    {
        public long ArticleCategoryId { get; private set; }
        public string Title { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public ArticleCategory(string title)
        {
            Title = title;
            IsDeleted = false;
            CreatedDate = DateTime.Now;
        }
    }
}
