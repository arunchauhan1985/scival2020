CREATE TABLE `opportunity_location` (
  `LOCATION_ID` bigint NOT NULL,
  `COUNTRY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `ROOM` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `STREET` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `CITY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `STATE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `POSTALCODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `OPPORTUNITY_ID` bigint DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  PRIMARY KEY (`LOCATION_ID`)
);
