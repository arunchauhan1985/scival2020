CREATE TABLE `fundingpolicy` (
  `FUNDINGPOLICY_ID` bigint NOT NULL,
  `FUNDINGBODY_ID` bigint DEFAULT NULL,
  `OPPORTUNITY_ID` bigint DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  PRIMARY KEY (`FUNDINGPOLICY_ID`)
);
