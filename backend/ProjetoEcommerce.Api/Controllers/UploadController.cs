using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IS3StorageService _storageService;

        public UploadController(IS3StorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("image")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo enviado" });

            // Validar tipo de arquivo
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest(new { message = "Tipo de arquivo não permitido. Use: JPG, PNG, GIF ou WEBP" });

            // Validar tamanho (máximo 5MB)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new { message = "Arquivo muito grande. Máximo: 5MB" });

            try
            {
                using var stream = file.OpenReadStream();
                var url = await _storageService.UploadFileAsync(stream, file.FileName, file.ContentType);

                return Ok(new 
                { 
                    url, 
                    fileName = file.FileName, 
                    size = file.Length,
                    contentType = file.ContentType
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao fazer upload", error = ex.Message });
            }
        }

        [HttpPost("images")]
        [RequestSizeLimit(50 * 1024 * 1024)] // 50MB total
        public async Task<IActionResult> UploadMultipleImages([FromForm] List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return BadRequest(new { message = "Nenhum arquivo enviado" });

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            var results = new List<object>();
            var errors = new List<string>();

            foreach (var file in files)
            {
                if (!allowedTypes.Contains(file.ContentType.ToLower()))
                {
                    errors.Add($"{file.FileName}: Tipo não permitido");
                    continue;
                }

                if (file.Length > 5 * 1024 * 1024)
                {
                    errors.Add($"{file.FileName}: Arquivo muito grande");
                    continue;
                }

                try
                {
                    using var stream = file.OpenReadStream();
                    var url = await _storageService.UploadFileAsync(stream, file.FileName, file.ContentType);
                    results.Add(new { url, fileName = file.FileName, size = file.Length });
                }
                catch (Exception ex)
                {
                    errors.Add($"{file.FileName}: {ex.Message}");
                }
            }

            return Ok(new { uploaded = results, errors });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest(new { message = "URL não informada" });

            try
            {
                var result = await _storageService.DeleteFileAsync(url);
                if (result)
                    return Ok(new { message = "Arquivo excluído com sucesso" });

                return NotFound(new { message = "Arquivo não encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao excluir", error = ex.Message });
            }
        }
    }
}
