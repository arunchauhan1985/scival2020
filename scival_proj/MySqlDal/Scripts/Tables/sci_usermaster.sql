CREATE TABLE `sci_usermaster` (
  `USERID` bigint NOT NULL,
  `USERNAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `PASSWORD` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `NAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `EMPCODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `CONTACTNO` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `EMAIL` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `ISACTIVE` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'Y',
  `CREATEDDATE` datetime NOT NULL,
  `CREADTEDBY` bigint NOT NULL,
  PRIMARY KEY (`USERID`)
);
