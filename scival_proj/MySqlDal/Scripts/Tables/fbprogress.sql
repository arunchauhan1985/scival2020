CREATE TABLE `fbprogress` (
  `FUNDINGBODY_ID` bigint NOT NULL,
  `FUNDINGBODYNAME` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `FBUPDATE` datetime DEFAULT NULL,
  `FBSTATUS` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `OPUPDATE` datetime DEFAULT NULL,
  `OPSTATUS` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `AWUPDATE` datetime DEFAULT NULL,
  `AWSTATUS` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`FUNDINGBODY_ID`)
);
