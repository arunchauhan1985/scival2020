CREATE TABLE `fundingbody_master` (
  `FUNDINGBODY_ID` bigint NOT NULL,
  `FUNDINGBODYNAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `URL` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `CREATEDDATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CREATEDBY` bigint DEFAULT NULL,
  `CYCLECOMPLETIONDATE` datetime DEFAULT NULL,
  `CYCLECOMPLETEDBY` bigint DEFAULT NULL,
  `LASTUPDATEDDATE` datetime DEFAULT NULL,
  `LASTUPDATEDBY` bigint DEFAULT NULL,
  `CYCLE` bigint DEFAULT NULL,
  `STATUSCODE` bigint DEFAULT NULL,
  `RUSH` bigint DEFAULT NULL,
  `RUSHBY` bigint DEFAULT NULL,
  `ALLOCATIONMODE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `ISOPORTUNITY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `COUNTRY` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `DUEDATE` datetime DEFAULT NULL,
  `ISVIEWED` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `BATCHNO` bigint DEFAULT NULL,
  `BATCHRECIEVEDATE` datetime DEFAULT NULL,
  `SUBTYPE` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `HIDDEN_FLAG` bigint DEFAULT NULL,
  PRIMARY KEY (`FUNDINGBODY_ID`)
);
