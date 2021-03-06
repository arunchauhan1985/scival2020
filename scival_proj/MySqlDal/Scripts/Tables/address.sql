CREATE TABLE `address` (
  `COUNTRYTEST` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `ROOM` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `STREET` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `CITY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `STATE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `POSTALCODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `CONTACT_ID` bigint DEFAULT NULL,
  `AWARDMANAGER_ID` bigint DEFAULT NULL,
  `AFFILIATION_ID` bigint DEFAULT NULL,
  `Id` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
);
