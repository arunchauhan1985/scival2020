CREATE TABLE `sci_urls` (
  `ID` bigint NOT NULL,
  `MODULEID` bigint DEFAULT NULL,
  `PAGENAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `URL` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `USERID` bigint DEFAULT NULL,
  `ISACTIVE` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '0',
  `DELETEDBY` bigint DEFAULT NULL,
  PRIMARY KEY (`ID`)
);
