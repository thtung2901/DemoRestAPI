﻿// <auto-generated />
using System;
using ProductLibrary.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductLibrary.API.Migrations
{
    [DbContext(typeof(ProductLibraryContext))]
    [Migration("20190731122611_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0-preview7.19362.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductLibrary.API.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("MainCategory")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                            DateCreated = new DateTimeOffset(new DateTime(1970, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Berry",
                            FullName = "Griffin Beak Eldritch",
                            MainCategory = "Ships"
                        },
                        new
                        {
                            Id = new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                            DateCreated = new DateTimeOffset(new DateTime(1968, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Nancy",
                            FullName = "Swashbuckler Rye",
                            MainCategory = "Rum"
                        },
                        new
                        {
                            Id = new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                            DateCreated = new DateTimeOffset(new DateTime(1991, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)),
                            Name = "Eli",
                            FullName = "Ivory Bones Sweet",
                            MainCategory = "Singing"
                        },
                        new
                        {
                            Id = new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                            DateCreated = new DateTimeOffset(new DateTime(1984, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)),
                            Name = "Arnold",
                            FullName = "The Unseen Stafford",
                            MainCategory = "Singing"
                        },
                        new
                        {
                            Id = new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                            DateCreated = new DateTimeOffset(new DateTime(1990, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)),
                            Name = "Seabury",
                            FullName = "Toxic Reyson",
                            MainCategory = "Maps"
                        },
                        new
                        {
                            Id = new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                            DateCreated = new DateTimeOffset(new DateTime(1978, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Rutherford",
                            FullName = "Fearless Cloven",
                            MainCategory = "General debauchery"
                        },
                        new
                        {
                            Id = new Guid("2ee49fe3-edf2-4f91-8409-3eb25ce6ca51"),
                            DateCreated = new DateTimeOffset(new DateTime(1959, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Atherton",
                            FullName = "Crow Ridley",
                            MainCategory = "Rum"
                        },
                        new
                        {
                            Id = new Guid("71838f8b-6ab3-4539-9e67-4e77b8ede1c0"),
                            DateCreated = new DateTimeOffset(new DateTime(1969, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Huxford",
                            FullName = "The Hawk Morris",
                            MainCategory = "Maps"
                        },
                        new
                        {
                            Id = new Guid("119f9ccb-149d-4d3c-ad4f-40100f38e918"),
                            DateCreated = new DateTimeOffset(new DateTime(1972, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)),
                            Name = "Dwennon",
                            FullName = "Rigger Quye",
                            MainCategory = "Maps"
                        },
                        new
                        {
                            Id = new Guid("28c1db41-f104-46e6-8943-d31c0291e0e3"),
                            DateCreated = new DateTimeOffset(new DateTime(1982, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Rushford",
                            FullName = "Subtle Asema",
                            MainCategory = "Rum"
                        },
                        new
                        {
                            Id = new Guid("d94a64c2-2e8f-4162-9976-0ffe03d30767"),
                            DateCreated = new DateTimeOffset(new DateTime(1976, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Name = "Hagley",
                            FullName = "Imposter Grendel",
                            MainCategory = "Singing"
                        },
                        new
                        {
                            Id = new Guid("380c2c6b-0d1c-4b82-9d83-3cf635a3e62b"),
                            DateCreated = new DateTimeOffset(new DateTime(1977, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)),
                            Name = "Mabel",
                            FullName = "Barnacle Grendel",
                            MainCategory = "Maps"
                        });
                });

            modelBuilder.Entity("ProductLibrary.API.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("Description")
                        .HasMaxLength(1500);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"),
                            CategoryId = new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                            Description = "Commandeering a ship in rough waters isn't easy.  Commandeering it without getting caught is even harder.  In this product you'll learn how to sail away and avoid those pesky musketeers.",
                            Title = "Commandeering a Ship Without Getting Caught"
                        },
                        new
                        {
                            Id = new Guid("d8663e5e-7494-4f81-8739-6e0de1bea7ee"),
                            CategoryId = new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                            Description = "In this product, the category provides tips to avoid, or, if needed, overthrow pirate mutiny.",
                            Title = "Overthrowing Mutiny"
                        },
                        new
                        {
                            Id = new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"),
                            CategoryId = new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                            Description = "Every good pirate loves rum, but it also has a tendency to get you into trouble.  In this product you'll learn how to avoid that.  This new exclusive edition includes an additional chapter on how to run fast without falling while drunk.",
                            Title = "Avoiding Brawls While Drinking as Much Rum as You Desire"
                        },
                        new
                        {
                            Id = new Guid("40ff5488-fdab-45b5-bc3a-14302d59869a"),
                            CategoryId = new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                            Description = "In this product you'll learn how to sing all-time favourite pirate songs without sounding like you actually know the words or how to hold a note.",
                            Title = "Singalong Pirate Hits"
                        });
                });

            modelBuilder.Entity("ProductLibrary.API.Entities.Product", b =>
                {
                    b.HasOne("ProductLibrary.API.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
