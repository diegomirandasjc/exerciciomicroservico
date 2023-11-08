CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Accounts" (
    "Id" uuid NOT NULL,
    "Description" text NOT NULL,
    "Balance" numeric NOT NULL,
    CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id")
);

CREATE TABLE "AccountsMovimentations" (
    "Id" uuid NOT NULL,
    "UserId" text NOT NULL,
    "UserName" text NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "OperationType" integer NOT NULL,
    "Amount" numeric NOT NULL,
    "BalanceSnapShot" numeric NOT NULL,
    "FK_Account" uuid NOT NULL,
    CONSTRAINT "PK_AccountsMovimentations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AccountsMovimentations_Accounts_FK_Account" FOREIGN KEY ("FK_Account") REFERENCES "Accounts" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AccountsMovimentations_FK_Account" ON "AccountsMovimentations" ("FK_Account");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231105040003_Initial', '7.0.13');

COMMIT;

