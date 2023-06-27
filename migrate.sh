#!/bin/bash

dotnet ef database drop -s API --context TradeNContext --force

# Remove the latest migration from the Infrastructure project (TradeNContext)
dotnet ef migrations remove -p Infrastructure -s API --context TradeNContext --force


# Remove the latest migration from the TradeNIdentity project (TradeNIdentityDbContext)
dotnet ef migrations remove -p TradeNIdentity.cs -s API --context TradeNIdentityDbContext --force

# Run the migrations for the Infrastructure project (TradeNContext)
dotnet ef migrations add initial -p Infrastructure -s API -o Migrations --context TradeNContext

# Run the migrations for the TradeNIdentity project (TradeNIdentityDbContext)
dotnet ef migrations add initial -p TradeNIdentity.cs -s API -o Migrations --context TradeNIdentityDbContext

# Apply the migrations to the database
dotnet ef database update -s API --context TradeNContext
dotnet ef database update -s API --context TradeNIdentityDbContext