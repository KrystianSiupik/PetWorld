namespace PetWorld.Application.Chat.Models;

public sealed record ChatHistoryItem(Guid Id, DateTime Date, string Question, string Answer, int Iterations);
