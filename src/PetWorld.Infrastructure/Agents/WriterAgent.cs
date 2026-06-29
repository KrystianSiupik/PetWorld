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
        - Aby polecić produkty, użyj narzędzia SearchProducts z trafnym zapytaniem.
        - Polecaj WYŁĄCZNIE produkty zwrócone przez SearchProducts; podaj nazwę i krótkie
          uzasadnienie dopasowania.
        - Jeśli narzędzie nie zwróci pasujących produktów lub nie masz pewności — powiedz
          to wprost. NIE wymyślaj produktów spoza wyników wyszukiwania.
        - Odpowiadaj po polsku, rzeczowo i przyjaźnie.
        - Jeśli dostaniesz uwagi recenzenta, popraw odpowiedź zgodnie z nimi.
        """;

    private readonly AIAgent _agent;

    public WriterAgent(IChatClient chatClient, ProductSearchTool productSearchTool)
    {
        _agent = chatClient.AsAIAgent(
            name: "Writer",
            instructions: Instructions,
            tools: [AIFunctionFactory.Create(productSearchTool.SearchProducts)]);
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
