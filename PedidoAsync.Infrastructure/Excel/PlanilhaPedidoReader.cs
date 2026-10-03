using ClosedXML.Excel;
using PedidoAsync.Application.DTOs;
using PedidoAsync.Application.Interfaces;

namespace PedidoAsync.Infrastructure.Excel;

public class PlanilhaPedidoReader : IPlanilhaPedidoReader
{
    public Task<(List<CriarPedidoImportacaoDto> Pedidos, List<string> Erros)>
    LerAsync(Stream arquivo)
    {
        var pedidos = new List<CriarPedidoImportacaoDto>();
        var erros = new List<string>();
        var numerosPedidos = new HashSet<int>();


        using var workbook = new XLWorkbook(arquivo);

        var worksheet = workbook.Worksheets.FirstOrDefault();

        if (worksheet is null)
        {
            erros.Add("A planilha não possui nenhuma aba.");

            return Task.FromResult((pedidos, erros));
        }

        var primeiraLinha = worksheet.FirstRowUsed();

        if (primeiraLinha is null)
        {
            erros.Add("A planilha está vazia.");

            return Task.FromResult((pedidos, erros));
        }

        var headers = primeiraLinha
            .CellsUsed()
            .Select(c => c.GetString().Trim())
            .ToList();

        var colunasEsperadas = new[]
        {
        "NumeroPedido",
        "Cliente",
        "Email",
        "Valor",
        "DataPedido"
    };

        foreach (var coluna in colunasEsperadas)
        {
            if (!headers.Contains(coluna))
            {
                erros.Add(
                    $"A coluna obrigatória '{coluna}' não foi encontrada.");
            }
        }

        if (erros.Count > 0)
        {
            return Task.FromResult((pedidos, erros));
        }

        var ultimaLinha = worksheet.LastRowUsed()?.RowNumber() ?? 1;

        for (var numeroLinha = 2;
             numeroLinha <= ultimaLinha;
             numeroLinha++)
        {
            var linha = worksheet.Row(numeroLinha);

            var numeroPedidoTexto = linha
                .Cell(headers.IndexOf("NumeroPedido") + 1)
                .GetString()
                .Trim();

            var cliente = linha
                .Cell(headers.IndexOf("Cliente") + 1)
                .GetString()
                .Trim();

            var email = linha
                .Cell(headers.IndexOf("Email") + 1)
                .GetString()
                .Trim();

            var valorTexto = linha
                .Cell(headers.IndexOf("Valor") + 1)
                .GetString()
                .Trim();

            var dataPedidoTexto = linha
                .Cell(headers.IndexOf("DataPedido") + 1)
                .GetString()
                .Trim();

            if (!int.TryParse(
                    numeroPedidoTexto,
                    out var numeroPedido))
            {
                erros.Add(
                    $"Linha {numeroLinha}: NumeroPedido inválido.");

                continue;
            }

            if (!numerosPedidos.Add(numeroPedido))
            {
                erros.Add(
                    $"Linha {numeroLinha}: NumeroPedido {numeroPedido} duplicado na planilha.");

                continue;
            }

            if (string.IsNullOrWhiteSpace(cliente))
            {
                erros.Add(
                    $"Linha {numeroLinha}: Cliente não informado.");

                continue;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                erros.Add(
                    $"Linha {numeroLinha}: Email não informado.");

                continue;
            }

            if (!decimal.TryParse(
                    valorTexto,
                    out var valor))
            {
                erros.Add(
                    $"Linha {numeroLinha}: Valor inválido.");

                continue;
            }

            if (valor <= 0)
            {
                erros.Add(
                    $"Linha {numeroLinha}: Valor deve ser maior que zero.");

                continue;
            }

            if (!DateTime.TryParse(
                    dataPedidoTexto,
                    out var dataPedido))
            {
                erros.Add(
                    $"Linha {numeroLinha}: DataPedido inválida.");

                continue;
            }

            pedidos.Add(new CriarPedidoImportacaoDto
            {
                NumeroPedido = numeroPedido,
                Cliente = cliente,
                Email = email,
                Valor = valor,
                DataPedido = dataPedido
            });
        }

        return Task.FromResult((pedidos, erros));
    }
}

