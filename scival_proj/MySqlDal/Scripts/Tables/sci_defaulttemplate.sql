CREATE TABLE `sci_defaulttemplate` (
  `MODULEID` bigint NOT NULL,
  `TEMPLATEID` bigint NOT NULL,
  `ACTIVE` bigint NOT NULL,
  `USERID` bigint DEFAULT NULL,
  `UPDATEDDATE` datetime DEFAULT NULL,
  PRIMARY KEY (`TEMPLATEID`)
);
