CREATE TABLE `sci_groupurl` (
  `ID` bigint NOT NULL,
  `URLID` blob,
  `MODULEID` bigint DEFAULT NULL,
  `CREATEDATE` date DEFAULT NULL,
  `CREATEDBY` bigint DEFAULT NULL,
  `BATCH` bigint DEFAULT NULL,
  `GROUP_ID` bigint DEFAULT NULL,
  PRIMARY KEY (`ID`)
);
