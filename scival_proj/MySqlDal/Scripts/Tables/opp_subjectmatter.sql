CREATE TABLE `opp_subjectmatter` (
  `SUBJECTMATTER_ID` bigint NOT NULL,
  `OPPORTUNITY_ID` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`SUBJECTMATTER_ID`)
);
