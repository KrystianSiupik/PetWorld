using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetWorld.Domain.Chat;

namespace PetWorld.Infrastructure.Persistence.Configurations;

public sealed class ChatInteractionConfiguration : IEntityTypeConfiguration<ChatInteraction>
{
    public void Configure(EntityTypeBuilder<ChatInteraction> builder)
    {
        builder.ToTable("ChatInteractions");

        builder.HasKey(interaction => interaction.Id);

        builder.Property(interaction => interaction.Question)
            .HasConversion(
                question => question.Value,
                value => CustomerQuestion.Create(value))
            .HasColumnName("Question")
            .IsRequired();

        builder.Property(interaction => interaction.Answer)
            .HasConversion(
                answer => answer.Value,
                value => AssistantAnswer.Create(value))
            .HasColumnName("Answer")
            .IsRequired();

        builder.Property(interaction => interaction.IterationCount)
            .HasConversion(
                iterationCount => iterationCount.Value,
                value => IterationCount.Create(value))
            .HasColumnName("IterationCount")
            .IsRequired();

        builder.Property(interaction => interaction.CreatedAt)
            .IsRequired();
    }
}
