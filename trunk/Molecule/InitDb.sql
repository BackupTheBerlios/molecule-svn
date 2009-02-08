CREATE TABLE Roles
(
  Rolename Varchar (255) NOT NULL,
  ApplicationName varchar (255) NOT NULL
);

CREATE TABLE UsersInRoles
(
  Username Varchar (255) NOT NULL,
  Rolename Varchar (255) NOT NULL,
  ApplicationName Text (255) NOT NULL
);

CREATE TABLE `aspnet_applications` (
  `ApplicationName` text default NULL,
  `ApplicationId` int(11) default NULL,
  `Description` text default NULL,
  `PropertyValuesString` text default NULL,
  PRIMARY KEY  (`ApplicationId`)
);

CREATE TABLE `aspnet_profile` (
 `PKID` varchar(36)   NOT NULL default '',
 `LastUpdatedDate` datetime default NULL,
 `PropertyNames` text default NULL,
 `PropertyValuesString` text default NULL
);

CREATE TABLE `users` (
  'PKID' varchar(36)  NOT NULL default '',
  'Username' varchar(255)  NOT NULL default '',
  'ApplicationName' varchar(100) 
                     NOT NULL default '',
  'Email' varchar(100)  NOT NULL default '',
  'Comment' varchar(255)  default NULL,
  'Password' varchar(128)  NOT NULL default '',
  'PasswordQuestion' varchar(255)  default NULL,
  'PasswordAnswer' varchar(255)  default NULL,
  'IsApproved' tinyint(1) default NULL,
  'LastActivityDate' datetime default NULL,
  'LastLoginDate' datetime default NULL,
  'LastPasswordChangedDate' datetime default NULL,
  'CreationDate' datetime default NULL,
  'IsOnLine' tinyint(1) default NULL,
  'IsLockedOut' tinyint(1) default NULL,
  'LastLockedOutDate' datetime default NULL,
  'FailedPasswordAttemptCount' int(11) default NULL,
  'FailedPasswordAttemptWindowStart' datetime default NULL,
  'FailedPasswordAnswerAttemptCount' int(11) default NULL,
  'FailedPasswordAnswerAttemptWindowStart' datetime default NULL,
  PRIMARY KEY  ('PKID')
);

CREATE TABLE `semantic_event_messages` (
	'PKID'   INTEGER PRIMARY KEY,
    'Type' VARCHAR(255) NOT NULL, 
    'Title' VARCHAR(255) NOT NULL,
    'CreationDate' datetime NOT NULL,
	'ImageUri' VARCHAR(255) default NULL,
	'Description' VARCHAR(255) default NULL,
	'Url' VARCHAR(255) default NULL
);

CREATE INDEX idxRoles ON Roles ( 'Rolename' , 'ApplicationName' );

CREATE INDEX idxUIR ON UsersInroles ( 'Username', 'Rolename', 'ApplicationName');
