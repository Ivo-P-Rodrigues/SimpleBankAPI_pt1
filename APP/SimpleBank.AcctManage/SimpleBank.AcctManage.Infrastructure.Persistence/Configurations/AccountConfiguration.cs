﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("account_id")
                .HasDefaultValueSql("uuid_generate_v4()"); 

            builder.Property(e => e.Balance)
                .HasColumnName("balance");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("clock_timestamp()");

            builder.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasColumnName("currency")
                .HasDefaultValueSql("'EUR'::bpchar")
                .IsFixedLength();

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("account_user_fkey");

            builder.ToTable("Accounts", "SB-operational");
        }
    }
}
