CREATE TABLE [ryg].[UserPendRegAvailable]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [EncryptedEmailAddress] VARCHAR(512) NOT NULL, 
    [EncryptedCountryIsoCode] VARCHAR(64) NOT NULL, 
    [RegistrationDeniedReason] CHAR(1) NOT NULL, 
    [RequestDate] DATETIME2(0) NOT NULL DEFAULT GETDATE(), 
    [ConfirmationToken] VARCHAR(128) NULL, 
    [ConfirmationEmailSent] BIT NULL, 
    [ConfirmationEmailSentDate] DATETIME2(0) NULL, 
    [ConfirmedEmailDate] DATETIME2(0) NULL, 
    [AvailableEmailSent] BIT NULL, 
    [AvailableEmailSentDate] DATETIME2(0) NULL, 
    [UserRegisteredDate] DATETIME2(0) NULL
)
