CREATE TABLE `fundedsubproject` (
  `SUBFUND_ID` bigint NOT NULL,
  `FUND_ID` bigint NOT NULL,
  `AWARD_ID` bigint NOT NULL,
  `FUNDINGBODYPROJECTID` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `AMOUNT` bigint DEFAULT NULL,
  `CURRENCY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`FUND_ID`)
);
