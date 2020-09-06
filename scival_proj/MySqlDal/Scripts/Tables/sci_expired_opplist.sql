CREATE TABLE `sci_expired_opplist` (
  `MODULENAME` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `MODULEID` bigint DEFAULT NULL,
  `OPPNAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `FUNDINGBODYNAME` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `ID` bigint NOT NULL,
  `TASKNAME` varchar(6) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `TASKID` bigint DEFAULT NULL,
  `CYCLE` bigint DEFAULT NULL,
  `DUEDATE` datetime DEFAULT NULL,
  `CID` bigint DEFAULT NULL,
  `LOGDATE` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `FLAG` varchar(1) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`ID`)
);
