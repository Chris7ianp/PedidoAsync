using MongoDB.Driver;
using PedidoAsync.Application.Interfaces;
using PedidoAsync.Domain.Entities;
using PedidoAsync.Infrastructure.Data;

namespace PedidoAsync.Infrastructure.Repositories;

public class ImportacaoRepository : IImportacaoRepository
{
    private readonly IMongoCollection<Importacao> _importacoes;

    public ImportacaoRepository(MongoDbContext context)
    {
        _importacoes = context.Importacoes;
    }

    public async Task AdicionarAsync(Importacao importacao)
    {
        await _importacoes.InsertOneAsync(importacao);
    }

    public async Task<Importacao?> ObterPorIdAsync(Guid id)
    {
        return await _importacoes
            .Find(i => i.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AtualizarAsync(Importacao importacao)
    {
        await _importacoes.ReplaceOneAsync(
            i => i.Id == importacao.Id,
            importacao);
    }

    public async Task<List<Importacao>> ObterTodosAsync()
    {
        return await _importacoes
            .Find(_ => true)
            .ToListAsync();
    }    
}