CREATE TABLE `sci_workflow` (
  `WORKFLOWID` bigint NOT NULL,
  `MODULEID` bigint DEFAULT NULL,
  `ID` bigint DEFAULT NULL,
  `CYCLE` bigint DEFAULT NULL,
  `TEMPLATEID` bigint DEFAULT NULL,
  `TASKID` bigint DEFAULT NULL,
  `SEQUENCE` bigint DEFAULT NULL,
  `STARTDATE` datetime DEFAULT NULL,
  `STARTBY` bigint DEFAULT NULL,
  `COMPLETEDDATE` datetime DEFAULT NULL,
  `COMPLETEDBY` bigint DEFAULT NULL,
  `CREATEDBY` bigint DEFAULT NULL,
  `ASSIGNBY` bigint DEFAULT NULL,
  `ASSIGNDATE` datetime DEFAULT NULL,
  `STATUSID` bigint DEFAULT NULL,
  PRIMARY KEY (`WORKFLOWID`),
  KEY `IND_SCIWORKFLOW` (`CYCLE`,`ID`,`MODULEID`),
  KEY `IND_WORKFLOW` (`ID`)
);
