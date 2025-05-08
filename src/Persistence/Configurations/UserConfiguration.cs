using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

/// <summary>
/// Configures the entity model for the <see cref="User"/> entity using Fluent API.
/// </summary>
/// <remarks>
/// This class defines entity relationships, constraints, indexes, and field mappings for the <see cref="User"/> table.
/// </remarks>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Map to User table
        builder.ToTable(TableNames.User);
        
        // Primary Key
        builder
            .HasKey(u => u.Id);
        
        // Set the UserName with conversion to string and a maximum length of 20
        builder
            .Property(user => user.UserName)
            .HasConversion(
                userName => userName.Value,
                value => UserName.Create(value).Value)
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();
        
        // Set the Email with conversion to string and a maximum length of 50
        builder
            .Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value)
            .HasMaxLength(Email.MaxLength)
            .IsRequired();
        
        // Set unique index on UserName
        builder
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Set unique index on Email
        builder
            .HasIndex(u => u.Email)
            .IsUnique();
        
        // Set the FullName as a required property with a maximum length of 50
        builder
            .Property(u => u.FullName)
            .HasMaxLength(50)
            .IsRequired();
        
        // Set the PasswordHash as a required property
        builder
            .Property(u => u.PasswordHash)
            .IsRequired();
        
        // Filter for deleted users
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}