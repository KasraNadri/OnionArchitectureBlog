﻿using _0_Framework.Application.Model;
using Blog.Management.Application.Contracts.Article;
using Blog.Management.Application.Contracts.Article.Dtos;
using Blog.Management.Domain.ArticleAgg;
using System.Globalization;
using _0_Framework.Log;

namespace Blog.Management.Application
{
    public class ArticleApplication : IArticleApplication
    {

        #region INJECTION

        //---------- CONSTANT VARIABLE ----------\\
        const string className = nameof(ArticleApplication);

        private readonly IArticleRepository _articleRepository;
        private readonly ILogService _logService;

        public ArticleApplication(IArticleRepository articleRepository, ILogService logService)
        {
            _articleRepository = articleRepository;   
            _logService = logService;
        }

        #endregion


        #region CRUD

        public async Task<OperationResult> Create(CreateArticleDto article)
        {
            var operation = new OperationResult();

            try
            {
                var articleDto = new Article(article.Title, article.CategoryId, article.AuthorId, article.Content, article.Excerpt, article.FeaturedImage);
                var res = await _articleRepository.Create(articleDto);
                if (res == null)
                {
                    _logService.LogError(@$"{className}/Create", "create results were null"); //-- LOG (ERR) --
                    return operation.Failed();
                }

                _logService.LogInformation($@"{className}/Create", "create results were successful"); //-- LOG (INF) --
                return operation.Succeeded(res);
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in create"); //-- LOG (EXC) --
                throw;
            }
        }

        public async Task<OperationResultWithData<List<GetArticleDto>>> GetAll(long authorId)
        {
            var operation = new OperationResultWithData<List<GetArticleDto>>();

            try
            {
                var res = await _articleRepository.GetAll(authorId);

                if (res == null)
                {
                    _logService.LogWarning(@$"{className}/GetAll", "getall results were null"); //-- LOG (WAR) --
                    return operation.Failed();
                }

                var result = new List<GetArticleDto>();

                foreach (var article in res)
                {
                    result.Add(new GetArticleDto
                    {
                        ArticleId = article.ArticleId,
                        CategoryId = article.ArticleCategoryId,
                        Title = article.Title,
                        Excerpt = article.Excerpt,
                        Content = article.Content,
                        FeaturedImage = article.FeaturedImage,
                        CreatedDate = article.CreatedDate.ToString(CultureInfo.InvariantCulture),
                        LastEditedDate = article.LastEditedDate.ToString(CultureInfo.InvariantCulture)
                    });
                }

                _logService.LogInformation($@"{className}/GetAll", "getall results were successful"); //-- LOG (INF) --
                return operation.Succeeded(result);
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in getall"); //-- LOG (EXC) --
                return operation.Failed();
            }
        }

        public async Task<OperationResult> Update(UpdateArticleDto articleDto)
        {
            var operation = new OperationResult();

            try
            {
                Article article = await _articleRepository.GetById(articleDto.Id);
                article.Edit(articleDto.CategoryId, articleDto.Title, articleDto.Content, articleDto.Excerpt, articleDto.FeaturedImage);
                var res = await _articleRepository.Edit(article, articleDto.Id);

                if (res == null)
                {
                    _logService.LogError(@$"{className}/Update", "update results were null"); //-- LOG (ERR) --
                    return operation.Failed();
                }

                _logService.LogInformation($@"{className}/Update", "update results were successful"); //-- LOG (ERR) --
                return operation.Succeeded(res);
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in update"); //-- LOG (EXC) --
                throw;
            }
        }

        #endregion


        #region PUBLISH & ARCHIVE & ACTIVATE & DEACTIVATE

        public async Task<OperationResult> Publish(long articleId)
        {
            var operation = new OperationResult();
            try
            {
                var res = await _articleRepository.Publish(articleId);

                if (res)
                {
                    _logService.LogInformation($@"{className}/Publish", "publish results were true"); //-- LOG (INF) --
                    return operation.Succeeded(res);
                }

                _logService.LogError(@$"{className}/Publish", "publish results were false"); //-- LOG (ERR) --
                return operation.Failed();
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in publish"); //-- LOG (EXC) --
                throw;
            }
        }

        public async Task<OperationResult> Archive(long articleId)
        {
            var operation = new OperationResult();
            try
            {
                var res = await _articleRepository.Archive(articleId);

                if (res)
                {
                    _logService.LogInformation($@"{className}/Archive", "archive results were true"); //-- LOG (INF) --
                    return operation.Succeeded(res);
                }

                _logService.LogError(@$"{className}/Archive", "archive results were false"); //-- LOG (ERR) --
                return operation.Failed();
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in archive"); //-- LOG (EXC) --
                throw;
            }
        }

        public async Task<OperationResult> ActivateArticlesForAuthor(long authorId)
        {
            var operation = new OperationResult();
            try
            {
                if (authorId == 0)
                {
                    _logService.LogError(@$"{className}/Activate", "authorid is zero"); //-- LOG (ERR) --
                    return operation.Failed();
                }

                var articleList = await _articleRepository.GetAll(authorId);

                if (articleList == null)
                {
                    _logService.LogWarning(@$"{className}/GetAll", "getall results were null"); //-- LOG (WAR) --
                    return operation.Failed();
                }

                foreach (var article in articleList)
                {
                    var res = await _articleRepository.Activate(article.ArticleId);
                    if (!res)
                    {
                        _logService.LogError(@$"{className}/Activate", "activate result was false"); //-- LOG (ERR) --
                        return operation.Failed();
                    }
                }

                _logService.LogInformation($@"{className}/Activate", "activate results were true and successful"); //-- LOG (INF) --
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in activate"); //-- LOG (EXC) --
                throw;
            }
        }

        public async Task<OperationResult> DeactivateArticlesForAuthor(long authorId)
        {
            var operation = new OperationResult();
            try
            {
                if (authorId == 0)
                {
                    _logService.LogError(@$"{className}/Deactivate", "authorid is zero"); //-- LOG (ERR) --
                    return operation.Failed();
                }

                var articleList = await _articleRepository.GetAll(authorId);

                if (articleList == null)
                {
                    _logService.LogWarning(@$"{className}/GetAll", "getall results were null"); //-- LOG (WAR) --
                    return operation.Failed();
                }

                foreach (var article in articleList)
                {
                    var res = await _articleRepository.Deactivate(article.ArticleId);
                    if (!res)
                    {
                        _logService.LogError(@$"{className}/Deactivate", "deactivate result was false"); //-- LOG (ERR) --
                        return operation.Failed();
                    }
                }

                _logService.LogInformation($@"{className}/Dectivate", "deactivate results were true and successful"); //-- LOG (INF) --
                return operation.Succeeded();
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, className, "exception error in deactivate"); //-- LOG (EXC) --
                throw;
            }

        }

        #endregion

    }
}
