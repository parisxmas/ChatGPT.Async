/****** Object:  Table [dbo].[Messages]    Script Date: 3/21/2023 11:27:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Messages](
    [Id] [uniqueidentifier] NOT NULL,
    [Text] [nvarchar](max) NOT NULL,
    [SendDate] [datetime] NOT NULL,
    [SenderId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_Id]  DEFAULT (newid()) FOR [Id]
    GO

    SET ANSI_NULLS ON
    GO

    SET QUOTED_IDENTIFIER ON
    GO

CREATE TABLE [dbo].[Senders](
    [Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
    [Name] [nvarchar](400) NOT NULL,
    CONSTRAINT [PK_Senders] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO

ALTER TABLE [dbo].[Senders] ADD  CONSTRAINT [DF_Senders_Id]  DEFAULT (newid()) FOR [Id]
    GO

    
INSERT INTO [dbo].[Senders] (Name) VALUES ('User');
INSERT INTO [dbo].[Senders] (Name) VALUES ('System');

/****** Object:  StoredProcedure [dbo].[SP_GetChats]    Script Date: 3/21/2023 11:28:38 PM ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE PROCEDURE [dbo].[SP_GetChats]
@Page INT = NULL,
@PageSize INT = NULL,
@Search NVARCHAR(MAX) = NULL
AS
BEGIN

	IF @Page IS NULL
BEGIN
		SET @Page = 0
END

	IF @PageSize IS NULL OR @PageSize = 0
BEGIN
		SET @PageSize = 10
END

SELECT m.Text, m.SendDate, m.SenderId FROM Messages m
WHERE  (@Search IS NULL OR m.Text LIKE '%Search%')
ORDER BY m.SendDate OFFSET (@Page) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetSenders]
AS
BEGIN
SELECT * FROM Senders
END



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_InsertMessage]
@Text NVARCHAR(MAX) = NULL,
@SenderId UNIQUEIDENTIFIER = NULL
AS
BEGIN
	DECLARE @Status TABLE(
		Detail NVARCHAR(MAX) COLLATE DATABASE_DEFAULT NOT NULL,
		RecordId INT NULL,
		ErrorId INT NULL,
		Error BIT NULL DEFAULT 1);

	IF @Text IS NULL INSERT INTO @Status (Detail) VALUES ('Text should not be empty!')
	IF @SenderId IS NULL INSERT INTO @Status (Detail) VALUES ('Sender should not be empty!')

BEGIN TRY
INSERT INTO Messages (Text, SendDate, SenderId) VALUES (@Text, GETDATE(), @SenderId)
END TRY
BEGIN CATCH
INSERT INTO @Status (Detail, ErrorId, Error) VALUES (CONCAT(ERROR_MESSAGE(), ' || Line: ', ERROR_LINE(), ' || State: ', ERROR_STATE(), ' || Severity: ', ERROR_SEVERITY()), ERROR_NUMBER(), 1)
SELECT * FROM @Status
END CATCH
END
