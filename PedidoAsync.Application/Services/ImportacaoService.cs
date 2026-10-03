using PedidoAsync.Application.DTOs;
using PedidoAsync.Application.Interfaces;
using PedidoAsync.Domain.Entities;

namespace PedidoAsync.Application.Services
{
    public class ImportacaoService
    {
        private readonly IPlanilhaPedidoReader _planilhaPedidoReader;
        private readonly IImportacaoRepository _importacaoRepository;
        private readonly PedidoService _pedidoService;
        private readonly IPedidoRepository _pedidoRepository;

        public ImportacaoService(IPlanilhaPedidoReader planilhaPedidoReader, IImportacaoRepository importacaoRepository, PedidoService pedidoService, IPedidoRepository pedidoRepository)
        {
            _planilhaPedidoReader = planilhaPedidoReader;
            _importacaoRepository = importacaoRepository;
            _pedidoService = pedidoService;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<(Importacao Importacao, List<CriarPedidoImportacaoDto> Pedidos)> CriarImportacaoAsync(string nomeArquivo, Stream arquivo)
        {
            var resultado = await _planilhaPedidoReader
                .LerAsync(arquivo);

            var pedidosValidos = new List<CriarPedidoImportacaoDto>();

            var quantidadeErros = resultado.Erros.Count;

            foreach (var pedido in resultado.Pedidos)
            {
                var existe = await _pedidoRepository
                    .ExistePorNumeroAsync(pedido.NumeroPedido);

                if (existe)
                {
                    quantidadeErros++;

                    continue;
                }

                pedidosValidos.Add(pedido);
            }

            var totalRegistros =
                pedidosValidos.Count + quantidadeErros;

            var importacao = new Importacao(
                nomeArquivo,
                totalRegistros);

            await _importacaoRepository
                .AdicionarAsync(importacao);

            importacao.IniciarProcessamento();

            for (var i = 0; i < quantidadeErros; i++)
            {
                importacao.RegistrarErro();
            }

            await _importacaoRepository
                .AtualizarAsync(importacao);

            foreach (var pedido in pedidosValidos)
            {
                await _pedidoService.CriarAsync(
                    importacao.Id,
                    pedido.NumeroPedido,
                    pedido.Cliente,
                    pedido.Email,
                    pedido.Valor,
                    pedido.DataPedido);
            }

            return (importacao, pedidosValidos);
        }

        public async Task<ImportacaoDto?> ObterPorIdAsync(Guid id)
        {
            var importacao = await _importacaoRepository.ObterPorIdAsync(id);
            if (importacao == null)
            {
                return null;
            }

            return new ImportacaoDto
            {
                Id = importacao.Id,
                NomeArquivo = importacao.NomeArquivo,
                DataInicio = importacao.DataInicio,
                DataFim = importacao.DataFim,
                TotalRegistros = importacao.TotalRegistros,
                Processados = importacao.Processados,
                Erros = importacao.Erros,
                Status = importacao.Status.ToString()
            };
        }
    }
}
