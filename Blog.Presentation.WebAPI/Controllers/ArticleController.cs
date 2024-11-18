﻿using _0_Framework.Application.Model;
using Blog.Management.Application.Contracts.Author.Dtos;
using Blog.Provider.Contracts.Article;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRequestProvider _articleRequestProvider;
        public ArticleController(IArticleRequestProvider articleRequestProvider)
        {
            _articleRequestProvider = articleRequestProvider;
        }

        [HttpGet("GetAll")]
        public async Task<OperationResultWithData<List<GetArticleDto>>> GetAll()
        {
            var list = await _articleRequestProvider.GetAll();
            return list;
        }

        [HttpPost("Create")]
        public async Task<OperationResult> Create(CreateAuthorDto dto)
        {
            var res = await _articleRequestProvider.Create(dto);
            return res;
        }

        [HttpDelete("Delete")]
        public async Task<OperationResult> Delete(DeleteAuthorDto dto)
        {
            var res = await _articleRequestProvider.Delete(dto);
            return res;
        }

        [HttpPut("Update")]
        public async Task<OperationResult> Edit(UpdateAuthorDto dto)
        {
            var res = await _articleRequestProvider.Update(dto);
            return res;
        }
    }
}
