using Microsoft.AspNetCore.Mvc;
using PedidoAsync.Application.DTOs;
using PedidoAsync.Application.Interfaces;
using PedidoAsync.Application.Services;

namespace PedidoAsync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacoesController : ControllerBase
    {
        private readonly IPlanilhaPedidoReader _planilhaPedidoReader;
        private readonly ImportacaoService _importacaoService;

        public ImportacoesController(IPlanilhaPedidoReader planilhaPedidoReader, ImportacaoService importacaoService)
        {
            _planilhaPedidoReader = planilhaPedidoReader;
            _importacaoService = importacaoService;
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

            var resultado = await _importacaoService.CriarImportacaoAsync(arquivo.FileName, stream);

            return Ok(new
            {
                importacaoId = resultado.Importacao.Id,
                arquivo = resultado.Importacao.NomeArquivo,
                totalRegistros = resultado.Importacao.TotalRegistros,
                pedidosLidos = resultado.Pedidos.Count
            });
        }

    }
}
