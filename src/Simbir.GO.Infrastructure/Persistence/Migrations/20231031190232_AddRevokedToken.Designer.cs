﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Simbir.GO.Infrastructure.Persistence;

#nullable disable

namespace Simbir.GO.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231031190232_AddRevokedToken")]
    partial class AddRevokedToken
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Simbir.GO.Application.Services.Common.RevokedToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("added_date");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_revoked");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("jwt_id");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_refresh_tokens");

                    b.ToTable("refresh_tokens", (string)null);
                });

            modelBuilder.Entity("Simbir.GO.Domain.Accounts.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_salt");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_accounts");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("Simbir.GO.Domain.Rents.Rent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint")
                        .HasColumnName("account_id");

                    b.Property<double?>("FinalPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("final_price");

                    b.Property<double>("PriceOfUnit")
                        .HasColumnType("double precision")
                        .HasColumnName("price_of_unit");

                    b.Property<int>("PriceType")
                        .HasColumnType("integer")
                        .HasColumnName("price_type");

                    b.Property<DateTime?>("TimeEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time_end");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("time_start");

                    b.Property<long>("TransportId")
                        .HasColumnType("bigint")
                        .HasColumnName("transport_id");

                    b.HasKey("Id")
                        .HasName("pk_rents");

                    b.HasIndex("AccountId")
                        .HasDatabaseName("ix_rents_account_id");

                    b.HasIndex("TransportId")
                        .HasDatabaseName("ix_rents_transport_id");

                    b.ToTable("rents", (string)null);
                });

            modelBuilder.Entity("Simbir.GO.Domain.Transports.Transport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("CanBeRented")
                        .HasColumnType("boolean")
                        .HasColumnName("can_be_rented");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("color");

                    b.Property<double?>("DayPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("day_price");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identifier");

                    b.Property<double?>("MinutePrice")
                        .HasColumnType("double precision")
                        .HasColumnName("minute_price");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("model");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint")
                        .HasColumnName("owner_id");

                    b.Property<int>("TransportType")
                        .HasColumnType("integer")
                        .HasColumnName("transport_type");

                    b.HasKey("Id")
                        .HasName("pk_transports");

                    b.ToTable("transports", (string)null);
                });

            modelBuilder.Entity("Simbir.GO.Domain.Accounts.Account", b =>
                {
                    b.OwnsOne("Simbir.GO.Domain.Accounts.ValueObjects.Balance", "Balance", b1 =>
                        {
                            b1.Property<long>("AccountId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<double>("Value")
                                .HasColumnType("double precision")
                                .HasColumnName("balance_value");

                            b1.HasKey("AccountId");

                            b1.ToTable("accounts");

                            b1.WithOwner()
                                .HasForeignKey("AccountId")
                                .HasConstraintName("fk_accounts_accounts_id");
                        });

                    b.Navigation("Balance")
                        .IsRequired();
                });

            modelBuilder.Entity("Simbir.GO.Domain.Rents.Rent", b =>
                {
                    b.HasOne("Simbir.GO.Domain.Accounts.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_rents_accounts_account_temp_id");

                    b.HasOne("Simbir.GO.Domain.Transports.Transport", "Transport")
                        .WithMany()
                        .HasForeignKey("TransportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_rents_transports_transport_temp_id");

                    b.Navigation("Account");

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("Simbir.GO.Domain.Transports.Transport", b =>
                {
                    b.OwnsOne("Simbir.GO.Domain.Transports.ValueObjects.Coordinate", "Coordinate", b1 =>
                        {
                            b1.Property<long>("TransportId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision")
                                .HasColumnName("coordinate_latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision")
                                .HasColumnName("coordinate_longitude");

                            b1.HasKey("TransportId");

                            b1.ToTable("transports");

                            b1.WithOwner()
                                .HasForeignKey("TransportId")
                                .HasConstraintName("fk_transports_transports_id");
                        });

                    b.Navigation("Coordinate")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
