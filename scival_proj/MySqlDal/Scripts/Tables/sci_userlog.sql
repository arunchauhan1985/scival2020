CREATE TABLE `sci_userlog` (
  `WORKFLOWID` bigint NOT NULL,
  `MODULEID` bigint DEFAULT NULL,
  `ID` bigint DEFAULT NULL,
  `CYCLEID` bigint DEFAULT NULL,
  `PAGENAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `USERID` bigint DEFAULT NULL,
  `CREATEDDATE` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `ACTION` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`WORKFLOWID`)
);