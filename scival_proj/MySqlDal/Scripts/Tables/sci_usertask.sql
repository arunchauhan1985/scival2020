CREATE TABLE `sci_usertask` (
  `USERID` bigint NOT NULL,
  `TASKID` bigint NOT NULL,
  `MODULEID` bigint NOT NULL,
  PRIMARY KEY (`USERID`,`TASKID`,`MODULEID`)
);
