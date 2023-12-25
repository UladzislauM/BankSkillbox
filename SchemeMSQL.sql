CREATE TABLE [dbo].[clients]
(
	[id] BIGINT PRIMARY KEY NOT NULL, 
    [first_name] NVARCHAR(50) NULL, 
    [last_name] NVARCHAR(50) NULL, 
    [history] NVARCHAR(2000) NULL, 
    [prestige] INT NULL, 
    [status] NVARCHAR(30) DEFAULT 'General' NOT NULL
)

CREATE TABLE [dbo].[scores]
(
	[id] BIGINT PRIMARY KEY NOT NULL, 
    [balance] DECIMAL(25, 10) NULL, 
    [percent] DECIMAL(10, 10) NULL, 
    [date_score] DATETIME NULL, 
    [is_capitalization] BIT DEFAULT 0  NOT NULL, 
    [is_money] Bit DEFAULT  0  NOT NULL, 
    [deadline] DATETIME NULL, 
    [date_last_dividends] DATETIME NULL, 
    [client_id] BIGINT NOT NULL, 
    [score_type] NVARCHAR(30) NOT NULL, 
    [is_active] BIT NOT NULL
	FOREIGN KEY (client_id) REFERENCES [dbo].[clients](id)
)
