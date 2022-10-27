﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("transfer_id")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.Amount)
                .HasColumnName("amount");

            builder.Property(e => e.FromAccountId)
                .HasColumnName("from_account_id");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("clock_timestamp()");

            builder.Property(e => e.ToAccountId)
                .HasColumnName("to_account_id");

            builder.ToTable("Transfers", "SB-historic");
        }
    }
}
