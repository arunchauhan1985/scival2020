CREATE TABLE `sci_countries` (
  `COUNTRY_ID` bigint NOT NULL,
  `COUNTRYGROUP_ID` bigint DEFAULT NULL,
  `COUNTRY_NAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `COUNTRY_CODE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`COUNTRY_ID`),
  UNIQUE KEY `UK_COUNTRYCODEGROUP` (`COUNTRYGROUP_ID`,`COUNTRY_CODE`)
);