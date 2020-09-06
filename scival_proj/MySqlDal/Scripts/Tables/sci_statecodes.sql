CREATE TABLE `sci_statecodes` (
  `STATEID` bigint NOT NULL,
  `COUNTRYID` bigint DEFAULT NULL,
  `NAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `CODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`STATEID`)
);
