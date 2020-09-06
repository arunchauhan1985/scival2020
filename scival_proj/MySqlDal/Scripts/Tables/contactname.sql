CREATE TABLE `contactname` (
  `PREFIX` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `GIVENNAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `MIDDLENAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `SURNAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `SUFFIX` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `CONTACT_ID` bigint NOT NULL,
  PRIMARY KEY (`CONTACT_ID`)
);
