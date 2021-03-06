CREATE TABLE `fundingbody` (
  `FUNDINGBODY_ID` bigint NOT NULL,
  `ORGDBID` int DEFAULT NULL,
  `TYPE` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `TRUSTING` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `COUNTRY` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `STATE` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `COLLECTIONCODE` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `HIDDEN` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CANONICALNAME` blob,
  `PREFERREDORGNAME` blob,
  `CONTEXTNAME` blob,
  `ABBREVNAME` blob,
  `ELIGIBILITYDESCRIPTION` blob,
  `RECORDSOURCE` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `AWARDSUCCESRATE` decimal(10,2) DEFAULT NULL,
  `COMMENT_DESC` blob,
  `DEFUNCT` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CROSSREFID` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `EXTENDEDRECORD` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CAPTUREAWARDS` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CAPTUREOPPORTUNITIES` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `TIERINFO` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `AWARDSSUPPLIER` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `OPPORTUNITIESSUPPLIER` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `AWARDSFREQUENCY` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `OPPORTUNITIESFREQUENCY` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `PROFIT` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`FUNDINGBODY_ID`)
);
