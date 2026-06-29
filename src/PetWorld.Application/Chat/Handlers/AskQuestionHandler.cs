using PetWorld.Application.Chat.Abstractions;
using PetWorld.Application.Chat.Models;
using PetWorld.Domain.Chat;

namespace PetWorld.Application.Chat.Handlers;

public sealed class AskQuestionHandler : IAskQuestionHandler
{
    private const int MaxIterations = 3;

    private readonly IWriterAgent _writer;
    private readonly ICriticAgent _critic;
    private readonly IChatRepository _repository;

    public AskQuestionHandler(IWriterAgent writer, ICriticAgent critic, IChatRepository repository)
    {
        _writer = writer;
        _critic = critic;
        _repository = repository;
    }

    public async Task<ChatResult> HandleAsync(string question, CancellationToken cancellationToken)
    {
        var customerQuestion = CustomerQuestion.Create(question);

        AssistantAnswer answer;
        string? feedback = null;
        var iterations = 0;

        do
        {
            answer = await _writer.WriteAsync(customerQuestion, feedback, cancellationToken);
            iterations++;

            var verdict = await _critic.ReviewAsync(customerQuestion, answer, cancellationToken);
            if (verdict.Approved)
                break;

            feedback = verdict.Feedback;
        }
        while (iterations < MaxIterations);

        var interaction = ChatInteraction.Create(customerQuestion.Value, answer.Value, iterations);
        await _repository.AddAsync(interaction, cancellationToken);

        return new ChatResult(answer.Value, iterations);
    }
}
