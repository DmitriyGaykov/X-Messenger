use X_Messenger;

create table Users(
	UserID int primary key,
	Nickname nvarchar(50),
	[Login] nvarchar(30) unique,
	[Password] nvarchar(64),
	[Image] varbinary(MAX) null,
	[Descr] nvarchar(300) null
)

create table Messages
(
	UserFrom int,
	UserTo int,
	[Message] varbinary(max),
	[Date] date,

	constraint UserFrom_FK foreign key(UserFrom) references Users(UserID),
	constraint UserTo_FK foreign key(UserTo) references Users(UserID)
)

CREATE INDEX IX_Messages_Date ON Messages (Date ASC);
CREATE INDEX idx_Messages_UserFrom ON Messages(UserFrom);
CREATE INDEX idx_Messages_UserTo ON Messages(UserTo);
CREATE INDEX idx_Users_UserID ON Users (UserID);
CREATE INDEX idx_Users_Login ON Users (Login);


