CREATE TABLE `totalfunding` (
  `CURRENCY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `TOTALFUNDING_TEXT` bigint NOT NULL,
  `AWARDSTATISTICS_ID` bigint DEFAULT NULL,
  PRIMARY KEY (`TOTALFUNDING_TEXT`)
);
