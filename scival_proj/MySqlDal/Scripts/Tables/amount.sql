CREATE TABLE `amount` (
  `CURRENCY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `AMOUNT_TEXT` bigint NOT NULL,
  `AWARD_ID` bigint NOT NULL,
  `AWARDEE_ID` bigint DEFAULT NULL,
  `CREATE_DATE` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`AWARD_ID`,`AMOUNT_TEXT`),
  KEY `IND_AMNT` (`AWARD_ID`)
);