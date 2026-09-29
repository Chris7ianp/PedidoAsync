using Microsoft.AspNetCore.Mvc;
using PedidoAsync.Application.DTOs;
using PedidoAsync.Application.Interfaces;

namespace PedidoAsync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacoesController : ControllerBase
    {
        private readonly IPlanilhaPedidoReader _planilhaPedidoReader;

        public ImportacoesController(IPlanilhaPedidoReader planilhaPedidoReader)
        {
            _planilhaPedidoReader = planilhaPedidoReader;
        }

        [HttpPost("pedidos")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportarPedidos(IFormFile arquivo)
        {
            if (arquivo is null || arquivo.Length == 0)
            {
                return BadRequest("O arquivo não foi informado ou está vazio.");
            }

            var extensao = Path.GetExtension(arquivo.FileName);

            if (!string.Equals(extensao, ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Apenas arquivos .xlsx são permitidos.");
            }

            await using var stream = arquivo.OpenReadStream();

            var resultado = await _planilhaPedidoReader.LerAsync(stream);

            var response = new ResultadoImportacaoDto
            {
                TotalRegistros = resultado.Pedidos.Count + resultado.Erros.Count,
                RegistrosImportados = resultado.Pedidos.Count,
                RegistrosComErro = resultado.Erros.Count,
                Erros = resultado.Erros
            };

            return Ok(response);
        }

    }
}
