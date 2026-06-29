using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using PetWorld.Application.Chat.Abstractions;
using PetWorld.Domain.Chat;

namespace PetWorld.Infrastructure.Agents;

public sealed class WriterAgent : IWriterAgent
{
    private const string Instructions = """
        Jesteś konsultantem sklepu zoologicznego PetWorld. Pomagasz klientom dobrać
        produkty dla ich zwierząt.

        Zasady:
        - Użyj narzędzia GetProducts, aby pobrać pełną listę dostępnych produktów.
        - Polecaj WYŁĄCZNIE produkty z tej listy; podaj nazwę i krótkie uzasadnienie
          dopasowania do potrzeby klienta.
        - Jeśli na liście nie ma niczego pasującego — powiedz to wprost. NIE wymyślaj
          produktów spoza listy.
        - Odpowiadaj po polsku, rzeczowo i przyjaźnie.
        - Jeśli dostaniesz uwagi recenzenta, popraw odpowiedź zgodnie z nimi.
        """;

    private readonly AIAgent _agent;

    public WriterAgent(IChatClient chatClient, ProductCatalogTool productCatalogTool)
    {
        _agent = chatClient.AsAIAgent(
            name: "Writer",
            instructions: Instructions,
            tools: [AIFunctionFactory.Create(productCatalogTool.GetProducts)]);
    }

    public async Task<AssistantAnswer> WriteAsync(CustomerQuestion question, string? criticFeedback, CancellationToken cancellationToken)
    {
        var input = criticFeedback is null
            ? question.Value
            : $"""
               Pytanie klienta:
               {question.Value}

               Uwagi recenzenta do poprawy:
               {criticFeedback}

               Popraw odpowiedź, uwzględniając te uwagi.
               """;

        var response = await _agent.RunAsync(input, cancellationToken: cancellationToken);

        return AssistantAnswer.Create(response.Text);
    }
}
