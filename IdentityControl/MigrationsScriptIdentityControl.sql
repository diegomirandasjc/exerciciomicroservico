﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
    "Id" text NOT NULL,
    "UserName" character varying(256) NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

CREATE TABLE "OpenIddictApplications" (
    "Id" text NOT NULL,
    "ClientId" character varying(100) NULL,
    "ClientSecret" text NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "ConsentType" character varying(50) NULL,
    "DisplayName" text NULL,
    "DisplayNames" text NULL,
    "Permissions" text NULL,
    "PostLogoutRedirectUris" text NULL,
    "Properties" text NULL,
    "RedirectUris" text NULL,
    "Requirements" text NULL,
    "Type" character varying(50) NULL,
    CONSTRAINT "PK_OpenIddictApplications" PRIMARY KEY ("Id")
);

CREATE TABLE "OpenIddictScopes" (
    "Id" text NOT NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "Description" text NULL,
    "Descriptions" text NULL,
    "DisplayName" text NULL,
    "DisplayNames" text NULL,
    "Name" character varying(200) NULL,
    "Properties" text NULL,
    "Resources" text NULL,
    CONSTRAINT "PK_OpenIddictScopes" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OpenIddictAuthorizations" (
    "Id" text NOT NULL,
    "ApplicationId" text NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "CreationDate" timestamp with time zone NULL,
    "Properties" text NULL,
    "Scopes" text NULL,
    "Status" character varying(50) NULL,
    "Subject" character varying(400) NULL,
    "Type" character varying(50) NULL,
    CONSTRAINT "PK_OpenIddictAuthorizations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~" FOREIGN KEY ("ApplicationId") REFERENCES "OpenIddictApplications" ("Id")
);

CREATE TABLE "OpenIddictTokens" (
    "Id" text NOT NULL,
    "ApplicationId" text NULL,
    "AuthorizationId" text NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "CreationDate" timestamp with time zone NULL,
    "ExpirationDate" timestamp with time zone NULL,
    "Payload" text NULL,
    "Properties" text NULL,
    "RedemptionDate" timestamp with time zone NULL,
    "ReferenceId" character varying(100) NULL,
    "Status" character varying(50) NULL,
    "Subject" character varying(400) NULL,
    "Type" character varying(50) NULL,
    CONSTRAINT "PK_OpenIddictTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES "OpenIddictApplications" ("Id"),
    CONSTRAINT "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId" FOREIGN KEY ("AuthorizationId") REFERENCES "OpenIddictAuthorizations" ("Id")
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE UNIQUE INDEX "IX_OpenIddictApplications_ClientId" ON "OpenIddictApplications" ("ClientId");

CREATE INDEX "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type" ON "OpenIddictAuthorizations" ("ApplicationId", "Status", "Subject", "Type");

CREATE UNIQUE INDEX "IX_OpenIddictScopes_Name" ON "OpenIddictScopes" ("Name");

CREATE INDEX "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type" ON "OpenIddictTokens" ("ApplicationId", "Status", "Subject", "Type");

CREATE INDEX "IX_OpenIddictTokens_AuthorizationId" ON "OpenIddictTokens" ("AuthorizationId");

CREATE UNIQUE INDEX "IX_OpenIddictTokens_ReferenceId" ON "OpenIddictTokens" ("ReferenceId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231105193249_Initial', '7.0.13');

COMMIT;

START TRANSACTION;

DROP TABLE "OpenIddictScopes";

DROP TABLE "OpenIddictTokens";

DROP TABLE "OpenIddictAuthorizations";

DROP TABLE "OpenIddictApplications";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20231105214046_Remove tables openid', '7.0.13');

COMMIT;

