﻿using _0_Framework.Application.Model;
using Blog.Management.Application.Contracts.ArticleCategory;
using Blog.Management.Application.Contracts.ArticleCategory.Dtos;
using Blog.Management.Domain.ArticleCategoryAgg;
using System.Globalization;
using System.Net;

namespace Blog.Management.Application
{
    public class ArticleCategoryApplication : IArticleCategoryApplication
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
        }

        //----------------------------------- CREATE -----------------------------------\\
        public async Task<OperationResult> Create(CreateArticleCategoryDto articleCategoryDto)
        {
            var operation = new OperationResult();
            
            try
            {
                var articleCategory = new ArticleCategory(articleCategoryDto.Title);
                var res = await _articleCategoryRepository.Create(articleCategory);

                return operation.Succeeded(res);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        //----------------------------------- GET ALL -----------------------------------\\
        public async Task<OperationResultWithData<List<GetArticleCategoryDto>>> GetAll()
        {
            var operation = new OperationResultWithData<List<GetArticleCategoryDto>>();

            try
            {
                var res = await _articleCategoryRepository.Get();

                if (res == null)
                {
                    return operation.Failed();
                }

                var result = new List<GetArticleCategoryDto>();

                foreach (var articleCategory in res)
                {
                    result.Add(new GetArticleCategoryDto
                    {
                        ArticleCategoryId = articleCategory.ArticleCategoryId,
                        Title = articleCategory.Title,
                        IsDeleted = articleCategory.IsDeleted,
                        CreatedDate = articleCategory.CreatedDate.ToString(CultureInfo.InvariantCulture),
                        UpdatedDate = articleCategory.UpdatedDate.ToString(CultureInfo.InvariantCulture)
                    });
                }
                return operation.Succeeded(result);
            }
            catch (Exception ex)
            {
                return operation.Failed();
            }
        }

        //----------------------------------- UPDATE -----------------------------------\\
        public async Task<OperationResult> Update(UpdateArticleCategoryDto articleCategoryDto)
        {
            var operation = new OperationResult();

            try
            {
                var articleCategory = new ArticleCategory(articleCategoryDto.Title);
                var res = await _articleCategoryRepository.Edit(articleCategory, articleCategoryDto.Id);

                if (res == null)
                {
                    return operation.Failed();
                }

                return operation.Succeeded(res);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //----------------------------------- DELETE -----------------------------------\\
        public async Task<OperationResult> Delete(DeleteArticleCategoryDto articleCategoryDto)
        {
            var operation = new OperationResult();

            try
            {
                
                var res = await _articleCategoryRepository.Delete(articleCategoryDto.Id);

                if (!res)
                {
                    return operation.Failed();
                }

                return operation.Succeeded(res);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}