CREATE TABLE `keyword` (
  `KEYWORD_COLUMN` varchar(2000) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  `KEYWORDS_ID` bigint NOT NULL,
  `LANG` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '',
  PRIMARY KEY (`KEYWORDS_ID`)
);
