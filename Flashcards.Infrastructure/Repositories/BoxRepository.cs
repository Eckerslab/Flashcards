using System;
using System.Collections.Generic;
using System.Text;
using Flashcards.Domain.Model;
using Flashcards.Infrastructure.Database;
using Flashcards.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Repositories
{
    public class BoxRepository(IDbContextFactory<FlashcardsDbContext> contextFactory) : IBoxRepository
    {
        public async Task Add(BoxModel value)
        {
            var dbContext = await contextFactory.CreateDbContextAsync();
            await dbContext.Boxes.AddAsync(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(BoxModel value)
        {
            var dbContext = await contextFactory.CreateDbContextAsync();
            dbContext.Boxes.Remove(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(BoxModel value)
        {
            var dbContext = await contextFactory.CreateDbContextAsync();
            dbContext.Boxes.Update(value);
            await dbContext.SaveChangesAsync();
        }

        public async Task<BoxModel> Get(int id)
        {
            var dbContext = await contextFactory.CreateDbContextAsync();
            BoxModel? box = await dbContext.Boxes.FindAsync(id);
            return box ?? throw new InvalidOperationException("Box not found");
        }
    }
}
