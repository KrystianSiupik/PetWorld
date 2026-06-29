using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.Chat.Abstractions;
using PetWorld.Application.Chat.Handlers;

namespace PetWorld.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAskQuestionHandler, AskQuestionHandler>();
        services.AddScoped<IGetChatHistoryHandler, GetChatHistoryHandler>();

        return services;
    }
}
