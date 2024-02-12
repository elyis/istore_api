using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Entities.Shared;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogTopicRepository _blogTopicRepository;

        public BlogController(IBlogTopicRepository blogTopicRepository)
        {
            _blogTopicRepository = blogTopicRepository;
        }


        [HttpPost("blog"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать тему блога")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateBlogTopic(CreateBlogTopicBody blogBody)
        {
            var result = await _blogTopicRepository.AddAsync(blogBody);
            return result == null ? Conflict() : Ok();
        }

        [HttpPost("blogs")]
        [SwaggerOperation("Получить список тем блога")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<BlogTopicBody>))]

        public async Task<IActionResult> GetBlogTopics(DynamicDataLoadingOptions loadingOptions)
        {
            var blogTopics = await _blogTopicRepository.GetAll(loadingOptions.Count, loadingOptions.LoadPosition);
            var result = blogTopics.Select(e => e.ToBlogTopicBody());
            return Ok(result);
        }

        [HttpDelete("blog"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить блог")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveBlogTopic([FromQuery, Required] Guid blogId)
        {
            var result = await _blogTopicRepository.RemoveAsync(blogId);
            return result ? NoContent() : BadRequest();
        }


        [HttpGet("blog")]
        [SwaggerOperation("Получить блог по id")]
        [SwaggerResponse(200, Type = typeof(BlogTopicBody))]
        [SwaggerResponse(404)]

        public async Task<IActionResult> GetTopic([FromQuery, Required] Guid blogId)
        {
            var result = await _blogTopicRepository.GetAsync(blogId);
            return result == null ? NotFound() : Ok(result.ToBlogTopicBody());
        }
    }
}