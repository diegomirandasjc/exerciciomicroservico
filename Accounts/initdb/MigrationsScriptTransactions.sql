CREATE DATABASE "DCM_ACCOUNT"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

\c "DCM_ACCOUNT"

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

CREATE TABLE "AccountOperationPerformedMessageOutbox" (
    "Id" uuid NOT NULL,
    "FK_Account" uuid NOT NULL,
    "AccountDescription" text NOT NULL,
    "AccountBalanceBeforeOperation" numeric NOT NULL,
    "AccountBalanceAfeterOperation" numeric NOT NULL,
    "OperationDateTime" timestamp with time zone NOT NULL,
    "OperationAmount" numeric NOT NULL,
    "OperationUserId" text NOT NULL,
    "OperationUserName" text NOT NULL,
    CONSTRAINT "PK_AccountOperationPerformedMessageOutbox" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AccountOperationPerformedMessageOutbox_Accounts_FK_Account" FOREIGN KEY ("FK_Account") REFERENCES "Accounts" ("Id") ON DELETE CASCADE
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

CREATE INDEX "IX_AccountOperationPerformedMessageOutbox_FK_Account" ON "AccountOperationPerformedMessageOutbox" ("FK_Account");

CREATE INDEX "IX_AccountsMovimentations_FK_Account" ON "AccountsMovimentations" ("FK_Account");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231108063431_Initial', '7.0.13');

COMMIT;



