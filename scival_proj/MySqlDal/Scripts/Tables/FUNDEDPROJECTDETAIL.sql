CREATE TABLE `fundedprojectdetail` (
  `FUND_ID` bigint NOT NULL,
  `AWARD_ID` bigint DEFAULT NULL,
  `ACRONYM` varchar(200) DEFAULT NULL,
  `BUDGET_AMOUNT` bigint DEFAULT NULL,
  `BUDGET_CURRENCY` varchar(200) DEFAULT NULL,
  `ENDDATE` date DEFAULT NULL,
  `FUNDINGBODYPROJECTID` varchar(200) DEFAULT NULL,
  `STARTDATE` date DEFAULT NULL,
  `STATUS` varchar(200) DEFAULT NULL,
  `COUNTRY` varchar(20) DEFAULT NULL,
  `LOCALITY` varchar(200) DEFAULT NULL,
  `POSTALCODE` varchar(200) DEFAULT NULL,
  `REGION` varchar(200) DEFAULT NULL,
  `STREET` varchar(200) DEFAULT NULL,
  `POSTOFFICEBOXNUMBER` varchar(200) DEFAULT NULL,
  `LINK` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`FUND_ID`)
);
