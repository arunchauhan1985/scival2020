CREATE TABLE `keywords` (
  `KEYWORDS_ID` bigint NOT NULL,
  `FUNDINGBODY_ID` bigint DEFAULT NULL,
  `OPPORTUNITY_ID` bigint DEFAULT NULL,
  `AWARD_ID` bigint DEFAULT NULL,
  PRIMARY KEY (`KEYWORDS_ID`)
);
