-- =====================================================================
-- Run ONCE on an existing database created by the legacy MVC app.
--
-- The new API's first migration ("Baseline") describes exactly the schema
-- that the legacy migrations already created, so we only mark it as applied.
-- After running this script:   dotnet ef database update
-- will apply only the new migrations (e.g. AddRefreshTokens).
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20261120193736_Baseline')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261120193736_Baseline', N'10.0.15');
END
GO
