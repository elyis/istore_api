using System.Text.RegularExpressions;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IProductRepository _productRepository;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly ContentInspector _contentInspector;
        private readonly string _supportedIconMime;


        public UploadController(
            IFileUploaderService fileUploaderService,
            IProductRepository productRepository,
            IProductCharacteristicRepository productCharacteristicRepository,
            ContentInspector contentInspector
        )
        {
            _fileUploaderService = fileUploaderService;
            _contentInspector = contentInspector;
            _productRepository = productRepository;
            _productCharacteristicRepository = productCharacteristicRepository;
            _supportedIconMime = "image/";
        }


        [HttpPost("productIcon"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadIcons(
            [FromForm] Guid productId,
            [FromForm] string hex,
            [FromForm] string color,
            [FromForm] IFormFileCollection formFiles
        )
        {
            if (productId == Guid.Empty)
                return BadRequest("productId is not found");

            if (string.IsNullOrEmpty(color) || string.IsNullOrEmpty(hex))
                return BadRequest("color or hex field is empty");

            if (!IsHexFormat(hex))
                return BadRequest("hex format looks wrong");

            var files = Request.Form.Files;
            if (files.Count == 0)
                return BadRequest("the files are not attached");

            var product = await _productRepository.GetAsync(productId);
            if (product == null)
                return BadRequest("id is not found");

            var result = await UploadImagesAsync(Constants.localPathToProductIcons, files);
            if (result is OkObjectResult objectResult)
            {
                var filenames = (List<string>)objectResult.Value!;
                var colorString = CharacteristicType.Color.ToString();

                var productCharacteristic = new ProductCharacteristic
                {
                    Name = colorString,
                    Color = color,
                    Hex = hex,
                    Type = colorString,
                    Values = string.Join(";", filenames),
                    Product = product
                };

                await _productCharacteristicRepository.AddImagesToProduct(productCharacteristic, productId);
                return Ok();
            }

            return result;
        }

        [HttpGet("productIcon/{filename}")]
        [SwaggerOperation("Получить иконку профиля")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(File))]
        [SwaggerResponse(404, Description = "Неверное имя файла")]

        public async Task<IActionResult> GetProductIcon(string filename)
            => await GetIconAsync(Constants.localPathToProductIcons, filename);


        [HttpDelete("productIcon"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить фото продуктов")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]

        public async Task<IActionResult> RemoveProductIcon(
            [FromForm] Guid productId,
            [FromForm] string filename
        )
        {
            if (productId == Guid.Empty || string.IsNullOrEmpty(filename))
                return BadRequest("the product ID is empty or no files are specified");

            var result = await _productCharacteristicRepository.RemoveImageAsync(productId, filename);
            if (!result)
                return NotFound("product is not found");

            var isRemoved = await _fileUploaderService.RemoveFileAsync(Constants.localPathToProductIcons, filename);
            return isRemoved ? NoContent() : BadRequest();
        }


        private async Task<IActionResult> UploadImagesAsync(string path, IFormFileCollection files)
        {
            if (files.Count == 0)
                return BadRequest("No file uploaded");

            var streams = new List<(Stream stream, List<string> fileExtension)>();

            for (int i = 0; i < files.Count; i++)
            {
                var stream = files[i].OpenReadStream();
                var mimeTypes = _contentInspector.Inspect(stream).ByMimeType();

                var bestMatchMimeType = mimeTypes.MaxBy(e => e.Points)?.MimeType;
                if (string.IsNullOrEmpty(bestMatchMimeType) || !bestMatchMimeType.StartsWith(_supportedIconMime))
                    return new UnsupportedMediaTypeResult();

                var fileExtensions = new List<string>
                {
                    bestMatchMimeType.Split("/").Last(),
                    // "webp"
                };
                streams.Add((stream, fileExtensions));
            }

            var filenames = new List<string>();
            var tasks = new List<Task<List<string>>>();
            foreach (var (stream, fileExtension) in streams)
            {
                var temp = await _fileUploaderService.UploadFileAsync(path, stream, fileExtension);
                filenames.AddRange(temp);
            }

            return Ok(filenames);
        }

        private async Task<IActionResult> GetIconAsync(string path, string filename)
        {
            var bytes = await _fileUploaderService.GetStreamFileAsync(path, filename);
            if (bytes == null)
                return NotFound();

            var fileExtension = Path.GetExtension(filename);
            return File(bytes, $"image/{fileExtension}", filename);
        }

        private static bool IsHexFormat(string input)
        {
            string hexPattern = @"^[0-9a-fA-F]{6}$|^[0-9a-fA-F]{3}$";
            return Regex.IsMatch(input, hexPattern);
        }
    }
}