CREATE DATABASE "DCM_REPORT"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

\c "DCM_REPORT"

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "AccountsBalanceDates" (
    "Id" uuid NOT NULL,
    "FK_Account" uuid NOT NULL,
    "AccountDescription" text NOT NULL,
    "AccountBalance" numeric NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_AccountsBalanceDates" PRIMARY KEY ("Id")
);

CREATE TABLE "AccountsOperationDetails" (
    "Id" uuid NOT NULL,
    "FK_Account" uuid NOT NULL,
    "AccountDescription" text NOT NULL,
    "AccountBalanceBeforeOperation" numeric NOT NULL,
    "AccountBalanceAfeterOperation" numeric NOT NULL,
    "OperationDateTime" timestamp with time zone NOT NULL,
    "OperationAmount" numeric NOT NULL,
    "OperationUserId" text NOT NULL,
    "OperationUserName" text NOT NULL,
    CONSTRAINT "PK_AccountsOperationDetails" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231108062828_Initial', '7.0.13');

COMMIT;

