using Flashcards.Domain.Model;
using Flashcards.Infrastructure.Database;
using Flashcards.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Repositories;

internal class CardRepository : ICardRepository
{
    private readonly IDbContextFactory<FlashcardsDbContext> _contextFactory;

    public CardRepository(IDbContextFactory<FlashcardsDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    public async Task Add(CardModel value)
    {
        var dbContext = await _contextFactory.CreateDbContextAsync();
        await dbContext.Cards.AddAsync(value);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(CardModel value)
    {
        var dbContext = await _contextFactory.CreateDbContextAsync();
        dbContext.Cards.Remove(value);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(CardModel value)
    {
        var dbContext = await _contextFactory.CreateDbContextAsync();
        dbContext.Cards.Update(value);
        await dbContext.SaveChangesAsync();
    }

    public async Task<CardModel> Get(int id)
    {
        var dbContext = await _contextFactory.CreateDbContextAsync();
        CardModel? cards = await dbContext.Cards.FindAsync(id);
        return cards ?? throw new InvalidOperationException("Card not found");
    }
}
