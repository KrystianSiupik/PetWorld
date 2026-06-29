using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using PetWorld.Application.Chat.Abstractions;
using PetWorld.Application.Chat.Models;
using PetWorld.Domain.Chat;

namespace PetWorld.Infrastructure.Agents;

public sealed class CriticAgent : ICriticAgent
{
    private const string Instructions = """
        Oceniasz jakość odpowiedzi konsultanta sklepu PetWorld dla klienta.

        Kryteria akceptacji:
        - Odpowiedź jest trafna i odnosi się do pytania klienta.
        - Gdy pytanie dotyczy produktów — poleca konkretne pozycje z uzasadnieniem.
        - Polecane produkty wyglądają na pochodzące z oferty (nie zmyślone). Jeśli widać
          zmyślony produkt — odrzuć.
        - Język polski, jasny i pomocny.

        Zwróć approved=true tylko gdy wszystkie kryteria są spełnione. W przeciwnym razie
        approved=false oraz krótkie, konkretne wskazówki do poprawy w polu feedback.
        """;

    private readonly AIAgent _agent;

    public CriticAgent(IChatClient chatClient)
    {
        _agent = chatClient.AsAIAgent(
            name: "Critic",
            instructions: Instructions);
    }

    public async Task<CriticVerdict> ReviewAsync(CustomerQuestion question, AssistantAnswer answer, CancellationToken cancellationToken)
    {
        var input = $"""
            Pytanie klienta:
            {question.Value}

            Odpowiedź konsultanta do oceny:
            {answer.Value}
            """;

        var response = await _agent.RunAsync<CriticVerdict>(input, cancellationToken: cancellationToken);

        return response.Result;
    }
}
