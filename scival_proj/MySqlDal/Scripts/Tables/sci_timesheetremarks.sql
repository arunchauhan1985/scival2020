CREATE TABLE `sci_timesheetremarks` (
  `WORKFLOWID` bigint DEFAULT NULL,
  `TRANSITIONALID` bigint NOT NULL,
  `STATUSCODE` bigint DEFAULT NULL,
  `REMARKS` varchar(4000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `USERID` bigint DEFAULT NULL,
  `CREATEDDATE` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`TRANSITIONALID`)
);
