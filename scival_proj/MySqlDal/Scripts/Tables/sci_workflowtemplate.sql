CREATE TABLE `sci_workflowtemplate` (
  `TEMPLATEID` bigint NOT NULL,
  `TASKID` bigint DEFAULT NULL,
  `SEQUENCE` bigint DEFAULT NULL,
  PRIMARY KEY (`TEMPLATEID`)
);
